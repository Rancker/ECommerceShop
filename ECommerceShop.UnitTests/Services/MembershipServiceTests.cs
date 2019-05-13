using ECommerceShop.Core.Services;
using ECommerceShop.Data.Access.Repositories.Interfaces;
using ECommerceShop.Domain.Entities;
using ECommerceShop.Domain.Enums;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceShop.UnitTests.Services
{
    [TestFixture]
    public class MembershipServiceTests
    {
        private readonly Mock<IMembershipRepository> _membershipRepository;

        public MembershipServiceTests()
        {
            _membershipRepository = new Mock<IMembershipRepository>();
        }

        [Test]
        public void Should_Throw_Exception_When_Line_Item_Is_Not_Provided()
        {
            //Given
            var membershipService = new MembershipService(_membershipRepository.Object);

            //Then
            Assert.Throws<ArgumentNullException>(() => membershipService.ActivateMembership(1234, null));
        }

        [TestCase(ProductType.BookClub)]
        [TestCase(ProductType.VideoClub)]
        public void Should_Activate_The_Membership_When_Membership_Product_Ordered(ProductType productType)
        {
            //Given
            var customerId = 111134;
            var membershipService = new MembershipService(_membershipRepository.Object);
            var lineItem = new LineItem { ProductId = 12345, ProductType = productType };

            //When
            membershipService.ActivateMembership(customerId, lineItem);

            //Then
            _membershipRepository.Verify(c => c.AddMembership(customerId, It.IsAny<IEnumerable<Membership>>()));
        }
        [Test]
        public void Should_Activate_The_Membership__For_Both_Club_When_Premium_Membership_Ordered()
        {
            //Given
            var customerId = 111136;
            var membershipService = new MembershipService(_membershipRepository.Object);
            var lineItem = new LineItem { ProductId = 12347, ProductType = ProductType.PremiumClub };

            //When
            membershipService.ActivateMembership(customerId, lineItem);

            //Then
            _membershipRepository.Verify(
                c => c.AddMembership(
                                        customerId,
                                        It.Is<IEnumerable<Membership>>(
                                        x => x.FirstOrDefault().MembershipType == ProductType.BookClub &&
                                        x.LastOrDefault().MembershipType == ProductType.VideoClub))
            );
        }
    }
}
