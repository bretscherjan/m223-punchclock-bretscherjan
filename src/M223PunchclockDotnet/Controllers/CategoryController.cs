using System.Net.Mime;
using M223PunchclockDotnet.Dto;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<CategoryResponse>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get()
        {
            var all = await _categoryService.FindAll();
            return Ok(all.Select(CategoryResponse.FromEntity));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryResponse>> Post(CategoryRequest request)
        {
            var created = await _categoryService.Add(new Category { Title = request.Title });
            return Created($"/categories/{created.Id}", CategoryResponse.FromEntity(created));
        }
    }
}
