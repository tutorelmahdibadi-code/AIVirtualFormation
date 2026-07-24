using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Models;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService service;

        public ModulesController(IModuleService service)
        {
            this.service = service;
        }

        [HttpGet("formation/{formationId}")]
        public async Task<ActionResult<List<ModuleFormation>>> GetByFormation(int formationId)
            => Ok(await service.GetByFormationAsync(formationId));

        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleFormation>> GetById(int id)
        {
            var module = await service.GetByIdAsync(id);
            return module == null ? NotFound() : Ok(module);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<ActionResult<ModuleFormation>> Create([FromBody] ModuleFormation module)
        {
            try
            {
                var created = await service.CreateAsync(module);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<IActionResult> Update(int id, [FromBody] ModuleFormation module)
        {
            var success = await service.UpdateAsync(id, module);
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
