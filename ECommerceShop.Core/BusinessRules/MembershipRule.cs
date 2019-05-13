using ECommerceShop.Core.Services;
using ECommerceShop.Domain.Entities;

namespace ECommerceShop.Core.BusinessRules
{
    public class MembershipRule : IBusinessRule
    {
        private readonly IMembershipService _membershipService;

        public MembershipRule(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public void Apply(PurchaseOrder purchaseOrder, LineItem lineItem)
        {
            _membershipService.ActivateMembership(purchaseOrder.CustomerId, lineItem);
        }
    }
}