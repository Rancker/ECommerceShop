using ECommerceShop.Domain.Entities;

namespace ECommerceShop.Core.Services
{
    public interface IShippingService
    {
        void GenerateShippingSlip( long purchaseOrderId, LineItem lineItem);
    }
}