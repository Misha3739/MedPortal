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
            CreateMap<HDoctor, DoctorSearchModel>()
                .ForMember(c => c.City, c => c.MapFrom(hc => hc.City));
            CreateMap<HSpeciality, SpecialitySearchModel>();
        }
    }
}