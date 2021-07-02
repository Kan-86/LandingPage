using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Areas.Identity.Data
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Display(Name = "First name")]
        [StringLength(25, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        [StringLength(25, MinimumLength = 3)]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Day of Birth")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime BirthDate { get; set; }
        public User User { get; set; }
        [Display(Name = "Admin")]
        [Required]
        public bool IsAdmin { get; set; }
        [Display(Name = "Serial Number")]
        [StringLength(40, MinimumLength = 30)]
        public List<Product> UserProducts { get; set; }
        public UserInDraw UserInDraw { get; set; }
        public int? UserInDrawId { get; set; }
    }
}
