using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Models;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormateursController : ControllerBase
    {
        private readonly IFormateurService service;

        public FormateursController(IFormateurService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Formateur>>> GetAll()
            => Ok(await service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Formateur>> GetById(int id)
        {
            var formateur = await service.GetByIdAsync(id);
            return formateur == null ? NotFound() : Ok(formateur);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Formateur>> Create([FromBody] Formateur formateur)
        {
            var created = await service.CreateAsync(formateur);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<IActionResult> Update(int id, [FromBody] Formateur formateur)
        {
            var success = await service.UpdateAsync(id, formateur);
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
