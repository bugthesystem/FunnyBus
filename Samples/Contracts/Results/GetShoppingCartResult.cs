using System.Collections.Generic;

namespace Sample.Contracts.Results
{
    public class GetShoppingCartResult
    {
        public List<CartItem> Orders { get; set; }
    }
}