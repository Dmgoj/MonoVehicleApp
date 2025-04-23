using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Common
{
    public interface IVehicleMakeService
    {
        Task<PaginatedResult<VehicleMakeDto>> GetAllAsync(QueryParameters parameters);
        Task<VehicleMakeDto> GetByIdAsync(int id);
        Task<VehicleMakeDto> CreateAsync(VehicleMakeForCreateDto dto);
        Task UpdateAsync(int id, VehicleMakeForUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
