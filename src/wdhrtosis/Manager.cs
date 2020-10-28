using System;
//using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using wdhrtosis.Data;
using wdhrtosis.ImportManagers;
using wdhrtosis.Models;

namespace wdhrtosis
{
    public interface IManager
    {
        Task RunAsync();
    }

    public class Manager : IManager
    {
        private readonly ILogger _logger;
        private readonly IMulesoftManager _mulesoftManager;
        private readonly IIntegrationDatabaseWrapper _integrationDatabaseWrapper;
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        //private readonly IMapper _mapper;
        //private readonly INameManager _nameManager;
        private readonly IAddressManager _addressManager;
        //private readonly IEmailAddressManager _emailaddressManager;
        ////private readonly IOrganizationManager _organizationManager;
        //private readonly IEmploymentProfileManager _employmentProfileManager;
        ////private readonly IEmploymentPositionManager _employmentPositionManager;
        //private readonly IContactManager _contactManager;
        //private readonly IVisaManager _visaManager;

        public Manager(IMulesoftManager mulesoftManager
                        , IIntegrationDatabaseWrapper integrationDatabaseWrapper
                        , ILogger<Manager> logger
                        , PersonImportContext personImport
                        , IPersonImportDatabaseWrapper sisImportDatabaseManager
                        //, INameManager nameManager
                        , IAddressManager addressManager
                    //, IEmailAddressManager emailaddressManager
                    ////, IOrganizationManager organizationManager
                    //, IEmploymentProfileManager employmentProfileManager
                    ////, IEmploymentPositionManager employmentPositionManager
                    //, IContactManager contactManager
                    //, IVisaManager visaManager
                    //, IMapper mapper
                    )
        {
            _mulesoftManager = mulesoftManager;
            _integrationDatabaseWrapper = integrationDatabaseWrapper;
            _personImport = personImport;
            _logger = logger;
            _personImportDatabaseWrapper = sisImportDatabaseManager;
            //_nameManager = nameManager;
            _addressManager = addressManager;
            //_emailaddressManager = emailaddressManager;
            ////_organizationManager = organizationManager;
            //_employmentProfileManager = employmentProfileManager;
            ////_employmentPositionManager = employmentPositionManager;
            //_contactManager = contactManager;
            //_visaManager = visaManager;
            //_mapper = mapper;

        }

        public async Task RunAsync()
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //string //correlationId = Utility.CorrelationId;

            double memoryUsed;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before getting workers collection", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            bool useBulkEndpoint = Startup.StaticConfig.GetValue<bool>("WorkerOptions:UseBulkEndPoint");

            _logger.LogInformation($"UseBulkEndpoint option: {useBulkEndpoint}");

            var workers = await _mulesoftManager.GetAllWorkersAsync();

            var mappedAddresses = _addressManager.GetAddresses(workers);

            if (workers == null || workers.Count == 0)
            {
                _logger.LogInformation("No data from Workday to process: workers object is empty.");
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "mulesoftManager.GetAllWorkersAsync", ObjectCount = 0, Description = "No data from Workday to process: workers object is empty" });
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
                        ////_integrationDatabaseWrapper.InsertAddresses(workers);
                        //summaryProcess.AddRange(_nameManager.ProcessSmall(workers));
                        summaryProcess.AddRange(_addressManager.ProcessSmall(mappedAddresses));
                        //summaryProcess.AddRange(_contactManager.ProcessSmall(workers));
                        //summaryProcess.AddRange(_contactManager.ProcessContactsToSISIntegration(workers));
                        //summaryProcess.AddRange(_emailaddressManager.ProcessSmall(workers));
                        //summaryProcess.AddRange(_employmentPositionManager.ProcessSmall(workers));
                        //summaryProcess.AddRange(_employmentProfileManager.ProcessSmall(workers));
                        //summaryProcess.AddRange(_visaManager.ProcessSmall(workers));

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
                _personImportDatabaseWrapper.WriteListofProcessSummaryRecords(summaryProcess);
            }
            else
            {
                //For larger amounts of data, use multiple db transactions, in separate processes. 
                //Experiencing performance issues if we try to do large processing all in one transaction.

                //_nameManager.ProcessLarge(workers);
                //_addressManager.ProcessLarge(workers);
                //_emailaddressManager.ProcessLarge(workers);
                //_employmentProfileManager.ProcessLarge(workers);
                ////_employmentPositionManager.ProcessLarge(workers);
                //_contactManager.ProcessLarge(workers);
                //_visaManager.ProcessLarge(workers);
            }

            workers.Clear();
            Startup.TaskWasSuccess = true;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() at end of processing in Manager.RunAsync()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }
        }
    }
}
