using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementSystem.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Preis")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Menge")]
        public int Quantity {  get; set; }
        public DateTime CreationDate { get; set; }

        public List<ProductImage> ProductImages{ get; set; }

    }

}
