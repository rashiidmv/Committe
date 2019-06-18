using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MahalluManager.DataAccess {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
        protected readonly DbContext Context;
        internal DbSet<TEntity> dbSet;
        public Repository(DbContext context) {
            Context = context;
            dbSet = context.Set<TEntity>();
        }
        public void Add(TEntity entity) {
            Context.Set<TEntity>().Add(entity); 
        }

        public void AddRange(IEnumerable<TEntity> entities) {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate) {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(int id) {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll() {
            return Context.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity) {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities) {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity) {
            dbSet.Attach(entity);
            var entry = Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
