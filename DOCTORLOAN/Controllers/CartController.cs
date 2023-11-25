using DOCTORLOAN.Models.Contents;
using DOCTORLOAN.Models.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       /* public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }*/

        /*public async Task<IActionResult> AddToCart(int id, int SoLuong, string type = "Normal")
        {
            var _order = Order;
            var item = _order.SingleOrDefault(p => p.asidasd == id);

            HttpContent _content = new StringContent(jsonData, Encoding.UTF8, "application/json");

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
        }*/

        public IActionResult Payment()
        {
            return View();
        }
    }
}
