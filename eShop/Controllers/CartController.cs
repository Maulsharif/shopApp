using System.Collections.Generic;
using System.Linq;
using eShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers
{
    public class CartController : Controller
    {
        private ShopContext _context;

        public CartController(ShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            if (cart != null)
            {
                  ViewBag.cart = cart;
                            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            }
            else
            {
                ViewBag.cart = new List<Item>{};

            }
          
            return View();
        }

       [HttpGet]
        public IActionResult AddToCart(int id)
        {
            List<Item> cart = new List<Item>();
          
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (HttpContext.Session.GetObjectFromJson<List<Item>>("cart") == null)
            {
               
                
               
                cart.Add(new Item { Product = product, Quantity = 1 });
                HttpContext.Session.SetObjectAsJson("cart", cart);
            }
            else
            {
                 cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = product, Quantity = 1 });
                }
                HttpContext.Session.SetObjectAsJson("cart", cart);
            }

            return Ok(cart.Sum(p=>p.Quantity));
        }

      
        public IActionResult Remove(int id)
        {
            List<Item> cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            HttpContext.Session.SetObjectAsJson("cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        
        public int CartState()
        {
            List<Item> cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            if (cart.Count > 0)
            {
                return cart.Sum(p => p.Quantity);
            }
            
            return 0;
        }
        
    }
}