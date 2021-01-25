using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels
{
    public class OrderViewModel
    {
        [Required (ErrorMessage = "Поле объязательно для заполнения")]
        public string CustomerName { get; set; }
        [Required (ErrorMessage = "Поле объязательно для заполнения")]
        public string Address { get; set; }
        [Required (ErrorMessage = "Поле объязательно для заполнения")]
        public string PhoneNumber { get; set; }
       
    }
}