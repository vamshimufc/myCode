using EA.Mulesoft.Models.;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;

namespace wdhrtosis.ImportManagers
{
    public interface IEmploymentPositionManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);
    }
    public class EmploymentPositionManager : IEmploymentPositionManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;
        //private string correlationId;

        public EmploymentPositionManager(
                  ILogger<Manager> logger
                , PersonImportContext sisImport
                , IPersonImportDatabaseWrapper sisImportDatabaseManager
            )
        {
            _personImport = sisImport;
            _logger = logger;
            _personImportDatabaseWrapper = sisImportDatabaseManager;
        }


        public void ProcessLarge(IList<Worker> workers) //Using db transactionss here
        {
            //correlationId = Utility.CorrelationId;
            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = correlationId, ObjectProcessed = "Utility.GetMemoryUsage() before processing employmentPositions collection in EmploymentPositionManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Inactive in case the update to existing data (which follows in the transaction further below) fails.
            var employmentPositions = (from m in workers
                                       from ep in m.EmploymentProfile.EmploymentPosition

                                       select new EmploymentPosition
                                       {
                                           EmployeeId = m.Worker_id,
                                           UniversalId = m.Universal_id,
                                           ImportCorrelationId = correlationId,
                                           ImportCreatedDate = DateTime.Now,
                                           ImportIsActiveRecord = false,
                                           PrimaryJobIndicator = ep.PrimaryJobIndicator,
                                           EmployeeType = ep.EmployeeType,
                                           JobCode = ep.JobCode,
                                           JobTitle = ep.JobTitle,
                                           JobClassificationGroup = ep.JobClassificationGroup,
                                           BusinessTitle = ep.BusinessTitle,
                                           JobActiveIndicator = ep.JobActiveIndicator,
                                           JobStatus = ep.JobStatus,
                                           JobBenefitsActiveIndicator = ep.JobBenefitsActiveIndicator,
                                           Manager = ep.Manager,
                                           JobEffectiveDate = ep.JobEffectiveDate,
                                           JobEndDate = ep.JobEndDate,
                                           BuildingNumber = ep.BuildingNumber,
                                           BuildingName = ep.BuildingName,
                                           RoomNumber = ep.RoomNumber,
                                           FloorNumber = ep.FloorNumber,
                                           TimeType = ep.TimeType,
                                           JobFTE = ep.JobFTE,
                                           PayGroup = ep.PayGroup,
                                           TerminationReason = ep.TerminationReason,
                                           AnnualWorkPeriodStartDate = ep.AnnualWorkPeriodStartDate,
                                           AnnualWorkPeriodEndDate = ep.AnnualWorkPeriodEndDate,
                                           WorkPeriodPercentOfYear = ep.WorkPeriodPercentOfYear,
                                           DisbursementPlanPeriodStartDate = ep.DisbursementPlanPeriodStartDate,
                                           DisbursementPlanPeriodEndDate = ep.DisbursementPlanPeriodEndDate,
                                           PayType = ep.PayType
                                       }
                                      )
                                   .ToList();


            if (employmentPositions?.Count > 0)
            {
                // Add new rows, but as Inactive in case the update (which follows in the transaction below) fails.
                _logger.LogInformation($"Row count of names object: {employmentPositions.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertEmploymentPositions(employmentPositions);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = correlationId, ObjectProcessed = "PersonEmploymentPosition", ObjectCount = employmentPositions.Count, Description = "Rows added. Source = EmploymentPositionManager.ProcessLarge()" });

                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    try
                    {
                        /*  For each new record (names), update the related records that exist in sisImport.PersonEmploymentPosition where ImportIsActiveRecord = true
                            Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                        */
                        List<EmploymentPosition> EmploymentPositionsToUpdate = (from o in _personImport.PersonEmploymentPosition
                                                                                where o.ImportIsActiveRecord == true
                                                                                join newEP in employmentPositions on
                                                                                     new { o.EmployeeId, o.JobCode }
                                                                                     equals
                                                                                     new { newEP.EmployeeId, newEP.JobCode }
                                                                                select o).ToList();
                        if (EmploymentPositionsToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of EmploymentPositionsToUpdate object: {EmploymentPositionsToUpdate.Count} rows being replaced / deactivated.");
                            foreach (EmploymentPosition o in EmploymentPositionsToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }
                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<EmploymentPosition> epToActivate = (from x in _personImport.PersonEmploymentPosition
                                                                 where x.ImportIsActiveRecord == false && x.ImportCorrelationId == correlationId
                                                                 select x).ToList();
                        foreach (EmploymentPosition x in epToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }

                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = correlationId, ObjectProcessed = "PersonEmploymentPositions", ObjectCount = EmploymentPositionsToUpdate.Count, Description = "Rows updated / removed. Source = EmploymentPositionManager.ProcessLarge()" });
                        EmploymentPositionsToUpdate.Clear();
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
            employmentPositions.Clear();
        }
        public List<ProcessSummary> ProcessSmall(IList<Worker> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = correlationId, ObjectProcessed = "Utility.GetMemoryUsage() before processing employmentPositions collection in EmploymentPositionManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }


            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var employmentPositions = (from m in workers
                                       from ep in m.EmploymentProfile.EmploymentPosition

                                       select new EmploymentPosition
                                       {
                                           EmployeeId = m.Worker_id,
                                           UniversalId = m.Universal_id,
                                           ImportCorrelationId = correlationId,
                                           ImportCreatedDate = DateTime.Now,
                                           ImportIsActiveRecord = true,
                                           PrimaryJobIndicator = ep.PrimaryJobIndicator,
                                           EmployeeType = ep.EmployeeType,
                                           JobCode = ep.JobCode,
                                           JobTitle = ep.JobTitle,
                                           JobClassificationGroup = ep.JobClassificationGroup,
                                           BusinessTitle = ep.BusinessTitle,
                                           JobActiveIndicator = ep.JobActiveIndicator,
                                           JobStatus = ep.JobStatus,
                                           JobBenefitsActiveIndicator = ep.JobBenefitsActiveIndicator,
                                           Manager = ep.Manager,
                                           JobEffectiveDate = ep.JobEffectiveDate,
                                           JobEndDate = ep.JobEndDate,
                                           BuildingNumber = ep.BuildingNumber,
                                           BuildingName = ep.BuildingName,
                                           RoomNumber = ep.RoomNumber,
                                           FloorNumber = ep.FloorNumber,
                                           TimeType = ep.TimeType,
                                           JobFTE = ep.JobFTE,
                                           PayGroup = ep.PayGroup,
                                           TerminationReason = ep.TerminationReason,
                                           AnnualWorkPeriodStartDate = ep.AnnualWorkPeriodStartDate,
                                           AnnualWorkPeriodEndDate = ep.AnnualWorkPeriodEndDate,
                                           WorkPeriodPercentOfYear = ep.WorkPeriodPercentOfYear,
                                           DisbursementPlanPeriodStartDate = ep.DisbursementPlanPeriodStartDate,
                                           DisbursementPlanPeriodEndDate = ep.DisbursementPlanPeriodEndDate,
                                           PayType = ep.PayType
                                       }
                                      )
                                   .ToList();


            if (employmentPositions?.Count > 0)
            {
                // Add new rows, but as Inactive in case the update (which follows in the transaction below) fails.
                _logger.LogInformation($"Row count of names object: {employmentPositions.Count} rows retrieved and being inserted.");
                _personImport.AddRange(employmentPositions);
                summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = correlationId, ObjectProcessed = "PersonEmploymentPositions", ObjectCount = employmentPositions.Count, Description = "Rows added. Source = EmploymentPositionsManager.ProcessSmall()" });

                try
                {
                    /*  For each new record (names), update the related records that exist in sisImport.PersonEmploymentPositions where ImportIsActiveRecord = true
                        Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                    */
                    List<EmploymentPosition> EmploymentPositionsToUpdate = (from o in _personImport.PersonEmploymentPosition
                                                                            where o.ImportIsActiveRecord == true
                                                                            join newEP in employmentPositions on
                                                                                 new { o.EmployeeId, o.JobCode }
                                                                                 equals
                                                                                 new { newEP.EmployeeId, newEP.JobCode }
                                                                            select o).ToList();
                    if (EmploymentPositionsToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of EmploymentPositionsToUpdate object: {EmploymentPositionsToUpdate.Count} rows being replaced / deactivated.");
                        foreach (EmploymentPosition o in EmploymentPositionsToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }
                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { ExecutionTime = DateTime.Now, CorrelationId = correlationId, ObjectProcessed = "PersonEmploymentPositions", ObjectCount = EmploymentPositionsToUpdate.Count, Description = "Rows updated / removed. Source = EmploymentPositionManager.ProcessSmall()" });
                    EmploymentPositionsToUpdate.Clear();
                }
                catch
                {
                    throw;
                }
            }
            employmentPositions.Clear();
            return summaryProcess;
        }
    }
}
