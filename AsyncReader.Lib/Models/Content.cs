namespace AsyncReader.Lib.Models
{
    using AsyncReader.Core;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// This class is the content table in database.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Gets or sets ContentId. AutoIncrement
        /// </summary>
        [BsonId] // If this attribute isn't, the id name must be Id.
        public ObjectId ContentId { get; set; }

        /// <summary>
        /// Gets or sets Title. The Content Title
        /// </summary>
        [BsonExists]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Text. Information and details for The Content.
        /// </summary>
        [BsonExists]        
        public string Text { get; set; }
    }
}