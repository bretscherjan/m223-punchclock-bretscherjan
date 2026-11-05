using System.Net.Mime;
using M223PunchclockDotnet.Model;
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
        [ProducesResponseType<Entry>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var allEntries = await _entryService.FindAll();
            return Ok(allEntries);
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Entry>(StatusCodes.Status201Created)]
        public async Task<ActionResult<Entry>> AddEntry(Entry entry){
            var newElement = await _entryService.AddEntry(entry);

            return CreatedAtAction(nameof(Get), new{id = entry.Id}, entry);
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

        [HttpDelete]
        [ProducesResponseType<Entry>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            var entry = await _entryService.DeleteById(id);
            if (entry == null)
            {
                return NotFound();
            }
            await _entryService.DeleteById(id);
            return Ok(entry);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Entry>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateById(int id, Entry entry)
        {
            var existingEntry = await _entryService.FindById(id);
            if (existingEntry == null)
            {
                return NotFound();
            }
            existingEntry.CheckIn = entry.CheckIn;
            existingEntry.CheckOut = entry.CheckOut;
            await _entryService.UpdateEntry(existingEntry);
            return Ok(existingEntry);
        }
    }
}
