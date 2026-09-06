using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TourAgencyApp.Models
{
    public class TourAgencyDbContext : DbContext
    {
        public TourAgencyDbContext() 
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TourAgencyDB"].ConnectionString;
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tourist> Clients { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>().HasData(
            //    new User { ID = 1, Username = "Tom", Password = "", Role = RoleEnum.Client },
            //    new User { ID = 2, Username = "Alice", Password = "", Role = RoleEnum.Employee }


            // Явная настройка связи многие-ко-многим
            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Tourists)
                .WithMany(t => t.ClientTours)
                .UsingEntity<Dictionary<string, object>>( "TourTourists",
                j => j.HasOne<Tourist>()
                      .WithMany()
                      .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Tour>()
                      .WithMany()
                      .OnDelete(DeleteBehavior.Restrict) 
                );
        }
    }
}
