using M223PunchclockDotnet.Dto;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("entry-details")]
    public class EntryDetailController : ControllerBase
    {
        private readonly EntryService _entryService;

        public EntryDetailController(EntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<EntryDetailDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EntryDetailDto>>> Get()
        {
            var allEntries = await _entryService.FindAll();
            return Ok(allEntries.Select(EntryDetailDto.FromEntity));
        }

        [HttpGet("{id}")]
        [ProducesResponseType<EntryDetailDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EntryDetailDto>> GetById(int id)
        {
            var entry = await _entryService.FindById(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(EntryDetailDto.FromEntity(entry));
        }
    }
}
