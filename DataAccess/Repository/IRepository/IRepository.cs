using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category or any Entity that you want to CRUD operation       
        IEnumerable<T> GetAll(string? includeProperties = null);
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);  
    }
}
