using System.IO;
using System.Threading.Tasks;
using eShop.Models;
using eShop.Services;
using eShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace eShop.Controllers
{
    public class ProductController : Controller
    {
        private ShopContext _db;
        private readonly FileUploadService _uploadServices;
        private IHostEnvironment _environment;
        public ProductController(ShopContext db, FileUploadService uploadServices, IHostEnvironment environment)
        {
            _db = db;
            _uploadServices = uploadServices;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
          
            return View(  _db.Products);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult>  Add(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot\\images\\");
               Product product =new Product();
                if (model.File !=null)
                {
                    string photoPath = $"images/{model.File.FileName}";  
                    _uploadServices.Upload(path, model.File.FileName,model.File);
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
    }
}