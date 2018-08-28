

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

        public static explicit operator Models.Jsons.QuizItem(QuizItem s)
        {
            Jsons.Operator op;
            if (s.Operator == Operator.Addition)
            {
                op = Jsons.Operator.Addition;
            } else if (s.Operator == Operator.Subtraction)
            {
                op = Jsons.Operator.Subtraction;
            } else if (s.Operator == Operator.Multiplication)
            {
                op = Jsons.Operator.Multiplication;
            }
            else
            {
                op = Jsons.Operator.Division;
            }

            return new Models.Jsons.QuizItem
            {
                Id = s.Id,
                LeftOperand = s.LeftOperand,
                RightOperand = s.RightOperand,
                Answer = s.Answer,
                Operator = op,
            };
        }
    }
}