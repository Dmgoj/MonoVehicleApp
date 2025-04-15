using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.DAL;
using Project.Repository.Common;

namespace Project.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDbContext _context;
        private readonly IVehicleMakeRepository VehicleMake;
        private readonly IVehicleModelRepository VehicleModel;
        private readonly IVehicleOwnerRepository VehicleOwner;
        private readonly IVehicleEngineTypeRepository VehicleEngineType;
        private readonly IVehicleRegistrationRepository VehicleRegistration;

        public UnitOfWork(IVehicleMakeRepository vehicleMakeRepository,
                    IVehicleModelRepository vehicleModelRepository,
                    IVehicleOwnerRepository vehicleOwnerRepository,
                    IVehicleEngineTypeRepository vehicleEngineTypeRepository,
                    IVehicleRegistrationRepository vehicleRegistrationRepository,
                    ProjectDbContext context)
        {
            _context = context;
            VehicleMake = vehicleMakeRepository;
            VehicleModel = vehicleModelRepository;
            VehicleOwner = vehicleOwnerRepository;
            VehicleEngineType = vehicleEngineTypeRepository;
            VehicleRegistration = vehicleRegistrationRepository;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
