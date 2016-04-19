using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.GradeBook;
using HomeRoom.Membership;

namespace HomeRoom.TestGenerator
{
    public class AssignmentAnswers : Entity
    {
        /// <summary>
        /// Gets or sets the assignment identifier.
        /// </summary>
        /// <value>
        /// The assignment identifier.
        /// </value>
        public virtual int AssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        /// <value>
        /// The student identifier.
        /// </value>
        public virtual long StudentId { get; set; }

        /// <summary>
        /// Gets or sets the answer choice identifier.
        /// </summary>
        /// <value>
        /// The answer choice identifier.
        /// </value>
        public virtual int? AnswerChoiceId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the earned points.
        /// </summary>
        /// <value>
        /// The earned points.
        /// </value>
        public virtual int EarnedPoints { get; set; }

        /// <summary>
        /// Gets or sets the assignment.
        /// </summary>
        /// <value>
        /// The assignment.
        /// </value>
        public virtual Assignment Assignment { get; set; }

        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Gets or sets the answer choices.
        /// </summary>
        /// <value>
        /// The answer choices.
        /// </value>
        public virtual AnswerChoices AnswerChoices { get; set; }
    }
}
