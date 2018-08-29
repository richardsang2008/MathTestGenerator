using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Jsons
{
    [DataContract]
    public class Quiz
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public decimal Score { get; set; }
        [DataMember] public DateTime QuizDate { get; set; }
        [DataMember] public Student Student { get; set; }
        [DataMember] public IEnumerable<QuizItem> QuizItems { get; set; }
    }

    [DataContract]
    public class Student
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string FirstName { get; set; }
        [DataMember] public string MidName { get; set; }
        [DataMember] public string LastName { get; set; }
        [DataMember] public DateTime EnrollmentDate { get; set; }
        [DataMember] public string StudentId { get; set; }
    }

    [DataContract]
    public class QuizItem
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public decimal LeftOperand { get; set; }
        [DataMember] public decimal RightOperand { get; set; }
        [DataMember] public int QuizId { get; set; }
        [DataMember, JsonConverter(typeof(StringEnumConverter))]
        public Operator Operator { get; set; }
        [DataMember] public decimal Answer { get; set; }
        public static explicit operator Models.Entities.QuizItem(QuizItem s)
        {
            Models.Entities.Operator op;
            if (s.Operator == Operator.Addition)
            {
                op = Entities.Operator.Addition;
            } else if (s.Operator == Operator.Subtraction)
            {
                op = Entities.Operator.Subtraction;
            } else if (s.Operator == Operator.Multiplication)
            {
                op = Entities.Operator.Multiplication;
            }
            else
            {
                op = Entities.Operator.Division;
            }

            return new Models.Entities.QuizItem
            {
                Id = s.Id,
                LeftOperand = s.LeftOperand,
                RightOperand = s.RightOperand,
                Answer = s.Answer,
                Operator = op,
                QuizId = s.QuizId
            };
        }
    }

    [DataContract]
    public enum Operator
    {
        Addition = 1,
        Subtraction,
        Multiplication,
        Division
    }
}