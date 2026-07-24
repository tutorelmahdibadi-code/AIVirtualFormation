using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Models;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModalitesController : ControllerBase
    {
        private readonly IModaliteService service;

        public ModalitesController(IModaliteService service)
        {
            this.service = service;
        }

        [HttpGet("module/{moduleId}")]
        public async Task<ActionResult<List<Modalite>>> GetByModule(int moduleId)
            => Ok(await service.GetByModuleAsync(moduleId));

        [HttpGet("{id}")]
        public async Task<ActionResult<Modalite>> GetById(int id)
        {
            var modalite = await service.GetByIdAsync(id);
            return modalite == null ? NotFound() : Ok(modalite);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<ActionResult<Modalite>> Create([FromBody] Modalite modalite)
        {
            try
            {
                var created = await service.CreateAsync(modalite);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<IActionResult> Update(int id, [FromBody] Modalite modalite)
        {
            var success = await service.UpdateAsync(id, modalite);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
