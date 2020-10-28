using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EA.Mulesoft;
using EA.Mulesoft.Models;
using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Configuration;
using wdhrtosis.Models;
using Microsoft.EntityFrameworkCore;
using wdhrtosis.Data;
using wdhrtosis.ImportManagers;

namespace wdhrtosis
{
    public interface IMulesoftManager
    {
        Task<IList<Worker>> GetAllWorkersAsync();
    }

    public class MulesoftManager : IMulesoftManager
    {
        private readonly IWorkerProvider _workerProvider;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private double memoryUsed;
        private List<Worker> workers;
        private readonly WorkdayImportContext _workdayImportContext;
        private readonly IAddressManager _addressManager;
        public MulesoftManager(IWorkerProvider workerProvider
                               , IPersonImportDatabaseWrapper personImportDatabaseManager
                               , WorkdayImportContext workdayImportContext
                               , IAddressManager addressManager)
        {
            _workerProvider = workerProvider;
            _personImportDatabaseWrapper = personImportDatabaseManager;
            _workdayImportContext = workdayImportContext;
            _addressManager = addressManager;
    }

        public async Task<IList<Worker>> GetAllWorkersAsync()
        {
            var emptyWorkers = new List<Worker>();

            DateTimeOffset lastGoodRun = DateTime.Parse("01/14/2019  3:19:47 PM");

            //DateTimeOffset lastGoodRun = _personImportDatabaseWrapper.GetLastSucessfullRun();
            DateTimeOffset entrydate = DateTime.UtcNow;
            DateTimeOffset? deltaFrom = null;
            DateTimeOffset? deltaTo = null;

            int totalRecords = 0;
            //int pageCount = 0;

            bool useBulkEndpoint = Startup.StaticConfig.GetValue<bool>("WorkerOptions:UseBulkEndPoint");
            //string correlationId = Utility.CorrelationId;

            // Only use deltas if NOT doing a full data load. 
            // A lastGoodRun of 01/01/1900 will intentionally be entered into the History table (manually with the stored procedure 
            // spAddFullLoadTriggerRecordToWDHR) to trigger a full data load
            if (lastGoodRun >= DateTimeOffset.UtcNow.AddYears(-100))
            {
                deltaFrom = lastGoodRun;
                deltaTo = DateTime.UtcNow;
            }

            /* 
                For the inital (page 1) call, max_record is 0 and that parameter is not included in the API call.
                For successive calls when useBulkEndPoint is true, the max_record is determined from the metadata alnd is included in the API calls.
                For successive calls when useBulkEndPoint is false, the max_record is mot included in the API calls.

                If we fail to fetch on any page, return an empty resultset (emptyWorkers). 
             */

            var options = new WorkerRequestOptions
            {
                Count = 100, //100 is the default
                Page = 1,
                Sections = new List<SectionType>
                    {
                        SectionType.Personal,
                        SectionType.Employment,
                    },
                Entry_Date = entrydate,
                UseBulkEndPoint = useBulkEndpoint,
                DeltaFrom = deltaFrom,
                DeltaTo = deltaTo,
                MaxRecord = totalRecords
            };

            var responseValue = await _workerProvider.GetWorkersAsync(options, _workdayImportContext, ProcessSummaryCallback);

            if (responseValue <= 0)
            {
                return emptyWorkers;
            }

            _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "_workerProvider.GetWorkersAsync() TotalRecords", ObjectCount = responseValue, Description = "Source = MulesoftManager.GetAllWorkersAsync" });

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() after all calls to _workerProvider.GetWorkersAsync() in MulesoftManager", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            if (workers != null && workers.Count > 0)
            {
                return workers;
            }

            return emptyWorkers;

            #region OLDCODE

            //var responseValue = await _workerProvider.GetWorkersAsync(options, _integrationContext, ProcessSummaryCallback);


            //if (workerResponse == null || workerResponse.Data == null || workerResponse.Data.Count == 0)
            //{
            //    return emptyWorkers;
            //}

            //workers.AddRange(workerResponse.Data);

