using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.ClassEnrollment;
using HomeRoom.GradeBook;
using HomeRoom.TestGenerator;
using HomeRoom.Users;

namespace HomeRoom.Membership
{
    public class Student : Entity<long>
    {
        public Student()
        {
        }
        // Database Properties

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [Key, ForeignKey("Account")]
        public override long Id { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public virtual long? ParentId { get; set; }

        // Navigational Properties

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public virtual User Account { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }

        /// <summary>
        /// Gets or sets the enrollments.
        /// </summary>
        /// <value>
        /// The enrollments.
        /// </value>
        public virtual ICollection<Enrollment> Enrollments { get; set; } 

        /// <summary>
        /// Gets or sets the extra credits.
        /// </summary>
        /// <value>
        /// The extra credits.
        /// </value>
        public virtual ICollection<ExtraCredit> ExtraCredits { get; set; }

        /// <summary>
        /// Gets or sets the grades.
        /// </summary>
        /// <value>
        /// The grades.
        /// </value>
        public virtual ICollection<Grade> Grades { get; set; }

        /// <summary>
        /// Gets or sets the assignment answerses.
        /// </summary>
        /// <value>
        /// The assignment answerses.
        /// </value>
        public virtual ICollection<AssignmentAnswers> AssignmentAnswerses { get; set; } 
    }
}
