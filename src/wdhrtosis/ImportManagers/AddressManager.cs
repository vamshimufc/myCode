using EA.Mulesoft.Models.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using wdhrtosis.Data;
using wdhrtosis.Models;
using Address = wdhrtosis.Models.Address;

namespace wdhrtosis.ImportManagers
{
    public interface IAddressManager
    {
        void ProcessLarge(IList<Worker> workers);
        List<ProcessSummary> ProcessSmall(IList<Worker> workers);

    }
    public class AddressManager : IAddressManager
    {
        private readonly PersonImportContext _personImport;
        private readonly IPersonImportDatabaseWrapper _personImportDatabaseWrapper;
        private readonly ILogger _logger;
        private double memoryUsed;
        //private string correlationId;

        public AddressManager(
                  PersonImportContext personImport
                , IPersonImportDatabaseWrapper personImportDatabaseManager
                , ILogger<AddressManager> logger
            )
        {
            _personImport = personImport;
            _logger = logger;
            _personImportDatabaseWrapper = personImportDatabaseManager;
        }


        public void ProcessLarge(IList<Worker> workers)
        {
            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing addresses collection in AddressManager.ProcessLarge()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var addresses = (from m in workers
                             from a in m.Contact.Addresses

                             select new wdhrtosis.Models.Address
                             {
                                 EmployeeId = m.WorkerId,
                                 UniversalId = m.UniversalId,
                                 ImportCreatedDate = DateTime.Now,
                                 ImportIsActiveRecord = false,
                                 AddressType = a.AddressType,
                                 AddressId = a.AddressId,
                                 AddressEffectiveDate = a.AddressEffectiveDate,
                                 DefaultedBusinessAddress = a.DefaultedBusinessAddress,
                                 PrimaryIndicator = a.PrimaryIndicator,
                                 PublicIndicator = a.PublicIndicator,
                                 AddressLine1 = a.AddressLine1,
                                 AddressLine2 = a.AddressLine2,
                                 AddressLine3 = a.AddressLine3,
                                 AddressLine4 = a.AddressLine4,
                                 AddressLine5 = a.AddressLine5,
                                 City = a.City,
                                 StateProvince = a.StateProvince,
                                 StateProvinceCode = a.StateProvinceCode,
                                 PostalCode = a.PostalCode,
                                 Country = a.Country,
                                 CountryCode = a.CountryCode,
                             }
                         )
                    .ToList();

            if (addresses?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of addresses object: {addresses.Count} rows retrieved and being inserted.");
                _personImportDatabaseWrapper.InsertAddresses(addresses);

                _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonAddress", ObjectCount = addresses.Count, Description = "Rows added. Source = AddressManager.ProcessLarge()" });

                //Begin EF Core Transaction
                using (var transaction = _personImport.Database.BeginTransaction())
                {
                    /*  For each new record (addresses), update the related records that exist in Integrations.PersonAddress where ImportIsActiveRecord = true
                         Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                     */

                    try
                    {
                        List<Address> addressesToUpdate = (from o in _personImport.PersonAddress
                                                           where o.ImportIsActiveRecord == true
                                                           join newAddr in addresses on
                                                                new { o.UniversalId, o.AddressType }
                                                                equals
                                                                new { newAddr.UniversalId, newAddr.AddressType }
                                                           select o).ToList();

                        if (addressesToUpdate?.Count > 0)
                        {
                            _logger.LogInformation($"Row count of addressesToUpdate object: {addressesToUpdate.Count} rows being replaced / deactivated.");
                            foreach (Address o in addressesToUpdate)
                            {
                                o.ImportIsActiveRecord = false;
                                o.ImportLastUpdatedDate = DateTime.Now;
                            }
                        }
                        //Now, as part of this transaction, update the newly inserted records as Active
                        List<Address> addressesToActivate = (from x in _personImport.PersonAddress
                                                             where x.ImportIsActiveRecord == false
                                                             select x).ToList();
                        foreach (Address x in addressesToActivate)
                        {
                            x.ImportIsActiveRecord = true;
                        }
                        _personImport.SaveChanges();
                        transaction.Commit();
                        _personImportDatabaseWrapper.WriteOneProcessSummaryRecord(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonAddress", ObjectCount = addressesToUpdate.Count, Description = "Rows updated / removed. Source = AddressManager.ProcessLarge()" });
                        addressesToUpdate.Clear();
                        addressesToActivate.Clear();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Dispose();

                }
            }
            addresses.Clear();
        }

        public List<ProcessSummary> ProcessSmall(IList<Worker> workers) //No db transactions here. The db transaction is in the calling method in Manager.cs
        {
            List<ProcessSummary> summaryProcess = new List<ProcessSummary>();

            if (Utility.LogMemoryUsage)
            {
                memoryUsed = Utility.GetMemoryUsage();
                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "Utility.GetMemoryUsage() before processing addresses collection in AddressManager.ProcessSmall()", ObjectCount = 0, Description = memoryUsed + " MB used" });
            }

            // get this sub-object from workers. Note that data is being added as Active since we are wrapping everything in a db transaction.
            var addresses = (from m in workers
                             from a in m.Contact.Addresses

                             select new wdhrtosis.Models.Address
                             {
                                 EmployeeId = m.WorkerId,
                                 UniversalId = m.UniversalId,
                                 ImportCreatedDate = DateTime.Now,
                                 ImportIsActiveRecord = true,
                                 AddressType = a.AddressType,
                                 AddressId = a.AddressId,
                                 AddressEffectiveDate = a.AddressEffectiveDate,
                                 DefaultedBusinessAddress = a.DefaultedBusinessAddress,
                                 PrimaryIndicator = a.PrimaryIndicator,
                                 PublicIndicator = a.PublicIndicator,
                                 AddressLine1 = a.AddressLine1,
                                 AddressLine2 = a.AddressLine2,
                                 AddressLine3 = a.AddressLine3,
                                 AddressLine4 = a.AddressLine4,
                                 AddressLine5 = a.AddressLine5,
                                 City = a.City,
                                 StateProvince = a.StateProvince,
                                 StateProvinceCode = a.StateProvinceCode,
                                 PostalCode = a.PostalCode,
                                 Country = a.Country,
                                 CountryCode = a.CountryCode,
                             }
                         )
                    .ToList();
            if (addresses?.Count > 0)
            {
                // Add new rows
                _logger.LogInformation($"Row count of addresses object: {addresses.Count} rows retrieved and being inserted.");
                _personImport.AddRange(addresses);

                summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonAddress", ObjectCount = addresses.Count, Description = "Rows added. Source = AddressManager.ProcessSmall()" });

                /*  For each new record (addresses), update the related records that exist in Integrations.PersonAddress where ImportIsActiveRecord = true
                     Set ImportIsActiveRecord to false and LastUpdatedDate to current date
                 */

                try
                {
                    List<Address> addressesToUpdate = (from o in _personImport.PersonAddress
                                                       where o.ImportIsActiveRecord == true
                                                       join newAddr in addresses on
                                                            new { o.UniversalId, o.AddressType }
                                                            equals
                                                            new { newAddr.UniversalId, newAddr.AddressType }
                                                       select o).ToList();

                    if (addressesToUpdate?.Count > 0)
                    {
                        _logger.LogInformation($"Row count of addressesToUpdate object: {addressesToUpdate.Count} rows being replaced / deactivated.");
                        foreach (Address o in addressesToUpdate)
                        {
                            o.ImportIsActiveRecord = false;
                            o.ImportLastUpdatedDate = DateTime.Now;
                        }
                    }
                    _personImport.SaveChanges();
                    summaryProcess.Add(new ProcessSummary { LastRun = DateTime.Now, ObjectProcessed = "PersonAddress", ObjectCount = addressesToUpdate.Count, Description = "Rows updated / removed. Source = AddressManager.ProcessSmall()" });
                    addressesToUpdate.Clear();
                }
                catch
                {
                    throw;
                }
            }
            addresses.Clear();
            return summaryProcess;
        }

    }
}
