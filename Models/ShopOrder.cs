using System.ComponentModel.DataAnnotations;
namespace HomeCakes.Models;
public class ShopOrder { public int Id { get; set; } public string Customer { get; set; } = ""; [Required] public string Phone { get; set; } = ""; public string Address { get; set; } = ""; public string Items { get; set; } = ""; public decimal Total { get; set; } public string Status { get; set; } = "Новый"; public DateTime Created { get; set; } = DateTime.Now; }
