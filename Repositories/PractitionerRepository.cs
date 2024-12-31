using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace HealthCareApi_dev_v3.Repositories
{
    public class PractitionerRepository:IPractitionerRepository
    {
        public HealthcareContext Context { get; set; }
        public IMapper Mapper { get; set; }

        public PractitionerRepository(HealthcareContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;

        }             
       
        async Task<IEnumerable<PractitionerDTO>> IPractitionerRepository.GetAll()
        {
            //Traigo los practitioners de la db
            var practitioners = await Context.Practitioner.ToListAsync();

            //Select hace que los elementos de una secuencia puedan tener otra forma. Le pido que retorne practitioners, pero mapeando cada practitioner a PractitionerDTO.
            return practitioners.Select(practitioner => Mapper.Map<PractitionerDTO>(practitioner));
        }
        
        //No tiene sentido que el Create Practitioner devuelva un dto, si lo estoy creando necesito todos los datos, al menos por primera vez.
        public async Task<Practitioner> CreatePractitioner(Practitioner practitioner)
        {
            await Context.Practitioner.AddAsync(practitioner);

            Context.SaveChanges();

            return practitioner;
        }

        public async Task<PractitionerDTO> GetById(Guid id)
        {
            var practitioner = await Context.Practitioner.FindAsync(id);
            
            return Mapper.Map<PractitionerDTO>(practitioner);
        }

        public async Task<Practitioner> UpdatePractitioner(PractitionerUpdateDTO practitioner)
        {
            var existingPractitioner = Mapper.Map<Practitioner>(GetById(practitioner.Id));

            Mapper.Map(practitioner, existingPractitioner);

            Context.Practitioner.Update(existingPractitioner);

            await Context.SaveChangesAsync();

            return Mapper.Map<Practitioner>(existingPractitioner);
        }
    }
}
