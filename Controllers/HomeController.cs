using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using ProductManagementSystem.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace ProductManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string searchOption = null, string searchString = null)
        {
            IQueryable<Product> productsQuery = _context.Products.Include(p => p.ProductImages);

            if (!string.IsNullOrEmpty(searchString))
            {
                if ("ID".Equals(searchOption, System.StringComparison.OrdinalIgnoreCase) && int.TryParse(searchString, out int id))
                {
                    productsQuery = productsQuery.Where(p => p.ProductID == id);
                }
                else if ("Name".Equals(searchOption, System.StringComparison.OrdinalIgnoreCase))
                {
                    productsQuery = productsQuery.Where(p => p.Name.Contains(searchString));
                }
            }

            var productViewModels = await productsQuery.Select(p => new ProductViewModel
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                ProductImages = p.ProductImages.Select(img => new ProductImageViewModel
                {
                    ImagePath = $"/Pictures/{p.ProductID}_{p.Name.Replace(" ", "_")}/{Path.GetFileName(img.ImagePath)}"
                }).ToList()
            }).ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("~/Views/Products/ProductsList.cshtml", productViewModels);
            }
            return View(productViewModels);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
