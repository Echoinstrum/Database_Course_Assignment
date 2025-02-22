namespace Business.Models;

public class ProjectModel // available properties that we want to be able to show the user in the frontend
{
    public int Id { get; set; }
    public string ProjectNumber { get; set; } = string.Empty;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ProjectManager { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Ej påbörjat";
    public string Service { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}
