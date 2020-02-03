using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XYZOnline.BusinessLogic
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemStatus
    {
        OutOfStock,
        Instock,
        Unknown
    }
}
