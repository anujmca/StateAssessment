using Microsoft.AspNetCore.Identity;

namespace StateAssessment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public override string UserName { get => this.FirstName + " " + this.LastName; set => base.UserName = value; }
    }
}
