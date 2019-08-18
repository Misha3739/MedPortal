using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.WebApiClient.Models;

namespace MedPortal.Proxy.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HCity, CitySearchModel>();
            CreateMap<HStreet, GeoSearchModel>()
                .ForMember(s => s.Type, s => s.MapFrom(t => GeoCategoryEnum.Street));
            CreateMap<HStation, GeoSearchModel>()
                .ForMember(s => s.Type, s => s.MapFrom(t => GeoCategoryEnum.MetroStation));
            CreateMap<HDistrict, GeoSearchModel>()
                .ForMember(s => s.Type, s => s.MapFrom(t => GeoCategoryEnum.District));

            CreateMap<HClinic, ClinicSearchModel>()
                .ForMember(c => c.City, c=> c.MapFrom(hc => hc.HCity))
                .ForMember(c => c.Alias, c => c.MapFrom(hc => hc.RewriteName));
            CreateMap<HClinic, ClinicModel>()
                .IncludeBase<HClinic, ClinicSearchModel>()
                .ForMember(c => c.Address, c => c.MapFrom(hc => $"{hc.HStreet.Name}, {hc.House}"));
            CreateMap<HClinic, ClinicDetailsModel>()
                .IncludeBase<HClinic, ClinicModel>();

            CreateMap<HDoctor, DoctorSearchModel>()
                .ForMember(c => c.City, c => c.MapFrom(hc => hc.City));
            CreateMap<HDoctor, DoctorModel>()
                .IncludeBase<HDoctor, DoctorSearchModel>()
                .ForMember(c => c.PhotoUrl, c => c.MapFrom(hc => hc.Img))
                .ForMember(c => c.Specialities, c => c.Ignore())
                .ForMember(c => c.Clinics, c => c.Ignore())
                .ForMember(c => c.Experience, c => c.MapFrom(hc => hc.ExperienceYear));
            CreateMap<HDoctor, DoctorDetailsModel>()
             .IncludeBase<HDoctor, DoctorModel>();

            CreateMap<HSpeciality, ClinicSpecialitySearchModel>()
                .ForMember(c => c.Alias, c => c.MapFrom(hc => hc.BranchAlias))
                .ForMember(c => c.Name, c => c.MapFrom(hc => hc.BranchName));
            CreateMap<HSpeciality, DoctorSpecialitySearchModel>()
                .ForMember(c => c.Alias, c => c.MapFrom(hc => hc.BranchAlias))
                .ForMember(c => c.Name, c => c.MapFrom(hc => hc.BranchName));
        }
    }
}