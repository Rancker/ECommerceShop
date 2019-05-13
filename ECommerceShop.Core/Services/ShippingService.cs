using ECommerceShop.Data.Access.Repositories.Interfaces;
using ECommerceShop.Domain.Entities;
using System;
using System.Linq;

namespace ECommerceShop.Core.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingSlipsRepository _shippingSlipsRepository;

        public ShippingService(IShippingSlipsRepository shippingSlipsRepository)
        {
            _shippingSlipsRepository = shippingSlipsRepository;
        }

        public void GenerateShippingSlip(long purchaseOrderId, LineItem lineItem)
        {
            if (lineItem == null)
            { throw new ArgumentNullException("LineItems cannot be null"); }

            var existingShippingSlips = _shippingSlipsRepository.GetShippingSlip(purchaseOrderId);

            if (!existingShippingSlips.ToList().Any())
            {
                var shippingSlip = new ShippingSlip
                {
                    PurchaseOrderId = purchaseOrderId,
                    ProductId = new[] { lineItem.ProductId }
                };
                _shippingSlipsRepository.AddShippingSlip(purchaseOrderId, shippingSlip);
                return;
            }

            var existingShippingSlipId = existingShippingSlips.FirstOrDefault().ShippingSlipId;
            _shippingSlipsRepository.UpdateShippingSlip(existingShippingSlipId, lineItem);
        }
    }
}