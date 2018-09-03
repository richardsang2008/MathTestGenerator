using System;
using System.Collections.Generic;
using System.Linq;
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
            var student = await _context.Students.SingleOrDefaultAsync(o => o.StudentId == studentId);
            Guard.AgainstNullArgument(nameof(student),student);
            return student;
        }
        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            var student = await _context.Students.SingleOrDefaultAsync(o => o.Email == email);
            Guard.AgainstNullArgument(nameof(student),student);
            return student;
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

        public async Task<int> UpdateQuizItemAnswerAsync(int id, decimal answer)
        {
            //locate the quizitem by id 
            var quizItem = await _context.QuizItems.FindAsync(id);
            Guard.AgainstNullArgument(nameof(quizItem),quizItem);
            quizItem.Answer = answer;
            return await _context.SaveChangesAsync();
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

        private QuizItem CreateQuizItem(Operator op, int quizId)
        {
            var num1 = _random.Next(1,10000);
            var num2 = _random.Next(1,10000);
            QuizItem quizItem ;
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
            quizItem =  qi;
            return quizItem;
        }

        public async Task<Models.CompositEntities.Quiz> GenerateAQuiz(string studentId, Operator op)
        {
            //find out if studentId is correct
            var student = await GetStudentByStudentIdAsync(studentId);
            Guard.AgainstNullArgument(nameof(student),student);
            var now = DateTime.Now;
            var retQuiz = new Models.CompositEntities.Quiz
                {Id = 0, QuizDate = now, Score = 0, Student = student,QuizItems = new List<QuizItem>()};
            int quizId = await AddQuizAsync(new Quiz {QuizDate = now, Score = 0, StudentId = student.StudentId});
            //create 10 quiz items
            List<QuizItem> quizItemList = new List<QuizItem>();
            for (int i = 0; i < 10; i++)
            {
                quizItemList.Add(CreateQuizItem(op,quizId));
            }
            //save the quizitems
            await CreateQuizItems(quizItemList);
            //map quiz
            retQuiz.Id = quizId;
            retQuiz.QuizDate = now;
            retQuiz.Student = student;
            ((List<QuizItem>)retQuiz.QuizItems).AddRange(quizItemList);

            return retQuiz;
        }

        public async Task<Models.CompositEntities.Quiz> GetAQuiz(int id)
        {
            var quiz = await _context.Quizes.FindAsync(id);
            Guard.AgainstNullArgument(nameof(quiz),quiz);
            //find the student
            var student = await GetStudentByStudentIdAsync(quiz.StudentId);
            var retQuiz = new Models.CompositEntities.Quiz
            {
                Id = quiz.Id,
                QuizDate = quiz.QuizDate,
                Score =  quiz.Score,
                Student = student,
                QuizItems =  new List<QuizItem>()
            };
            var quizItems = await _context.QuizItems.Where(o => o.QuizId == id).ToListAsync();
            Guard.AgainstNullArgument(nameof(quizItems), quizItems);
            foreach (var quizItem in quizItems)
            {
                ((List<QuizItem>)retQuiz.QuizItems).Add(quizItem);
            }

            if (retQuiz.Score == 0 && retQuiz.QuizItems.Count() >0)
            {
                int size = retQuiz.QuizItems.Count();
                int correctCount = 0;
                foreach (var item in retQuiz.QuizItems)
                {
                    if (item.Operator == Operator.Addition)
                    {
                        if (Math.Round(item.Answer,2) == Math.Round(item.LeftOperand + item.RightOperand,2))
                            correctCount++;
                    }
                    else if (item.Operator == Operator.Subtraction)
                    {
                        if (Math.Round(item.Answer,2) == Math.Round(item.LeftOperand - item.RightOperand,2))
                            correctCount++;
                    }
                    else if (item.Operator == Operator.Multiplication)
                    {
                        if (Math.Round(item.Answer,2) == Math.Round(item.LeftOperand * item.RightOperand,2))
                        correctCount++;
                    } 
                    else if (item.Operator == Operator.Division)
                    {
                        if (Math.Round(item.Answer,2) == Math.Round(item.LeftOperand / item.RightOperand,2))
                        correctCount++;
                    }
                }

                retQuiz.Score = Math.Round((decimal)correctCount / size ,2);
                //update teh quiz score
                quiz.Score = retQuiz.Score;
                await _context.SaveChangesAsync();
            }

            return retQuiz;

        }

        public async Task<int> CreateQuizItems(IEnumerable<QuizItem> quizItems)
        {
            foreach (var qi in quizItems)
            {
                _context.QuizItems.Add( qi);
            }

            var recordAffected = await _context.SaveChangesAsync();
            return recordAffected;
        }

        #endregion
    }
}