using CRMRepository.Entities;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>?> FetchAllAsync();
        Task AddOrUpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity?> GetAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(string Id);
        Task DeleteRangeAsync(List<Customer> customerSubList);
        Task AddOrUpdateRangeAsync(List<Customer> customerSubList);
        Task<ValidationResult> ValidateEntity(Customer entity);
    }
}
