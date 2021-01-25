using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eShop.Models;
using eShop.Services;
using eShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace eShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopContext _db;
        private  readonly FileUploadService _uploadServices;
        private readonly IHostEnvironment _environment;
        public ProductController(ShopContext db, FileUploadService uploadServices, IHostEnvironment environment)
        {  
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _uploadServices = uploadServices ?? throw new ArgumentNullException(nameof(uploadServices));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
       
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(  _db.Products);
        }
        
        [HttpGet]
        [Authorize(Roles = "admin")] 
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")] 
        public async Task<ActionResult> Add(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot\\images\\");
                Product product = new Product();
                if (model.File != null)
                {
                    string photoPath = $"images/{model.File.FileName}";
                    _uploadServices.Upload(path, model.File.FileName, model.File);
                    product.Image = photoPath;
                }

                product.Name = model.Name;
                product.Price = model.Price;

                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        
        [HttpGet]
        [Authorize(Roles = "admin")] 
        public IActionResult Edit(int id)
        {
            Product product =_db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel model = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
                
           
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")] 
        public async Task<IActionResult> Edit(ProductViewModel model)
        {  Product product = _db.Products.FirstOrDefault(p=>p.Id==model.Id);
            if (ModelState.IsValid && product!=null)
            {     
               
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot\\images\\");
          
                if (model.File != null)
                {
                    string photoPath = $"images/{model.File.FileName}";
                    _uploadServices.Upload(path, model.File.FileName, model.File);
                    product.Image = photoPath;
                }

                product.Name = model.Name;
                product.Price = model.Price;

              
                _db.Attach(product).State = EntityState.Modified;
              await  _db.SaveChangesAsync();
              return RedirectToAction("Index");
            }

            return View(model);
            
        }
        
        [HttpGet]
        public IActionResult Detail(int id)
        {
          Product product =  _db.Products.FirstOrDefault(p => p.Id == id);
          if (product == null)
          {
              return NotFound();
          }
          return View(product);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult>Delete(int id)
        {
            Product product=_db.Products.FirstOrDefault(p=>p.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Entry(product).State = EntityState.Deleted;
             await _db.SaveChangesAsync();
             return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult SearchAjaxResult(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                ViewBag.Error = "Введите имя  для поиска";
                return PartialView("PartialView", _db.Products.ToList());
            }

            search = search.ToUpper();
           var products = _db.Products
                .Where(e => e.Name.Contains(search)).ToList();
            if (products.Count == 0)
            {
                ViewBag.Error = "Совпадений не найдено";
            }

            return PartialView("PartialView", products);
        }
    }
}