using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.Membership;

namespace HomeRoom.GradeBook
{
    public class Grade : Entity
    {
        // Database Properties    
            
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
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }


        // Navigational Properties

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
    }
}
