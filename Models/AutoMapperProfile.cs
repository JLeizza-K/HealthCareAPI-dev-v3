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
                    .AfterMap<PractitionerMappingAction>();
            //Ignora PractitionerSpeciality y lo gestiona con un aftermap. 

            CreateMap<Speciality, SpecialityDTO>()
                .ReverseMap();

            CreateMap<Patient, PatientDTO>()
               .ReverseMap();

            CreateMap<Patient, PatientUpdateDTO>()
               .ReverseMap();

            CreateMap<PatientCreateDTO, Patient>();

            CreateMap<AvailabilityCreateDTO, Availability>();

        }
    }

    public class PractitionerMappingAction : IMappingAction<PractitionerCreateDTO, Practitioner>
    {
        public HealthcareContext Context { get; set; }

        public PractitionerMappingAction (HealthcareContext context)
        {
            Context = context;
        }

            public void Process(PractitionerCreateDTO source, Practitioner destination, ResolutionContext context)
        {
            destination.PractitionerSpeciality = source.Speciality
                .Select(specialityDTO => new PractitionerSpeciality
                {
                    SpecialityId = Context.Speciality
                    .FirstOrDefault(speciality => speciality.Name == specialityDTO.Name).Id,
                    PractitionerId = destination.Id
                })
                .ToList();
        }
    }

}
