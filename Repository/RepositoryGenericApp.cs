using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PortalVioo.Context;
using PortalVioo.Interface;
using System.Linq.Expressions;

namespace PortalVioo.Repository
{
    public class RepositoryGenericApp<TEntity> : IRepositoryGenericApp<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> tab;
        public RepositoryGenericApp(ApplicationDbContext context)
        {
            _context = context;
            tab = _context.Set<TEntity>();
        }
   
        public TEntity GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(TEntity entity)
        {
            tab.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public TEntity Delete(int id)
        {
            TEntity exist = tab.Find(id);
            tab.Remove(exist);
            _context.SaveChanges();

            return exist;
        }

        public TEntity Update(TEntity entity)
        {
            tab.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public TEntity Get(int id)
        {
            return tab.Find(id);
        }
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> condition = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (condition != null)
            {
                return [.. query.Where(condition)];
            }

            return [.. query];
        }
    }
}
