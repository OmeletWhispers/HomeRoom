using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeRoom.Users;

namespace HomeRoom.Membership
{
    public class Parent
    {
        public Parent()
        {
            Students = new List<Student>();
        }
        // Database Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Key, ForeignKey("Account")]
        public long UserId { get; set; }

        // Navigational Properties        
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public virtual User Account { get; set; }

        /// <summary>
        /// Gets or sets the students.
        /// </summary>
        /// <value>
        /// The students.
        /// </value>
        public virtual ICollection<Student> Students { get; set; } 
    }
}
