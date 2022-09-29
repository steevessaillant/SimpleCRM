using CRMRepository.Entities;
using System.Collections.Generic;

namespace CRMRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> FetchAll();
        void AddOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        TEntity Get(TEntity entity);
        void Update(TEntity entity);
        void Clear();
        TEntity GetById(string Id);
        void DeleteRange(List<Customer> customerSubList);
        void AddOrUpdateRange(List<Customer> customerSubList);
    }
}
