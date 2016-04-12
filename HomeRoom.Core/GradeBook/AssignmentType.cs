using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.ClassEnrollment;
using HomeRoom.Membership;

namespace HomeRoom.GradeBook
{
    public class AssignmentType : Entity
    {
        public AssignmentType()
        {
        }
        // Database Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public virtual double Percentage { get; set; }

        /// <summary>
        /// Gets or sets the class identifier.
        /// </summary>
        /// <value>
        /// The class identifier.
        /// </value>
        public virtual int ClassId { get; set; }

        /// <summary>
        /// Gets or sets the course.
        /// </summary>
        /// <value>
        /// The course.
        /// </value>
        [ForeignKey("ClassId")]
        public virtual Class Course { get; set; }

        /// <summary>
        /// Gets or sets the assignments.
        /// </summary>
        /// <value>
        /// The assignments.
        /// </value>
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
