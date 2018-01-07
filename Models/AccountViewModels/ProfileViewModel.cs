using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Models.ProfileViewModels
{

    public class ProfileViewModel
    {

        [Display(Name = "Id")]
        [Required(ErrorMessage = "The {0} is required")]
        public Guid Id { get; set; }

        [Display(Name = "User id")]
        //[Required(ErrorMessage = "The {0} is required")]
        public string UserId { get; set; }


        [Display(Name = "Site user name")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {1} and at max {2} characters long.", MinimumLength = 3)]
        public string Name {get;set;}

        public virtual ApplicationUser User { get; set; }


    }
}
