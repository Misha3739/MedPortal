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
            CreateMap<HClinic, ClinicSearchModel>()
                .ForMember(c => c.City, c=> c.MapFrom(hc => hc.HCity))
                .ForMember(c => c.Alias, c => c.MapFrom(hc => hc.RewriteName));
            CreateMap<HClinic, ClinicModel>()
                .ForMember(c => c.City, c => c.MapFrom(hc => hc.HCity))
                .ForMember(c => c.Alias, c => c.MapFrom(hc => hc.RewriteName));
            CreateMap<HDoctor, DoctorSearchModel>()
                .ForMember(c => c.City, c => c.MapFrom(hc => hc.City));
            CreateMap<HDoctor, DoctorModel>()
                .ForMember(c => c.City, c => c.MapFrom(hc => hc.City))
                .ForMember(c => c.PhotoUrl, c => c.MapFrom(hc => hc.Img))
                .ForMember(c => c.Specialities, c => c.Ignore())
                .ForMember(c => c.Experience, c => c.MapFrom(hc => hc.ExperienceYear));
            CreateMap<HSpeciality, ClinicSpecialitySearchModel>()
                .ForMember(c => c.Alias, c => c.MapFrom(hc => hc.BranchAlias))
                .ForMember(c => c.Name, c => c.MapFrom(hc => hc.BranchName));
            CreateMap<HSpeciality, DoctorSpecialitySearchModel>();
        }
    }
}