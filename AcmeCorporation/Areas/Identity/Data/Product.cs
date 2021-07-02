using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Areas.Identity.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string ProductName { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public double Price { get; set; }
        public int ProductSerialNumberId { get; set; }
        [Display(Name = "Serial Number")]
        public ProductSerialNumbers ProductSerialNumber { get; set; }
        public int? UserProfileId { get; set; }
        [Display(Name = "Product Owner")]
        public UserProfile UserProfile { get; set; }
    }
}
