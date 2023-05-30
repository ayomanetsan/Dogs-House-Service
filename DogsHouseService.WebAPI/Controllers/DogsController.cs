using DogsHouseService.BLL.Interfaces;
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
        public async Task<IActionResult> GetDogs([FromQuery] string attribute, [FromQuery] string order)
        {
            if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
            {
                try
                {
                    return Ok(await _dogService.GetSortedDogsAsync(attribute, order));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }  
            }
            else
            {
                return Ok(await _dogService.GetAllDogsAsync());
            }
        }

    }
}
