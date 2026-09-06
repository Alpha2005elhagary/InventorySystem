using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int MinStock { get; set; } = 5;

    // Foreign Keys
    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    public int? SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public Supplier? Supplier { get; set; }

    // Navigation properties
    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    // Helper business properties
    [NotMapped]
    public bool IsLowStock => Quantity <= MinStock && Quantity > 0;

    [NotMapped]
    public bool IsOutOfStock => Quantity <= 0;
}
