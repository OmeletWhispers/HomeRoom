using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.ClassEnrollment;
using HomeRoom.GradeBook;
using HomeRoom.TestGenerator;
using HomeRoom.Users;

namespace HomeRoom.Membership
{
    public class Teacher : Entity<long>
    {
        public Teacher()
        {
        }

        // Database Properties

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [Key, ForeignKey("Account")]
        public override long Id { get; set; }

        // Navigational properties   
             
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public virtual User Account { get; set; }

        /// <summary>
        /// Gets or sets the classes.
        /// </summary>
        /// <value>
        /// The classes.
        /// </value>
        public virtual ICollection<Class> Classes { get; set; }

        /// <summary>
        /// Gets or sets the subjects.
        /// </summary>
        /// <value>
        /// The subjects.
        /// </value>
        public virtual ICollection<Subject> Subjects { get; set; } 
    }
}
