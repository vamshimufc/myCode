using EA.Mulesoft.Models.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;

namespace wdhrtosis.ImportManagers
{
    public interface IOrganizationManager
    {
        void ProcessLarge(IList<WorkerViewModel> workers);
        List<ProcessSummary> ProcessSmall(IList<WorkerViewModel> workers);
    }
    public class OrganizationManager : IOrganizationManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;
        //private string correlationId;

        public OrganizationManager(
                  ILogger<Manager> logger
                , PersonImportContext ssisImport
                , IPersonImportDatabaseWrapper sisImportDatabaseManager
            )
        {
            _personImport = ssisImport;
            _logger = logger;
            _personImportDatabaseWrapper = sisImportDatabaseManager;
        }


        public void ProcessLarge(IList<WorkerViewModel> workers) //Using db transactionss here
        {
            //correlationId = Utility.CorrelationId;
            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = "", ObjectProcessed = "Utility.GetMemoryUsage() before processing organizations in OrganizationManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Inactive in case the update to existing data (which follows in the transaction further below) fails.
            var organizations = (from m in workers
                                 from ep in m.EmploymentProfile.EmploymentPosition
                                 from o in ep.Organization

                                 select new Organization
                                 {
                                     EmployeeId = m.Worker_id,
                                     UniversalId = m.Universal_id,
                                     ImportCorrelationId = "",
                                     ImportCreatedDate = DateTime.Now,
                                     ImportIsActiveRecord = false,
                                     OrganizationReferenceID = o.OrganizationReferenceID,
                                     OrganizationName = o.OrganizationName,
                                     OrganizationCode = o.OrganizationCode,
                                     OrganizationType = o.OrganizationType
                                 }
                        )
                     .ToList();

            if (organizations?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of organizations object: {organizations.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertOrganizations(organizations);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = "", ObjectProcessed = "PersonOrganization", ObjectCount = organizations.Count, Description = "Rows added. Source = OrganizationManager.ProcessLarge()" });


                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    /*  For each new record (addresses), update the related records that exist in ssisImport.PersonOrganization where ImportIsActiveRecord = true
                         Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                     */

                    try
                    {
                        List<Organization> orgsToUpdate = (from o in _personImport.PersonOrganization
                                                           where o.ImportIsActiveRecord == true
                                                           join newOrgs in organizations on
                                                                new { o.EmployeeId, o.OrganizationReferenceID }
                                                                equals
                                                                new { newOrgs.EmployeeId, newOrgs.OrganizationReferenceID }
                                                           select o).ToList();
                        if (orgsToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of orgsToUpdate object: {orgsToUpdate.Count} rows being replaced / deactivated.");
                            foreach (Organization o in orgsToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }
                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<Organization> orgsToActivate = (from x in _personImport.PersonOrganization
                                                             where x.ImportIsActiveRecord == false && x.ImportCorrelationId == ""
                                                             select x).ToList();
                        foreach (Organization x in orgsToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }

                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = "", ObjectProcessed = "PersonOrganization", ObjectCount = orgsToUpdate.Count, Description = "Rows updated / removed. Source = OrganizationManager.ProcessLarge()" });
                        orgsToUpdate.Clear();
                        orgsToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                } //end EF Core transaction
            }
            organizations.Clear();
        }
        public List<ProcessSummary> ProcessSmall(IList<WorkerViewModel> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = "", ObjectProcessed = "Utility.GetMemoryUsage() before processing organizations in OrganizationManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }


            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var organizations = (from m in workers
                                 from ep in m.EmploymentProfile.EmploymentPosition
                                 from o in ep.Organization

                                 select new Organization
                                 {
                                     EmployeeId = m.Worker_id,
                                     UniversalId = m.Universal_id,
                                     ImportCorrelationId = "",
                                     ImportCreatedDate = DateTime.Now,
                                     ImportIsActiveRecord = true,
                                     OrganizationReferenceID = o.OrganizationReferenceID,
                                     OrganizationName = o.OrganizationName,
                                     OrganizationCode = o.OrganizationCode,
                                     OrganizationType = o.OrganizationType
                                 }
                        )
                     .ToList();

            if (organizations?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of organizations object: {organizations.Count} rows retrieved and being inserted.");
                _personImport.AddRange(organizations);
                summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = "", ObjectProcessed = "PersonOrganization", ObjectCount = organizations.Count, Description = "Rows added. Source = OrganizationManager.ProcessSmall()" });


                /*  For each new record (addresses), update the related records that exist in ssisImport.PersonOrganization where ImportIsActiveRecord = true
                     Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                 */

                try
                {
                    List<Organization> orgsToUpdate = (from o in _personImport.PersonOrganization
                                                       where o.ImportIsActiveRecord == true
                                                       join newOrgs in organizations on
                                                            new { o.EmployeeId, o.OrganizationReferenceID }
                                                            equals
                                                            new { newOrgs.EmployeeId, newOrgs.OrganizationReferenceID }
                                                       select o).ToList();
                    if (orgsToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of orgsToUpdate object: {orgsToUpdate.Count} rows being replaced / deactivated.");
                        foreach (Organization o in orgsToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }

                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = "", ObjectProcessed = "PersonOrganization", ObjectCount = orgsToUpdate.Count, Description = "Rows updated / removed. Source = OrganizationManager.ProcessSmall()" });
                    orgsToUpdate.Clear();
                }
                catch
                {
                    throw;
                }

            }
            organizations.Clear();
            return summaryProcess;
        }
    }
}
