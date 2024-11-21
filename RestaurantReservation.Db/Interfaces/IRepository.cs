using RestaurantReservation.Db.Models;
using System.Linq.Expressions;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<(IEnumerable<TEntity> Entities, PaginationMetadata PaginationMetadata)> GetAllAsync(
            Expression<Func<TEntity, bool>> filter, int pageNumber, int pageSize);
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(int id);
        public Task<TEntity> GetByIdAsync(int id);
    }
}
