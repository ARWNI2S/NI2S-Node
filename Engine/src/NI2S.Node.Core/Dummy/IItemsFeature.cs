using System.Collections.Generic;

namespace NI2S.Node.Dummy
{
    internal interface IItemsFeature
    {
        IDictionary<object, object> Items { get; set; }
    }
}