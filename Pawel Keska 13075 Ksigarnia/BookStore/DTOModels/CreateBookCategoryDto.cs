using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOModels;

public class CreateBookCategoryDto
{
    [Required] 
    public string Name { get; set; }
}