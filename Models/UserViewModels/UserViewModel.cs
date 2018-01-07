using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuthService.Models.ProfileViewModels;
using AuthService.Models.UserRoleViewModels;

namespace AuthService.Models.UserViewModels
{

    public class UserViewModel
    {

        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The field {0} must be a type of email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [StringLength(256, ErrorMessage = "The {0} must be at least {1} and at max {2} characters long.", MinimumLength = 3)]
        [Display(Name = "User name")]
        public string UserName { get; set; }


        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {1} and at max {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password confirm")]
        [Compare("Password", ErrorMessage = "The field {0} must be the same")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<UserRoleViewModel> UserRoles {get;set;} 
        
        public virtual ProfileViewModel Profile {get;set;} 

    }
}
