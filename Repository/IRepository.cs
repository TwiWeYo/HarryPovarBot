using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HarryPovarBot.Repository
{
    public interface IRepository<TId, TValue>
    {
        IEnumerable<TValue> GetAll();
        TValue Get(TId id);
        IEnumerable<TValue> Get(Expression<Func<TValue, bool>> expression);
        bool Upsert(TValue value);
        int Upsert(IEnumerable<TValue> values);
        bool Delete(TId value);
        int Delete(Expression<Func<TValue, bool>> expression);
    }
}
