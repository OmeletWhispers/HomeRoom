using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.Enumerations;
using HomeRoom.GradeBook;

namespace HomeRoom.TestGenerator
{
    public class Question: Entity
    {
        public Question()
        {
            
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public virtual QuestionType QuestionType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is public; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the assignment questionses.
        /// </summary>
        /// <value>
        /// The assignment questionses.
        /// </value>
        public virtual ICollection<AssignmentQuestions> AssignmentQuestionses { get; set; }

        /// <summary>
        /// Gets or sets the answer choiceses.
        /// </summary>
        /// <value>
        /// The answer choiceses.
        /// </value>
        public virtual ICollection<AnswerChoices> AnswerChoiceses { get; set; } 

    }
}
