namespace ProductManagementSystem.Models
{
    public class ProductImage
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public Product Product { get; set; }
    }

}
