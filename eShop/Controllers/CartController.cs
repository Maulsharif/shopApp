using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop.Models;
using eShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopContext _db;
        public CartController(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        [HttpGet]
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
           var cart = new List<Item>();
           Product product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (HttpContext.Session.GetObjectFromJson<List<Item>>("cart") == null)
            {
                cart.Add(new Item { Product = product, Quantity = 1 });
                HttpContext.Session.SetObjectAsJson("cart", cart);
            }
            else
            {
                 cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
                int index = IsExist(id);
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
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            return PartialView("CartPartialView", cart);
         
        }

      
        public IActionResult Remove(int id)
        {
           var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            int index = IsExist(id);
            if (cart[index].Quantity == 1)
            {
                cart.RemoveAt(index);
            }
            else if (index != -1)
            {
                cart[index].Quantity--;
            }
            
            HttpContext.Session.SetObjectAsJson("cart", cart);
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            return PartialView("CartPartialView", cart);
            
        }

        public void CleanCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            cart.Clear();
            HttpContext.Session.SetObjectAsJson("cart", cart);
        }

        private int IsExist(int id)
        {
           var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
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
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            return cart?.Sum(p => p.Quantity) ?? 0;
        }

        public IActionResult Order()
        {
         var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
         
            if (cart != null)
            {
                ViewBag.Cart = cart;
                ViewBag.Total = cart.Sum(item => item.Product.Price * item.Quantity);
            }
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Order(OrderViewModel order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
            
            if (ModelState.IsValid)
            {
                if (cart != null)
                {
                    for (int i = 0; i < cart.Count; i++)
                    {
                      await _db.Orders.AddAsync(new Order()
                        {
                            CustomerName = order.CustomerName,
                            ProductId = cart[i].Product.Id,
                            Quantity = cart[i].Quantity,
                            PhoneNumber = order.PhoneNumber,
                            Address = order.Address
                        });
                    }
                    
                    await _db.SaveChangesAsync();
                    CleanCart();
                   
                    return RedirectToAction("Index", "Product");
                }
            }
            return View(order);
        }
    }
}