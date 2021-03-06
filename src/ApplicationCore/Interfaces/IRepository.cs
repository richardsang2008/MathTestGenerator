using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(int id);
        Task<Student> GetStudentByStudentIdAsync(string studentId);
        Task<Student> GetStudentByEmailAsync(string email);
        Task<int> AddStudentAsync(Student student);
        Task<IEnumerable<QuizItem>> GetQuizItemsAsync();
        Task<QuizItem> GetQuizItemAsync(int id);
        Task<int> UpdateQuizItemAnswerAsync(int id, decimal answer);
        Task<int> AddQuizItemAsync(QuizItem quizItem);
        Task<IEnumerable<Quiz>> GetQuizesAsync();
        Task<Quiz> GetQuizAsync(int id);
        Task<int> AddQuizAsync(Quiz quiz);
        Task<Models.CompositEntities.Quiz> GenerateAQuiz(string studentId, Operator op);
        Task<Models.CompositEntities.Quiz> GetAQuiz(int id);
        Task<int> CreateQuizItems(IEnumerable<QuizItem> quizItems);
        Task<decimal> ScoreAQuiz(int id);
    }
}