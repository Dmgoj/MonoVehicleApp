using Microsoft.AspNetCore.Mvc;
using Project.Service.Common;
using Project.Service.Common.Parameters;
using System.Threading.Tasks;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleOwnersController : ControllerBase
    {
        private readonly IVehicleOwnerService _service;

        public VehicleOwnersController(IVehicleOwnerService service)
        {
            _service = service;
        }

        // GET api/vehicleowners?Filter=&SortBy=&SortDescending=&PageNumber=&PageSize=
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<VehicleOwnerDto>>> GetAll([FromQuery] QueryParameters parameters)
        {
            var result = await _service.GetAllAsync(parameters);
            return Ok(result);
        }

        // GET api/vehicleowners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleOwnerDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return Ok(dto);
        }

        // POST api/vehicleowners
        [HttpPost]
        public async Task<ActionResult<VehicleOwnerDto>> Create([FromBody] VehicleOwnerForCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // PUT api/vehicleowners/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleOwnerForUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE api/vehicleowners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
