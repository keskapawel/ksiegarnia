using BookStore.DataBaseEntities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataBase;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<EBook> EBooks { get; set; }
    public DbSet<BookCategory> Categories { get; set; }
}