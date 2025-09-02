using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetWithSpecAsync(ISpecification<T> spec);
        public Task<IReadOnlyList<T>?> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        public void Detach(T entity);
        public Task AddRangeAsync(IEnumerable<T> entities);
        public Task AddAsync(T entity);
        public void DeleteRange(IEnumerable<T> entities);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
