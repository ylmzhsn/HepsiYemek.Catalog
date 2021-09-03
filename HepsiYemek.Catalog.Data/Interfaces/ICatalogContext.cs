namespace HepsiYemek.Catalog.Data.Interfaces
{
    using MongoDB.Driver;

    using HepsiYemek.Catalog.Data.Entities;
    
    /// <summary>
    /// Interface of mongodb context for hepsiyemek catalog
    /// </summary>
    public interface ICatalogContext
    {
        // <summary>
        /// Get mongo low level client
        /// </summary>
        MongoClient Client { get; }

        /// <summary>
        /// Get the main mongo database
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// Product collection
        /// </summary>
        IMongoCollection<Product> Products { get; set; }

        /// <summary>
        /// Category collection
        /// </summary>
        IMongoCollection<Category> Categories { get; set; }
    }
}
