using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.ClassEnrollment;
using HomeRoom.Enumerations;

namespace HomeRoom.GradeBook
{
    public class Assignment : Entity
    {
        public Assignment()
        {
        }
        // Database Properties

        /// <summary>
        /// Gets or sets the assignment type identifier.
        /// </summary>
        /// <value>
        /// The assignment type identifier.
        /// </value>
        public virtual int AssignmentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the class identifier.
        /// </summary>
        /// <value>
        /// The class identifier.
        /// </value>
        public virtual int ClassId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        public virtual DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public virtual AssignmentStatus Status { get; set; }


        // Navigatinal Properties

        /// <summary>
        /// Gets or sets the type of the assignment.
        /// </summary>
        /// <value>
        /// The type of the assignment.
        /// </value>
        [ForeignKey("AssignmentTypeId")]
        public virtual AssignmentType AssignmentType { get; set; }


        /// <summary>
        /// Gets or sets the course.
        /// </summary>
        /// <value>
        /// The course.
        /// </value>
        [ForeignKey("ClassId")]
        public virtual Class Course { get; set; }

        /// <summary>
        /// Gets or sets the grades.
        /// </summary>
        /// <value>
        /// The grades.
        /// </value>
        public virtual ICollection<Grade> Grades { get; set; } 


    }
}
