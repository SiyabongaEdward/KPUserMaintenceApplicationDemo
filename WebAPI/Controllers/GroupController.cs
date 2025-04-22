using Application.DTOs;
using Application.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var group = await _groupService.GetByIdAsync(id);
            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupDto groupDto)
        {
            await _groupService.CreateAsync(groupDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] GroupDto groupDto)
        {
            await _groupService.UpdateAsync(id, groupDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _groupService.DeleteAsync(id);
            return Ok();
        }
    }
}
