using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Model;

namespace WebApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Walker> Walker { get; set; }
        public DbSet<WalkerSchedule> WalkerSchedule { get; set; }
        public DbSet<Walk> Walks{ get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }

        public DbSet<Petowner> Owners { get; set; }
        public DbSet<AppConfiguration> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasAlternateKey(e => e.Username);
                
            });

            modelBuilder.Entity<AppConfiguration>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasIndex(e => new { e.Property }).IsUnique();
            });

            modelBuilder.Entity<WalkerSchedule>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasIndex(e => new { e.WalkerId, e.Date }).IsUnique();
            });


        }
    }
}
