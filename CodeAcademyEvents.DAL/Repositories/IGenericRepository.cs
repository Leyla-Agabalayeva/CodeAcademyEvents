using CodeAcademyEvents.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
