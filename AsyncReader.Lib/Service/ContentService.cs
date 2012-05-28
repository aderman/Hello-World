namespace AsyncReader.Lib.Service
{
    using AsyncReader.Core;
    using AsyncReader.Lib.Models;
    using AsyncReader.Lib.Repository;

    using MongoDB.Bson;

    public interface IContentService : IMongoService
    {
        
    }

    public class ContentService : MongoService, IContentService
    {
        /// <summary>
        /// The Content Repository
        /// </summary>
        private readonly IContentRepo contentRepo;

        public ContentService(IContentRepo contentRepo)
        {
            this.contentRepo = contentRepo;
        }

        public ObjectId Insert(Content content)
        {
            var id = this.contentRepo.Insert(content, true);
            if (id == ObjectId.Empty)
            {
                ValidationList.Add("There is same a record in the database.");
            }

            return id;
        }
    }
}