using System;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;

namespace Quiz.ViewModel
{
    using Model;
    using Quiz.ViewModel.BaseClass;

    class GenerateQuiz : ViewModelBase
    {
        #region Properties
        public Quiz? quiz;

        private ObservableCollection<Question>? questions = null;
        public ObservableCollection<Question>? Questions
        {
            get
            {
                return questions;
            }
            set
            {
                questions = value;
                onPropertyChanged(nameof(Questions));
            }
        }

        private ObservableCollection<Answer>? answers = null;
        public ObservableCollection<Answer>? Answers
        {
            get
            {
                return answers;
            }
            set
            {
                answers = value;
                onPropertyChanged(nameof(Answers));
            }
        }


        private Question? selectedQuestion;
        public Question? SelectedQuestion
        {
            get
            {
                return selectedQuestion;
            }
            set
            {
                selectedQuestion = value;
                onPropertyChanged(nameof(SelectedQuestion));
                if (selectedQuestion == null) 
                { 
                    SelectedQuestionAnswers = null;
                }   
                else if (Answers != null)
                {
                    SelectedQuestionAnswers = Answers.Where(answer => answer.Question_Id == SelectedQuestion!.Id).ToArray();
                }
            }
        }

        private Answer[]? selectedQuestionAnswers;
        public Answer[]? SelectedQuestionAnswers { 
            get
            {
                return selectedQuestionAnswers;
            }
            set
            {
                selectedQuestionAnswers = value;
                onPropertyChanged(nameof(SelectedQuestionAnswers));
            }      
        }            
        #endregion 

        #region Methods
        private void createQuizFile()
        {
            try
            {
                Quiz? newQuiz = QuizFileDialog.createFile();
                if (newQuiz != null)
                {
                    quiz = newQuiz;
                    quiz.loadFromFile();
                    Questions = quiz.Questions;
                    Answers = quiz.Answers;
                    MessageBox.Show("Pomyślnie utworzono nowy quiz.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                quiz = null;
                Questions = null;
                Answers = null;
                MessageBox.Show($"Błąd podczas tworzenia bazy danych z quizem:\n{ex.Message}", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void loadQuizFile()
        {
            try
            {
                Quiz? newQuiz = QuizFileDialog.openFile();
                if (newQuiz != null) {
                    quiz = newQuiz;
                    quiz.loadFromFile();
                    Questions = quiz.Questions;
                    Answers = quiz.Answers;
                    MessageBox.Show("Pomyślnie załadowano quiz.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                quiz = null;
                Questions = null;
                Answers = null;
                MessageBox.Show($"Błąd podczas wczytywania bazy danych z quizem:\n{ex.Message}", "Błąd odczytu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Commands

        public ICommand CreateQuizCommand
        {
            get 
            {
                return new RelayCommand(execute => createQuizFile(), canExecute => true);
            }
        }

        public ICommand OpenQuizCommand
        {
            get
            {
                return new RelayCommand(execute => loadQuizFile(), canExecute => true); ;
            }
        }

        public ICommand QuitCommand
        {
            get
            {
                return new RelayCommand(execute => Application.Current.Shutdown(), canExecute => true);
            }
        }

        public ICommand AddQuestionCommand
        {
            get
            {
                return new RelayCommand(execute => quiz!.AddQuestion(), canExecute => (quiz != null));
            }
        }

        public ICommand RemoveQuestionCommand
        {
            get
            {
                return new RelayCommand(
                    execute =>
                    {
                        quiz!.RemoveQuestion(selectedQuestion!.Id);
                        SelectedQuestion = null;
                    },
                    canExecute => (quiz != null && SelectedQuestion != null));
            }
        }
        #endregion
    }
}
