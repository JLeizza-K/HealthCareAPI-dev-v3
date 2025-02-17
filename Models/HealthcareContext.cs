using Microsoft.EntityFrameworkCore;
using HealthCareApi_dev_v3.Models.Entities;


namespace HealthCareApi_dev_v3.Models
{
    public class HealthcareContext: DbContext
    {
        public HealthcareContext(DbContextOptions<HealthcareContext> options) : base(options) { }
        public virtual DbSet<Practitioner> Practitioner { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Speciality> Speciality { get; set; }
        public virtual DbSet<Office> Office { get; set; }
        public virtual DbSet<Availability> Availability { get; set; }
        public virtual DbSet<PractitionerSpeciality> PractitionerSpeciality { get; set; }
        public virtual DbSet<OfficeSpeciality> OfficeSpeciality { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
       // public virtual DbSet<Constants> StatusTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PractitionerSpeciality>()
                .HasKey(ps => new { ps.SpecialityId, ps.PractitionerId });
            //Genera la compound key para practitioner speciality usando specialityid y practitionerid. 
            
            modelBuilder.Entity<PractitionerSpeciality>()
                .HasOne(a => a.Practitioner)
                .WithMany(x => x.PractitionerSpeciality)
                .HasForeignKey(e => e.PractitionerId);

            modelBuilder.Entity<PractitionerSpeciality>()
                .HasOne(a => a.Speciality)
                .WithMany(x => x.PractitionerSpeciality)
                .HasForeignKey(e => e.SpecialityId);

            modelBuilder.Entity<OfficeSpeciality>()
              .HasOne(os => os.Office)
              .WithMany(o => o.OfficeSpeciality)
              .HasForeignKey(os => os.OfficeId);

            modelBuilder.Entity<OfficeSpeciality>()
              .HasOne(os => os.Speciality)
              .WithMany(s => s.OfficeSpeciality)
              .HasForeignKey(os => os.SpecialityId);

            modelBuilder.Entity<Availability>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Id).IsRequired();
            });

            modelBuilder.Entity<PractitionerSpeciality>().ToTable("PractitionerSpeciality");

            modelBuilder.Entity<OfficeSpeciality>().ToTable("OfficeSpeciality");
            
            modelBuilder.Entity<Practitioner>().ToTable("Practitioner");

            modelBuilder.Entity<Speciality>().ToTable("Speciality");

            modelBuilder.Entity<Patient>().ToTable("Patient");

            modelBuilder.Entity<Office>().ToTable("Office");

            modelBuilder.Entity<Availability>().ToTable("Availability");

            modelBuilder.Entity<TimeSlot>().ToTable("TimeSlot");
        }
    }
}
