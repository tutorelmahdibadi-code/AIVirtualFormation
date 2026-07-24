using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Dtos;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarIaService avatarService;

        public AvatarController(IAvatarIaService avatarService)
        {
            this.avatarService = avatarService;
        }

        // Demarre une session avec l'avatar IA (Anam.ai) pour presenter un module donne.
        // Le frontend recupere le SessionToken et l'utilise avec le SDK JS Anam
        // (@anam-ai/js-sdk) pour afficher/connecter directement le flux video+audio.
        [HttpPost("session")]
        public async Task<ActionResult<AvatarSessionDto>> DemarrerSession([FromBody] StartAvatarSessionDto dto)
        {
            try
            {
                var session = await avatarService.DemarrerSessionAsync(dto);
                return Ok(session);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
