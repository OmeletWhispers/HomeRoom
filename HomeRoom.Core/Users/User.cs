using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using HomeRoom.Enumerations;
using HomeRoom.Membership;
using HomeRoom.Messaging;
using HomeRoom.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace HomeRoom.Users
{
    public class User : AbpUser<Tenant, User>
    {
        // Database Properties        
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        public virtual AccountType AccountType { get; set; }

        // Navigational Properties        
        /// <summary>
        /// Gets or sets the teacher.
        /// </summary>
        /// <value>
        /// The teacher.
        /// </value>
        public virtual Teacher Teacher { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public virtual Parent Parent { get; set; }

        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public virtual ICollection<Message> SentMessages { get; set; }

        /// <summary>
        /// Gets or sets the received messages.
        /// </summary>
        /// <value>
        /// The received messages.
        /// </value>
        public virtual ICollection<Message> ReceivedMessages { get; set; } 

        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}