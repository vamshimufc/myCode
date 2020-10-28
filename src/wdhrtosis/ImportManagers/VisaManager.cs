using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;
using Visa = wdhrtosis.Models.Visa;

namespace wdhrtosis.ImportManagers
{
    public interface IVisaManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);
    }
    public class VisaManager : IVisaManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;

        public VisaManager(
                  ILogger<VisaManager> logger
                , PersonImportContext personImport
                , IPersonImportDatabaseWrapper personImportDatabaseManager
            )
        {
            _personImport = personImport;
            _logger = logger;
            _personImportDatabaseWrapper = personImportDatabaseManager;
        }


        public void ProcessLarge(IList<Worker> workers) //Using db transactionss here
        {
            //correlationId = Utility.CorrelationId;
            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing visas collection in VisaManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Inactive in case the update to existing data (which follows in the transaction further below) fails.
            var visas = (from m in workers
                         from v in m.Visas

                         select new wdhrtosis.Models.Visa
                         {
                             EmployeeId = m.WorkerId,
                             UniversalId = m.UniversalId,
                             VisaNumber = v.VisaNumber,
                             VisaType = v.VisaType,
                             VisaCountry = v.VisaCountry,
                             VisaPermitStatus = v.VisaPermitStatus,
                             VisaPermitDuration = v.VisaPermitDuration,
                             VisaPermitDurationType = v.VisaPermitDurationType,
                             VisaIssuingAuthority = v.VisaIssuingAuthority,
                             VisaIssueDate = v.VisaIssueDate,
                             VisaExpiryDate = v.VisaExpiryDate,
                             VisaVerificationDate = v.VisaVerificationDate,
                             ImportCreatedDate = DateTime.Now,
                             ImportIsActiveRecord = false,
                         }
                        )
                        .ToList();

            if (visas?.Count > 0)
            {
                // Add new rows, but as Inactive in case the update (which follows in the transaction below) fails.
                _logger.LogInformation($"Row count of visas object: {visas.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertVisas(visas);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonVisa", ObjectCount = visas.Count, Description = "Rows added. Source = VisaManager.ProcessLarge()" });

                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    try
                    {
                        /*  For each new record (visas), update the related records that exist in Integrations.PersonVisa where ImportIsActiveRecord = true
                            Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                        */
                        List<Visa> visasToUpdate = (from o in _personImport.PersonVisa
                                                    where o.ImportIsActiveRecord == true
                                                    join newVisas in visas on o.UniversalId equals newVisas.UniversalId
                                                    select o).ToList();

                        if (visasToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of visasToUpdate object: {visasToUpdate.Count} rows being replaced / deactivated.");
                            foreach (Visa o in visasToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }

                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<Visa> visasToActivate = (from x in _personImport.PersonVisa
                                                      where x.ImportIsActiveRecord == false
                                                      select x).ToList();
                        foreach (Visa x in visasToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }

                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonVisa", ObjectCount = visasToUpdate.Count, Description = "Rows updated / removed. Source = VisaManager.ProcessLarge()" });
                        visasToUpdate.Clear();
                        visasToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                } //end EF Core transaction
            }
            visas.Clear();
        }

        public List<ProcessSummary> ProcessSmall(IList<Worker> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing visas collection in VisaManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var visas = (from m in workers
                         from v in m.Visas

                         select new wdhrtosis.Models.Visa
                         {
                             EmployeeId = m.WorkerId,
                             UniversalId = m.UniversalId,
                             VisaNumber = v.VisaNumber,
                             VisaType = v.VisaType,
                             VisaCountry = v.VisaCountry,
                             VisaPermitStatus = v.VisaPermitStatus,
                             VisaPermitDuration = v.VisaPermitDuration,
                             VisaPermitDurationType = v.VisaPermitDurationType,
                             VisaIssuingAuthority = v.VisaIssuingAuthority,
                             VisaIssueDate = v.VisaIssueDate,
                             VisaExpiryDate = v.VisaExpiryDate,
                             VisaVerificationDate = v.VisaVerificationDate,
                             ImportCreatedDate = DateTime.Now,
                             ImportIsActiveRecord = true,
                         }
                        )
                        .ToList();

            if (visas?.Count > 0)
            {
                _logger.LogInformation($"Row count of visas object: {visas.Count} rows retrieved and being inserted.");
                _personImport.AddRange(visas);
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonVisa", ObjectCount = visas.Count, Description = "Rows added. Source = VisaManager.ProcessSmall()" });

                try
                {
                    /*  For each new record (visas), update the related records that exist in Integrations.PersonVisa where ImportIsActiveRecord = true
                        Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                    */
                    List<Visa> visasToUpdate = (from o in _personImport.PersonVisa
                                                where o.ImportIsActiveRecord == true
                                                join newVisas in visas 
                                                on o.UniversalId equals newVisas.UniversalId
                                                select o).ToList();

                    if (visasToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of visasToUpdate object: {visasToUpdate.Count} rows being replaced / deactivated.");
                        foreach (Visa o in visasToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }
                    _personImport.SaveChanges(); 
                    summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonVisa", ObjectCount = visasToUpdate.Count, Description = "Rows updated / removed. Source = VisaManager.ProcessSmall()" });
                    visasToUpdate.Clear();
                }
                catch
                {
                    throw;
                }
            }
            visas.Clear();
            return summaryProcess;
        }
    }
}
