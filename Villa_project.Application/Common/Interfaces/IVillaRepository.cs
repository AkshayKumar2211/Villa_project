using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Villa_project.Domain.Entities;

namespace Villa_project.Application.Common.Interfaces
{
    public interface IVillaRepository:IRepository<Villa>
    {

   
        void Update(Villa entity);
        
       
    }
}
