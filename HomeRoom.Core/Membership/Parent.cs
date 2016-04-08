using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.Users;

namespace HomeRoom.Membership
{
    public class Parent : Entity<long>
    {
        public Parent()
        {
        }
        // Database Properties

        [Key, ForeignKey("Account")]
        public override long Id { get; set; }
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
