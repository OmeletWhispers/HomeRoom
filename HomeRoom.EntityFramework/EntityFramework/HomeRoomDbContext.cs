using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using HomeRoom.Authorization.Roles;
using HomeRoom.Membership;
using HomeRoom.MultiTenancy;
using HomeRoom.Users;

namespace HomeRoom.EntityFramework
{
    public class HomeRoomDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        public virtual IDbSet<Teacher> Teachers { get; set; } 
        public virtual IDbSet<Parent> Parents { get; set; }
        public virtual IDbSet<Student> Students { get; set; }  

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public HomeRoomDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in HomeRoomDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of HomeRoomDbContext since ABP automatically handles it.
         */
        public HomeRoomDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public HomeRoomDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}
