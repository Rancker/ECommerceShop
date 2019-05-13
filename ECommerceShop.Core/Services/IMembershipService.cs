using ECommerceShop.Domain.Entities;

namespace ECommerceShop.Core.Services
{
    public interface IMembershipService
    {
        void ActivateMembership(long customerId, LineItem lineItem);
    }
}