using System.Data.Entity.Migrations;
using HomeRoom.Migrations.SeedData;

namespace HomeRoom.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HomeRoom.EntityFramework.HomeRoomDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HomeRoom";
        }

        protected override void Seed(HomeRoom.EntityFramework.HomeRoomDbContext context)
        {
            new InitialDataBuilder(context).Build();
        }
    }
}
