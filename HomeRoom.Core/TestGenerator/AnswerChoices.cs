using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace HomeRoom.TestGenerator
{
    public class AnswerChoices : Entity
    {
        public AnswerChoices()
        {
            

        }

        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public virtual int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is correct.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is correct; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsCorrect { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public virtual Question Question { get; set; }

        /// <summary>
        /// Gets or sets the assignment answerses.
        /// </summary>
        /// <value>
        /// The assignment answerses.
        /// </value>
        public virtual ICollection<AssignmentAnswers> AssignmentAnswerses { get; set; } 
    }
}
