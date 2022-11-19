using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }

        [Key]
        public string QuestionTypeCode { get; set; } = null!;
        public string? QuestionTypeName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
