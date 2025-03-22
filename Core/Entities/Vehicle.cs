using System.ComponentModel.DataAnnotations;

namespace Bakalauras.Core.Entities;

public class Vehicle
{
    public string Id { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public Customer Customer { get; set; } = null!;
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
} 