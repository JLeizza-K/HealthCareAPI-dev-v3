using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;


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
         async Task<IEnumerable<Practitioner>> IPractitionerRepository.GetAll()
        {
            var practitioners = await Context.Practitioner.ToListAsync();

            return practitioners;
        }

        async Task<Practitioner> IPractitionerRepository.CreatePractitioner(Practitioner practitioner)
        {
            var newPractitioner = Mapper.Map<Practitioner>(practitioner);
            
            await Context.Practitioner.AddAsync(newPractitioner);

            Context.SaveChanges();

            return newPractitioner;
        }

        //Cree el GetById para que al hacer el post, me pueda traer el practitioner a modificar necesitando solo el id.
        public async Task<Practitioner> GetById(Guid id)
        {
            var practitioner = await Context.Practitioner.FindAsync(id);

            //Implementé response, copiando a los chicos, porque queria agregarle validación al GetById y que el Put devuelva algo. Lo voy a implementar en el resto de métodos. 
           /* if (practitioner == null)
             {
                 return response = new Response{ Code = 400, Message="Practitioner not found"};
             } */
            return practitioner;            
        }

        public async Task<Response> UpdatePractitioner(Guid id, Practitioner practitioner)
        {
          
           var existingPractitioner = await GetById(id);

            if (existingPractitioner != null)
            {
                Mapper.Map(practitioner, existingPractitioner);
                
                Context.Practitioner.Update(existingPractitioner);
                
                await Context.SaveChangesAsync();

                return new Response { Code = 200, Message = "Practitioner updated" };
            }
            else
            {
                return new Response { Code = 400, Message = "Practitioner doesn't exists" };
            }

        }
    }
}
