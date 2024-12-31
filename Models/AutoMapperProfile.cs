using AutoMapper;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;


namespace HealthCareApi_dev_v3.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Practitioner, PractitionerDTO>()
                .ReverseMap();

            CreateMap<Practitioner, PractitionerUpdateDTO>()
                .ReverseMap();
        }
    }
}
