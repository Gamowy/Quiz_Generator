using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    class Answer
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
