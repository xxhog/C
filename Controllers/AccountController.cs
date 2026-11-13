using HomeCakes.Data;
using HomeCakes.Models;
using Microsoft.AspNetCore.Mvc;
namespace HomeCakes.Controllers;
public class AccountController(ShopData data) : Controller
{
    public IActionResult Login()=>View(new LoginModel());
    [HttpPost] public IActionResult Login(LoginModel model)
    {
        if(!ModelState.IsValid) return View(model);
        var user=data.Users.FirstOrDefault(x=>x.Email.Equals(model.Email,StringComparison.OrdinalIgnoreCase));
        if(user==null || !ShopData.VerifyPassword(model.Password,user.PasswordHash)){ModelState.AddModelError("","Неверный e-mail или пароль");return View(model);}
        HttpContext.Session.SetString("UserName",user.Name); HttpContext.Session.SetString("UserEmail",user.Email); HttpContext.Session.SetString("Role",user.Role);
        return RedirectToAction("Index","Home");
    }
    public IActionResult Register()=>View(new RegisterModel());
    [HttpPost] public IActionResult Register(RegisterModel model)
    {
        if(data.Users.Any(x=>x.Email.Equals(model.Email,StringComparison.OrdinalIgnoreCase))) ModelState.AddModelError("Email","Этот e-mail уже зарегистрирован");
        if(!ModelState.IsValid)return View(model);
        var user=new ShopUser{Name=model.Name,Email=model.Email,PasswordHash=ShopData.HashPassword(model.Password)}; data.Users.Add(user);
        HttpContext.Session.SetString("UserName",user.Name); HttpContext.Session.SetString("UserEmail",user.Email); HttpContext.Session.SetString("Role",user.Role);
        return RedirectToAction("Index","Home");
    }
    public IActionResult Profile(){if(HttpContext.Session.GetString("UserEmail")==null)return RedirectToAction("Login"); ViewBag.Orders=data.Orders.Where(o=>o.Customer==HttpContext.Session.GetString("UserName")).ToList();return View();}
    public IActionResult Edit(){if(HttpContext.Session.GetString("UserEmail")==null)return RedirectToAction("Login");return View();}
    [HttpPost] public IActionResult Edit(string name){if(HttpContext.Session.GetString("UserEmail")==null)return RedirectToAction("Login");if(string.IsNullOrWhiteSpace(name)){TempData["Message"]="Укажите имя.";return RedirectToAction("Edit");}var user=data.Users.FirstOrDefault(u=>u.Email==HttpContext.Session.GetString("UserEmail"));if(user!=null)user.Name=name.Trim();HttpContext.Session.SetString("UserName",name.Trim());TempData["Message"]="Профиль сохранён.";return RedirectToAction("Profile");}
    public IActionResult Logout(){HttpContext.Session.Clear();return RedirectToAction("Index","Home");}
}
