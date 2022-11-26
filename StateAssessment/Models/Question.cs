using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateAssessment.Models
{
    [Table("Question")]
    public partial class Question
    {
        public Question()
        {
            QuestionSuggestedAnswers = new HashSet<QuestionSuggestedAnswer>();
            Answers = new HashSet<Answer>();
            Assessments = new HashSet<Assessment>();
        }

        [Key]
        public long QuestionId { get; set; }
        public long InventoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string QuestionTypeCode { get; set; } = null!;
        public int? TimeRequiredInMinutes { get; set; }
        public int DisplaySequence { get; set; }
        public string? QuestionCategory { get; set; }
        public virtual Inventory Inventory { get; set; } = null!;
        public virtual QuestionType QuestionTypeCodeNavigation { get; set; } = null!;
        public virtual ICollection<QuestionSuggestedAnswer> QuestionSuggestedAnswers { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
    }
}
