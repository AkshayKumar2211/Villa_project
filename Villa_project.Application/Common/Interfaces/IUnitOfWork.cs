using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villa_project.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository villa { get; }
        IVillaNumberRepository villaNumber { get; }
        IBookingRepository Booking { get; }
        IApplicationUserRepository User { get; }
        IAmenityRepository amenity { get; }

        void Save();
    }
}
