using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Domain.Entities;
using Villa_project.Infrastructure.Data;

namespace Villa_project.Infrastructure.Repository
{
    public class AmenityRepository:Repository<Amenity>,IAmenityRepository
    {
        private readonly ApplicationDbContext _db;
        public AmenityRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Amenity amenity)
        {
            _db.Amenities.Update(amenity);
        }
    }
}
