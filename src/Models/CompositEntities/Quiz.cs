using System;
using System.Collections.Generic;
using Models.Entities;

namespace Models.CompositEntities
{
    
    public class Quiz
    {
        public int Id { get; set; }
        public decimal Score { get; set; }
        public DateTime QuizDate { get; set; }
        public Student Student { get; set; }
        public IEnumerable<QuizItem> QuizItems { get; set; }
    }

}