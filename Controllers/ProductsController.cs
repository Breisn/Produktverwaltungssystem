using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchOption, string searchString)
        {
            IQueryable<Product> products = _context.Products;

            bool isFormSubmitted = !String.IsNullOrEmpty(searchString);

            if (isFormSubmitted)
            {
                if (searchOption == "ID" && !int.TryParse(searchString, out _))
                {
                    // Setzen der Fehlermeldung, wenn die ID-Suche ausgewählt wurde und searchString keine gültige Zahl ist
                    ViewBag.ErrorMessage = "Die Suche mithilfe der ID ist nur mit Zahlen möglich!";
                }
                else if (searchOption == "Name")
                {
                    products = products.Where(s => s.Name.Contains(searchString));
                }
                else if (searchOption == "ID")
                {
                    int productId = int.Parse(searchString); // Bereits durch TryParse oben validiert
                    products = products.Where(s => s.ProductID == productId);
                }
            }

            // Keine Fehlermeldung setzen, wenn das Formular nicht abgesendet wurde
            return View(await products.ToListAsync());
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreationDate = DateTime.Now; // Setzen des Erstellungsdatums
                _context.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Produkt {product.Name} wurde erstellt.");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Bearbeitungsansicht wurde mit einer null-ID aufgerufen.");
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Produkt mit ID {id} für Bearbeitung nicht gefunden.");
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Price,Quantity")] Product productModel)
        {
            if (id != productModel.ProductID)
            {
                return NotFound();
            }

            var productToUpdate = await _context.Products.FindAsync(id);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productToUpdate.Name = productModel.Name;
                    productToUpdate.Price = productModel.Price;
                    productToUpdate.Quantity = productModel.Quantity;

                    // Das CreationDate wird hier nicht verändert

                    _context.Update(productToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productModel.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Löschansicht wurde mit einer null-ID aufgerufen.");
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                _logger.LogWarning($"Produkt mit ID {id} zum Löschen nicht gefunden.");
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Produkt {product.Name} wurde gelöscht.");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
