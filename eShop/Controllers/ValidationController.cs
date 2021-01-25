using System;
using System.Linq;
using eShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ShopContext _db;
        public ValidationController(ShopContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        [AcceptVerbs("GET", "POST")]
        public bool CheckPrice(decimal price)
        {
            return price > 0;
        }
        
        [AcceptVerbs("GET", "POST")]
        public bool NameLength(string name)
        {
            return name.Length >= 3;
        }
       
        public bool CheckRegisterEmail(string email)
        {
            return _db.Users.Any(b => b.Email.ToUpper() == email.ToUpper());
        }
    }
}