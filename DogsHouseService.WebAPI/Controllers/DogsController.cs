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
        public ActionResult GetApiVersion()
        {
            return Ok("Dogs house service. Version 1.0.1");
        }
    }
}
