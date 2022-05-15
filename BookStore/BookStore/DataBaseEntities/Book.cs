using System.ComponentModel.DataAnnotations;

namespace BookStore.DataBaseEntities;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Publisher { get; set; }
    public int Year { get; set; }
    public int Pages { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
}