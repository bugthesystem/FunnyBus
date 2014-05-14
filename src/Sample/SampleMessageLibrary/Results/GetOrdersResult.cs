using System.Collections.Generic;

namespace Sample.Contracts.Results
{
    public class GetOrdersResult
    {
        public List<OrderItemModel> Orders { get; set; }
    }
}