
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Model
{
    public class Item
    {
        [Required]
        
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Brand { get; set; }

        
        public DateTime? ExpiryDate { get; set; }

        [Required]
        public string ImageFolderPath { get; set; }

        public List<byte[]> ImageData { get; set; }
    }

    
}
