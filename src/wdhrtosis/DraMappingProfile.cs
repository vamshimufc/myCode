using System.Linq;
using AutoMapper;
using EA.Mulesoft.Models.ViewModels;
using EA.Mulesoft;
using EA.Mulesoft.Mappers;
using Microsoft.Extensions.DependencyInjection;
using wdhrtosis.Models;



namespace wdhrtosis
{
    public class DraMappingProfile : Profile
    {
        public DraMappingProfile()
        {
            CreateMap<WorkerViewModel, Name>()
                .ForMember(source => source.EmployeeId, dest => dest.MapFrom(x => x.Worker_id))
                .ForMember(source => source.UniversalId, dest => dest.MapFrom(x => x.Universal_id))
                .ForMember(dest => dest.NationalId,
                    source => source.MapFrom(x => x.GovernmentIDs.Count > 0 ? x.GovernmentIDs.FirstOrDefault().IDNumber : null))
                .ForMember(dest => dest.Prefix,
                    source => source.MapFrom(x => x.Name.Prefix))
                .ForMember(dest => dest.FirstName,
                    source => source.MapFrom(x => x.Name.FirstName))
                .ForMember(dest => dest.MiddleName,
                    source => source.MapFrom(x => x.Name.MiddleName))
                .ForMember(dest => dest.LastName,
                    source => source.MapFrom(x => x.Name.LastName))
                .ForMember(dest => dest.Suffix,
                    source => source.MapFrom(x => x.Name.Suffix))
                .ForMember(dest => dest.PreferredPrefix,
                    source => source.MapFrom(x => x.Name.PreferredPrefix))
                .ForMember(dest => dest.PreferredFirstName,
                    source => source.MapFrom(x => x.Name.PreferredFirstName))
                .ForMember(dest => dest.PreferredMiddleName,
                    source => source.MapFrom(x => x.Name.PreferredMiddleName))
                .ForMember(dest => dest.PreferredLastName,
                    source => source.MapFrom(x => x.Name.PreferredLastName))
                .ForMember(dest => dest.PreferredSuffix,
                    source => source.MapFrom(x => x.Name.PreferredSuffix))
                .ForMember(dest => dest.ReportingName,
                    source => source.MapFrom(x => x.Name.ReportingName))
                .ForMember(dest => dest.Pronunciation,
                    source => source.MapFrom(x => x.Name.Pronunciation))
                .ForMember(dest => dest.Gender,
                    source => source.MapFrom(x => x.Biographic.Genders.FirstOrDefault().GenderType))
                .ForMember(dest => dest.Gender,
                    source => source.MapFrom(x => x.Biographic.LifeEvents.BirthDate))
                .ForAllOtherMembers(t => t.Ignore());


            CreateMap<WorkerViewModel, Address>()
                .ForMember(source => source.EmployeeId, dest => dest.MapFrom(x => x.Worker_id))
                .ForMember(source => source.UniversalId, dest => dest.MapFrom(x => x.Universal_id))
                .ForMember(dest => dest.AddressType,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressType))
                .ForMember(dest => dest.AddressId,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressId))
                .ForMember(dest => dest.AddressLine1,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressLine1))
                .ForMember(dest => dest.AddressLine2,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressLine2))
                .ForMember(dest => dest.AddressLine2,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressLine3))
                .ForMember(dest => dest.AddressLine2,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressLine4))
                .ForMember(dest => dest.AddressLine2,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().AddressLine5))
                .ForMember(dest => dest.City,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().City))
                .ForMember(dest => dest.StateProvince,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().StateProvince))
                .ForMember(dest => dest.StateProvince,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().StateProvinceCode))
                .ForMember(dest => dest.PostalCode,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault().PostalCode))
                .ForMember(dest => dest.CountryCode,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault(y => y.PrimaryIndicator).Country))
                .ForMember(dest => dest.CountryCode,
                    source => source.MapFrom(x =>
                        x.Contact.Addresses.FirstOrDefault(y => y.PrimaryIndicator).CountryCode))
                .ForAllOtherMembers(t => t.Ignore());

            CreateMap<WorkerViewModel, Contact>()
                .ForMember(source => source.EmployeeId, dest => dest.MapFrom(x => x.Worker_id))
                .ForMember(source => source.UniversalId, dest => dest.MapFrom(x => x.Universal_id))
                .ForMember(dest => dest.CountryAccessCode,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().CountryAccessCode))
                .ForMember(dest => dest.AreaCityCode,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().AreaCityCode))
                .ForMember(dest => dest.PhoneNumber,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().PhoneNumber))
                .ForMember(dest => dest.DeviceType,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().DeviceType))
                .ForMember(dest => dest.PhoneType,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().PhoneType))
                .ForMember(dest => dest.PhoneNumberFormatted,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().PhoneNumberFormatted))
                .ForMember(dest => dest.PrimaryIndicator,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().PrimaryIndicator))
                .ForMember(dest => dest.PublicIndicator,
                    source => source.MapFrom(x =>
                        x.Contact.PhoneNumbers.FirstOrDefault().PublicIndicator))
                .ForAllOtherMembers(t => t.Ignore());

            CreateMap<WorkerViewModel, Email>()
               .ForMember(source => source.EmployeeId, dest => dest.MapFrom(x => x.Worker_id))
               .ForMember(source => source.UniversalId, dest => dest.MapFrom(x => x.Universal_id))
               .ForMember(dest => dest.EmailAddress,
                   source => source.MapFrom(x => x.Contact.EmailAddresses.FirstOrDefault().EmailAddress))
               .ForMember(dest => dest.EmailAddress,
                   source => source.MapFrom(x => x.Contact.EmailAddresses.FirstOrDefault().EmailType))
               .ForMember(dest => dest.EmailAddress,
                   source => source.MapFrom(x => x.Contact.EmailAddresses.FirstOrDefault().EmailComment))
               .ForMember(dest => dest.PrimaryIndicator,
                   source => source.MapFrom(x =>
                       x.Contact.EmailAddresses.FirstOrDefault().PrimaryIndicator))
               .ForMember(dest => dest.PublicIndicator,
                   source => source.MapFrom(x =>
                       x.Contact.EmailAddresses.FirstOrDefault().PublicIndicator))
               .ForAllOtherMembers(t => t.Ignore());

            CreateMap<WorkerViewModel, Visa>()
                .ForMember(source => source.EmployeeId, dest => dest.MapFrom(x => x.Worker_id))
                .ForMember(source => source.UniversalId, dest => dest.MapFrom(x => x.Universal_id))
                .ForMember(dest => dest.VisaNumber,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaNumber))
                .ForMember(dest => dest.VisaType,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaType))
                .ForMember(dest => dest.VisaPermitStatus,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaPermitStatus))
                .ForMember(dest => dest.VisaPermitDuration,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaPermitDuration))
                .ForMember(dest => dest.VisaPermitDurationType,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaPermitDurationType))
                .ForMember(dest => dest.VisaIssuingAuthority,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaIssuingAuthority))
                .ForMember(dest => dest.VisaIssueDate,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaIssueDate))
                .ForMember(dest => dest.VisaExpiryDate,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaExpiryDate))
                .ForMember(dest => dest.VisaVerificationDate,
                    source => source.MapFrom(x =>
                        x.Visas.FirstOrDefault().VisaVerificationDate))
                .ForAllOtherMembers(t => t.Ignore());
        }
    }
}