using Microsoft.AspNetCore.Mvc;

namespace Web_253505_Tarhonski.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartItems = 0;

            ViewBag.TotalAmount = "00,0 руб";
            ViewBag.TotalItems = cartItems;

            return View();
        }
    }
}
