namespace Bakalauras.Core.Entities;

public class Service
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string VisitId { get; set; } = string.Empty;
    public Visit Visit { get; set; } = null!;
    public string MechanicId { get; set; } = string.Empty;
    public Mechanic Mechanic { get; set; } = null!;
} 