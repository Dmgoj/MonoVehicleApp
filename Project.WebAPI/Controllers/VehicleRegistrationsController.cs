using Microsoft.AspNetCore.Mvc;
using Project.Service.Common;
using Project.Service.Common.Parameters;
using System.Threading.Tasks;

namespace Project.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleRegistrationsController : ControllerBase
    {
        private readonly IVehicleRegistrationService _service;

        public VehicleRegistrationsController(IVehicleRegistrationService service)
        {
            _service = service;
        }

        // GET: api/VehicleRegistrations
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<VehicleRegistrationDto>>> GetAll([FromQuery] QueryParameters parameters)
        {
            var result = await _service.GetAllAsync(parameters);
            return Ok(result);
        }

        // GET: api/VehicleRegistrations/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleRegistrationDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return Ok(dto);
        }

        // POST: api/VehicleRegistrations
        [HttpPost]
        public async Task<ActionResult<VehicleRegistrationDto>> Create([FromBody] VehicleRegistrationForCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/VehicleRegistrations/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleRegistrationForUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/VehicleRegistrations/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
