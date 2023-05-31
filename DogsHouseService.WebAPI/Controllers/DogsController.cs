using DogsHouseService.BLL.Interfaces;
using DogsHouseService.Common.DTO.Dog;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.WebAPI.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogService _dogService;

        public DogsController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("ping")]
        public IActionResult GetApiVersion()
        {
            return Ok("Dogs house service. Version 1.0.1");
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 0, [FromQuery] string attribute = null, [FromQuery] string order = null)
        {
            if (pageNumber != 0 && pageSize != 0 && !string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
            {
                return Ok(await _dogService.GetPagedAndSortedDogsAsync(pageNumber,pageSize, attribute, order));
            }
            else if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
            {
                return Ok(await _dogService.GetSortedDogsAsync(attribute, order));
            }
            else if (pageNumber != 0 && pageSize != 0)
            {
                return Ok(await _dogService.GetPagedDogsAsync(pageNumber, pageSize));
            }
            else
            {
                return Ok(await _dogService.GetAllDogsAsync());
            }
        }

        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] DogDto dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("dogs", await _dogService.CreateDogAsync(dog));
        }
    }
}
