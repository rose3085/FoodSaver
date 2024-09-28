//using Application.Interfaces.Data;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;

//using Infrastructure.Data.ApplicationDbContext;
//using System.Threading.Tasks;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Data;
using Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;




namespace Infrastructure.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }





        public async Task<T> AddAsync(T entity)
        {
            return (await _db.AddAsync(entity)).Entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Remove(entity);
        }

        public async Task<bool> EmailExists(Expression<Func<T, bool>> filter)
        {
            return await _db.AnyAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _db.ToListAsync();
        }

        public async Task<T> GetById<TPrimaryKey>(TPrimaryKey id)
        {
            var result = await _db.FindAsync(id);
            if (result is null)
            {
                return null;
            }
            return result;
        }

        public async Task<T> GetByName(Expression<Func<T, bool>> filter)
        {
            return await _db.FirstAsync(filter);
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }
    }
}
