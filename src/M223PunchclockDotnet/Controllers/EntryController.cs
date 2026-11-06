using System.Net.Mime;
using M223PunchclockDotnet.Dto;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("entries")]
    public class EntryController : ControllerBase
    {
        private readonly EntryService _entryService;

        public EntryController(EntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<EntryResponse>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EntryResponse>>> Get()
        {
            var allEntries = await _entryService.FindAll();
            return Ok(allEntries.Select(EntryResponse.FromEntity));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<EntryResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EntryResponse>> AddEntry(EntryRequest request)
        {
            var created = await _entryService.AddEntry(request.ToEntity());
            return CreatedAtAction(
                nameof(EntryByIdController.GetById),
                "EntryById",
                new { id = created.Id },
                EntryResponse.FromEntity(created));
        }
    }


    [ApiController]
    [Route("entries/{id}")]
    public class EntryByIdController : ControllerBase
    {
        private readonly EntryService _entryService;

        public EntryByIdController(EntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [ProducesResponseType<EntryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EntryResponse>> GetById(int id)
        {
            var entry = await _entryService.FindById(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(EntryResponse.FromEntity(entry));
        }

        [HttpDelete]
        [ProducesResponseType<EntryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EntryResponse>> DeleteById(int id)
        {
            var entry = await _entryService.DeleteById(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(EntryResponse.FromEntity(entry));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<EntryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EntryResponse>> UpdateById(int id, EntryRequest request)
        {
            var existingEntry = await _entryService.FindById(id);
            if (existingEntry == null)
            {
                return NotFound();
            }
            request.ApplyTo(existingEntry);
            await _entryService.UpdateEntry(existingEntry);
            return Ok(EntryResponse.FromEntity(existingEntry));
        }
    }
}
