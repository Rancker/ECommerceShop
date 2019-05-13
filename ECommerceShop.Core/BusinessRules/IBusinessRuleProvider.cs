using ECommerceShop.Domain.Enums;
using System.Collections.Generic;

namespace ECommerceShop.Core.BusinessRules
{
    public interface IBusinessRuleProvider
    {
        IEnumerable<IBusinessRule> GetBusinessRules(ProductType productType);
    }
}