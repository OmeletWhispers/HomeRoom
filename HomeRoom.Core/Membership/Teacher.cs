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
    public class Teacher : Entity
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Key, ForeignKey("Account")]
        public virtual long UserId { get; set; }


        // Navigational properties
        public virtual User Account { get; set; }
    }
}
