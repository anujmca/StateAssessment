﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StateAssessment.Models
{
    public partial class QuestionSuggestedAnswer
    {
        public QuestionSuggestedAnswer()
        {
        }

        [Key]
        public long QuestionSuggestedAnswerId { get; set; }
        public long QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Score { get; set; }
        public virtual Question Question { get; set; } = null!;
    }
}
