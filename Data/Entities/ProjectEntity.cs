using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Data.Entities;

public class ProjectEntity
{
    public int Id { get; set; }
    //Got some help with the "ProjectNumber" from ChatGpt-4o, to set up the ProjectNumber increment with a "p-" in ProjectService 
    public string ProjectNumber { get; private set; } = string.Empty;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }
    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }
    public string ProjectManager { get; set; } = string.Empty!;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Ej påbörjat";
    public string Service { get; set; } = string.Empty!;
    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;

    //public int StatusTypeId { get; set; }
    //public StatusTypeEntity Status { get; set; } = null!;

    //public int UserId { get; set; }
    //public UserEntity User { get; set; } = null!;

    //public int ProductId { get; set; }
    //public ProductEntity Product { get; set; } = null!;

    public void SetProjectNumber(string projectNumber)
    {
        ProjectNumber = projectNumber;
    }
}
