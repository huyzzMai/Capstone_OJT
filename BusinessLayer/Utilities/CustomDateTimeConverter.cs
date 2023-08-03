using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utilities
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "dd/MM/yyyy";
        }
    }
}
