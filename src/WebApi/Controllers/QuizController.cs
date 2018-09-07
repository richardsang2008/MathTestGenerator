using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        
        [HttpPatch("quizitems")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<int>> AnswerAQuizItemAsync([FromBody]AnswerQuizItemReq answer)
        {
            try
            {
                return Ok(await _repository.UpdateQuizItemAnswerAsync(answer.QuizItemId, answer.Answer));
            } catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e);
            }
        }

        [HttpGet("{id}/score")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<decimal>> ScoreTheQuiz(int id)
        {
            try
            {
                return Ok(await _repository.ScoreAQuiz(id));
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
                //check to see if there is any quiz score below 60%
                var quizes = await _repository.GetQuizesAsync();
                var remainQuizes = quizes.Where(o => o.Score < (decimal) 0.6 && o.StudentId == createQuizRequest.StudentId);
                if (remainQuizes.Any() )
                {
                    //get one of the quiz
                    var remainQuiz =  remainQuizes.First();
                    retQuiz= await _repository.GetAQuiz(remainQuiz.Id);
                    return retQuiz;
                }
                else
                {
                    //generate a new quiz
                    //force to be ether add or subtraction
                    var random = new Random();
                    var quizType =random.Next((int)Operator.Addition, (int)Operator.Subtraction);
                 
                
                    if ((Operator)quizType == Operator.Addition)
                    {
                        retQuiz = await _repository.GenerateAQuiz(createQuizRequest.StudentId,
                            Operator.Addition);
                    }
                    else if ((Operator)quizType == Operator.Subtraction)
                    {
                        retQuiz = await _repository.GenerateAQuiz(createQuizRequest.StudentId,
                            Operator.Subtraction);
                    }
                    else if ((Operator)quizType == Operator.Multiplication)
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