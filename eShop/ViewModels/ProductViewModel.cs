using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ViewModels
{
    public class ProductViewModel
    {
      
        [Required(ErrorMessage = "Поле название обязательно для заполнения")]
   
      //  [Remote("NameLength","Validation", ErrorMessage = "Название должно быть длинее двух символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле цена обязательно для заполнения")]
       // [Remote("CheckPrice","Validation",ErrorMessage = "Цена должна быть больше 0 единиц")]
        public decimal Price { get; set; }
    

        public IFormFile File   { get; set; }
       
    }
}