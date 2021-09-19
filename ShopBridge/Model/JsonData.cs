using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Model
{
    public class JsonData
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Byte[]> Images { get; set; }
    }
}
