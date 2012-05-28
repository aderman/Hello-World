namespace AsyncReader.Lib.Repository
{
    using AsyncReader.Core;
    using AsyncReader.Lib.Models;


    public interface IContentRepo : IMongoRepository<Content>
    {
        
    }

    public class ContentRepo : MongoRepository<Content>, IContentRepo
    {
         
    }
}