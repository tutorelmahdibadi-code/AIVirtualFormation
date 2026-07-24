using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Models;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategorieService service;

        public CategoriesController(ICategorieService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categorie>>> GetAll()
            => Ok(await service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Categorie>> GetById(int id)
        {
            var categorie = await service.GetByIdAsync(id);
            return categorie == null ? NotFound() : Ok(categorie);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Categorie>> Create([FromBody] Categorie categorie)
        {
            var created = await service.CreateAsync(categorie);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Categorie categorie)
        {
            var success = await service.UpdateAsync(id, categorie);
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
