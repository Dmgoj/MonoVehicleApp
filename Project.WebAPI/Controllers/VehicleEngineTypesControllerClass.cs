using Microsoft.AspNetCore.Mvc;
using Project.Service.Common;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleEngineTypesController : ControllerBase
    {
        private readonly IVehicleEngineTypeService _service;

        public VehicleEngineTypesController(IVehicleEngineTypeService service)
        {
            _service = service;
        }

        // GET: api/VehicleEngineTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleEngineTypeDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET: api/VehicleEngineTypes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleEngineTypeDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return Ok(dto);
        }
    }
}
