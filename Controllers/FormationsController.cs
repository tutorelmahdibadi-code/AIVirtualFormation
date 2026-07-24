using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Models;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormationsController : ControllerBase
    {
        private readonly IFormationService service;

        public FormationsController(IFormationService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Formation>>> GetAll()
            => Ok(await service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Formation>> GetById(int id)
        {
            var formation = await service.GetByIdAsync(id);
            return formation == null ? NotFound() : Ok(formation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<ActionResult<Formation>> Create([FromBody] Formation formation)
        {
            try
            {
                var created = await service.CreateAsync(formation);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<IActionResult> Update(int id, [FromBody] Formation formation)
        {
            var success = await service.UpdateAsync(id, formation);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
