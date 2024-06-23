namespace QuizApp.Model
{
    public static class QuizFileDialog
    {
        public static Quiz? createFile()
        {
            string filePath = string.Empty;

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "db files (*.db)|*.db|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                if (filePath != string.Empty)
                {
                    Quiz quiz = new Quiz(filePath);
                    quiz.create();
                    return quiz;
                }
            }
            return null;
        }
        public static Quiz? openFile()
        {
            string filePath = string.Empty;

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "db files (*.db)|*.db|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                if (filePath != string.Empty)
                {
                    return new Quiz(filePath);
                }
            }
            return null;
        }
    }
}
