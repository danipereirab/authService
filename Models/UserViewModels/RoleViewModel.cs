using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AuthService.Models.UserViewModels;
using AuthService.Data;

namespace AuthService.Models.RoleViewModels
{

    public class RoleViewModel 
    {

        [Display(Name = "Rol Id")]
        [Required(ErrorMessage = "The {0} is required")]
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }


        [Display(Name = "Name")]
        [StringLength(256, ErrorMessage = "The {0} must be at least {1} and at max {2} characters long.", MinimumLength = 6)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = CommonResources.UpperCaseText(value);
            }
        }

        [Display(Name = "Normalized name")]
        [StringLength(256, ErrorMessage = "The {0} must be at least {1} and at max {2} characters long.", MinimumLength = 6)]
        public string NormalizedName { get; set; }


        public virtual ICollection<UserViewModel> Users {get;set;}
        private string _Name { get; set; }
    }


}
