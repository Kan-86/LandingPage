using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Areas.Identity.Data
{
    public class UserInDraw
    {
        public int Id { get; set; }
        [Display(Name = "User in Draw")]
        public List<UserProfile> UsersInDraw { get; set; }
        [Display(Name = "User has entered")]
        public int HasEnteredAmount { get; set; }
    }
}
