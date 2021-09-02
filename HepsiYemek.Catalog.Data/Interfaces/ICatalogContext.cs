namespace HepsiYemek.Catalog.Data.Interfaces
{
    using MongoDB.Driver;

    using HepsiYemek.Catalog.Data.Entities;
    
    /// <summary>
    /// Interface of mongodb context for hepsiyemek catalog
    /// </summary>
    public interface ICatalogContext
    {
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
