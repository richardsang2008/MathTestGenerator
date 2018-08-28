using System;

namespace Models.Entities
{
    public class Student  : BaseEntity
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string StudentId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public static explicit operator Models.Jsons.Student(Student s)
        {
            return new Models.Jsons.Student
            {
                Id = s.Id,
                EnrollmentDate =  s.EnrollmentDate,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MidName = s.MidName,
                StudentId =s.StudentId
                    
            };
        }
    }
}