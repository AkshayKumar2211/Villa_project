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
    public class ApplicationUserRepository:Repository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
