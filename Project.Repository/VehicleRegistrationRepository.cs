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
    public class VehicleRegistrationRepository : Repository<VehicleRegistration>, IVehicleRegistrationRepository
    {
        public VehicleRegistrationRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
