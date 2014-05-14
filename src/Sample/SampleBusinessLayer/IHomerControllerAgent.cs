using System.Collections.Generic;
using Sample.BusinessLayer.Models;

namespace Sample.BusinessLayer
{
    public interface IHomerControllerAgent
    {
        List<SampleItem> GetOrders(GetOrdersModel model);
    }
}