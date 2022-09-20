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
        void Save();
        void Update(TEntity entity);
        void Clear();

    }
}
