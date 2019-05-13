using ECommerceShop.Domain.Enums;

namespace ECommerceShop.Domain.Entities
{
    public class Membership
    {
        public long MembershipId { get; set; }

        public long CustomerId { get; set; }

        public bool Active { get; set; }

        public ProductType MembershipType { get; set; }
    }
}