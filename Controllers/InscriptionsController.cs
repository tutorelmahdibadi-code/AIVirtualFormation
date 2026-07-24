using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Models;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InscriptionsController : ControllerBase
    {
        private readonly IInscriptionService service;

        public InscriptionsController(IInscriptionService service)
        {
            this.service = service;
        }

        [HttpGet("apprenant/{apprenantId}")]
        public async Task<ActionResult<List<Inscription>>> GetByApprenant(int apprenantId)
            => Ok(await service.GetByApprenantAsync(apprenantId));

        [HttpGet("formation/{formationId}")]
        [Authorize(Roles = "Admin,Formateur")]
        public async Task<ActionResult<List<Inscription>>> GetByFormation(int formationId)
            => Ok(await service.GetByFormationAsync(formationId));

        [HttpPost]
        public async Task<ActionResult<Inscription>> Inscrire([FromQuery] int apprenantId, [FromQuery] int formationId)
        {
            try
            {
                var inscription = await service.InscrireAsync(apprenantId, formationId);
                return Ok(inscription);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/progression")]
        public async Task<IActionResult> MettreAJourProgression(int id, [FromQuery] int progression)
        {
            var success = await service.MettreAJourProgressionAsync(id, progression);
            return success ? NoContent() : NotFound();
        }
    }
}
