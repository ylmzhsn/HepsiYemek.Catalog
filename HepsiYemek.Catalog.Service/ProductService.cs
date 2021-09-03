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
    using HepsiYemek.Catalog.Service.DTO;

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
            /// TODO : there is a bug about deserialization will be fixed until then redis cache is not available
            
            /// 
            //if (_cacheService.Any(string.Format(Constants.Redis.Product, id)))
            //{
            ///var cachedProduct = _cacheService.Get<Product>(string.Format(Constants.Redis.Product, id));
            //    return cachedProduct;
            //}

            var product = await _context
                           .Products
                           .Find(p => p._id == new ObjectId(id))
                           .FirstOrDefaultAsync();

            //_cacheService.Add(string.Format(Constants.Redis.Product, id), product);

            //_cacheService.SetTTL(Constants.Redis.Product, TimeSpan.FromMinutes(5));

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
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category._id, new ObjectId(categoryId));

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
        public async Task<Product> CreateProduct(ProductDto productDto)
        {
            var category = await _context
                           .Categories
                           .Find(p => p._id == new ObjectId(productDto.CategoryId))
                           .FirstOrDefaultAsync();

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = category,
                Price = productDto.Price,
                Currency = productDto.Currency
            };

            await _context.Products.InsertOneAsync(product);

            return product;
        }

        /// <summary>
        /// Updates a product in collection
        /// </summary>
        /// <param name="product"><see cref="Product"/></param>
        /// <param name="id"><see cref="string"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> UpdateProduct(ProductDto productDto, string id)
        {
            var category = await _context
                           .Categories
                           .Find(p => p._id == new ObjectId(productDto.CategoryId))
                           .FirstOrDefaultAsync();

            var filter = Builders<Product>.Filter.Eq(x => x._id, new ObjectId(id));

            var update = Builders<Product>.Update
                .Set(x => x.Name, productDto.Name)
                .Set(x => x.Description, productDto.Description)
                .Set(x => x.Currency, productDto.Currency)
                .Set(x => x.Price, productDto.Price)
                .Set(x => x.Category, category);

            var updateResult = await _context.Products.UpdateOneAsync(filter, update);

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
