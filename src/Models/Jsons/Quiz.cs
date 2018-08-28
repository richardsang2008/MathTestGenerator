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
        [DataMember] public IEnumerable<QuizItem> QuizItems { get; set; }
        [DataMember] public decimal Score { get; set; }
        [DataMember] public DateTime QuizDate { get; set; }
        [DataMember] public Student Student { get; set; }
    }

    [DataContract]
    public class Student
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string FirstName { get; set; }
        [DataMember] public string MidName { get; set; }
        [DataMember] public string LastName { get; set; }
        [DataMember] public DateTime EnrollmentDate { get; set; }
    }

    [DataContract]
    public class QuizItem
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public decimal LeftOperand { get; set; }
        [DataMember] public decimal RightOperand { get; set; }

        [DataMember, JsonConverter(typeof(StringEnumConverter))]
        public Operator Operator { get; set; }

        [DataMember] public decimal Answer { get; set; }
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