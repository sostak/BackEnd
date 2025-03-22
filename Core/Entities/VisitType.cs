using System.ComponentModel.DataAnnotations;

namespace Bakalauras.Core.Entities;

public class VisitType
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int EstimatedDuration { get; set; } // in minutes
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
} 