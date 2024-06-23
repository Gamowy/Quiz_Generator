namespace QuizApp.Tests
{
    public class QuizTests
    {
        QuizApp.Model.Quiz quiz;
        [SetUp]
        public void Init()
        {
            // Arrange
            quiz = new QuizApp.Model.Quiz("test.db");
        }
        [Test]
        public void AssertAddQuestion()
        {
            // Act
            quiz.addQuestion();

            // Assert
            Assert.That(quiz.Questions.Count, Is.EqualTo(1));
            Assert.That(quiz.Answers.Count, Is.EqualTo(4));
        }

        [Test]
        public void AssertRemoveQuestion()
        {
            // Arrange
            quiz.addQuestion();

            // Act
            quiz.removeQuestion(1);

            // Assert
            Assert.That(quiz.Questions.Count, Is.EqualTo(0));
            Assert.That(quiz.Answers.Count, Is.EqualTo(0));
        }

    }
}