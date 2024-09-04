using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudy.DAL.DomainClasses
{
    public class Product
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string? ProductName { get; set; }
        public string? GraphicName { get; set; }
        
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        
        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyOnBackOrder { get; set; }
        [StringLength(2000,ErrorMessage = "Description should not exceed 2000 chars")]
        public string? Description { get; set; }
    }
}
