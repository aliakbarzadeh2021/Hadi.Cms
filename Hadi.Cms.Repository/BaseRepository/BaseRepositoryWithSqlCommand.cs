using Hadi.Cms.Model;
using System.Data.Entity;

namespace Hadi.Cms.Repository.BaseRepository
{
    public class BaseRepositoryWithSqlCommand<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        private DatabaseContext _context;
        private DbSet<TEntity> _dbSet;

        public BaseRepositoryWithSqlCommand(DatabaseContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual void ExecuteSqlCommand(string query, params object[] parameters)
        {
            _dbSet.SqlQuery(query, parameters);
        }

    }
}
