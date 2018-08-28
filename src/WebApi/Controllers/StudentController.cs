using System;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController  : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly Random _random;

        public StudentController(IRepository repository)
        {
            _repository = repository;
            _random = new Random();
        }
        private string GenerateCoupon(int length) {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                result.Append(characters[_random.Next(characters.Length)]);
            }
            return result.ToString();
        }
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Jsons.Student))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Models.Jsons.Student>> CreateAsync(string firstName, string lastName,
            string midName)
        {
            //verify firstname and lastname is not null
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                throw new ValidationNotEmptyException("first name and last name can not be empty");
            }
            //create a new user
            var student = new Student
            {
                FirstName = firstName, LastName = lastName, MidName = midName, EnrollmentDate = DateTime.Now,
                StudentId = GenerateCoupon(12)
            }; 
            student.Id = await _repository.AddStudentAsync(student);

            return Ok((Models.Jsons.Student) student);
        }

        [HttpGet("byStudentId")]
        [ProducesResponseType(200, Type = typeof(Models.Jsons.Student))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Models.Jsons.Student>> GetStudentByStudentIdAysnc(string studnetId)
        {
            if (string.IsNullOrEmpty(studnetId))
            {
                //throw new ValidationNotEmptyException("studentId");
                return BadRequest();
            }

            var student = await _repository.GetStudentByStudentIdAsync(studnetId);
            if (student != null)
            {
                return Ok((Models.Jsons.Student) student);
            }
            else
            {
                return NotFound();
            }
        }


    }
}