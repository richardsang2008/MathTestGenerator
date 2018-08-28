using System.Threading.Tasks;
using ApplicationCore.Interfaces;
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Models.Jsons.Quiz>> CreateAsync(string studentId, Models.Jsons.Operator @operator)
        {
            if (@operator == Models.Jsons.Operator.Addition)
            {
                await _repository.GenerateAQuiz(studentId,Models.Entities.Operator.Addition);
            } else if (@operator == Models.Jsons.Operator.Subtraction)
            {
                await _repository.GenerateAQuiz(studentId,global::Models.Entities.Operator.Subtraction);
            } else if (@operator == Models.Jsons.Operator.Multiplication)
            {
                await _repository.GenerateAQuiz(studentId, global::Models.Entities.Operator.Multiplication);
            }
            else
            {
                await _repository.GenerateAQuiz(studentId, global::Models.Entities.Operator.Division);
            }

            return new Models.Jsons.Quiz();

        }
    }
}