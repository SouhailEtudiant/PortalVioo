using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PortalVioo.Interface
{
    public interface IRepositoryGenericApp<TEntity> where TEntity : class
    {
        TEntity GetByName(string name);
        TEntity Get(int id);
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(int id);
        List<TEntity> GetAll(Expression<Func<TEntity, bool>> condition = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
    }
}
