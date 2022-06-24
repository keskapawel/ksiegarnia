using AutoMapper;
using BookStore.DataBase;
using BookStore.DataBaseEntities;
using BookStore.DTOModels;
using BookStore.Exception;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public interface IBookCategoryService
{
    Task CreateAsync(CreateBookCategoryDto createBookCategoryDto);
    Task UpdateAsync(int id ,CreateBookCategoryDto createBookCategoryDto);
    Task RemoveAsync(int id);
    Task<IEnumerable<BookCategory>> GetAllAsync();
}

public class BookCategoryService : IBookCategoryService
{
    private readonly BookStoreDbContext _context;

    public BookCategoryService(BookStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateAsync(CreateBookCategoryDto createBookCategoryDto)
    {
        var category = new BookCategory()
        {
            Name = createBookCategoryDto.Name,
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(int id ,CreateBookCategoryDto createBookCategoryDto)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is null) throw new NotFoundException("BookCategory not found");
        category.Name = createBookCategoryDto.Name;
        await _context.SaveChangesAsync();
    }
    
    public async Task RemoveAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if(category==null) throw new NotFoundException("BookCategory not found");
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<BookCategory>> GetAllAsync()
    {
        var categories = await _context
            .Categories
            .ToListAsync();
        return categories;
    }
}