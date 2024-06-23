using QuizApp.Model.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace QuizApp.Model
{
    public class Quiz
    {
        #region CREATE_COMMANDS
        private const string CREATE_QUESTIONS_TB = "CREATE TABLE questions (id int primary key, content varchar(255))";
        private const string CREATE_ANSWERS_TB = "CREATE TABLE answers (question_id int, content varchar(127), is_correct int, FOREIGN KEY(question_id) REFERENCES questions(id))";
        #endregion

        #region SELECT_COMMANDS
        private const string GET_QUESTIONS = "SELECT * FROM QUESTIONS";
        private const string GET_ANSWERS = "SELECT * FROM ANSWERS";
        #endregion

        #region INSERT_COMMANDS
        private const string INSERT_QUESTIONS = "INSERT INTO questions VALUES (@id, @content)";
        private const string INSERT_ANSWERS = "INSERT INTO answers VALUES (@question_id, @content, @is_correct)";
        #endregion

        public string DatabasePath { get; private set; }
        private int lastId;
        public ObservableCollection<Question> Questions { get; private set; } = new ObservableCollection<Question>();
        public ObservableCollection<Answer> Answers { get; private set; } = new ObservableCollection<Answer>();
        public Quiz(string path)
        {
            lastId = 0;
            DatabasePath = path;
        }

        public void create()
        {
            try
            {
                if (!File.Exists(DatabasePath))
                {
                    File.Delete(DatabasePath);
                }

                SQLiteConnection.CreateFile(DatabasePath);
                SQLiteConnection connection = DbConnection.Instance.getConnection(DatabasePath);
                connection.Open();

                // create questions table
                SQLiteCommand createCommand = new SQLiteCommand(CREATE_QUESTIONS_TB, connection);
                createCommand.ExecuteNonQuery();
                //create answers table
                createCommand = new SQLiteCommand(CREATE_ANSWERS_TB, connection);
                createCommand.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void loadFromFile()
        {
            try
            {
                SQLiteConnection connection = DbConnection.Instance.getConnection(DatabasePath);
                connection.Open();

                // read questions from database
                SQLiteCommand selectCommand = new SQLiteCommand(GET_QUESTIONS, connection);
                SQLiteDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Question question = new Question((int)reader["id"]);
                    if (question.Id > lastId)
                    {
                        lastId = question.Id;
                    }
                    question.Content = reader["content"].ToString();
                    Questions.Add(question);
                }
                // read answers from database
                selectCommand = new SQLiteCommand(GET_ANSWERS, connection);
                reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Answer answer = new Answer((int)reader["question_id"]);
                    answer.Content = reader["content"].ToString();
                    answer.IsCorrect = Convert.ToBoolean(reader["is_correct"]);
                    Answers.Add(answer);
                }
                connection.Clone();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void addQuestion()
        {
            Question newQuestion = new Question(++lastId);
            newQuestion.Content = "Nowe pytanie";
            Questions.Add(newQuestion);

            for (int i = 0; i < 4; i++)
            {
                Answers.Add(new Answer(lastId));
            }
        }

        public void removeQuestion(int removeQuestionId)
        {
            foreach (Question question in Questions.ToList())
            {
                if (question.Id == removeQuestionId)
                {
                    Questions.Remove(question);
                }
            }
            foreach (Answer answer in Answers.ToList())
            {
                if (answer.Question_Id == removeQuestionId)
                {
                    Answers.Remove(answer);
                }
            }
        }

        public void save()
        {
            // check if none of the values is null or empty before saving
            var hasAnswerCheck = new Dictionary<int, bool>();
            foreach (Question question in Questions.ToList())
            {
                if (question.Content == null || question.Content.Equals(""))
                {
                    throw new Exception($"Pytanie o ID{question.Id} nie zawiera treści!");
                }
                hasAnswerCheck.Add(question.Id, false);
            }
            foreach (Answer answer in Answers.ToList())
            {
                if (answer.Content == null || answer.Content.Equals(""))
                {
                    throw new Exception($"Jedna z odpowiedzi w pytaniu o ID{answer.Question_Id} nie zawiera treści!");
                }
                if (answer.IsCorrect)
                {
                    hasAnswerCheck[answer.Question_Id] = true;
                }
            }
            if (hasAnswerCheck.ContainsValue(false))
            {
                var id = hasAnswerCheck.FirstOrDefault(x => x.Value == false).Key;
                throw new Exception($"Pytanie o ID{id} nie posiada poprawnej odpowiedzi!");
            }

            try
            {
                SQLiteConnection connection = DbConnection.Instance.getConnection(DatabasePath);
                connection.Open();

                // delete old questions and insert updated questions
                SQLiteCommand deleteOld = new SQLiteCommand("DELETE FROM questions", connection);
                SQLiteCommand insertNew = new SQLiteCommand(INSERT_QUESTIONS, connection);
                deleteOld.ExecuteNonQuery();
                foreach (Question question in Questions)
                {
                    insertNew.Parameters.AddWithValue("id", question.Id);
                    insertNew.Parameters.AddWithValue("content", question.Content);
                    insertNew.ExecuteNonQuery();
                }

                // delete old answers and insert updated answers
                deleteOld = new SQLiteCommand("DELETE FROM answers", connection);
                insertNew = new SQLiteCommand(INSERT_ANSWERS, connection);
                foreach (Answer answer in Answers)
                {
                    insertNew.Parameters.AddWithValue("question_id", answer.Question_Id);
                    insertNew.Parameters.AddWithValue("content", answer.Content);
                    insertNew.Parameters.AddWithValue("is_correct", answer.IsCorrect);
                    insertNew.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
