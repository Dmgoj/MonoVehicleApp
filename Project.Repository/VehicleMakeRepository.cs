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
    public class VehicleMakeRepository : Repository<VehicleMake>, IVehicleMakeRepository
    {
        public VehicleMakeRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
