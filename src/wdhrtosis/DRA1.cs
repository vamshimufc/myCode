using System;
using System.Threading;
using System.Threading.Tasks;
using wdhrtosis.Options;
using EA.TaskRunner;
using Microsoft.Extensions.Logging;
using wdhrtosis.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using wdhrtosis.ImportManagers;
using wdhrtosis.Data;

namespace wdhrtosis
{
    public class DRA1 : IRunnable
    {
        private readonly ILogger<DRA1> _logger;
        private readonly DRATaskOptions _draTaskOptions;
        private readonly PersonImportContext _personImport;
        public IMulesoftManager _mulesoftManager;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;


        public IAddressManager _addressManager;
        public IContactManager _contactManager;
        public IEmailAddressManager _emailAddressManager;
        public INameManager _nameManager;
        public IVisaManager _visaManager;

        public DRA1(ILogger<DRA1> logger
                    , DRATaskOptions draTaskOptions
                    , IMulesoftManager mulesoftManager
                    , IPersonImportDatabaseWrapper personImportDatabaseManager
                    , IAddressManager addressManager
                    , PersonImportContext personImport
                    , IContactManager contactManager
                    , IEmailAddressManager emailAddressManager
                    , INameManager nameManager
                    , IVisaManager visaManager
                    )

        {
            _draTaskOptions = draTaskOptions;
            _logger = logger;
            _mulesoftManager = mulesoftManager;
            _personImportDatabaseWrapper = personImportDatabaseManager;
            _personImport = personImport;
            _addressManager = addressManager;
            _contactManager = contactManager;
            _emailAddressManager = emailAddressManager;
            _nameManager = nameManager;
            _visaManager = visaManager;
        }

        public Task RunAsync(ITask currentTask, CancellationToken cancellationToken)
        {
            var correlationId = currentTask.TaskStatus.LastRunId;
            var draid = _draTaskOptions.TaskID;

            try
            {

                _logger.LogInformation("{draid}, {correlationId}, {message}", draid, correlationId, "begin work");
                _personImportDatabaseWrapper.WriteHistoryRecord(new History { Message = "Starting", LastRun = DateTime.Now, Success = false, CorrelationId = correlationId });

                //Set these values for easy access throughout application
                Utility.LogMemoryUsage = Startup.StaticConfig.GetValue<bool>("WorkerOptions:LogMemoryUsage");
                //Utility.CorrelationId = correlationId;

                //Reset for each execution
                Startup.TaskWasSuccess = false;

                ProcessWorkers().GetAwaiter().GetResult();

                /* To run / debug / test:

                1. In Postman, Execute  IDM DraJWT (POST https://is-login.wustl.edu/connect/token) to get access token for local testing
                2. Optionally set one or more break points
                3. Run this app in Debug mode
                4. In Postman, Execute DRA Run Task (POST https://localhost:5001/api/DRA1/runtask). Break point(s) should be hit in VS after a period of time (depending
                    on how much data is being returned) 

                    Multiple objects in Manager.cs should be populated with data from Mulesoft.
                    Inspect these objects via the Locals window (workers, names, addresses, emails, etc.)
                    Tables in Integrations database will be populated (PersonName, PersonAddress, PersonHistory, PersonProcessSummary, etc.)
                */

                _logger.LogInformation("{draid}, {correlationId}, {message}", draid, correlationId, "end work");

                cancellationToken.ThrowIfCancellationRequested();

            }
            catch (TaskCanceledException)
            {
                _logger.LogError("{draid}, {correlationId}, {message}", draid, correlationId, "Task Cancelled");

                return Task.FromCanceled(cancellationToken);

            }
            catch (Exception e)
            {
                _logger.LogError("{draid}, {correlationId}, {message}", draid, correlationId, "Failed work");

                var es = e?.ToString();
                var ein = e?.InnerException?.ToString();

                if (ein == null) ein = "";
                if (es == null) es = "";

                var errormessage = "Exception: " + es + "InnerException: " + ein;

                _logger.LogError("{draid}, {correlationId}, {message}, {exception}, {innerexception}", draid, correlationId, errormessage, es, ein);

                _logger.LogError("{draid}, {correlationId}, {message}", draid, correlationId, "exit task");

                return Task.FromException(e);

            }

            _personImportDatabaseWrapper.WriteHistoryRecord(new History { Message = "Finishing", LastRun = DateTime.Now, Success = Startup.TaskWasSuccess, CorrelationId = correlationId });

            return Task.CompletedTask;

        }

        private async Task ProcessWorkers()
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //string correlationId = Utility.CorrelationId;

            double memoryUsed;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before getting workers collection", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            bool useBulkEndpoint = Startup.StaticConfig.GetValue<bool>("WorkerOptions:UseBulkEndPoint");

            _logger.LogInformation($"UseBulkEndpoint option: {useBulkEndpoint}");

            var workers = await _mulesoftManager.GetAllWorkersAsync();

            if (workers == null || workers.Count == 0)
            {
                _logger.LogInformation("No data from Workday to process: workers object is empty.");
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "mulesoftManager.GetAllWorkersAsync", ObjectCount = 0, Description = "No data from Workday to process: workers object is empty" });
                Startup.TaskWasSuccess = false;
                return;
            }

            int smallProcessMaxCount = Startup.StaticConfig.GetValue<int>("WorkerOptions:SmallLoadThreshold");

            // process the sub-objects from workers. 
            if (workers.Count <= smallProcessMaxCount)
            {
                //For smaller amounts of data, wrap everything in a db transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    try
                    {
                        summaryProcess.AddRange(_addressManager.ProcessSmall(workers));
                        summaryProcess.AddRange(_emailAddressManager.ProcessSmall(workers));
                        summaryProcess.AddRange(_nameManager.ProcessSmall(workers));
                        summaryProcess.AddRange(_contactManager.ProcessSmall(workers));
                        summaryProcess.AddRange(_visaManager.ProcessSmall(workers));

                        _personImport.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();
                }
                //_adisImportDatabaseWrapper.WriteListofProcessSummaryRecords(summaryProcess);
            }
            else
            {
                //For larger amounts of data, use multiple db transactions, in separate processes. 
                //Experiencing performance issues if we try to do large processing all in one transaction.

                _nameManager.ProcessLarge(workers);
                _addressManager.ProcessLarge(workers);
                _emailAddressManager.ProcessLarge(workers);
                _contactManager.ProcessLarge(workers);
                _visaManager.ProcessLarge(workers);
            }

            workers.Clear();
            Startup.TaskWasSuccess = true;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() at end of processing in Manager.RunAsync()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }
        }
    }

}

