using ECommerceShop.Domain.Entities;

namespace ECommerceShop.Core.BusinessRules
{
    public interface IBusinessRule
    {
        void Apply(PurchaseOrder purchaseOrder, LineItem lineItem);
    }
}