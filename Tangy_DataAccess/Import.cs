using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_DataAccess
{
    public class Import
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Import Quantity is required.")]
        public int ImportQuantity { get; set; }
        [Required(ErrorMessage = "Unit Price is required.")]
        public double UnitPrice { get; set; }
        [Required(ErrorMessage = "Date Added is required.")]
        public DateTime DateAdded { get; set; }
        [Required(ErrorMessage = "Product is required.")]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
