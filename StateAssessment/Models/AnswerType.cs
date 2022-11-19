using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class AnswerType
    {
        public AnswerType()
        {
            Answers = new HashSet<Answer>();
        }

        [Key]
        public string AnswerTypeCode { get; set; } = null!;
        public string? AnswerTypeName { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
