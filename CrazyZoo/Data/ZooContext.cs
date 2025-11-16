using CrazyZoo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Data
{
    public class ZooContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }

        public ZooContext(DbContextOptions<ZooContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(b =>
            {
                b.HasKey(a => a.Id);

                b.Property(a => a.Name).IsRequired();
                b.Property(a => a.Age).IsRequired();

                b.HasDiscriminator<string>("AnimalType")
                    .HasValue<Cat>("Cat")
                    .HasValue<Dog>("Dog")
                    .HasValue<Bird>("Bird")
                    .HasValue<Raccoon>("Raccoon")
                    .HasValue<Monkey>("Monkey");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

