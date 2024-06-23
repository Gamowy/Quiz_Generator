using QuizApp;
using QuizApp.Model;

namespace Quiz.Tests
{
    class TestDbFunctionality
    {
        [Test]
        public void QuizDbFunctionalityTest()
        {
            // Arrange
            QuizApp.Model.Quiz quiz = new QuizApp.Model.Quiz("test.db");      
            
            // Act

            // Create new quiz, add question/answers and save
            quiz.create();
            quiz.loadFromFile();
            Question question = new Question(1);
            question.Content = "test question";
            Answer answer = new Answer(1);
            answer.Content = "test answer";
            answer.IsCorrect = true;
            quiz.Questions.Add(question);
            for (int i = 0; i < 4; i++)
            {
                quiz.Answers.Add(answer);
            }
            quiz.save();

            // Load quiz from file again
            QuizApp.Model.Quiz loadedQuiz = new QuizApp.Model.Quiz("test.db");
            loadedQuiz.loadFromFile();

            // Assert if values were loaded correctly
            Question? loadedQuestion = loadedQuiz.Questions.FirstOrDefault(x => x.Id == 1);
            Assert.IsNotNull(loadedQuestion, "Loaded question is null");
            Assert.That(loadedQuestion.Content, Is.EqualTo("test question"), "Loaded question content is incorrect");

            IEnumerable<Answer>? loadedAnswers = loadedQuiz.Answers.Where(x => x.Question_Id == 1);
            Assert.IsNotNull(loadedAnswers, "Loaded answers is null");
            Assert.That(loadedAnswers.Count(), Is.EqualTo(4), "Incorrect number of answers for loaded question");
        }
    }
}
