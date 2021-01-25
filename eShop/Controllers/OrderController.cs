using System;
using System.Linq;
using System.Threading.Tasks;
using eShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShopContext _db;

        public OrderController(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        [Authorize(Roles = "admin")] 
        [HttpGet]
        public IActionResult Index()
        {
           return  View(_db.Orders.Include(b => b.Product));
        }
      
    }
}