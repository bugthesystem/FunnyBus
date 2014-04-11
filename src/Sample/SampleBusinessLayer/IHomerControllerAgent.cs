using System.Collections.Generic;
using Sample.BusinessLayer.Models;

namespace Sample.BusinessLayer
{
    public interface IHomerControllerAgent
    {
        List<SampleItem> Get(LoadItemsModel model);
    }
}