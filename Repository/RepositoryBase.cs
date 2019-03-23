using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected VdarDbContext RepositoryContext { get; set; }

        public RepositoryBase(VdarDbContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IEnumerable<T> FindAll() =>
            this.RepositoryContext.Set<T>();

        public IEnumerable<T> FindByCondition(Expression<Func<T,bool>> expression) =>
            this.RepositoryContext.Set<T>().Where(expression);

        public void Create(T entity) =>
            this.RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) =>
            this.RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) =>
            this.RepositoryContext.Set<T>().Remove(entity);

        public void Delete(IEnumerable<T> entities) =>
            this.RepositoryContext.Set<T>().RemoveRange(entities);

        async public Task SaveAsync() =>
            await this.RepositoryContext.SaveChangesAsync();

    }
}
