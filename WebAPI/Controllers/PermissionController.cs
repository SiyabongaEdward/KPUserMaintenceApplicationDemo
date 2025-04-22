using Application.DTOs;
using Application.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _permissionService.GetAllAsync();
            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionDto dto)
        {
            await _permissionService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PermissionDto dto)
        {
            await _permissionService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _permissionService.DeleteAsync(id);
            return Ok();
        }
    }
}
