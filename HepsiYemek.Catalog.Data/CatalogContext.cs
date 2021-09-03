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
        /// <summary>
        /// ctor
        /// </summary>
        public CatalogContext(CatalogDatabaseSettings catalogDatabaseSettings)
        {
            if (Client == null)
            {
                try
                {
                    Client = new MongoClient(catalogDatabaseSettings.ConnectionString);

                    Database = Client.GetDatabase(catalogDatabaseSettings.DatabaseName);
                }
                catch
                {
                    throw;
                }
            }

            Products = Database.GetCollection<Product>(catalogDatabaseSettings.ProductsCollectionName);

            Categories = Database.GetCollection<Category>(catalogDatabaseSettings.CategoriesCollectionName);

            CatalogContextSeed.SeedData(Products, Categories);
        }

        /// <summary>
        /// Get low level mongo client
        /// </summary>
        public MongoClient Client { get; }

        /// <summary>
        /// Get the main mongo database
        /// </summary>
        public IMongoDatabase Database { get; }

        public IMongoCollection<Product> Products { get; set; }

        public IMongoCollection<Category> Categories { get; set; }
    }
}
