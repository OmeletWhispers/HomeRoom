using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.Membership;

namespace HomeRoom.TestGenerator
{
    public class Subject : Entity
    {
        public Subject()
        {
            
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the teacher identifier.
        /// </summary>
        /// <value>
        /// The teacher identifier.
        /// </value>
        public virtual long TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the teacher.
        /// </summary>
        /// <value>
        /// The teacher.
        /// </value>
        public virtual Teacher Teacher { get; set; }
    }
}
