using ECommerceShop.Domain.Enums;

namespace ECommerceShop.Domain.Entities
{
    public class LineItem
    {
        public long ProductId { get; set; }

        public decimal Price { get; }

        public string Title { get; set; }

        public ProductType ProductType { get; set; }

    }
}
