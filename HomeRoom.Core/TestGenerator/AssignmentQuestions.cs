using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.GradeBook;

namespace HomeRoom.TestGenerator
{
    public class AssignmentQuestions : Entity
    {
        public AssignmentQuestions()
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
        /// Gets or sets the assignment identifier.
        /// </summary>
        /// <value>
        /// The assignment identifier.
        /// </value>
        public virtual int AssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the point value.
        /// </summary>
        /// <value>
        /// The point value.
        /// </value>
        public virtual int PointValue { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public virtual Question Question { get; set; }

        /// <summary>
        /// Gets or sets the assignment.
        /// </summary>
        /// <value>
        /// The assignment.
        /// </value>
        public virtual Assignment Assignment { get; set; }
    }
}
