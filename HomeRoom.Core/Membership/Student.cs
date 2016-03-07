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
    public class Student : Entity<long>
    {
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
        public virtual long ParentId { get; set; }

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
    }
}
