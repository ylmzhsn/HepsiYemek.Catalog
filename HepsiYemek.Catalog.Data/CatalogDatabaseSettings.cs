namespace HepsiYemek.Catalog.Data
{
    using HepsiYemek.Catalog.Data.Interfaces;

    /// <summary>
    /// Class of catalog db settings
    /// </summary>
    public class CatalogDatabaseSettings : ICatalogDatabaseSettings
    {
        /// <summary>
        /// Products collection name
        /// </summary>
        public string ProductsCollectionName { get; set; }

        /// <summary>
        /// Categories collection name 
        /// </summary>
        public string CategoriesCollectionName { get; set; }

        /// <summary>
        /// Connection string of catalog db
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database name of catalog db
        /// </summary>
        public string DatabaseName { get; set; }
    }
}

