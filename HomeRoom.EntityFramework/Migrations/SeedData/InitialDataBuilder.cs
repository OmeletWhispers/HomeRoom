using HomeRoom.EntityFramework;
using EntityFramework.DynamicFilters;

namespace HomeRoom.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly HomeRoomDbContext _context;

        public InitialDataBuilder(HomeRoomDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new DefaultEditionsBuilder(_context).Build();
            new DefaultTenantRoleAndUserBuilder(_context).Build();
            new DefaultLanguagesBuilder(_context).Build();
        }
    }
}
