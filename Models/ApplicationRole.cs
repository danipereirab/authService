using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace AuthService.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class


    public class ApplicationRole : IdentityRole 
    {
        public ApplicationRole(){
            this.Id = Guid.NewGuid().ToString();
        }

    }

}
