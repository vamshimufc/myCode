using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;
using Contact = wdhrtosis.Models.Contact;

namespace wdhrtosis.ImportManagers
{
    public interface IContactManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);
    }
    public class ContactManager : IContactManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;

        //private string correlationId;

        public ContactManager(
                  ILogger<ContactManager> logger
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
            ////correlationId = Utility.CorrelationId;
            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing Contacts in ContactManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            var contacts = (from m in workers
                            from c in m.Contact.PhoneNumbers

                            select new Contact
                            {
                                EmployeeId = m.WorkerId,
                                UniversalId = m.UniversalId,
                                CountryAccessCode = c.CountryAccessCode,
                                AreaCityCode = c.AreaCityCode,
                                PhoneNumber = c.Number,
                                Extension = c.Extension,
                                DeviceType = c.DeviceType,
                                PhoneType = c.PhoneType,
                                PhoneNumberFormatted = c.PhoneNumberFormatted,
                                PrimaryIndicator = c.PrimaryIndicator,
                                PublicIndicator = c.PublicIndicator,
                                ImportCreatedDate = DateTime.Now,
                                ImportIsActiveRecord = false,
                            }
                       )
                    .ToList();

            if (contacts?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of Contacts object: {contacts.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertContacts(contacts);
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonContact", ObjectCount = contacts.Count, Description = "Rows added. Source = ContactManager.ProcessLarge()" });


                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    /*  For each new record (addresses), update the related records that exist in ssisImport.PersonContact where ImportIsActiveRecord = true
                         Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                     */

                    try
                    {
                        List<Contact> contactsToUpdate = (from c in _personImport.PersonContact
                                                           where c.ImportIsActiveRecord == true
                                                           join newContacts in contacts on
                                                                new { c.UniversalId }
                                                                equals
                                                                new { newContacts.UniversalId}
                                                           select c).ToList();
                        if (contactsToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of contactsToUpdate object: {contactsToUpdate.Count} rows being replaced / deactivated.");
                            foreach (Contact o in contactsToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }
                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<Contact> contactsToActivate = (from x in _personImport.PersonContact
                                                             where x.ImportIsActiveRecord == false
                                                             select x).ToList();
                        foreach (Contact x in contactsToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }

                        //ProcessContactsToSISIntegration(contacts);
                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonContact", ObjectCount = contactsToUpdate.Count, Description = "Rows updated / removed. Source = ContactManager.ProcessLarge()" });
                        contactsToUpdate.Clear();
                        contactsToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                } //end EF Core transaction
            }
            contacts.Clear();
        }

        public List<ProcessSummary> ProcessSmall(IList<Worker> workers)
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();
            ////correlationId = Utility.CorrelationId;

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing Contacts in ContactManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            var contacts = (from m in workers
                            from c in m.Contact.PhoneNumbers

                            select new Contact
                            {
                                EmployeeId = m.WorkerId,
                                UniversalId = m.UniversalId,
                                CountryAccessCode = c.CountryAccessCode,
                                AreaCityCode = c.AreaCityCode,
                                PhoneNumber = c.Number,
                                Extension = c.Extension,
                                DeviceType = c.DeviceType,
                                PhoneType = c.PhoneType,
                                PhoneNumberFormatted = c.PhoneNumberFormatted,
                                PrimaryIndicator = c.PrimaryIndicator,
                                PublicIndicator = c.PublicIndicator,
                                ImportCreatedDate = DateTime.Now,
                                ImportIsActiveRecord = true,
                            }
                        )
                     .ToList();

            if (contacts?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of Contacts object: {contacts.Count} rows retrieved and being inserted.");
                _personImport.AddRange(contacts);
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonContact", ObjectCount = contacts.Count, Description = "Rows added. Source = ContactManager.ProcessSmall()" });


                /*  For each new record (addresses), update the related records that exist in ssisImport.PersonContact where ImportIsActiveRecord = true
                     Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                 */

                try
                {
                    List<Contact> contactsToUpdate = (from c in _personImport.PersonContact
                                                       where c.ImportIsActiveRecord == true
                                                       join newContacts in contacts on
                                                            new { c.UniversalId }
                                                            equals
                                                            new { newContacts.UniversalId }
                                                       select c).ToList();
                    if (contactsToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of contactsToUpdate object: {contactsToUpdate.Count} rows being replaced / deactivated.");
                        foreach (Contact o in contactsToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }
                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonContact", ObjectCount = contactsToUpdate.Count, Description = "Rows updated / removed. Source = ContactManager.ProcessSmall()" });
                    contactsToUpdate.Clear();
                }
                catch
                {
                    throw;
                }

            }
            contacts.Clear();
            return summaryProcess;
        }

    }
}
