using CRMRepository.Entities;
using System.Collections.Generic;

namespace CRMRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> FetchAll();
        void AddOrUpdate(TEntity entity);
        bool Delete(TEntity entity);
        TEntity Get(TEntity entity);
        void Update(TEntity entity);
        TEntity GetById(string Id);
        void DeleteRange(List<Customer> customerSubList);
        void AddOrUpdateRange(List<Customer> customerSubList);
    }
}
