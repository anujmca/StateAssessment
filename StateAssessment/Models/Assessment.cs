using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class Assessment
    {
        public Assessment()
        {
            AssessmentAnswers = new HashSet<AssessmentAnswer>();
        }

        [Key]
        public long AssessmentId { get; set; }
        public long UserId { get; set; }
        public long QuestionId { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public bool IsPaused { get; set; }
        public DateTime? LastPausedAt { get; set; }
        public int? PauseCount { get; set; }
        public decimal? EarnedScore { get; set; }

        public virtual Question Question { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<AssessmentAnswer> AssessmentAnswers { get; set; }
    }
}
