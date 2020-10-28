using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;
using EmploymentProfile = wdhrtosis.Models.EmploymentProfile;

namespace wdhrtosis.ImportManagers
{
    public interface IEmploymentProfileManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);
    }
    public class EmploymentProfileManager : IEmploymentProfileManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;
        //private string correlationId;

        public EmploymentProfileManager(
                  ILogger<EmploymentProfileManager> logger
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
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing employmentProfiles collection in EmploymentProfileManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Inactive in case the update to existing data (which follows in the transaction further below) fails.
            var employmentProfiles = (from m in workers

                                      select new EmploymentProfile
                                      {
                                          EmployeeId = m.WorkerId,
                                          UniversalId = m.UniversalId,
                                          ImportCreatedDate = DateTime.Now,
                                          ImportIsActiveRecord = false,
                                          Status = m.EmploymentProfile.Status,
                                          Campus = m.EmploymentProfile.Campus,
                                          TotalFTE = m.EmploymentProfile.TotalFTE,
                                          ContinuousServiceDate = m.EmploymentProfile.ContinuousServiceDate,
                                          OptOutWURecord = m.EmploymentProfile.OptOutWURecord,
                                          LocationHierarchy = m.EmploymentProfile.LocationHierarchy,
                                          OptOutTotalDirectoryInfo = m.EmploymentProfile.OptOutTotalDirectoryInfo,
                                          OrigHireDate = m.EmploymentProfile.OrigHireDate,
                                          TerminationDate = m.EmploymentProfile.TerminationDate,
                                          WorkerType = m.EmploymentProfile.WorkerType,
                                          CampusMailStop = m.EmploymentProfile.CampusMailStop,
                                          HireDate = m.EmploymentProfile.HireDate,
                                          RetireeIndicator = m.EmploymentProfile.RetireeIndicator,
                                          WorkerPrimaryPositionType = m.EmploymentProfile.WorkerPrimaryPositionType
                                      }
                           )
                        .ToList();

            if (employmentProfiles?.Count > 0)
            {
                // Add new rows, but as Inactive in case the update (which follows in the transaction below) fails.
                _logger.LogInformation($"Row count of names object: {employmentProfiles.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertEmploymentProfiles(employmentProfiles);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "PersonEmploymentProfile", ObjectCount = employmentProfiles.Count, Description = "Rows added. Source = EmploymentProfileManager.ProcessLarge()" });


                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    try
                    {
                        /*  For each new record (names), update the related records that exist in sisImport.PersonEmploymentProfile where ImportIsActiveRecord = true
                            Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                        */
                        List<EmploymentProfile> employmentProfilesToUpdate = (from o in _personImport.PersonEmploymentProfile
                                                                              where o.ImportIsActiveRecord == true
                                                                              join newEP in employmentProfiles on o.UniversalId equals newEP.UniversalId
                                                                              select o).ToList();

                        if (employmentProfilesToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of employmentProfilesToUpdate object: {employmentProfilesToUpdate.Count} rows being replaced / deactivated.");
                            foreach (EmploymentProfile o in employmentProfilesToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }
                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<EmploymentProfile> epToActivate = (from x in _personImport.PersonEmploymentProfile
                                                                where x.ImportIsActiveRecord == false
                                                                select x).ToList();
                        foreach (EmploymentProfile x in epToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }
                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "PersonEmploymentProfile", ObjectCount = employmentProfilesToUpdate.Count, Description = "Rows updated / removed. Source = EmploymentProfileManager.ProcessLarge()" });
                        employmentProfilesToUpdate.Clear();
                        epToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                } //end EF Core transaction
            }
            employmentProfiles.Clear();
        }
        public List<ProcessSummary> ProcessSmall(IList<Worker> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing employmentProfiles collection in EmploymentProfileManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }


            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var employmentProfiles = (from m in workers

                                      select new EmploymentProfile
                                      {
                                          EmployeeId = m.WorkerId,
                                          UniversalId = m.UniversalId,
                                          ImportCreatedDate = DateTime.Now,
                                          ImportIsActiveRecord = true,
                                          Status = m.EmploymentProfile.Status,
                                          Campus = m.EmploymentProfile.Campus,
                                          TotalFTE = m.EmploymentProfile.TotalFTE,
                                          ContinuousServiceDate = m.EmploymentProfile.ContinuousServiceDate,
                                          OptOutWURecord = m.EmploymentProfile.OptOutWURecord,
                                          LocationHierarchy = m.EmploymentProfile.LocationHierarchy,
                                          OptOutTotalDirectoryInfo = m.EmploymentProfile.OptOutTotalDirectoryInfo,
                                          OrigHireDate = m.EmploymentProfile.OrigHireDate,
                                          TerminationDate = m.EmploymentProfile.TerminationDate,
                                          WorkerType = m.EmploymentProfile.WorkerType,
                                          CampusMailStop = m.EmploymentProfile.CampusMailStop,
                                          HireDate = m.EmploymentProfile.HireDate,
                                          RetireeIndicator = m.EmploymentProfile.RetireeIndicator,
                                          WorkerPrimaryPositionType = m.EmploymentProfile.WorkerPrimaryPositionType
                                      }
                           )
                        .ToList();

            if (employmentProfiles?.Count > 0)
            {
                _logger.LogInformation($"Row count of names object: {employmentProfiles.Count} rows retrieved and being inserted.");
                _personImport.AddRange(employmentProfiles);
                summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "PersonEmploymentProfile", ObjectCount = employmentProfiles.Count, Description = "Rows added. Source = EmploymentProfilesManager.ProcessSmall()" });
                try
                {
                    /*  For each new record (names), update the related records that exist in sisImport.PersonEmploymentProfile where ImportIsActiveRecord = true
                        Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                    */
                    List<EmploymentProfile> employmentProfilesToUpdate = (from o in _personImport.PersonEmploymentProfile
                                                                          where o.ImportIsActiveRecord == true
                                                                          join newEP in employmentProfiles on o.UniversalId equals newEP.UniversalId
                                                                          select o).ToList();

                    if (employmentProfilesToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of employmentProfilesToUpdate object: {employmentProfilesToUpdate.Count} rows being replaced / deactivated.");
                        foreach (EmploymentProfile o in employmentProfilesToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }
                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, ObjectProcessed = "PersonEmploymentProfile", ObjectCount = employmentProfilesToUpdate.Count, Description = "Rows updated / removed. Source = EmploymentProfilesManager.ProcessSmall()" });
                    employmentProfilesToUpdate.Clear();
                }
                catch
                {
                    throw;
                }
            }
            employmentProfiles.Clear();
            return summaryProcess;
        }

    }
}
