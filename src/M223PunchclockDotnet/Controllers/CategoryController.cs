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

    [ApiController]
    [Route("categories/{id}")]
    public class CategoryByIdController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryByIdController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> GetById(int id)
        {
            var category = await _categoryService.FindById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(CategoryResponse.FromEntity(category));
        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> Update(int id, CategoryRequest request)
        {
            var category = await _categoryService.Update(id, request.Title);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(CategoryResponse.FromEntity(category));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
