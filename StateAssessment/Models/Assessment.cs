using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateAssessment.Models
{
    [Table("Assessment")]
    public partial class Assessment
    {
        public Assessment()
        {
            AssessmentAnswers = new HashSet<AssessmentAnswer>();
            AssesseeUserId = String.Empty;
        }

        [Key]
        public long AssessmentId { get; set; }
        public string AssesseeUserId { get; set; }
        public long InventoryId { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public bool IsPaused { get; set; }
        public DateTime? LastPausedAt { get; set; }
        public int? PauseCount { get; set; }
        public decimal? EarnedScore { get; set; }

        public virtual Inventory Inventory { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
        public virtual ICollection<AssessmentAnswer> AssessmentAnswers { get; set; }
    }
}
