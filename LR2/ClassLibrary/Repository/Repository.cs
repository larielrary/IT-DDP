using JsonFlatFileDataStore;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IDocumentCollection<T> _collection;
        private readonly DataStore _store;

        public Repository(string filename)
        {
            
            _store = new DataStore(filename);
            _collection = _store.GetCollection<T>();
        }

        public void Create(T item)
        {
            _collection.InsertOne(item);
        }

        public void Delete(int id)
        {
            _collection.DeleteOne(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _collection.AsQueryable();
        }

        public T GetById(int id)
        {
            return _collection.Find(item => item.Id == id).FirstOrDefault();
        }

        public void Update(T item)
        {
            _collection.UpdateOne(entity => entity.Id == item.Id, item);
        }
    }
}
