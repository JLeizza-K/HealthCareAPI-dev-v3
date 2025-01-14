using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Concurrent;


namespace HealthCareApi_dev_v3.Repositories
{
    public class PractitionerRepository:IPractitionerRepository
    {
        public HealthcareContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public ISpecialityRepository SpecialityRepository { get; set; }


        public PractitionerRepository(HealthcareContext context, IMapper mapper, ISpecialityRepository specialityRepository)
        {
            Context = context;
            Mapper = mapper;
            SpecialityRepository = specialityRepository;

        }             
       
        async Task<IEnumerable<PractitionerDTO>> IPractitionerRepository.GetAll()
        {
            var practitioners = await Context.Practitioner.ToListAsync();

            //Select hace que los elementos de una secuencia puedan tener otra forma. Le pido que retorne practitioners, pero mapeando cada practitioner a PractitionerDTO.
            return practitioners.Select(practitioner => Mapper.Map<PractitionerDTO>(practitioner));
        }

        public async Task<PractitionerDTO> GetById(Guid id)
        {
            var practitioner = await Context.Practitioner
                .Include(p => p.PractitionerSpeciality)
                    .ThenInclude(ps => ps.Speciality)
                .FirstOrDefaultAsync(p => (p.Id) == id);

            return Mapper.Map<PractitionerDTO>(practitioner);
        }

        public async Task<Practitioner> GetByEmail(string email)
        {
            var practitioner = await Context.Practitioner
                .Include(p => p.PractitionerSpeciality)
                    .ThenInclude(ps => ps.Speciality)
                .FirstOrDefaultAsync(p => (p.Email) == email);

            return practitioner;
        }

        public async Task<PractitionerCreateDTO> CreatePractitioner(PractitionerCreateDTO practitioner)
        {
            var newPractitioner = Mapper.Map<Practitioner>(practitioner);

            newPractitioner.Id = Guid.NewGuid();
            
            await Context.Practitioner.AddAsync(newPractitioner);

            var practitionerSpeciality = new List<PractitionerSpeciality>();

            foreach (var speciality in practitioner.Speciality)
            {
                practitionerSpeciality.Add(new PractitionerSpeciality { PractitionerId = newPractitioner.Id, SpecialityId = speciality.Id });

            }

            Context.SaveChanges();

            return practitioner;
        }

        public async Task<PractitionerUpdateDTO> UpdatePractitioner(PractitionerUpdateDTO practitioner)
        {
            var existingPractitioner = await GetByEmail(practitioner.Email);

            Mapper.Map(practitioner, existingPractitioner);
       
            Context.Practitioner.Update(existingPractitioner);

            await Context.SaveChangesAsync();

            return Mapper.Map<PractitionerUpdateDTO>(existingPractitioner);
        }
    }
}
