using System.ComponentModel.DataAnnotations;

namespace BookStore.DataBaseEntities;

public class BookCategory
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}