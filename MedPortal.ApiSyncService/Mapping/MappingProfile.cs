using System.Linq;
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
            CreateMap<Street, HStreet>().ForMember(c => c.OriginId,
		            opt => opt.MapFrom(c => c.Id))
	            .ForMember(c => c.Name,
		            opt => opt.MapFrom(c => c.Title))
				.ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Alias,c => c.MapFrom(s => s.RewriteName));
            CreateMap<Doctor, HDoctor>().ForMember(c => c.OriginId,
                    opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Specialities, opt => opt.Ignore())
                .ForMember(c => c.Clinics, opt => opt.Ignore())
                .ForMember(c => c.Sex, opt => opt.MapFrom(c => c.Sex == 0 ? Sex.Male : Sex.Female));

            CreateMap<Clinic, HClinic>().ForMember(c => c.OriginId,
                opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Stations, opt => opt.Ignore())
                .ForMember(c => c.Specialities, opt => opt.Ignore())
                .ForMember(c => c.Latitude, opt => opt.MapFrom(c => c.Latitude ?? 0))
                .ForMember(c => c.Longitude, opt => opt.MapFrom(c => c.Longitude ?? 0));

            CreateMap<Speciality, HSpeciality>().ForMember(c => c.OriginId, 
                opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}