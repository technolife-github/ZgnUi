using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;

namespace ZgnWebApi.Core.DataAccess.EntityFramework
{
    public abstract class EfBaseRepository<TEntity, TContext> : EfBaseAsyncRepository<TEntity, TContext>, IBaseRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public virtual IPaginationResult<List<TEntity>> GetAllWithPagination(IPageableFilter<TEntity>? pageableFilter)
        {
            using TContext context = new();
            var result = from x in context.Set<TEntity>()
                         select x;
            result = pageableFilter == null ? result : pageableFilter.Filter == null
                ? result
                : result.Where(pageableFilter.Filter);
            result = pageableFilter == null ? result : result.SortByData(pageableFilter.Sort);
            var count = result.LongCount();
            result = pageableFilter == null ? result : pageableFilter.Pagination != null && pageableFilter.Pagination.Limit > 0 ? result.Skip(pageableFilter.Pagination.Offset).Take(pageableFilter.Pagination.Limit) : result;
            return new PaginationResult<List<TEntity>>(result.ToList(), count);
        }
        public virtual TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Where(filter).FirstOrDefault();
            }
        }

        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter)
        {
            using (var context = new TContext())
            {
                return (filter == null) ?
                    context.Set<TEntity>().ToList() :
                    context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public virtual bool Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                typeof(TEntity).GetProperty("UpdatedAt")?.SetValue(entity, DateTime.Now);
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
        }
        public virtual bool Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
                return true;
            }
        }
        public virtual bool Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
        }

        public virtual bool SoftDelete(TEntity entity)
        {
            using (var context = new TContext())
            {
                dynamic entityClone = entity;
                entityClone.DeletedAt = DateTime.Now;
                entityClone.DeletedUser = 1;// ServiceTool.GetUserId();
                var updatedEntity = context.Entry((TEntity)entityClone);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
        }
    }
}
