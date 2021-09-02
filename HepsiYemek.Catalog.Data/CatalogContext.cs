namespace HepsiYemek.Catalog.Data
{    
    using MongoDB.Driver;

    using HepsiYemek.Catalog.Data.Entities;
    using HepsiYemek.Catalog.Data.Interfaces;

    /// <summary>
    /// Mongodb context for hepsiyemek catalog
    /// </summary>
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; set; }

        public IMongoCollection<Category> Categories { get; set; }
    }
}
