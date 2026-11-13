using Microsoft.AspNetCore.Mvc;
using HomeCakes.Data;
namespace HomeCakes.Controllers;
public class HomeController(ShopData data) : Controller
{
    public IActionResult Index() => View(data.Cakes.Where(c=>c.IsPopular).ToList());
    public IActionResult About() => View();
    public IActionResult Offers() => View();
    public IActionResult Contacts() => View();
    [HttpPost] public IActionResult ContactSubmit(string name, string contact, string message)
    {
        if(string.IsNullOrWhiteSpace(name)||string.IsNullOrWhiteSpace(contact)||string.IsNullOrWhiteSpace(message)) TempData["Message"]="Заполните все поля формы.";
        else TempData["Message"]="Спасибо, "+name+"! Мы свяжемся с вами в ближайшее время.";
        return RedirectToAction("Contacts");
    }
    public IActionResult Error() => View();
}
