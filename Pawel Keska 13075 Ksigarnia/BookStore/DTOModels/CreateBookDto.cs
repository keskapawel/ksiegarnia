using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOModels;

public class CreateBookDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public string Description { get; set; }
    public string Publisher { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public int Pages { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public int BookCategoryId { get; set; }
}