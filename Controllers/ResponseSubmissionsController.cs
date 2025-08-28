using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVerification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseSubmissionsController : ControllerBase
    {
        private readonly IResponseSubmissionService _service;
        public ResponseSubmissionsController(IResponseSubmissionService service) => _service = service;

        [HttpGet]
        [Authorize] // users must be authenticated to view
        public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await _service.GetAllAsync(ct));

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            var item = await _service.GetAsync(id, ct);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateResponseSubmissionDto dto, CancellationToken ct)
        {
            var created = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.SubmissionID }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateResponseSubmissionDto dto, CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, dto, ct);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
