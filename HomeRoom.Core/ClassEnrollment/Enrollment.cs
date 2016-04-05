using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.Membership;

namespace HomeRoom.ClassEnrollment
{
    public class Enrollment : Entity
    {
        // Database Properties

        /// <summary>
        /// Gets or sets the class identifier.
        /// </summary>
        /// <value>
        /// The class identifier.
        /// </value>
        public virtual int ClassId { get; set; }

        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        /// <value>
        /// The student identifier.
        /// </value>
        public virtual long StudentId { get; set; }

        // Navigational Properties

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        public virtual Class Class { get; set; }

        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}
