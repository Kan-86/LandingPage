using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AcmeCorporation.Areas.Identity.Data
{
    public class ProductSerialNumbers
    {
        public int Id { get; set; }
        [Display(Name = "Serialnumber")]
        public Guid ProductSerialNumber { get; set; }
        public int ProductsId { get; set; }
        [Display(Name ="Valid Serial")]
        public bool ValidForLottery { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}