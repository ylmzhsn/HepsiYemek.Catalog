namespace HepsiYemek.Catalog.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Driver;
   
    using HepsiYemek.Catalog.Data.Entities;

    /// <summary>
    /// Catalog context initial seed
    /// </summary>
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection, IMongoCollection<Category> categoryCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }

            bool existCategory = categoryCollection.Find(p => true).Any();
            if (!existCategory)
            {
                categoryCollection.InsertManyAsync(GetPreconfiguredCategories());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    _id = "b2dc7b494d9e2c75d64cc722ade4e63",
                    Name = "Et Döner",
                    Description = "1 Porsiyon yaprak döner",
                    CategoryId = "10aeda2dfe374764e33eb14b208b262f",
                    Price = 25.90m,
                    Currency = "TL"               
                },
                 new Product()
                {
                     _id = "b2dc7b494d9e2c75d64cc722cef4e63",
                    Name = "Tavuk Döner",
                    Description = "1 Porsiyon tavuk döner",
                    CategoryId = "10aeda2dfe374764e33eb14b208b262f",
                    Price = 15.90m,
                    Currency = "TL"
                },
            };
        }

        private static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    _id = "10aeda2dfe374764e33eb14b208b262f",
                    Name = "Döner",
                    Description = "Döner",
                }
            };
        }
    }
}
