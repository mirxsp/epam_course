using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WEB.DAL.Contexts;

namespace WEB.DAL.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal SalesContext context { get; set; }
        internal DbSet<TEntity> entitySet { get; private set; }

        public GenericRepository(SalesContext context)
        {
            this.context = context;
            entitySet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            entitySet.Add(entity);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression!=null? entitySet.Where(expression).AsEnumerable() : entitySet.AsEnumerable();
        }

        public virtual void Remove(TEntity entity)
        {
            if(context.Entry(entity).State == EntityState.Detached)
            {
                entitySet.Attach(entity);
            }
            entitySet.Remove(entity);
        }

        public virtual void Save()
        {
            context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            entitySet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
