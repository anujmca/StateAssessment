using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class Answer
    {
        public Answer()
        {
            
        }

        [Key]
        public long AnswerId { get; set; }
        public long QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string AnswerTypeCode { get; set; } = null!;
        public decimal? Score { get; set; }

        public virtual AnswerType AnswerTypeCodeNavigation { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
        
    }
}
