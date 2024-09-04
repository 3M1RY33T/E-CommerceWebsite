using CaseStudy.DAL.DomainClasses;

namespace CaseStudy.Helpers
{
    public class OrderSelectionHelper
    {
        public int Qty { get; set; }
        public OrderLineItem? Item { get; set; }
    }
}
