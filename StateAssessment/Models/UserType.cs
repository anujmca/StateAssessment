using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public string UserTypeCode { get; set; } = null!;
        public string? UserTypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
