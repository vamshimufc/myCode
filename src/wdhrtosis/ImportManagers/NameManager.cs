using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis;
using wdhrtosis.Models;
using Name = wdhrtosis.Models.Name;

namespace wdhrtosis.ImportManagers
{
    public interface INameManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);
    }
    public class NameManager : INameManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;
        //private string correlationId;

        public NameManager(
                  ILogger<NameManager> logger
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
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing names collection in NameManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Inactive in case the update to existing data (which follows in the transaction further below) fails.
            var names = (from m in workers

                         select new wdhrtosis.Models.Name
                         {
                             EmployeeId = m.WorkerId,
                             UniversalId = m.UniversalId,
                             NationalId = m.GovernmentIds.Count > 0 ? m.GovernmentIds.FirstOrDefault()?.IdNumber : null,
                             ImportCreatedDate = DateTime.Now,
                             ImportIsActiveRecord = false,
                             Prefix = m.Name.Prefix,
                             FirstName = m.Name.FirstName,
                             MiddleName = m.Name.MiddleName,
                             LastName = m.Name.LastName,
                             Suffix = m.Name.Suffix,
                             PreferredPrefix = m.Name.PreferredPrefix,
                             PreferredFirstName = m.Name.PreferredFirstName,
                             PreferredMiddleName = m.Name.PreferredMiddleName,
                             PreferredLastName = m.Name.PreferredLastName,
                             PreferredSuffix = m.Name.PreferredSuffix,
                             ReportingName = m.Name.ReportingName,
                             Pronunciation = m.Name.Pronunciation,
                             Gender = m.Biographic.Genders.FirstOrDefault().GenderType,
                             DateofBirth = m.Biographic.LifeEvents.BirthDate
                         }
                        )
                        .ToList();

            if (names?.Count > 0)
            {
                // Add new rows, but as Inactive in case the update (which follows in the transaction below) fails.
                _logger.LogInformation($"Row count of names object: {names.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertNames(names);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonName", ObjectCount = names.Count, Description = "Rows added. Source = NameManager.ProcessLarge()" });

                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    try
                    {
                        /*  For each new record (names), update the related records that exist in Integrations.PersonName where ImportIsActiveRecord = true
                            Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                        */
                        List<Name> namesToUpdate = (from o in _personImport.PersonName
                                                    where o.ImportIsActiveRecord == true
                                                    join newNames in names on o.UniversalId equals newNames.UniversalId
                                                    select o).ToList();

                        if (namesToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of namesToUpdate object: {namesToUpdate.Count} rows being replaced / deactivated.");
                            foreach (Name o in namesToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }

                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<Name> namesToActivate = (from x in _personImport.PersonName
                                                      where x.ImportIsActiveRecord == false 
                                                      select x).ToList();
                        foreach (Name x in namesToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }
                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonName", ObjectCount = namesToUpdate.Count, Description = "Rows updated / removed. Source = NameManager.ProcessLarge()" });
                        namesToUpdate.Clear();
                        namesToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                } //end EF Core transaction
            }
            names.Clear();
        }

        public List<ProcessSummary> ProcessSmall(IList<Worker> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing names collection in NameManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var names = (from m in workers

                         select new Name
                         {
                             EmployeeId = m.WorkerId,
                             UniversalId = m.UniversalId,
                             NationalId = m.GovernmentIds != null && m.GovernmentIds.Count > 0 ? m.GovernmentIds.FirstOrDefault()?.IdNumber : null,
                             ImportCreatedDate = DateTime.Now,
                             ImportIsActiveRecord = true,
                             Prefix = m.Name.Prefix,
                             FirstName = m.Name.FirstName,
                             MiddleName = m.Name.MiddleName,
                             LastName = m.Name.LastName,
                             Suffix = m.Name.Suffix,
                             PreferredPrefix = m.Name.PreferredPrefix,
                             PreferredFirstName = m.Name.PreferredFirstName,
                             PreferredMiddleName = m.Name.PreferredMiddleName,
                             PreferredLastName = m.Name.PreferredLastName,
                             PreferredSuffix = m.Name.PreferredSuffix,
                             ReportingName = m.Name.ReportingName,
                             Pronunciation = m.Name.Pronunciation,
                             Gender = m.Biographic != null && m.Biographic.Genders != null && m.Biographic.Genders.Count > 0 ?
                                        m.Biographic.Genders.FirstOrDefault()?.GenderType : null,
                             DateofBirth = m.Biographic != null && m.Biographic.LifeEvents != null ? m.Biographic.LifeEvents.BirthDate : null
                         }
                        )
                        .ToList();

            if (names?.Count > 0)
            {
                _logger.LogInformation($"Row count of names object: {names.Count} rows retrieved and being inserted.");
                _personImport.AddRange(names); //DRA Server Integration
                //_personImport.AddRange(names);
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonName", ObjectCount = names.Count, Description = "Rows added. Source = NameManager.ProcessSmall()" });

                try
                {
                    /*  For each new record (names), update the related records that exist in Integrations.PersonName where ImportIsActiveRecord = true
                        Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                    */
                    List<Name> namesToUpdate = (from o in _personImport.PersonName
                                                where o.ImportIsActiveRecord == true
                                                join newNames in names on o.EmployeeId equals newNames.EmployeeId
                                                select o).ToList();

                    if (namesToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of namesToUpdate object: {namesToUpdate.Count} rows being replaced / deactivated.");
                        foreach (Name o in namesToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }
                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonName", ObjectCount = namesToUpdate.Count, Description = "Rows updated / removed. Source = NameManager.ProcessSmall()" });
                    namesToUpdate.Clear();
                }
                catch
                {
                    throw;
                }
            }
            names.Clear();
            return summaryProcess;
        }

    }
}
