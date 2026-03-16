using Microsoft.EntityFrameworkCore;
using MyERP.Application.Interfaces;
using MyERP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyERP.Infrastructure.Repositories
{
    public class MainRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext db;
        private readonly DbSet<T> Table;
        public MainRepository(AppDbContext _db)
        {
            db = _db;
            Table = _db.Set<T>();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await Table.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.Where(predicate).ToListAsync();
        }
        public async Task AddAsync(T row)
        {
           await Table.AddAsync(row);
        }

        public void Update(T row)
        {
             Table.Update(row);
        }

        public void Delete(T row)
        {
            Table.Remove(row);
        }





    }
}
