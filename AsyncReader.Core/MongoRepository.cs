namespace AsyncReader.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    public interface IMongoRepository<T> : IDisposable where T : class
    {
        ObjectId Insert(T item, bool existsControl = false);

        T Save(T item, bool existsControl = false);

        T GetItemById(ObjectId id);

        void Drop();

        MongoCursor<T> GetAll();

        MongoCursor<T> GetItemsByQuery(QueryComplete queryComplete);
    }

    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly MongoServer server;

        private readonly MongoDatabase mongoDatabase;

        private readonly MongoCollection<T> mongoCollection;

        public MongoRepository()
        {
            var con = new MongoConnectionStringBuilder(Helper.GetConString(Helper.DataBaseName, ""));
            server = MongoServer.Create(con);
            mongoDatabase = server.GetDatabase(con.DatabaseName);
            mongoCollection = mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public void Dispose()
        {
            // server.Disconnect();
            throw new System.NotImplementedException();
        }

        public ObjectId Insert(T item, bool existsControl = false)
        {
            var prop = item.GetType().GetProperties().Single(x => x.PropertyType == typeof(ObjectId));
            if (existsControl)
            {
                if (this.ExistsControl(item))
                {
                    return ObjectId.Empty;
                }
            }

            mongoCollection.Insert(item);
            return (ObjectId)prop.GetValue(item, null);
        }

        private bool ExistsControl(T item)
        {
            var queryList = new List<IMongoQuery>();
            var props = item.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                var flag = Attribute.GetCustomAttribute(propertyInfo, typeof(BsonExists));
                if (flag != null)
                {
                    queryList.Add(Query.EQ(propertyInfo.Name, BsonValue.Create(propertyInfo.GetValue(item, null))));
                }
            }

            var list = this.GetItemsByQuery(Query.And(queryList.ToArray()));
            if (list == null)
            {
                return false;
            }

            if (list.Count() > 1)
            {
                return true;
            }

            if (list.Count() == 1)
            {
                var p1 = list.Single().GetType().GetProperties().Single(x => x.PropertyType == typeof(ObjectId));
                var p2 = item.GetType().GetProperties().Single(x => x.PropertyType == typeof(ObjectId));
                var v1 = p1.GetValue(list.Single(), null);
                var v2 = p2.GetValue(item, null);
                if (!v1.Equals(v2))
                {
                    return true;
                }
            }

            return false;
        }

        public T GetItemById(ObjectId id)
        {
            return mongoCollection.FindOneById(id);
        }

        public MongoCursor<T> GetItemsByQuery(QueryComplete queryComplete)
        {
            return mongoCollection.FindAs<T>(queryComplete);
        }

        public T Save(T item, bool existsControl = false)
        {
            if (existsControl)
            {
                if (this.ExistsControl(item))
                {
                    return null;
                }
            }

            mongoCollection.Save(item);
            return item;
        }

        public void Drop()
        {
            mongoCollection.Drop();
        }

        public MongoCursor<T> GetAll()
        {
            return mongoCollection.FindAllAs<T>();
        }
    }
}