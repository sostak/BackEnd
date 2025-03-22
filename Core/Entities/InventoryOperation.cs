using System.ComponentModel.DataAnnotations;

namespace Bakalauras.Core.Entities;

public class InventoryOperation
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string OperationType { get; set; } = string.Empty; // "IN" or "OUT"
    public int Quantity { get; set; }
    public string InventoryItemId { get; set; } = string.Empty;
    public InventoryItem InventoryItem { get; set; } = null!;
    public string? ServiceId { get; set; }
    public Service? Service { get; set; }
    public string? Notes { get; set; }
} 