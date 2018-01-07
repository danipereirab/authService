using System.ComponentModel.DataAnnotations;
using AuthService.Models.RoleViewModels;
using AuthService.Models.UserViewModels;

namespace AuthService.Models.UserRoleViewModels
{

    public class UserRoleViewModel
    {
        
        [Display(Name = "User id")]
        [Required(ErrorMessage = "The {0} is required")]
        public string UserId { get; set; }

        [Display(Name = "Role id")]
        [Required(ErrorMessage = "The {0} is required")]
        public string RoleId { get; set; }

        [Display(Name = "User name")]
        public string UserName {get;set;}

        [Display(Name = "Role name")]
        public string RoleName {get;set;}
        public virtual RoleViewModel Role {get;set;}
        public virtual UserViewModel User {get;set;}
        
    }

   
}
