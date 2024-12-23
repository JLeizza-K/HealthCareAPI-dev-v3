using Microsoft.EntityFrameworkCore;
using HealthCareApi_dev_v3.Models.Entities;


namespace HealthCareApi_dev_v3.Models
{
    public class HealthcareContext: DbContext
    {
        public HealthcareContext(DbContextOptions<HealthcareContext> options) : base(options) { }
        public virtual DbSet<Practitioner> Practitioner {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Practitioner>().ToTable("Practitioner");
        }
    }
}
