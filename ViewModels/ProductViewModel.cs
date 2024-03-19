using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<ProductImageViewModel> ProductImages { get; set; } = new List<ProductImageViewModel>();
    }

}
