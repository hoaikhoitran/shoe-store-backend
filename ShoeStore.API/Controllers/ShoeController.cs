using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Models.DTOs;
using ShoeStore.API.Services.Interfaces;

namespace ShoeStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoeController : ControllerBase
    {
        private readonly IShoeServices _shoeServices;

        public ShoeController(IShoeServices shoeServices)
        {
            _shoeServices = shoeServices;
        }

        // ===================== CREATE =====================
        // POST: api/shoe
        [HttpPost]
        public async Task<IActionResult> CreateShoe([FromBody] ShoeCreateDto dto)
        {
            var result = await _shoeServices.CreateShoeAsync(dto);
            return Ok(result);
        }

        // ===================== GET BY ID =====================
        // GET: api/shoe/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShoeById(int id)
        {
            var shoe = await _shoeServices.GetShoeByIdAsync(id);
            return Ok(shoe);
        }

        // ===================== GET LIST + FILTER + PAGING =====================
        // GET: api/shoe?pageNumber=1&pageSize=10&searchTerm=nike
        [HttpGet]
        public async Task<IActionResult> GetShoes(
            [FromQuery] FilterParams filters,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _shoeServices.GetShoesAsync(filters, pageNumber, pageSize);
            return Ok(result);
        }

        // ===================== UPDATE =====================
        // PUT: api/shoe/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShoe(
            int id,
            [FromBody] ShoeCreateDto dto)
        {
            var result = await _shoeServices.UpdateShoeAsync(id, dto);
            return Ok(result);
        }

        // ===================== SOFT DELETE =====================
        // DELETE: api/shoe/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShoe(int id)
        {
            var result = await _shoeServices.DeleteShoeAsync(id);
            return Ok(result);
        }
    }
}
