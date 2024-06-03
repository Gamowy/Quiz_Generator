using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    class Question
    {
        public int Id { get; }
        public string? Content { get; set; }

        public Question(int id)
        {
            Id = id;
        }
    }
}
