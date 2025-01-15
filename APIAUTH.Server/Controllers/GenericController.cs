using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    public abstract class GenericController<TEntityService, TDto>
         : ControllerBase
         where TEntityService : IGenericService<TDto>
         where TDto : BaseDto
    {
        protected readonly TEntityService _entityService;

        public GenericController(TEntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> Get(int id)
        {
            var result = await _entityService.Get(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("")]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto)
        {
            if (dto == null) return BadRequest();

            var (isValid, message) = await _entityService.Validate(null, dto);
            if (!isValid) return BadRequest(message);

           
            return Ok(await _entityService.Save(dto));
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] TDto dto)
        {
            if (dto == null) return BadRequest();
            if (!await _entityService.Exists(id)) return BadRequest();

            var (isValid, message) = await _entityService.Validate(id, dto);
            if (!isValid) return BadRequest(message);

            await _entityService.Save(dto);
            return Ok();
        }

        [HttpPut("{id}/activate")]
        public virtual async Task<IActionResult> PutActivate([FromRoute] int id)
        {
            if (!await _entityService.Exists(id)) return BadRequest();
            await _entityService.Activate(id);
            return Ok();
        }

        [HttpPut("{id}/inactivate")]
        public virtual async Task<IActionResult> PutInactivate([FromRoute] int id)
        {
            if (!await _entityService.Exists(id)) return BadRequest();
            await _entityService.Inactivate(id);
            return Ok();
        }
    }
}
