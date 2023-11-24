using DOCTORLOAN.Models.Contents;
using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int id, int SoLuong, string type = "Normal")
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.MaHh == id);

            if (item == null)//chưa có
            {
                var hangHoa = _context.HangHoa.SingleOrDefault(p => p.MaHh == id);
                item = new CartItem
                {
                    MaHh = id,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia.Value,
                    SoLuong = SoLuong,
                    Hinh = hangHoa.Hinh
                };
                myCart.Add(item);
            }
            else
            {
                item.SoLuong += SoLuong;
            }
            HttpContext.Session.Set("GioHang", myCart);

            if (type == "ajax")
            {
                return Json(new
                {
                    SoLuong = Carts.Sum(c => c.SoLuong)
                });
            }
            return RedirectToAction("Index");
        }

        public IActionResult Payment()
        {
            return View();
        }
    }
}
