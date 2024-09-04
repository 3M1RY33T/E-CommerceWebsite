namespace CaseStudy.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int QtyOrdered { get; set; }
        public int QtySold { get; set; }
        public int QtyBackOrdered { get; set; }
        public string? Description { get; set; }
        public int CustomerId { get; set; }
        public string? OrderDate { get; set; }
        public decimal CostPrice { get; set; }
    }
}
