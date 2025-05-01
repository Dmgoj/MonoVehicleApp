using Microsoft.AspNetCore.Mvc;
using Project.Service.Common;
using Project.Service.Common.Parameters;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleMakesController : ControllerBase
    {
        private readonly IVehicleMakeService _service;

        public VehicleMakesController(IVehicleMakeService service)
        {
            _service = service;
        }

        // GET: api/VehicleMakes
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<VehicleMakeDto>>> GetAll([FromQuery] QueryParameters parameters)
        {
            var result = await _service.GetAllAsync(parameters);
            return Ok(result);
        }

        // GET: api/VehicleMakes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleMakeDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return Ok(dto);
        }

        // POST: api/VehicleMakes
        [HttpPost]
        public async Task<ActionResult<VehicleMakeDto>> Create([FromBody] VehicleMakeForCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/VehicleMakes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleMakeForUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/VehicleMakes/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
