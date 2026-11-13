using HomeCakes.Data;
using HomeCakes.Models;
using Microsoft.AspNetCore.Mvc;
namespace HomeCakes.Controllers;
public class AdminController(ShopData data) : Controller
{
    private bool IsAdmin()=>HttpContext.Session.GetString("Role")=="Администратор";
    public IActionResult Index(){if(!IsAdmin())return RedirectToAction("Login","Account"); ViewBag.IsAdmin=true; ViewBag.Orders=data.Orders; ViewBag.Reviews=data.Reviews; ViewBag.Cakes=data.Cakes; ViewBag.Users=data.Users; return View();}
    [HttpPost] public IActionResult Status(int id,string status){if(!IsAdmin())return Forbid();var order=data.Orders.FirstOrDefault(o=>o.Id==id);if(order!=null)order.Status=status;return RedirectToAction("Index");}
    [HttpPost] public IActionResult ApproveReview(int id){if(!IsAdmin())return Forbid();var review=data.Reviews.FirstOrDefault(r=>r.Id==id);if(review!=null)review.IsApproved=true;return RedirectToAction("Index");}
    [HttpPost] public IActionResult DeleteReview(int id){if(!IsAdmin())return Forbid();var review=data.Reviews.FirstOrDefault(r=>r.Id==id);if(review!=null)data.Reviews.Remove(review);return RedirectToAction("Index");}
    [HttpPost] public IActionResult AddCake(Cake cake){if(!IsAdmin())return Forbid();if(ModelState.IsValid){cake.Id=data.Cakes.Max(c=>c.Id)+1;data.Cakes.Add(cake);}return RedirectToAction("Index");}
    [HttpPost] public IActionResult DeleteCake(int id){if(!IsAdmin())return Forbid();var cake=data.Cakes.FirstOrDefault(c=>c.Id==id);if(cake!=null)data.Cakes.Remove(cake);return RedirectToAction("Index");}
    [HttpPost] public IActionResult SetRole(string email,string role){if(!IsAdmin())return Forbid();var user=data.Users.FirstOrDefault(u=>u.Email==email);if(user!=null && role is "Пользователь" or "Администратор")user.Role=role;return RedirectToAction("Index");}
}
