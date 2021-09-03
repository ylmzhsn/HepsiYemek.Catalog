namespace HepsiYemek.Catalog.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    using HepsiYemek.Catalog.Data.Entities;
    using HepsiYemek.Catalog.Data.Interfaces;
    using HepsiYemek.Catalog.Service.Interfaces;
    using HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Abstract;
    using HepsiYemek.Catalog.Core.Utilities;
    using MongoDB.Bson;

    /// <summary>
    /// Product repository service
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ICatalogContext _context;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"><see cref="ICatalogContext"/></param>
        public ProductService(ICatalogContext context, ICacheService cacheService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>IEnumarable list of <see cref="Product"/></returns>
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }

        /// <summary>
        /// Gets the product by id
        /// </summary>
        /// <param name="id">ObjectId of product</param>
        /// <returns><see cref="Product"/></returns>
        public async Task<Product> GetProduct(string id)
        {
            var collection = _context.Database.GetCollection<Product>("Products");

            var docs = collection.Aggregate()
                                 .Lookup("Categories", "CategoryId", "_id", "asCategories")
                                 .As<BsonDocument>()
                                 .ToList();

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.ToJson());
            }


            var product = new Product();

            if (_cacheService.Any(Constants.Redis.Product))
            {
                product = _cacheService.Get<Product>(Constants.Redis.Product);

                return product;
            }

            product = await _context
                           .Products
                           .Find(p => p._id == new ObjectId(id))
                           .FirstOrDefaultAsync();

            _cacheService.Add(Constants.Redis.Product, product);

            _cacheService.SetTTL(Constants.Redis.Product, TimeSpan.FromMinutes(5));

            return product;
        }

        /// <summary>
        /// Gets the products by name
        /// </summary>
        /// <param name="name">name of the product</param>
        /// <returns>IEnumarable list of <see cref="Product"/></returns>
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        /// <summary>
        /// Gets products by categoryId
        /// </summary>
        /// <param name="categoryId">categoryId of product</param>
        /// <returns>IEnumarable list of <see cref="Product"/></returns>
        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryId)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.CategoryId, new ObjectId(categoryId));

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        /// <summary>
        /// Creates a new product into collection
        /// </summary>
        /// <param name="product"><see cref="Product"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        /// <summary>
        /// Updates a product in collection
        /// </summary>
        /// <param name="product"><see cref="Product"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g._id == product._id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes product from collection
        /// </summary>
        /// <param name="id">Id(ObjectId) of <see cref="Product"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p._id, new ObjectId(id));

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
