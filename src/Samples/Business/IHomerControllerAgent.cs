using System.Collections.Generic;
using Sample.Business.Models;

namespace Sample.Business
{
    public interface IHomerControllerAgent
    {
        List<SampleItem> GetShoppingCart(GetShoppingCartFormModel formModel);
    }
}