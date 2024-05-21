using CarWorkshop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshop.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.BirthDate)
                .HasColumnType("date");

            modelBuilder.Entity<Employee>()
                .Property(e => e.WorkingSince)
                .HasColumnType("date");
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<DateTimeSlot> DateTimeSlots { get; set; }
        public DbSet<AdditionalTicketInfo> AdditionalTicketInfos { get; set; }
        public DbSet<Part> Parts { get; set; }

    }
}
