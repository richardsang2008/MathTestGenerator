using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CompositEntities.Requests;
using Models.Entities;
using Quiz = Models.CompositEntities.Quiz;

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

        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Quiz))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Quiz>> GetAQuizAsync(int id)
        {
            try
            {
                //quizeItemId is unique
                return Ok(await _repository.GetAQuiz(id));
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
        
        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<int>> AnswerAQuizItemAsync(int id, decimal answer)
        {
            try
            {
                return Ok(await _repository.UpdateQuizItemAnswerAsync(id, answer));
            } catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e);
            }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Quiz))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Quiz>> CreateAsync( [FromBody] CreateQuizReq createQuizRequest)
        {
            Quiz retQuiz;
            try
            {
                if (createQuizRequest.Operator == Operator.Addition)
                {
                    retQuiz = await _repository.GenerateAQuiz(createQuizRequest.StudentId,
                        Operator.Addition);
                }
                else if (createQuizRequest.Operator == Operator.Subtraction)
                {
                    retQuiz = await _repository.GenerateAQuiz(createQuizRequest.StudentId,
                        Operator.Subtraction);
                }
                else if (createQuizRequest.Operator == Operator.Multiplication)
                {
                    retQuiz = await _repository.GenerateAQuiz(createQuizRequest.StudentId,
                        Operator.Multiplication);
                }
                else
                {
                    retQuiz = await _repository.GenerateAQuiz(createQuizRequest.StudentId,
                        Operator.Division);
                }
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e);
            }

            return Ok(retQuiz);
        }
        
    }
    
}