using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }
    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }


    public int CustomerId { get; set; }
    CustomerEntity Customer { get; set; } = null!;

    public int StatusId { get; set; }
    StatusTypeEntity Status { get; set; } = null!;

    public int UserId { get; set; }
    UserEntity User { get; set; } = null!;

    public int ProductId { get; set; }
    ProductEntity Product { get; set; } = null!;
}
