using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProductManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Leitet direkt zur Produktliste im ProductsController um
            _logger.LogInformation("Umleitung von der Startseite zur Produktliste");
            return RedirectToAction("Index", "Products");
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

