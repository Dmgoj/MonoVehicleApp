using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Service.Common.Parameters;

namespace Project.Service.Common
{
    public interface IVehicleOwnerService
    {
        Task<PaginatedResult<VehicleOwnerDto>> GetAllAsync(QueryParameters parameters);
        Task<VehicleOwnerDto> GetByIdAsync(int id);
        Task<VehicleOwnerDto> CreateAsync(VehicleOwnerForCreateDto dto);
        Task UpdateAsync(int id, VehicleOwnerForUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
