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
                .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src => src.PractitionerSpeciality.Select(ps=>ps.Speciality)))
                //Para el miembro speciality, (opt = options) mapealo desde practitionerspeciality, transformalos a speciality
                .ReverseMap();

            CreateMap<Practitioner, PractitionerUpdateDTO>()
                .ReverseMap();

            CreateMap<PractitionerCreateDTO, Practitioner>()
                .ForMember(dest => dest.PractitionerSpeciality, opt => opt.Ignore()) // Lo ignoramos para manejarlo manualmente
                    .AfterMap((src, dest) =>
                    {
                        dest.PractitionerSpeciality = src.Speciality
                                .Select(s => new PractitionerSpeciality
                                {
                                    SpecialityId = s.Id,
                                    PractitionerId = dest.Id 
                                })
                                .ToList();
                    });
            //Ignora PractitionerSpeciality y lo gestiona con un aftermap, le pasa las dos entidades a mapear por parametro, y recorre speciality con el 
            //select, creando un practitionerspeciality y asignandole el specialityid de la specialidad donde esta parado, y el practitionerid del practitioner. 

            CreateMap<Speciality, SpecialityDTO>()
                .ReverseMap();

            CreateMap<Speciality, SpecialityListPractitionerDTO>()
                  .ForMember(dest => dest.Practitioner, opt => opt.MapFrom(src => src.PractitionerSpeciality.Select(ps => ps.Practitioner)))
                  .ReverseMap();

             CreateMap<Patient, PatientDTO>()
                .ReverseMap();
            
            CreateMap<Patient, PatientUpdateDTO>()
               .ReverseMap();
            
            CreateMap<PatientCreateDTO, Patient>();

        }
    }
}
