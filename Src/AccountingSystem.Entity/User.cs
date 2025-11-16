using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountingSystem.Entity
{
    public class User : IdentityUser, IValidatableObject
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Token { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            return (ClaimsIdentity)(await manager.CreateAsync(this)).Principal.Identity!;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Add custom user claims here
            var principal = await manager.CreateAsync(this);
            return (ClaimsIdentity)principal.Principal.Identity!;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PhoneNumber) && string.IsNullOrEmpty(Email))
            {
                yield return new ValidationResult("You have to enter email or phone number.", new[] { "Email", "PhoneNumber" });
            }
        }
    }
}
