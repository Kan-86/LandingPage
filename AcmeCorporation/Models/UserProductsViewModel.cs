using AcmeCorporation.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Models
{
    public class UserProductsViewModel
    {
        public List<UserProfile> Users { get; set; }
        public SelectList LastName { get; set; }
        public string UserLastNames { get; set; }
        public string SearchString { get; set; }
    }
}
