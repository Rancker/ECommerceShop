using ECommerceShop.Core.BusinessRules;
using ECommerceShop.Core.Services;
using ECommerceShop.Domain.Entities;
using ECommerceShop.Domain.Enums;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ECommerceShop.UnitTests.Services
{
    [TestFixture]
    public class PurchaseOrderServiceTests
    {
        private readonly IBusinessRuleProvider _businessRuleProvider;
        private readonly Mock<IShippingService> _shippingServiceMock;
        private readonly Mock<IMembershipService> _membershipServiceMock;

        public PurchaseOrderServiceTests()
        {
            _membershipServiceMock = new Mock<IMembershipService>();
            _shippingServiceMock = new Mock<IShippingService>();
            _businessRuleProvider = new BusinessRuleProvider(GetBusinessRules(_shippingServiceMock.Object, _membershipServiceMock.Object));
        }

        [Test]
        public void Should_Throw_Exception_When_Purchase_Order_Is_Not_Provided()
        {
            //Given
            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //Then
            Assert.Throws<ArgumentNullException>(() => purchaseOrderService.Process(null));
        }

        [Test]
        public void Should_Throw_Exception_When_Purchase_Order_Does_Not_Contain_Any_Line_Item()
        {
            //Given
            var customerId = 11119;
            var purchaseOrderId = 22229;
            var totalPrice = 1111.22m;
            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //Then
            Assert.Throws<ArgumentNullException>(() => purchaseOrderService.Process(CreatePurchaseOrder(customerId, purchaseOrderId, totalPrice)));
        }

        [Test]
        public void Should_Activate_Membership_When_Membership_Ordered()
        {
            //Given
            var customerId = 11112;
            var purchaseOrderId = 22222;
            var totalPrice = 1111.22m;
            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //When
            purchaseOrderService.Process(CreatePurchaseOrderWithMembership(customerId, purchaseOrderId, totalPrice));

            //Then
            _membershipServiceMock.Verify(c => c.ActivateMembership(customerId, It.IsAny<LineItem>()));
        }

        [Test]
        public void Should_Not_Activate_Membership_When_Physical_Products_Ordered()
        {
            //Given
            var customerId = 22223;
            var purchaseOrderId = 33333;
            var totalPrice = 2222.33m;
            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //When
            purchaseOrderService.Process(CreatePurchaseOrderWithPhysicalProduct(customerId, purchaseOrderId, totalPrice));
            //Then
            _membershipServiceMock.Verify(c => c.ActivateMembership(customerId, It.IsAny<LineItem>()), Times.Never);
        }
        [Test]
        public void Should_Generate_Shipping_Slip_When_Physical_Products_Ordered()
        {
            //Given
            var customerId = 33334;
            var purchaseOrderId = 44444;
            var totalPrice = 3333.44m;
            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //When
            purchaseOrderService.Process(CreatePurchaseOrderWithPhysicalProduct(customerId, purchaseOrderId, totalPrice));

            //Then
            _shippingServiceMock.Verify(c => c.GenerateShippingSlip(purchaseOrderId, It.IsAny<LineItem>()), Times.Once);
        }

        [Test]
        public void Should_Not_Generate_Shipping_Slip_When_Membership_Product_Ordered()
        {
            //Given
            var customerId = 44445;
            var purchaseOrderId = 55555;
            var totalPrice = 4444.55m;
            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //When
            purchaseOrderService.Process(CreatePurchaseOrderWithMembership(customerId, purchaseOrderId, totalPrice));

            //Then
            _shippingServiceMock.Verify(c => c.GenerateShippingSlip(purchaseOrderId, It.IsAny<LineItem>()), Times.Never);
        }


        [Test]
        public void Should_Generate_Shipping_Slip_And_Activate_Membership_When_Both_Products_Ordered()
        {
            //Given 
            var customerId = 55556;
            var purchaseOrderId = 66666;
            var totalPrice = 5555.66m;

            var purchaseOrderService = new PurchaseOrderService(_businessRuleProvider);

            //When
            purchaseOrderService.Process(CreatePurchaseOrderWithBothPhysicalProductAndMembership(customerId, purchaseOrderId, totalPrice));

            //Then
            _shippingServiceMock.Verify(c => c.GenerateShippingSlip(purchaseOrderId, It.IsAny<LineItem>()), Times.Exactly(2));
            _membershipServiceMock.Verify(c => c.ActivateMembership(customerId, It.IsAny<LineItem>()));

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

        private PurchaseOrder CreatePurchaseOrderWithMembership(long customerId, long purchaseOrderId, decimal totalPrice)
        {
            var purchaseOrder = CreatePurchaseOrder(customerId, purchaseOrderId, totalPrice);
            var products = new List<LineItem> { LineItemWithMembership() };
            purchaseOrder.LineItems = products;

            return purchaseOrder;
        }

        private PurchaseOrder CreatePurchaseOrderWithPhysicalProduct(long customerId, long purchaseOrderId, decimal totalPrice)
        {
            var purchaseOrder = CreatePurchaseOrder(customerId, purchaseOrderId, totalPrice);
            var products = new List<LineItem> { LineItemWithPhysicalProductBook() };
            purchaseOrder.LineItems = products;

            return purchaseOrder;
        }

        private PurchaseOrder CreatePurchaseOrderWithBothPhysicalProductAndMembership(long customerId, long purchaseOrderId, decimal totalPrice)
        {
            var purchaseOrder = CreatePurchaseOrder(customerId, purchaseOrderId, totalPrice);
            var products = new List<LineItem> { LineItemWithPhysicalProductBook(), LineItemWithPhysicalProductVideo(), LineItemWithMembership() };
            purchaseOrder.LineItems = products;

            return purchaseOrder;
        }

        private PurchaseOrder CreatePurchaseOrder(long customerId, long purchaseOrderId, decimal totalPrice)
        {
            return new PurchaseOrder
            {
                CustomerId = customerId,
                PurchaseOrderId = purchaseOrderId,
                TotalPrice = totalPrice
            };
        }

        private LineItem LineItemWithMembership()
        {
            return new LineItem { ProductType = ProductType.BookClub, ProductId = 1111 };
        }

        private LineItem LineItemWithPhysicalProductBook()
        {
            return new LineItem { ProductType = ProductType.Book, ProductId = 2222, Title = "The Girl on the train" };
        }

        private LineItem LineItemWithPhysicalProductVideo()
        {
            return new LineItem { ProductType = ProductType.Video, ProductId = 3333, Title = "Comprehensive First Aid Training" };
        }
    }
}

