using System.ComponentModel.DataAnnotations;

namespace Bakalauras.Core.Entities;

public class InventoryItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public ICollection<InventoryOperation> InventoryOperations { get; set; } = new List<InventoryOperation>();
} 