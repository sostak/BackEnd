using System.ComponentModel.DataAnnotations;

namespace Bakalauras.Core.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
} 