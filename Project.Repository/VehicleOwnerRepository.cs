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
    public class VehicleOwnerRepository : Repository<VehicleOwner>, IVehicleOwnerRepository
    {
        public VehicleOwnerRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
