using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace HomeRoom.Web.Models.JsonModels
{
    public class TestJson
    {
        public int AssignmentId { get; set; }

        public List<QuestionJson> Questions { get; set; }

        public TestJson()
        {
            
        }
    }

    public class QuestionJson
    {
        public int QuestionId { get; set; }
        public int PointValue { get; set; }
    }


    public class AnswerChoiceJson
    {
        public int? AnswerChoiceId { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string Text { get; set; }
    }
}