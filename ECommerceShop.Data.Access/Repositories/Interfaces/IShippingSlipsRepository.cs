using ECommerceShop.Domain.Entities;
using System.Collections.Generic;

namespace ECommerceShop.Data.Access.Repositories.Interfaces
{
    public interface IShippingSlipsRepository
    {
        void AddShippingSlip(long purchaseOrderId, ShippingSlip shippingSlip);
        void UpdateShippingSlip(long shippingSlipId, LineItem lineItem);
        IEnumerable<ShippingSlip> GetShippingSlip(long purchaseOrderId);
    }
}