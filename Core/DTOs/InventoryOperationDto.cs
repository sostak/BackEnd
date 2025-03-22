namespace Bakalauras.Core.DTOs;

public class InventoryOperationDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string InventoryItemId { get; set; } = string.Empty;
    public string? ServiceId { get; set; }
    public string? Notes { get; set; }
}

public class CreateInventoryOperationDto
{
    public string OperationType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string InventoryItemId { get; set; } = string.Empty;
    public string? ServiceId { get; set; }
    public string? Notes { get; set; }
}

public class UpdateInventoryOperationDto
{
    public string OperationType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Notes { get; set; }
} 