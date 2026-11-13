using System.Text.Json;
using HomeCakes.Data;
using HomeCakes.Models;
using Microsoft.AspNetCore.Mvc;
namespace HomeCakes.Controllers;
public class CartController(ShopData data) : Controller
{
    private Dictionary<int,int> GetCart() => JsonSerializer.Deserialize<Dictionary<int,int>>(HttpContext.Session.GetString("Cart") ?? "{}") ?? new();
    private void Save(Dictionary<int,int> cart) => HttpContext.Session.SetString("Cart",JsonSerializer.Serialize(cart));
    public IActionResult Index() { var cart=GetCart(); ViewBag.Lines=cart.Select(x=>(Cake:data.Cakes.FirstOrDefault(c=>c.Id==x.Key),Qty:x.Value)).Where(x=>x.Cake!=null).ToList(); return View(); }
    [HttpPost] public IActionResult Add(int id) { var cart=GetCart(); cart[id]=cart.GetValueOrDefault(id)+1; Save(cart); return RedirectToAction("Index"); }
    [HttpPost] public IActionResult Remove(int id) { var cart=GetCart(); cart.Remove(id); Save(cart); return RedirectToAction("Index"); }
    [HttpPost] public IActionResult Checkout(string phone,string address)
    {
        var cart=GetCart(); if(cart.Count==0) return RedirectToAction("Index");
        var lines=cart.Select(x=>data.Cakes.First(c=>c.Id==x.Key).Name+" × "+x.Value).ToList();
        var total=cart.Sum(x=>data.Cakes.First(c=>c.Id==x.Key).Price*x.Value);
        data.Orders.Add(new ShopOrder{Id=data.Orders.Count+1,Customer=HttpContext.Session.GetString("UserName")??"Гость",Phone=phone,Address=address,Items=string.Join(", ",lines),Total=total});
        Save(new()); TempData["Message"]="Заказ принят! мы скоро свяжемся с вами."; return RedirectToAction("Index");
    }
}

