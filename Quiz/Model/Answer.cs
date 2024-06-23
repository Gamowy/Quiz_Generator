namespace QuizApp.Model
{
    public class Answer
    {
        public int Question_Id { get; }
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }

        public Answer(int question_id)
        {
            Question_Id = question_id;
        }
    }
}
