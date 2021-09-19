using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ShopBridge.Entities
{
    public partial class ItemEntity
    {
        public ItemEntity()
        {
            Images = new HashSet<ImageEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public virtual ICollection<ImageEntity> Images { get; set; }
    }
}
