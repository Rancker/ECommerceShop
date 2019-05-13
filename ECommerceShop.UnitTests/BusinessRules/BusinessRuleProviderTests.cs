using ECommerceShop.Core.BusinessRules;
using ECommerceShop.Core.Services;
using ECommerceShop.Domain.Enums;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceShop.UnitTests.BusinessRules
{
    [TestFixture()]
    public class BusinessRuleProviderTests
    {
        private readonly Mock<IShippingService> _shippingServiceMock;
        private readonly Mock<IMembershipService> _membershipServiceMock;

        public BusinessRuleProviderTests()
        {
            _membershipServiceMock = new Mock<IMembershipService>();
            _shippingServiceMock = new Mock<IShippingService>();
        }

        [Test]
        public void Should_Return_Physical_Product_Rule_When_Physical_Product_Ordered()
        {
            //Given
            var businessRuleProvider = new BusinessRuleProvider(GetBusinessRules(_shippingServiceMock.Object, _membershipServiceMock.Object));

            //When
            var businessRules = businessRuleProvider.GetBusinessRules(ProductType.Book);

            //Then
            var actualType = businessRules.FirstOrDefault().GetType();
            Assert.AreEqual(typeof(PhysicalProductRule), actualType);

        }

        [Test]
        public void Should_Return_Membership_Rule_When_Membership_Product_Ordered()
        {
            //Given
            var businessRuleProvider = new BusinessRuleProvider(GetBusinessRules(_shippingServiceMock.Object, _membershipServiceMock.Object));

            //When
            var businessRules = businessRuleProvider.GetBusinessRules(ProductType.VideoClub);

            //Then
            var actualType = businessRules.FirstOrDefault().GetType();
            Assert.AreEqual(typeof(MembershipRule), actualType);
        }

        [Test]
        public void Should_Return_Membership_Rule_When_Premium_Membership_Product_Ordered()
        {
            //Given
            var businessRuleProvider = new BusinessRuleProvider(GetBusinessRules(_shippingServiceMock.Object, _membershipServiceMock.Object));

            //When
            var businessRules = businessRuleProvider.GetBusinessRules(ProductType.PremiumClub);

            //Then
            var actualType = businessRules.FirstOrDefault().GetType();
            Assert.AreEqual(typeof(MembershipRule), actualType);
        }

        private Dictionary<ProductType, IEnumerable<IBusinessRule>> GetBusinessRules(IShippingService shippingService, IMembershipService membershipService)
        {
            var businessRules = new Dictionary<ProductType, IEnumerable<IBusinessRule>>
            {
                { ProductType.Book, new[] { new PhysicalProductRule(shippingService) } },
                { ProductType.Video, new[] { new PhysicalProductRule(shippingService) } },
                { ProductType.BookClub, new[] { new MembershipRule(membershipService), } },
                { ProductType.VideoClub, new[] { new MembershipRule(membershipService) } },
                { ProductType.PremiumClub, new[] { new MembershipRule(membershipService) } }
            };

            return businessRules;
        }
    }
}
