using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_project.Domain.Entities;

namespace Villa_project.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }

        public DbSet<VillaNumber> VillaNumbers { get; set; }

        public DbSet<Amenity> Amenities { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name="Villa1",
                    Description="Villa one discription",
                    Price=1000,
                    Sqft=1000,
                    Occupancy=3,
                    ImageUrl="https://plus.unsplash.com/premium_photo-1661915661139-5b6a4e4a6fcc?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8dmlsbGF8ZW58MHx8MHx8fDA%3D"
                },
                new Villa
                {
                    Id = 2,
                    Name="Villa 2",
                    Description="Villa two discription",
                    Price=4000,
                    Sqft=3000,
                    Occupancy=4,
                    ImageUrl="https://plus.unsplash.com/premium_photo-1661915661139-5b6a4e4a6fcc?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8dmlsbGF8ZW58MHx8MHx8fDA%3D"
                },
                new Villa
                {
                    Id = 3,
                    Name="Villa3",
                    Description="Villa three discription",
                    Price=2000,
                    Sqft=5000,
                    Occupancy=6,
                    ImageUrl="https://plus.unsplash.com/premium_photo-1661915661139-5b6a4e4a6fcc?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8dmlsbGF8ZW58MHx8MHx8fDA%3D"
                }
               
                );

            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number=101,
                    VillaId=7
                },
                new VillaNumber
                {
                    Villa_Number=102,
                    VillaId=7
                },
                new VillaNumber
                {
                    Villa_Number=103,
                    VillaId=8
                },
                new VillaNumber
                {
                    Villa_Number=104,
                    VillaId=7
                },
                new VillaNumber
                {
                    Villa_Number=105,
                    VillaId=8
                }
                );
        }
    }
}
