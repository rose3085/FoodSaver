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


        public async Task<IEnumerable<T>> GetRandomAsync()
        {
            var results = await _db.ToListAsync(); // Load all entities into memory

            // Return all entities but in a random order
            return results.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public async Task<T> GetByName(Expression<Func<T, bool>> filter)
        {
            //return await _db.FirstAsync(filter);
            var results = await _db
        .Where(filter)
        .ToListAsync(); // Load results into memory

            // Return a random result from the filtered results
            return results.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }


        public async Task<IEnumerable<T>> GetWithInclude(Expression<Func<T, object>>[] children)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                foreach (var childrens in children)
                {
                    query = query.Include(childrens);
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetWithIncludeAndId<TPrimaryKey>(TPrimaryKey id, Expression<Func<T, object>>[] children)
        {
            IQueryable<T> query = _db;

            // Loop through the children to include related entities
            foreach (var child in children)
            {
                query = query.Include(child);
            }

            // Filter by the primary key (id) to get only the entity with that specific id
            var result = await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));

            return result;
        }

        public async Task DeleteById<TPrimaryKey>(TPrimaryKey id)
        {
            var entity = await _db.FindAsync(id);
            if (entity != null)
            {
                _db.Remove(entity);
            }
            
        }



        //public async Task<T> GetWithIncludeAndFilter(
        //    Expression<Func<T, object>>[] children,
        //    Expression<Func<T, string>> filter
        //)
        //{
        //    IQueryable<T> query = await _db.FirstOrDefaultAsync(string);
        //    foreach (var child in children)
        //    {
        //        query = query.Include(child);
        //    }

        //    return await query.FirstOrDefaultAsync();
        //}
    }
}
