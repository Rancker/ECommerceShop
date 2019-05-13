using ECommerceShop.Data.Access.Repositories.Interfaces;
using ECommerceShop.Domain.Entities;
using ECommerceShop.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ECommerceShop.Core.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public void ActivateMembership(long customerId, LineItem lineItem)
        {
            if (lineItem == null)
            { throw new ArgumentNullException("LineItems cannot be null"); }

            if (lineItem.ProductType == ProductType.PremiumClub)
            {
                _membershipRepository.AddMembership(customerId, GetPremiumMemberships(customerId));
                return;
            }
            _membershipRepository.AddMembership(customerId, new[] { CreateMembership(customerId, lineItem.ProductType) });
        }

        private IEnumerable<Membership> GetPremiumMemberships(long customerId)
        {
            var memberships = new List<Membership>
            {
                CreateMembership(customerId, ProductType.BookClub),
                CreateMembership(customerId, ProductType.VideoClub)
            };

            return memberships;
        }

        private Membership CreateMembership(long customerId, ProductType productType)
        {
            return new Membership { Active = true, CustomerId = customerId, MembershipType = productType };
        }
    }
}