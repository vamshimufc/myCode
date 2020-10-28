using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;

namespace wdhrtosis
{
    public interface IPersonImportDatabaseWrapper
    {
        DateTimeOffset GetLastSucessfullRun();
        void WriteHistoryRecord(History history);
        void WriteOneProcessSummaryRecord(ProcessSummary processSummary);
        void WriteListofProcessSummaryRecords(List<ProcessSummary> processSummary);
        void InsertNames(IList<Name> names);
        void InsertEmailAddresses(IList<Email> emails);
        void InsertAddresses(IList<Address> addresses);
        void InsertContacts(IList<Contact> contact);
        void InsertVisas(IList<Visa> visas);
        void InsertEmploymentProfiles(IList<EmploymentProfile> employmentprofiles);
        void InsertEmploymentPositions(IList<EmploymentPosition> employmentpositions);

    }

    public class PersonImportDatabaseWrapper : IPersonImportDatabaseWrapper
    {

        private readonly PersonImportContext _context;

        public PersonImportDatabaseWrapper(PersonImportContext context)
        {
            _context = context;
        }

        public DateTimeOffset GetLastSucessfullRun()
        {
            int ID = 0;
            try
                {
                // get the Id for the Starting entry that corresponds (via CorrelationId) with the last successfuil Finishing entry
                ID = 
                (
                from o in _context.PersonHistory
                where o.CorrelationId ==
                    (from i in _context.PersonHistory
                     where i.Success == true && i.Message == "Finishing"
                     orderby i.Id descending
                     select i.CorrelationId
                    ).First()
                where o.Message == "Starting"
                orderby 1 ascending
                select o.Id
                ).First();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }

            //Return the LastRun for this ID
            return _context.PersonHistory.Where(x => x.Id == ID).Select(x => x.LastRun).FirstOrDefault();
        }

        public void WriteHistoryRecord(History history)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonHistory.Add(history));
        }

        public void WriteOneProcessSummaryRecord(ProcessSummary processSummary)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonImportProcessSummary.Add(processSummary));
        }

        public void WriteListofProcessSummaryRecords(List<ProcessSummary> processSummary)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonImportProcessSummary.AddRange(processSummary));
        }

        public void InsertNames(IList<Name> names)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonName.AddRange(names));
        }

        public void InsertEmailAddresses(IList<Email> emails)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonEmailAddress.AddRange(emails));
        }

        public void InsertAddresses(IList<Address> addresses)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonAddress.AddRange(addresses));
        }

        public void InsertEmploymentProfiles(IList<EmploymentProfile> employmentprofiles)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonEmploymentProfile.AddRange(employmentprofiles));
        }
        public void InsertEmploymentPositions(IList<EmploymentPosition> employmentpositions)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonEmploymentPosition.AddRange(employmentpositions));
        }
        public void InsertContacts(IList<Contact> contact)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonContact.AddRange(contact));
        }

        public void InsertVisas(IList<Visa> visa)
        {
            ExecuteInEFCoreTransaction(() => _context.PersonVisa.AddRange(visa));
        }

        private void ExecuteInEFCoreTransaction(Action action)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    action();
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

    }
}
