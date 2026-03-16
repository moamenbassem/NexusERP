using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyERP.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {

        public Task<T> GetByIdAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);


    }
}
