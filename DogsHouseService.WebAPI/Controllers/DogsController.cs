﻿using DogsHouseService.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> GetDogs([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string attribute, [FromQuery] string order)
        {
            if (pageNumber != 0 && pageSize != 0 && !string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
            {
                try
                {
                    return Ok(await _dogService.GetPagedAndSortedDogsAsync(pageNumber,pageSize, attribute, order));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
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
            else if (pageNumber != 0 && pageSize != 0)
            {
                try
                {
                    return Ok(await _dogService.GetPagedDogsAsync(pageNumber, pageSize));
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