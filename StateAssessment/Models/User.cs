using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class User
    {
        public User()
        {
            Assessments = new HashSet<Assessment>();
        }

        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserTypeCode { get; set; } = null!;

        public virtual UserType UserTypeCodeNavigation { get; set; } = null!;
        public virtual ICollection<Assessment> Assessments { get; set; }
    }
}
