using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventorySystem.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 10000000, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, 100000, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Minimum stock alert is required")]
    [Range(0, 10000, ErrorMessage = "Minimum stock cannot be negative")]
    [Display(Name = "Min Stock Alert Level")]
    public int MinStock { get; set; } = 5;

    [Required(ErrorMessage = "Please select a category")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Supplier")]
    public int? SupplierId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Suppliers { get; set; } = new List<SelectListItem>();
}
