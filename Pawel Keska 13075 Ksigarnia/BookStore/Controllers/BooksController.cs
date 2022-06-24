using System.Net.Sockets;
using BookStore.DataBaseEntities;
using BookStore.DTOModels;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[ApiController]
[Authorize]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("category")]

    public async Task<IActionResult> GetByCategory([FromBody] CreateBookCategoryDto bookCategory)
    {
        var books = await _bookService.GetByCategoryAsync(bookCategory);
        return Ok(books);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("available")]

    public async Task<IActionResult> GetOnlyAvailable()
    {
        var books = await _bookService.GetOnlyAvailableAsync();
        return Ok(books);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        return Ok(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto book)
    {
        await _bookService.CreateAsync(book);
        return Created($"/api/books", null);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] CreateBookDto book)
    {
        await _bookService.UpdateAsync(id, book);
        return Ok();
    }

    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook([FromRoute] int id)
    {
        await _bookService.RemoveAsync(id);
        return NoContent();
    }

    [HttpPost]
    [Route("buy/{id}")]
    public async Task<IActionResult> BuyBook(int id)
    {
        await _bookService.BuyBook(id);
        return Created($"/api/books", null);
    }
}