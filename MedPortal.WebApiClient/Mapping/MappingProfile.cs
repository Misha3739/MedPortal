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
        }
    }
}