using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models;

[Table("StockTransactions")]
public class StockTransaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = "Stock In"; // "Stock In" or "Stock Out"

    [Required]
    public int Quantity { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [StringLength(250)]
    public string? Notes { get; set; }
}
