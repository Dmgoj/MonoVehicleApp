using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Service.Common.Parameters;

namespace Project.Service.Common
{
    public interface IVehicleModelService
    {
        Task<PaginatedResult<VehicleModelDto>> GetAllAsync(QueryParameters parameters);
        Task<VehicleModelDto> GetByIdAsync(int id);
        Task<VehicleModelDto> CreateAsync(VehicleModelForCreateDto dto);
        Task UpdateAsync(int id, VehicleModelForUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
