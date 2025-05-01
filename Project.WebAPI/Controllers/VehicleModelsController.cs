using Microsoft.AspNetCore.Mvc;
using Project.Service.Common;
using Project.Service.Common.Parameters;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleModelsController : ControllerBase
    {
        private readonly IVehicleModelService _service;

        public VehicleModelsController(IVehicleModelService service)
        {
            _service = service;
        }

        // GET: api/VehicleModels
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<VehicleModelDto>>> GetAll([FromQuery] QueryParameters parameters)
        {
            var result = await _service.GetAllAsync(parameters);
            return Ok(result);
        }

        // GET: api/VehicleModels/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleModelDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return Ok(dto);
        }

        // POST: api/VehicleModels
        [HttpPost]
        public async Task<ActionResult<VehicleModelDto>> Create([FromBody] VehicleModelForCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/VehicleModels/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleModelForUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/VehicleModels/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
