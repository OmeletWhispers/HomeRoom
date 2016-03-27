using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace HomeRoom.GradeBook
{
    public class AssignmentType : Entity
    {
        public AssignmentType()
        {
            Assignments = new List<Assignment>();
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
        /// Gets or sets the assignments.
        /// </summary>
        /// <value>
        /// The assignments.
        /// </value>
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
