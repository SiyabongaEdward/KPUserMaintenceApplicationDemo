using Application.DTOs;
using Application.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPermissionController : ControllerBase
    {
        private readonly IGroupPermissionService _service;

        public GroupPermissionController(IGroupPermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupPermissionDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpDelete("{groupId}/{permissionId}")]
        public async Task<IActionResult> Delete(Guid groupId, Guid permissionId)
        {
            await _service.DeleteAsync(groupId, permissionId);
            return Ok();
        }
    }
}
