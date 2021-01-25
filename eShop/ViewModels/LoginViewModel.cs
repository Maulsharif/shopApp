using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ViewModels
{
    public class LoginViewModel
    {
        [Remote("CheckRegisterEmail","Validation",ErrorMessage = "Email не заргеистрирован")]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }
   
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
            
    }

    
}