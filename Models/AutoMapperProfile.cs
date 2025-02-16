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
                .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src => src.PractitionerSpeciality.Select(ps => ps.Speciality)))
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

            CreateMap<Speciality, SpecialityDTO>()
                .ReverseMap();

            CreateMap<Patient, PatientDTO>()
               .ReverseMap();

            CreateMap<Patient, PatientUpdateDTO>()
               .ReverseMap();

            CreateMap<PatientCreateDTO, Patient>();

            CreateMap<AvailabilityCreateDTO, Availability>();
                //.ForMember(dest => dest.PractitionerId, opt => opt.Ignore())
                //.AfterMap((src, dest) =>
                //{
                //    dest.Practitioner.Id = src.PractitionerId;
                //    dest.Speciality.Id = src.SpecialityId;
                //});
             CreateMap<AppointmentDTO, Appointment>();

        }
    }



}
