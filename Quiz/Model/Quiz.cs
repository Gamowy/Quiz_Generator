using Quiz.Model.Database;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Quiz.Model
{
    class Quiz
    {
        #region CREATE_COMMANDS
        private const string CREATE_QUESTIONS_TB = "CREATE TABLE questions (id int primary key, content varchar(255))";
        private const string CREATE_ANSWERS_TB = "CREATE TABLE answers (question_id int, content varchar(127), int is_correct, FOREIGN KEY(question_id) REFERENCES questions(id))";
        #endregion

        #region SELCECT_COMMANDS
        private const string GET_QUESTIONS = "SELECT * FROM QUESTIONS";
        private const string GET_ANSWERS = "SELECT * FROM ANSWERS";
        #endregion

        private string databasePath;
        private int lastId;
        public ObservableCollection<Question> Questions { get; private set; } = new ObservableCollection<Question>();
        public ObservableCollection<Answer> Answers { get; private set; } = new ObservableCollection<Answer>();
        public Quiz(string path)
        {
            lastId = 0;
            databasePath = path;
        }

        public void create()
        {
            try
            {
                if (!File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }

                SQLiteConnection.CreateFile(databasePath);
                SQLiteConnection connection = DBConnection.Instance.getConnection(databasePath);
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
                SQLiteConnection connection = DBConnection.Instance.getConnection(databasePath);
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

        public void AddQuestion()
        {
            Question newQuestion = new Question(++lastId);
            newQuestion.Content = "Nowe pytanie";
            Questions.Add(newQuestion);

            for (int i = 0; i < 4; i++)
            {
                Answers.Add(new Answer(lastId));
            }
        }

        public void RemoveQuestion(int removeQuestionId)
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
            throw new NotImplementedException();
        }
    }
}
