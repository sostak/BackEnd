namespace Bakalauras.Core.Entities;

public class Visit
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public Customer Customer { get; set; } = null!;
    public string MechanicId { get; set; } = string.Empty;
    public Mechanic Mechanic { get; set; } = null!;
    public string VehicleId { get; set; } = string.Empty;
    public Vehicle Vehicle { get; set; } = null!;
    public string VisitTypeId { get; set; } = string.Empty;
    public VisitType VisitType { get; set; } = null!;
    public ICollection<Service> Services { get; set; } = new List<Service>();
} 