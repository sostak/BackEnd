using System.ComponentModel.DataAnnotations;

namespace Bakalauras.Core.Entities;

public class Mechanic
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public string Specialization { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = string.Empty;
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
} 