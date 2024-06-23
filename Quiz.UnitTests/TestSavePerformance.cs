using QuizApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Tests
{
    class TestSavePerformance
    {
        [Test]
        public void DbSavePerformanceTest()
        {
            // Arrange
            QuizApp.Model.Quiz quiz = new QuizApp.Model.Quiz("test.db");
            quiz.create();
            quiz.loadFromFile();
            Question question;
            Answer answer;
            for (int i = 1; i <= 100; i++)
            {
                question = new Question(i);
                question.Content = "test question";
                answer = new Answer(i);
                answer.Content = "test answer";
                answer.IsCorrect = true;
                quiz.Questions.Add(question);
                for (int j = 0; j < 4; j++)
                {
                    quiz.Answers.Add(answer);
                }
            }

            // Act
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            quiz.save();
            timer.Stop();
            var elapsedMs = timer.ElapsedMilliseconds;

            // Assert
            Assert.That(elapsedMs, Is.LessThan(5000));
        }
    }
}
