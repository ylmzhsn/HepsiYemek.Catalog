namespace HepsiYemek.Catalog.Data.Interfaces
{
    /// <summary>
    /// Inferface of catalog db settings
    /// </summary>
    public interface ICatalogDatabaseSettings
    {
        /// <summary>
        /// Products collection name
        /// </summary>
        string ProductsCollectionName { get; set; }

        /// <summary>
        /// Categories collection name 
        /// </summary>
        string CategoriesCollectionName { get; set; }

        /// <summary>
        /// Connection string of catalog db
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Database name of catalog db
        /// </summary>
        string DatabaseName { get; set; }
    }
}
