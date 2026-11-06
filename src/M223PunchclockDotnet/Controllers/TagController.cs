using System.Net.Mime;
using M223PunchclockDotnet.Dto;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("tags")]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<TagResponse>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TagResponse>>> Get()
        {
            var all = await _tagService.FindAll();
            return Ok(all.Select(TagResponse.FromEntity));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<TagResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TagResponse>> Post(TagRequest request)
        {
            var created = await _tagService.Add(new Tag { Title = request.Title });
            return Created($"/tags/{created.Id}", TagResponse.FromEntity(created));
        }
    }

    [ApiController]
    [Route("tags/{id}")]
    public class TagByIdController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagByIdController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagResponse>> GetById(int id)
        {
            var tag = await _tagService.FindById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(TagResponse.FromEntity(tag));
        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagResponse>> Update(int id, TagRequest request)
        {
            var tag = await _tagService.Update(id, request.Title);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(TagResponse.FromEntity(tag));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tagService.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
