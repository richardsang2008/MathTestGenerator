using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController :ControllerBase
    {
        private readonly IRepository _repository;

        public QuizController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Jsons.Quiz))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Models.Jsons.Quiz>> CreateAsync(string studentId, Models.Jsons.Operator @operator)
        {
            Models.Jsons.Quiz retQuiz = new Models.Jsons.Quiz();
            try
            {
                if (@operator == Models.Jsons.Operator.Addition)
                {
                    retQuiz = await _repository.GenerateAQuiz(studentId, Models.Entities.Operator.Addition);
                }
                else if (@operator == Models.Jsons.Operator.Subtraction)
                {
                    retQuiz = await _repository.GenerateAQuiz(studentId, Models.Entities.Operator.Subtraction);
                }
                else if (@operator == Models.Jsons.Operator.Multiplication)
                {
                    retQuiz = await _repository.GenerateAQuiz(studentId, Models.Entities.Operator.Multiplication);
                }
                else
                {
                    retQuiz = await _repository.GenerateAQuiz(studentId, Models.Entities.Operator.Division);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }



            return Ok(retQuiz);
        }
        
    }
    
}