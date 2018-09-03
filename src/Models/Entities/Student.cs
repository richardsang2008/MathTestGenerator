using System;

namespace Models.Entities
{
    public class Student  : BaseEntity
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string StudentId { get; set; }
        public string Email { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}