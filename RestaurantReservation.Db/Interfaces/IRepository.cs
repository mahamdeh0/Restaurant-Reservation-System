namespace RestaurantReservation.Db.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(); 
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(int id);
        public Task<TEntity> GetByIdAsync(int id);
    }
}
