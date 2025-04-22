using Application.DTOs;
using Application.Interfaces;
using BusinessLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IUserGroupService _service;

        public UserGroupController(IUserGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userGroups = await _service.GetAllAsync();
            return Ok(userGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userGroup = await _service.GetByIdAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            return Ok(userGroup);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserGroupDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserGroupDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            var success = await _service.UpdateAsync(dto);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
