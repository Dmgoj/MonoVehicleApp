using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Common
{
    public interface IVehicleEngineTypeService
    {
        Task<IEnumerable<VehicleEngineTypeDto>> GetAllAsync();
        Task<VehicleEngineTypeDto> GetByIdAsync(int id);
    }
}
