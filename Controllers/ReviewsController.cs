using HomeCakes.Data;
using HomeCakes.Models;
using Microsoft.AspNetCore.Mvc;
namespace HomeCakes.Controllers;
public class ReviewsController(ShopData data) : Controller
{
    public IActionResult Index()=>View(data.Reviews.Where(r=>r.IsApproved).ToList());
    [HttpPost] public IActionResult Add(Review review){var name=HttpContext.Session.GetString("UserName");if(name==null){TempData["Message"]="Чтобы оставить отзыв, войдите в личный кабинет.";return RedirectToAction("Login","Account");}if(!data.Orders.Any(o=>o.Customer==name&&o.Status=="Доставлен")){TempData["Message"]="Оставить отзыв можно после получения заказа.";return RedirectToAction("Index");}if(!ModelState.IsValid){TempData["Message"]="Проверьте имя и текст отзыва (не менее 5 символов).";return RedirectToAction("Index");} review.Name=name;review.Id=data.Reviews.Count+1;review.IsApproved=false;data.Reviews.Add(review);TempData["Message"]="Спасибо! Отзыв отправлен на модерацию.";return RedirectToAction("Index");}
}
