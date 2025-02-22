namespace Business.Models;

public class ProjectRegistrationForm
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ProjectManager { get; set; } = string.Empty!;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Ej påbörjat";
    public string Service { get; set; } = string.Empty;
    public string CustomerName { get; set; } = "Okänd kund";
}
