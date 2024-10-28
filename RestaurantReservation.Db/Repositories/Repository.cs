﻿using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using System.Linq.Expressions;

namespace RestaurantReservation.Db.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly RestaurantReservationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(RestaurantReservationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<(IEnumerable<TEntity> Entities, PaginationMetadata PaginationMetadata)> GetAllAsync(
        Expression<Func<TEntity, bool>> filter, int pageNumber, int pageSize)
        {
            var totalEntities = await _dbSet.CountAsync(); 
            var entities = await _dbSet
                .Where(filter)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)
                .ToListAsync();

            var paginationMetadata = new PaginationMetadata
            {
                TotalCount = totalEntities,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalEntities / pageSize)
            };

            return (entities, paginationMetadata);
        }

    }
}
