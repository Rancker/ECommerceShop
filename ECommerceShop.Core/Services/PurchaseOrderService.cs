using ECommerceShop.Core.BusinessRules;
using ECommerceShop.Domain.Entities;
using System;
using System.Linq;

namespace ECommerceShop.Core.Services
{
    public class PurchaseOrderService
    {
        private readonly IBusinessRuleProvider _businessRuleProvider;

        public PurchaseOrderService(IBusinessRuleProvider businessRuleProvider)
        {
            _businessRuleProvider = businessRuleProvider;
        }

        public void Process(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null)
            { throw new ArgumentNullException("order cannot be null");}

            if (purchaseOrder.LineItems == null)
            { throw new ArgumentNullException("LineItems cannot be null");}

            foreach (var lineItem in purchaseOrder.LineItems)
            {
                var businessRules = _businessRuleProvider.GetBusinessRules(lineItem.ProductType).ToList();
                if (businessRules.Any())
                {
                    businessRules.ForEach(x => x.Apply(purchaseOrder, lineItem));
                }
            }
        }
    }
}
