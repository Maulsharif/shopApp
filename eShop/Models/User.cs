using Microsoft.AspNetCore.Identity;

namespace eShop.Models
{
    public class User : IdentityUser
    { 
        public string Name { get; set; }
    }
}