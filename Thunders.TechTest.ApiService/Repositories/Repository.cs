using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Context;
using Thunders.TechTest.ApiService.Repositories.Interfaces;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ThundersTechTestDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ThundersTechTestDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
