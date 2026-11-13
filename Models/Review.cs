using System.ComponentModel.DataAnnotations;
namespace HomeCakes.Models;
public class Review { public int Id { get; set; } [Required] public string Name { get; set; } = ""; [Required, StringLength(500, MinimumLength=5)] public string Text { get; set; } = ""; public int Rating { get; set; } = 5; public bool IsApproved { get; set; } }
