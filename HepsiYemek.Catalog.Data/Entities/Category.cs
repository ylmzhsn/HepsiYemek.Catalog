namespace HepsiYemek.Catalog.Data.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Category of hepsiyemek in mongodb collection
    /// </summary>
    public class Category
    {
        /// <summary>
        /// ObjectId of the category in mongodb collection
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; private set; }

        /// <summary>
        /// Name of of the category
        /// </summary>
        [BsonElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the category
        /// </summary>
        public string Description { get; set; }
    }
}
