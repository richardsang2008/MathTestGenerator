using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Infrastructure.DataAccess;
using LiteGuard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Models.Entities;

namespace ApplicationCore.Repositories
{
    public class AppRepository  : IRepository
    {
        private readonly AppDbContext _context;
        private readonly Random _random;

        public AppRepository(AppDbContext appDbContext)
        {
            
            _context = appDbContext;
            _random = new Random();
        }

        #region Implemented Method

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> GetStudentByStudentIdAsync(string studentId)
        {
            return await _context.Students.SingleOrDefaultAsync(o => o.StudentId == studentId);
        }

        public async Task<int> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<IEnumerable<QuizItem>> GetQuizItemsAsync()
        {
            return await _context.QuizItems.ToListAsync();
        }

        public async Task<QuizItem> GetQuizItemAsync(int id)
        {
            return await _context.QuizItems.FindAsync(id);
        }

        public async Task<int> AddQuizItemAsync(QuizItem quizItem)
        {
            _context.QuizItems.Add(quizItem);
            await _context.SaveChangesAsync();
            return quizItem.Id;
        }

        public async Task<IEnumerable<Quiz>> GetQuizesAsync()
        {
            return await _context.Quizes.ToListAsync();
        }

        public async Task<Quiz> GetQuizAsync(int id)
        {
            return await _context.Quizes.FindAsync(id);
        }

        public async Task<int> AddQuizAsync(Quiz quiz)
        {
            _context.Quizes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;    
        }

        private Models.Jsons.QuizItem CreateQuizItem(Operator op, int quizId)
        {
            var num1 = _random.Next(1,10000);
            var num2 = _random.Next(1,10000);
            Models.Jsons.QuizItem quizItem ;
            QuizItem qi;
            if (num1 < num2)
            {
                qi = new QuizItem
                    {Answer = 0, LeftOperand = num2, RightOperand = num1, Operator = op, QuizId = quizId};
             
            }
            else
            {
                qi = new QuizItem {Answer = 0, LeftOperand = num1, RightOperand = num2, Operator = op, QuizId = quizId};
             
            }
            quizItem = (Models.Jsons.QuizItem) qi;
            return quizItem;
        }

        public async Task<Models.Jsons.Quiz> GenerateAQuiz(string studentId, Operator op)
        {
            //find out if studentId is correct
            var student = await GetStudentByStudentIdAsync(studentId);
            Guard.AgainstNullArgument(nameof(student),student);
            var now = DateTime.Now;
            var retQuiz = new Models.Jsons.Quiz
                {Id = 0, QuizDate = now, Score = 0, Student = (Models.Jsons.Student) student,QuizItems = new List<Models.Jsons.QuizItem>()};
            int quizId = await AddQuizAsync(new Quiz {QuizDate = now, Score = 0, StudentId = student.StudentId});
            //create 10 quiz items
            List<Models.Jsons.QuizItem> quizItemList = new List<Models.Jsons.QuizItem>();
            for (int i = 0; i < 10; i++)
            {
                quizItemList.Add(CreateQuizItem(op,quizId));
            }
            //save the quizitems
            int recordCount= await CreateQuizItems(quizItemList);

            //map quiz
            retQuiz.Id = quizId;
            retQuiz.QuizDate = now;
            retQuiz.Student.Id = student.Id;
            retQuiz.Student.EnrollmentDate = student.EnrollmentDate;
            retQuiz.Student.FirstName = student.FirstName;
            retQuiz.Student.LastName = student.LastName;
            retQuiz.Student.MidName = student.MidName;
            ((List<Models.Jsons.QuizItem>)retQuiz.QuizItems).AddRange(quizItemList);

            return retQuiz;
        }

        public async Task<int> CreateQuizItems(IEnumerable<Models.Jsons.QuizItem> quizItems)
        {
            int recordAffected = 0;
            foreach (var qi in quizItems)
            {
                _context.QuizItems.Add((QuizItem) qi);
            }

            recordAffected = await _context.SaveChangesAsync();
            return recordAffected;
        }

        #endregion
    }
}