            //totalRecords = workerResponse.Meta.TotalRecords;
            //pageCount = workerResponse.Meta.TotalPages;

            //if (deltaFrom.HasValue)
            //{
            //    _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "WorkerRequestOptions: DeltaFrom", ObjectCount = 0, Description = deltaFrom.ToString() });
            //}
            //else
            //{
            //    _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "WorkerRequestOptions: DeltaFrom", ObjectCount = 0, Description = "null" });
            //}
            //if (deltaTo.HasValue)
            //{
            //    _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "WorkerRequestOptions: DeltaTo", ObjectCount = 0, Description = deltaTo.ToString() });
            //}
            //else
            //{
            //    _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "WorkerRequestOptions: DeltaTo", ObjectCount = 0, Description = "null" });
            //}
            ////_personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "WorkerRequestOptions: UseBulkEndPoint", ObjectCount = 0, Description = GetWorkerRequest(1).UseBulkEndPoint.ToString() });
            ////_personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "responseViewModel.MetaInformation.TotalRecords", ObjectCount = totalRecords, Description = "Source = MulesoftManager.GetAllWorkersAsync" });
            ////_personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "responseViewModel.MetaInformation.TotalPages", ObjectCount = pageCount, Description = "Source = MulesoftManager.GetAllWorkersAsync" });

            //if (useBulkEndpoint == true)
            //{
            //    GetWorkerRequest(1).MaxRecord = totalRecords;
            //}

            ////process remaining pages 
            //for (int i = 2; i <= pageCount; i++)
            //{
            //    _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "responseViewModel current page being processed", ObjectCount = i, Description = "of " + pageCount + " total pages" });

            //    if (Utility.LogMemoryUsage)
            //    {
            //        memoryUsed = Utility.GetMemoryUsage();
            //        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before page " + i + " call to _workerProvider.GetWorkersAsync() in MulesoftManager", ObjectCount = 0, Description = memoryUsed + " MB used" });
            //    }
            //    workerResponse = await _workerProvider.GetWorkersAsync(GetWorkerRequest(i));

            //    if (workerResponse == null || workerResponse.Data == null || workerResponse.Data.Count == 0)
            //    {
            //        return emptyWorkers;
            //    }
            //    else
            //    {
            //        workers.AddRange(workerResponse.Data);
            //    }
            //}

            //if (Utility.LogMemoryUsage)
            //{
            //    memoryUsed = Utility.GetMemoryUsage();
            //    _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() after all calls to _workerProvider.GetWorkersAsync() in MulesoftManager", ObjectCount = 0, Description = memoryUsed + " MB used" });
            //}

            //workerResponse = null; //explicitly dispose 

            //if (workers == null)
            //{
            //    return emptyWorkers;
            //}

            //else
            //{
            //    // remove any null worker objects
            //    var nonNullWorkers = (from w in workers
            //                          where w != null
            //                          select w).ToList();


            //    workers.Clear(); //explicitly dispose 
            //    return nonNullWorkers;
            //}
            #endregion OLDCODE
        }

        private void ProcessSummaryCallback()
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            int smallProcessMaxCount = Startup.StaticConfig.GetValue<int>("WorkerOptions:SmallLoadThreshold");
            workers = _workdayImportContext.Workers
                           .Include(w => w.Contact.Addresses)
                           .Include(w => w.Visas)
                           .Include(w => w.Biographic)
                           .Include(w => w.EmploymentProfile)
                           .Include(w => w.Name)
                           .Include(w => w.GovernmentIds)
                           .Include(w => w.Contact.PhoneNumbers)
                           .Include(w => w.Contact.EmailAddresses)
                    .ToList();

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary
                {
                    LastRun = DateTime.Now,
                    //CorrelationId = correlationId,
                    ObjectProcessed = "Utility.GetMemoryUsage() from _workerProvider.GetWorkersAsync() in MulesoftManager",
                    ObjectCount = 0,
                    Description = $"{memoryUsed} MB used"
                });
            }

        }
    }
}
