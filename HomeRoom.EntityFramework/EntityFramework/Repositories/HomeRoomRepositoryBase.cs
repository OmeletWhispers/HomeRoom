using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace HomeRoom.EntityFramework.Repositories
{
    public abstract class HomeRoomRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<HomeRoomDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected HomeRoomRepositoryBase(IDbContextProvider<HomeRoomDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class HomeRoomRepositoryBase<TEntity> : HomeRoomRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected HomeRoomRepositoryBase(IDbContextProvider<HomeRoomDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
