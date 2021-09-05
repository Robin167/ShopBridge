using System;
using System.Collections.Generic;

#nullable disable

namespace ShopBridge.Entities
{
    public partial class ItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
