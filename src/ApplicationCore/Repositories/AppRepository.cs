using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Infrastructure.DataAccess;
using LiteGuard;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace ApplicationCore.Repositories
{
    public class AppRepository  : IRepository
    {
        private readonly AppDbContext _context;

        public AppRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
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
            int rowsAffected;
            _context.Students.Add(student);
            rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
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
            int rowsAffected;
            _context.QuizItems.Add(quizItem);
            rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
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
            int rowsAffected;
            _context.Quizes.Add(quiz);
            rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;    
        }

        public async Task<Models.Jsons.Quiz> GenerateAQuiz(string studentId, Operator op)
        {
            //find out if studentId is correct
            var student = await GetStudentByStudentIdAsync(studentId);
            Guard.AgainstNullArgument(nameof(student),student);
            var now = DateTime.Now;
            var retQuiz = new Models.Jsons.Quiz{Id =0,QuizDate = now,Score = 0,Student = new Models.Jsons.Student()};
            int quizId = await AddQuizAsync(new Quiz {QuizDate = now, Score = 0, StudentId = student.StudentId});
            //create 10 quiz items
            List<Models.Jsons.QuizItem> quizItemList = new List<Models.Jsons.QuizItem>();
            for (int i = 0; i < 10; i++)
            {
                var num1 = 1;
                var num2 = 2;
                int quizitemId;
                Models.Jsons.QuizItem quizItem ;
                QuizItem qi;
                if (num1 < num2)
                {
                    qi = new QuizItem {Answer = 0, LeftOperand = num2, RightOperand = num1, Operator = op, QuizId = quizId};
                    await AddQuizItemAsync(qi);
                }

                qi = new QuizItem {Answer = 0, LeftOperand = num1, RightOperand = num2, Operator = op, QuizId = quizId};
                quizitemId = await AddQuizItemAsync(qi);
                qi.Id = quizitemId;
                quizItem = (Models.Jsons.QuizItem) qi;
                quizItemList.Add(quizItem);
                
            }
            
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

        #endregion
    }
}