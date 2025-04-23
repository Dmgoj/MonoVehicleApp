using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Service.Common.Parameters;

namespace Project.Service.Common
{
    public interface IVehicleRegistrationService
    {
        Task<PaginatedResult<VehicleRegistrationDto>> GetAllAsync(QueryParameters parameters);
        Task<VehicleRegistrationDto> GetByIdAsync(int id);
        Task<VehicleRegistrationDto> CreateAsync(VehicleRegistrationForCreateDto dto);
        Task UpdateAsync(int id, VehicleRegistrationForUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
