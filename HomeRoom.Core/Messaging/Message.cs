using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using HomeRoom.Users;

namespace HomeRoom.Messaging
{
    public class Message : Entity
    {
        public Message()
        {
            
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the date sent.
        /// </summary>
        /// <value>
        /// The date sent.
        /// </value>
        public virtual DateTime DateSent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the sent by identifier.
        /// </summary>
        /// <value>
        /// The sent by identifier.
        /// </value>
        public virtual long SentById { get; set; }

        /// <summary>
        /// Gets or sets the sent to identifier.
        /// </summary>
        /// <value>
        /// The sent to identifier.
        /// </value>
        public virtual long SentToId { get; set; }

        /// <summary>
        /// Gets or sets the date read.
        /// </summary>
        /// <value>
        /// The date read.
        /// </value>
        public virtual DateTime DateRead { get; set; }

        /// <summary>
        /// Gets or sets the sent by.
        /// </summary>
        /// <value>
        /// The sent by.
        /// </value>
        [ForeignKey("SentById")]
        public virtual User SentBy { get; set; }

        /// <summary>
        /// Gets or sets the sent to.
        /// </summary>
        /// <value>
        /// The sent to.
        /// </value>
        [ForeignKey("SentToId")]
        public virtual User SentTo { get; set; }
    }
}
