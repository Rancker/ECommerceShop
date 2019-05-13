using ECommerceShop.Core.Services;
using ECommerceShop.Domain.Entities;

namespace ECommerceShop.Core.BusinessRules
{
    public class PhysicalProductRule : IBusinessRule
    {
        private readonly IShippingService _shippingService;

        public PhysicalProductRule(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        public void Apply(PurchaseOrder purchaseOrder, LineItem lineItem)
        {
            _shippingService.GenerateShippingSlip(purchaseOrder.PurchaseOrderId, lineItem);
        }
    }
}