using ECommerceShop.Core.Services;
using ECommerceShop.Data.Access.Repositories.Interfaces;
using ECommerceShop.Domain.Entities;
using ECommerceShop.Domain.Enums;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace ECommerceShop.UnitTests.Services
{
    [TestFixture()]
    public class ShippingServiceTests
    {
        private readonly Mock<IShippingSlipsRepository> _shippingSlipsRepositoryMock;

        public ShippingServiceTests()
        {
            _shippingSlipsRepositoryMock = new Mock<IShippingSlipsRepository>();
        }

        [Test]
        public void Should_Throw_Exception_When_Line_Item_Is_Not_Provided()
        {
            //Given
            var shippingService = new ShippingService(_shippingSlipsRepositoryMock.Object);

            //Then
            Assert.Throws<ArgumentNullException>(() => shippingService.GenerateShippingSlip(1234, null));
        }

        [Test]
        public void Should_Generate_The_Shipping_Slip_When_Physical_Product_Ordered()
        {
            //Given
            var purchaseOrderId = 66666;
            var shippingService = new ShippingService(_shippingSlipsRepositoryMock.Object);
            var lineItem = new LineItem
            {
                ProductType = ProductType.Book,
                ProductId = 2222,
                Title = "The Girl on the train"
            };

            //When
            shippingService.GenerateShippingSlip(purchaseOrderId, lineItem);

            //Then
            _shippingSlipsRepositoryMock.Verify(c => c.AddShippingSlip(purchaseOrderId, It.IsAny<ShippingSlip>()));
        }

        [Test]
        public void Should_Update_The_Shipping_Slip_When_Shipping_Slip_Already_Exists()
        {
            //Given
            var purchaseOrderId = 77778;
            var shippingService = new ShippingService(_shippingSlipsRepositoryMock.Object);
            var lineItem = new LineItem
            {
                ProductType = ProductType.Book,
                ProductId = 4444,
                Title = "The Girl on the train"
            };
            var shippingSlipId = 8888;

            var shippingSlips = new[] { new ShippingSlip { ShippingSlipId = shippingSlipId, ProductId = new[] { lineItem.ProductId } } };
            _shippingSlipsRepositoryMock.Setup(p => p.GetShippingSlip(purchaseOrderId))
                .Returns(shippingSlips);
            //When
            shippingService.GenerateShippingSlip(purchaseOrderId, lineItem);

            //Then
            _shippingSlipsRepositoryMock.Verify(c => c.UpdateShippingSlip(shippingSlipId, It.IsAny<LineItem>()));
            _shippingSlipsRepositoryMock.Verify(c => c.AddShippingSlip(purchaseOrderId, It.IsAny<ShippingSlip>()), Times.Never);
        }
    }
}
