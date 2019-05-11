using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.Proxy.Data;

namespace MedPortal.Proxy.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<City, HCity>().ForMember(c => c.OriginId, 
                opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<District, HDistrict>().ForMember(c => c.OriginId, 
                    opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<Station, HStation>().ForMember(c => c.OriginId,
		            opt => opt.MapFrom(c => c.Id))
	            .ForMember(c => c.Id, opt => opt.Ignore());
			CreateMap<Doctor, HDoctor>().ForMember(c => c.OriginId, 
                opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<Clinic, HClinic>().ForMember(c => c.OriginId, 
                opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<Speciality, HSpeciality>().ForMember(c => c.OriginId, 
                opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}