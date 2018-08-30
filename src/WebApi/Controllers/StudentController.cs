using System;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CompositEntities.Requests;
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
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Student>> CreateAsync([FromBody] StudentReq input)
        {
            try
            {
                //verify firstname and lastname is not null
                if (string.IsNullOrEmpty(input.FName) && string.IsNullOrEmpty(input.LName))
                {
                    throw new ValidationNotEmptyException("first name and last name can not be empty");
                }

                //create a new user
                var student = new Student
                {
                    FirstName = input.FName, LastName = input.LName, MidName = input.MName,
                    EnrollmentDate = DateTime.Now,
                    StudentId = GenerateCoupon(12)
                };
                student.Id = await _repository.AddStudentAsync(student);
                return Ok(student);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("byStudentId")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Student>> GetStudentByStudentIdAysnc(string studnetId)
        {
            try
            {
                if (string.IsNullOrEmpty(studnetId))
                {
                    //throw new ValidationNotEmptyException("studentId");
                    return BadRequest();
                }

                var student = await _repository.GetStudentByStudentIdAsync(studnetId);
                if (student != null)
                {
                    return Ok(student);
                }

                return NotFound();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e);
            }
            
        }


    }
}