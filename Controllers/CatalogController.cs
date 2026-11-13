using HomeCakes.Data;
using Microsoft.AspNetCore.Mvc;
namespace HomeCakes.Controllers;
public class CatalogController(ShopData data) : Controller
{
    public IActionResult Index(string? category, string? sort)
    {
        var cakes=data.Cakes.AsEnumerable();
        if(!string.IsNullOrWhiteSpace(category)) cakes=cakes.Where(x=>x.Category==category);
        cakes=sort switch { "price"=>cakes.OrderBy(x=>x.Price), "name"=>cakes.OrderBy(x=>x.Name), _=>cakes.OrderBy(x=>x.Id) };
        ViewBag.Categories=data.Cakes.Select(x=>x.Category).Distinct().ToList(); ViewBag.Fillings=data.Fillings;
        return View(cakes.ToList());
    }
}
