using AutoMapper;
using BookStore.DataBase;
using BookStore.DataBaseEntities;
using BookStore.DTOModels;
using BookStore.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public interface IBookService
{
    Task CreateAsync(CreateBookDto createBookDto);
    Task UpdateAsync(int id, CreateBookDto createBookDto);
    Task RemoveAsync(int id);
    Task<BookDto> GetByIdAsync(int id);
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<IEnumerable<BookDto>> GetByCategoryAsync(CreateBookCategoryDto bookCategory);
    Task<IEnumerable<BookDto>> GetOnlyAvailableAsync ();
    Task BuyBook(int id);
}

public class BookService : IBookService
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUserContextService _contextService;

    public BookService(BookStoreDbContext context,IMapper mapper,IUserContextService contextService)
    {
        _context = context;
        _mapper = mapper;
        _contextService = contextService;
    }
    
    public async Task CreateAsync(CreateBookDto createBookDto)
    {
        var newBook = new Book()
        {
            Title = createBookDto.Title,
            Author = createBookDto.Author,
            Description = createBookDto.Description,
            Publisher = createBookDto.Publisher,
            Year = createBookDto.Year,
            Pages = createBookDto.Pages,
            ImageUrl = createBookDto.ImageUrl,
            Price = createBookDto.Price,
            Quantity = createBookDto.Quantity,
            BookCategoryId = createBookDto.BookCategoryId
        };
        await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();
    }
    
    
    public async Task UpdateAsync(int id, CreateBookDto createBookDto)
    {
        var dbBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (dbBook == null) throw new NotFoundException("Book not found");

        dbBook.Title = createBookDto.Title;
        dbBook.Author = createBookDto.Author;
        dbBook.Description = createBookDto.Description;
        dbBook.Publisher = createBookDto.Publisher;
        dbBook.Year = createBookDto.Year;
        dbBook.Pages = createBookDto.Pages;
        dbBook.ImageUrl = createBookDto.ImageUrl;
        dbBook.Price = createBookDto.Price;
        dbBook.Quantity = createBookDto.Quantity;
        dbBook.BookCategoryId = createBookDto.BookCategoryId;

        await _context.SaveChangesAsync();
    }
    
    public async Task RemoveAsync(int id)
    {
        var dbBook = await _context.Books.Include(b=>b.Category).FirstOrDefaultAsync(b => b.Id == id);
        if(dbBook==null) throw new NotFoundException("Book not found");
        _context.Books.Remove(dbBook);
        await _context.SaveChangesAsync();
    }
    
    public async Task<BookDto> GetByIdAsync(int id)
    {
        var book = await _context.Books.Include(b=>b.Category).FirstOrDefaultAsync(b =>b.Id == id);
        if(book==null) throw new NotFoundException("Book not found");
        return _mapper.Map<BookDto>(book);
    }
    
    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        IQueryable<Book> booksQuery = _context.Books.Include(x => x.Category);
        var books = await booksQuery.ToListAsync();
        var booksDtos = _mapper.Map<IEnumerable<BookDto>>(books);
        return booksDtos;
    }
    
    public async Task<IEnumerable<BookDto>> GetByCategoryAsync(CreateBookCategoryDto bookCategory)
    {
        IQueryable<Book> booksQuery = _context.Books.Include(x => x.Category).Where(x=>x.Category.Name==bookCategory.Name);
        var books = await booksQuery.ToListAsync();
        var booksDtos = _mapper.Map<IEnumerable<BookDto>>(books);
        return booksDtos;
    }
    public async Task<IEnumerable<BookDto>> GetOnlyAvailableAsync()
    {
        IQueryable<Book> booksQuery = _context.Books.Include(x => x.Category).Where(x=>x.Quantity>=1);
        var books = await booksQuery.ToListAsync();
        var booksDtos = _mapper.Map<IEnumerable<BookDto>>(books);
        return booksDtos;
    }

    public async Task BuyBook(int id)
    {
        var dbBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if(dbBook == null) throw new NotFoundException("Book not found");
        if (dbBook.Quantity < 1) throw new NotFoundException("No books");
        dbBook.Quantity -= 1;
        await _context.SaveChangesAsync();
    }
}