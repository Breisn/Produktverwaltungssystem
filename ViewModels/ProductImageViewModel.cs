namespace ProductManagementSystem.ViewModels
{
    public class ProductImageViewModel
    {
        public string ImagePath { get; set; }
        public string FileName => Path.GetFileName(ImagePath);

    }
}
