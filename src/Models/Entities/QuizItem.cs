

namespace Models.Entities
{
    public enum Operator
    {
        Addition = 1,
        Subtraction,
        Multiplication,
        Division
    };

    public class QuizItem : BaseEntity
    {
        public decimal LeftOperand { get; set; }
        public decimal RightOperand { get; set; }
        public Operator Operator { get; set; }
        public decimal Answer { get; set; }
        public int QuizId { get; set; }
    }
}