using ECommerceShop.Domain.Enums;
using System.Collections.Generic;

namespace ECommerceShop.Core.BusinessRules
{
    public class BusinessRuleProvider : IBusinessRuleProvider
    {
        private readonly Dictionary<ProductType, IEnumerable<IBusinessRule>> _businessRules;

        public BusinessRuleProvider(Dictionary<ProductType, IEnumerable<IBusinessRule>> businessRules)
        {
            _businessRules = businessRules;
        }

        public IEnumerable<IBusinessRule> GetBusinessRules(ProductType productType)
        {
            return _businessRules[productType];
        }
    }
}