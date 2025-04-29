using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.DAL;
using Project.DAL.Entities;
using Project.Repository.Common;

namespace Project.Repository
{
    public class VehicleEngineTypeRepository : Repository<VehicleEngineType>, IVehicleEngineTypeRepository
    {
        public VehicleEngineTypeRepository(ProjectDbContext context, bool isReadonly=true) : base(context, isReadonly)
        {
        }
       
    }
}
