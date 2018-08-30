using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.CompositEntities.Requests;
using Models.Entities;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace XTests.UnitTests.Controllers
{
    public class StudentControllerTest
    {
        [Fact]
        public void Create_A_Fake_Student()
        {
            var mockRespo = new Mock<IRepository>();
            var mockController = new StudentController(mockRespo.Object);
            var fakeStudent = new Student
            {
                FirstName = "fake FName",
                LastName = "fake LName",
                MidName = "fake MName",
                EnrollmentDate = DateTime.Now,
                StudentId = "fake StudentId"
            };
            mockRespo.Setup(o => o.AddStudentAsync(fakeStudent)).Returns(Task.FromResult(1));
            ActionResult<Student> studentAr = Task.Run(() =>
                {
                    return mockController.CreateAsync(new StudentReq
                        {FName = "fake FName", LName = "fake LName", MName = "fake MName"});
                }
            ).Result;
            //mockRespo.Verify(m=>m.AddStudentAsync(fakeStudent),Times.Once);
            Assert.NotNull(studentAr);
            Assert.IsType<OkObjectResult>(studentAr.Result);
            var student = ((OkObjectResult)studentAr.Result).Value as Student;
            Assert.Equal(fakeStudent.FirstName, student.FirstName);
        }
    }
}