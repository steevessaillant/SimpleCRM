using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> FetchAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        TEntity Get(TEntity entity);
        void Save();

    }
}
