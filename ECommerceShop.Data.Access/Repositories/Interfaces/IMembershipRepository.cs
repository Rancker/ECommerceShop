using ECommerceShop.Domain.Entities;
using System.Collections.Generic;

namespace ECommerceShop.Data.Access.Repositories.Interfaces
{
    public interface IMembershipRepository
    {
        void AddMembership(long customerId, IEnumerable<Membership> memberships);
    }
}