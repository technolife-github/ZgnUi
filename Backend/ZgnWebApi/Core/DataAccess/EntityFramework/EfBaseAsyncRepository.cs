using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;

namespace ZgnWebApi.Core.DataAccess.EntityFramework
{
    public abstract class EfBaseAsyncRepository<TEntity, TContext> :  IBaseAsyncRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public virtual async Task<IPaginationResult<List<TEntity>>> GetAllWithPaginationAsync(IPageableFilter<TEntity>? pageableFilter)
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
            var asyncResult = await result.ToListAsync();
            return  new PaginationResult<List<TEntity>>(asyncResult, count);
        }

        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().Where(filter).FirstOrDefaultAsync();
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter)
        {
            using (var context = new TContext())
            {
                var result = (filter == null) ?
                    context.Set<TEntity>().ToListAsync() :
                    context.Set<TEntity>().Where(filter).ToListAsync();
                return await result;
            }
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                typeof(TEntity).GetProperty("UpdatedAt")?.SetValue(entity, DateTime.Now);
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
        }
        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                await context.SaveChangesAsync();
                return true;
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                await context.SaveChangesAsync();
                return true;
            }
        }

        public virtual async Task<bool> SoftDeleteAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                dynamic entityClone = entity;
                entityClone.DeletedAt = DateTime.Now;
                entityClone.DeletedUser = 1;// ServiceTool.GetUserId();
                var updatedEntity = context.Entry((TEntity)entityClone);
                updatedEntity.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
