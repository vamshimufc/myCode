using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;
using wdhrtosis.Validation;
using Email = wdhrtosis.Models.Email;

namespace wdhrtosis.ImportManagers
{
    public interface IEmailAddressManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);
    }
    public class EmailAddressManager : IEmailAddressManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;

        public EmailAddressManager(
                  ILogger<EmailAddressManager> logger
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
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing emails collection in EmailAddressManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Inactive in case the update to existing data (which follows in the transaction further below) fails.
            var emails = (from m in workers
                          from e in m.Contact.EmailAddresses

                          select new Email
                          {
                              EmployeeId = m.WorkerId,
                              UniversalId = m.UniversalId,
                              EmailAddress = e.EmailAddress,
                              EmailType = e.EmailType,
                              EmailComment = e.EmailComment,
                              PrimaryIndicator = e.PrimaryIndicator,
                              PublicIndicator = e.PublicIndicator,
                              ImportCreatedDate = DateTime.Now,
                              ImportIsActiveRecord = false,
                          }
                        )
                    .ToList();

            if (emails?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of emails object: {emails.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertEmailAddresses(emails);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonEmailAddress", ObjectCount = emails.Count, Description = "Rows added. Source = Manager.RunAsync" });
                
                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    /*  For each new record (emails), update the related records that exist in Integrations.PersonEmailAddress where ImportIsActiveRecord = true
                        Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                    */

                    try
                    {
                        List<Email> emailsToUpdate = (from o in _personImport.PersonEmailAddress
                                                      where o.ImportIsActiveRecord == true
                                                      join newEmailAddr in emails on
                                                           new { o.UniversalId, o.EmailType }
                                                           equals
                                                           new { newEmailAddr.UniversalId, newEmailAddr.EmailType }
                                                      select o).ToList();

                        if (emailsToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of emailsToUpdate object: {emailsToUpdate.Count} rows being replaced / deactivated.");
                            foreach (Email o in emailsToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }
                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<Email> emailsToActivate = (from x in _personImport.PersonEmailAddress
                                                        where x.ImportIsActiveRecord == false
                                                        select x).ToList();
                        foreach (Email x in emailsToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }

                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonEmailAddress", ObjectCount = emailsToUpdate.Count, Description = "Rows updated / removed. Source = Manager.RunAsync" });
                        emailsToUpdate.Clear();
                        emailsToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                } //end EF Core transaction
            }
            emails.Clear();
        }

        public List<ProcessSummary> ProcessSmall(IList<Worker> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            //correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing emails collection in EmailAddressManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }


            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var emails = (from m in workers
                          from e in m.Contact.EmailAddresses

                          select new Email
                          {
                              EmployeeId = m.WorkerId,
                              UniversalId = m.UniversalId,
                              EmailAddress = e.EmailAddress,
                              EmailType = e.EmailType,
                              EmailComment = e.EmailComment,
                              PrimaryIndicator = e.PrimaryIndicator,
                              PublicIndicator = e.PublicIndicator,
                              ImportCreatedDate = DateTime.Now,
                              ImportIsActiveRecord = true,
                          }
                        )
                    .ToList();


            if (emails?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of emails object: {emails.Count} rows retrieved and being inserted.");
                _personImport.AddRange(emails);
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonEmailAddress", ObjectCount = emails.Count, Description = "Rows added. Source = EmailAddressManager.ProcessLarge()" });

                /*  For each new record (emails), update the related records that exist in Integrations.PersonEmailAddress where ImportIsActiveRecord = true
                    Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                */

                try
                {
                    List<Email> emailsToUpdate = (from o in _personImport.PersonEmailAddress
                                                  where o.ImportIsActiveRecord == true
                                                  join newEmailAddr in emails on
                                                       new { o.UniversalId, o.EmailType }
                                                       equals
                                                       new { newEmailAddr.UniversalId, newEmailAddr.EmailType }
                                                  select o).ToList();

                    if (emailsToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of emailsToUpdate object: {emailsToUpdate.Count} rows being replaced / deactivated.");
                        foreach (Email o in emailsToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }

                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonEmailAddress", ObjectCount = emailsToUpdate.Count, Description = "Rows updated / removed. Source = EmailAddressManager.ProcessSmall()" });
                    emailsToUpdate.Clear();
                }
                catch
                {
                    throw;
                }
            }
            emails.Clear();
            return summaryProcess;
        }
    }
}
