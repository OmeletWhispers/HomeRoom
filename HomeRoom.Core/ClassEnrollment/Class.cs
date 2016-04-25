using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.GradeBook;
using HomeRoom.Membership;
using HomeRoom.Messaging;

namespace HomeRoom.ClassEnrollment
{
    public class Class : Entity
    {
        public Class()
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
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public virtual string Subject { get; set; }

        /// <summary>
        /// Gets or sets the teacher identifier.
        /// </summary>
        /// <value>
        /// The teacher identifier.
        /// </value>
        public virtual long TeacherId { get; set; }

        // Navigational Properties

        /// <summary>
        /// Gets or sets the teacher.
        /// </summary>
        /// <value>
        /// The teacher.
        /// </value>
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        /// <summary>
        /// Gets or sets the enrollments.
        /// </summary>
        /// <value>
        /// The enrollments.
        /// </value>
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// Gets or sets the assignments.
        /// </summary>
        /// <value>
        /// The assignments.
        /// </value>
        public virtual ICollection<Assignment> Assignments { get; set; }

        /// <summary>
        /// Gets or sets the extra credits.
        /// </summary>
        /// <value>
        /// The extra credits.
        /// </value>
        public virtual ICollection<ExtraCredit> ExtraCredits { get; set; }

        /// <summary>
        /// Gets or sets the assignment types.
        /// </summary>
        /// <value>
        /// The assignment types.
        /// </value>
        public virtual ICollection<AssignmentType> AssignmentTypes { get; set; }

        /// <summary>
        /// Gets or sets the announcements.
        /// </summary>
        /// <value>
        /// The announcements.
        /// </value>
        public virtual ICollection<Announcement> Announcements { get; set; } 
    }
}
