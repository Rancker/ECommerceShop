using System.Collections.Generic;

namespace ECommerceShop.Domain.Entities
{
    public class PurchaseOrder
    {
        public long PurchaseOrderId { get; set; }

        public long CustomerId { get; set; }

        public decimal TotalPrice { get; set; }

        public IEnumerable<LineItem> LineItems { get; set; }
    }
}