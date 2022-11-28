using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class AssessmentAnswer
    {
        [Key]
        public long AssessmentAnswerId { get; set; }
        public long AssessmentId { get; set; }
        public long QuestionId { get; set; }
        public long? SuggestedAnswerId { get; set; }
        public DateTime SubmittedOn { get; set; }

        public virtual QuestionSuggestedAnswer? Answer { get; set; }
        public virtual Assessment Assessment { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
