namespace QuizApp.Model
{
    public class Question
    {
        public int Id { get; }
        public string? Content { get; set; }

        public Question(int id)
        {
            Id = id;
        }
    }
}
