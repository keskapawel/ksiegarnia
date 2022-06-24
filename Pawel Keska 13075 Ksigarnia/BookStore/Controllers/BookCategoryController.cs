using BookStore.DataBaseEntities;
using BookStore.DTOModels;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/categories")]
public class BookCategoryController : ControllerBase
{
    private readonly IBookCategoryService _service;

    public BookCategoryController(IBookCategoryService service)
    {
        _service = service;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookCategory>>> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBookCategoryDto category)
    {
        await _service.CreateAsync(category);
        return Created($"/api/categories", null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] CreateBookCategoryDto category)
    {
        await _service.UpdateAsync(id, category);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await _service.RemoveAsync(id);
        return NoContent();
    }
}