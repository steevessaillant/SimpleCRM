using System.Collections.Generic;

namespace CRMRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        string Path { get;}

        List<TEntity> FetchAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        TEntity Get(TEntity entity);
        TEntity GetById(string Id);
        void Save();
        void Update(TEntity entity);
        void Clear();

    }
}
