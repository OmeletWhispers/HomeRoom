using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeRoom.Enumerations;

namespace HomeRoom.TestGenerator.Dto
{
    public class AssignmentQuestionDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Question { get; set; }

        public int PointValue { get; set; }

        public QuestionType Type { get; set; }

        public List<AnswerChoicesDto> AnswerChoices { get; set; }
    }

    public class AnswerChoicesDto
    {
        public int Id { get; set; } 
        public string ChoiceValue { get; set; }
    }
}
