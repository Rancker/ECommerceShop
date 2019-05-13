using System.Collections.Generic;

namespace ECommerceShop.Domain.Entities
{
    public class ShippingSlip
    {
        public long ShippingSlipId { get; set; }

        public long PurchaseOrderId { get; set; }

        public string AddressDetails { get; set; }

        public string CustomerName { get; set; }

        public IEnumerable<long> ProductId { get; set; }
    }
}