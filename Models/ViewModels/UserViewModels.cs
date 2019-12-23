using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Models.Users;

namespace Models.ViewModels {

    public class CreateModel {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string CodeId { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class LoginModel {
        [Required(ErrorMessage ="*")]
        [UIHint("email")]
        public string Email { get; set; }
        [UIHint("password")]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string returnUrl { get; set; }
    }

    public class RoleEditModel {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    public class RoleModificationModel {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

    public class SecurePageModel
    {
        public int Id { get; set; }
        public string desc { get; set; }
    }
}
