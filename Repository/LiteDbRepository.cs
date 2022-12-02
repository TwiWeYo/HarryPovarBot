using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HarryPovarBot.Repository
{
    class LiteDbRepository<TValue> : IRepository<BsonValue, TValue>
    {
        private readonly LiteDatabase db;
        private ILiteCollection<TValue> collection => db.GetCollection<TValue>();

        public LiteDbRepository(string path)
        {
            db = new LiteDatabase(path);
        }

        public bool Delete(BsonValue id)
        {
            return collection.Delete(id);
        }

        public int Delete(Expression<Func<TValue, bool>> expression)
        {
            return collection.DeleteMany(expression);
        }

        public TValue Get(BsonValue id)
        {
            return collection.FindById(id);
        }

        public IEnumerable<TValue> Get(Expression<Func<TValue, bool>> expression)
        {
            return collection.Find(expression);
        }

        public IEnumerable<TValue> GetAll()
        {
            return collection.FindAll();
        }

        public bool Upsert(TValue value)
        {
            return collection.Upsert(value);
        }

        public int Upsert(IEnumerable<TValue> values)
        {
            return collection.Upsert(values);
        }
    }
}
