using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;

namespace Project.DAL
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }
        // Define DbSets for your entities here
        public DbSet<VehicleEngineType> VehicleEngineTypes { get; set; }
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<VehicleOwner> VehicleOwners { get; set; }
        public DbSet<VehicleRegistration> VehicleRegistrations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entity mappings here
        }
    }
    {
    }
}
