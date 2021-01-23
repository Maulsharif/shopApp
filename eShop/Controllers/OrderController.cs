using System.Linq;
using System.Threading.Tasks;
using eShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers
{
    public class OrderController : Controller
    {
        private ShopContext _db;

        public OrderController(ShopContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
           return  View(_db.Orders);
        }
        public IActionResult Create()
        {
            return  View();
        }
        
        
        [HttpPost]
        public async Task<ActionResult>  Create(Order order)
        {
            if (order != null)
            {
               await _db.Orders.AddAsync(order);
               await  _db.SaveChangesAsync();
            }
            return  View(order);
        }
    }
}