using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : GenericController<ICollaboratorService, CollaboratorDto>
    {
        private readonly ICollaboratorService _collaboratorService;

        public CollaboratorController(ICollaboratorService collaboratorService): base(collaboratorService)
        {
            _collaboratorService = collaboratorService;
        }
       
        //[Authorize(Policy = "UserAndAdmin")] //TODO: Agregar los roles y politicas requeridas
        [HttpPost("PutImages")]
        public async Task<IActionResult> PutImages([FromForm] IFormFile image)
        {
            return Ok(await _collaboratorService.PutImage(image));
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _collaboratorService.GetRoles());
        }

        [HttpPost("Blocked")]
        public async Task<IActionResult> Blocked(int id)
        {
            await _collaboratorService.Blocked(id);
            return Ok();
        }
    }
}
