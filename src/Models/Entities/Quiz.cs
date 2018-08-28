using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public class Quiz : BaseEntity
    {
        public string StudentId { get; set; }
        public decimal Score { get; set; }
        public DateTime QuizDate { get; set; }
    }
}