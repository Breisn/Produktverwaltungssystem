using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using ProductManagementSystem.ViewModels;

namespace ProductManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products/Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                CreationDate = product.CreationDate,
                ProductImages = product.ProductImages.Select(img => new ProductImageViewModel
                {
                    ImagePath = $"/Pictures/{product.ProductID}_{product.Name.Replace(" ", "_")}/{Path.GetFileName(img.ImagePath)}"
                }).ToList()
            };

            return View(viewModel);
        }

        // GET: Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ProductViewModel();
            return View(viewModel);
        }
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var product = new Product
                {
                    Name = viewModel.Name,
                    Price = viewModel.Price,
                    Quantity = viewModel.Quantity,
                    CreationDate = DateTime.Now,
                    ProductImages = new List<ProductImage>()
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                int productId = product.ProductID;

                if (viewModel.Images != null && viewModel.Images.Any())
                {
                    foreach (var image in viewModel.Images)
                    {
                        // Produkt-ID an die SaveImageAsync-Methode übergeben
                        var imagePath = await SaveImageAsync(image, productId, product.Name);
                        product.ProductImages.Add(new ProductImage { ImagePath = imagePath });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Erstellen eines Produkts.");
                ModelState.AddModelError("", "Ein Fehler ist beim Erstellen des Produkts aufgetreten.");
                return View(viewModel);
            }
        }

        private async Task<string> SaveImageAsync(IFormFile image, int productId, string productName)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Ungültige Produkt-ID");
            }

            // Verzeichnispfad für das Bild erstellen
            var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Pictures", $"{productId}_{productName.Replace(" ", "_")}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Zähler für Dateinamen initialisieren
            int counter = 1;
            var fileName = $"{productId}_{productName.Replace(" ", "_")}_{counter}.jpeg";

            // Überprüfen, ob die Datei bereits vorhanden ist, und den Zähler inkrementieren
            while (System.IO.File.Exists(Path.Combine(directoryPath, fileName)))
            {
                counter++;
                fileName = $"{productId}_{productName.Replace(" ", "_")}_{counter}.jpeg";
            }

            // Pfad für die Bilddatei erstellen
            var filePath = Path.Combine(directoryPath, fileName);

            // Bild in das Dateiverzeichnis kopieren
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Rückgabe des relativen Dateipfads für das Bild
            return Path.Combine("/Pictures", $"{productId}_{productName.Replace(" ", "_")}", fileName);
        }



        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Bearbeitungsansicht wurde mit einer null-ID aufgerufen.");
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                _logger.LogWarning($"Produkt mit ID {id} für Bearbeitung nicht gefunden.");
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                ProductImages = product.ProductImages.Select(img => new ProductImageViewModel
                {
                    ImagePath = $"/Pictures/{product.ProductID}_{product.Name.Replace(" ", "_")}/{Path.GetFileName(img.ImagePath)}"
                }).ToList()
            };

            return View(viewModel);
        }

       
        // POST: Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel viewModel, List<IFormFile> images)
        {
            if (id != viewModel.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var productToUpdate = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.ProductID == id);

                if (productToUpdate == null)
                {
                    return NotFound();
                }

                productToUpdate.Name = viewModel.Name;
                productToUpdate.Price = viewModel.Price;
                productToUpdate.Quantity = viewModel.Quantity;

                if (images != null && images.Any())
                {
                    _context.ProductImages.RemoveRange(productToUpdate.ProductImages);
                    await _context.SaveChangesAsync();

                    foreach (var image in images)
                    {
                        var imagePath = await SaveImageAsync(image, productToUpdate.ProductID, productToUpdate.Name);
                        productToUpdate.ProductImages.Add(new ProductImage { ProductId = productToUpdate.ProductID, ImagePath = imagePath });
                    }
                }

                try
                {
                    _context.Update(productToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Fehler beim Aktualisieren des Produkts.");
                    ModelState.AddModelError("", "Ein Fehler ist beim Aktualisieren des Produkts aufgetreten.");
                }
            }

            return View(viewModel);
        }


        // GET: Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Produktbilder löschen
            var productImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Pictures", $"{product.ProductID}_{product.Name.Replace(" ", "_")}");
            if (Directory.Exists(productImagePath))
            {
                Directory.Delete(productImagePath, true); 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }



        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
