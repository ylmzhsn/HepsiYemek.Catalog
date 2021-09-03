namespace HepsiYemek.Catalog.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using HepsiYemek.Catalog.Data.Entities;
    using HepsiYemek.Catalog.Service.Interfaces;

    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories();

            return Ok(categories);
        }

        [HttpGet("{id:length(24)}", Name = "GetCategory")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Category>> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                _logger.LogError($"Category with id: {id}, not found.");

                return NotFound();
            }

            return Ok(category);
        }

        [Route("[action]/{name}", Name = "GetCategoryByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryByName(string name)
        {
            var items = await _categoryService.GetCategoryByName(name);

            if (items == null)
            {
                _logger.LogError($"Categories with name: {name} not found.");
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            await _categoryService.CreateCategory(category);

            return CreatedAtRoute("GetCategory", new { id = category._id }, category);
        }

        [HttpPut("{id:length(24)}", Name = "UpdateCategory")]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            return Ok(await _categoryService.UpdateCategory(category));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteCategory")]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCategoryById(string id)
        {
            return Ok(await _categoryService.DeleteCategory(id));
        }
    }
}
