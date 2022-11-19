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
        public long? AnswerId { get; set; }
        public DateTime SubmittedOn { get; set; }

        public virtual Answer? Answer { get; set; }
        public virtual Assessment Assessment { get; set; } = null!;
    }
}
