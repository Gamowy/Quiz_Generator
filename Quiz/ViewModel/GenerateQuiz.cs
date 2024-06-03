using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Quiz.ViewModel
{
    using Model;
    using Quiz.ViewModel.BaseClass;
    using System.Collections.ObjectModel;

    class GenerateQuiz : ViewModelBase
    {
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

        public string? QuestionContent { get; set; }
        public string? AnswerA { get; set; }
        public string? AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }

        public bool AnswerACorrect { get; set; }
        public bool AnswerBCorrect { get; set; }
        public bool AnswerCCorrect { get; set; }
        public bool AnswerDCorrect { get; set; }

        private void loadQuizFile()
        {
            string filePath = string.Empty;

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "db files (*.db)|*.db|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
                filePath = dialog.FileName;

            if (!filePath.Equals(string.Empty))
            {
                quiz = new Quiz(filePath);
                Questions = quiz.Questions;
                Answers = quiz.Answers;
            }
        }

        public ICommand OpenQuizCommand
        {
            get
            {
                return new RelayCommand(execute => { loadQuizFile(); }, canExecute => true); ;
            }
        }

        public ICommand QuitCommand
        {
            get
            {
                return new RelayCommand(execute => Application.Current.Shutdown(), canExecute => true);
            }
        }

    }
}
