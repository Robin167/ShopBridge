using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ShopBridge.Entities
{
    public partial class ImageEntity
    {
        [Key]
        public int ImageId { get; set; }
        public int ItemId { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }

        public virtual ItemEntity Item { get; set; }
    }
}
