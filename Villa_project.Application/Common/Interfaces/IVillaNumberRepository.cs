using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_project.Domain.Entities;

namespace Villa_project.Application.Common.Interfaces
{
    public interface IVillaNumberRepository:IRepository<VillaNumber>
    {
        void Update(VillaNumber entity);
    }
}
