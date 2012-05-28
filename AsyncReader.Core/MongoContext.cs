namespace AsyncReader.Core
{
    using System.Collections.Specialized;

    public interface IMongoService
    {
        StringCollection ValidationList { get; set; }
    }

    public class MongoService : IMongoService 
    {
        public StringCollection ValidationList { get; set; }
    }
}