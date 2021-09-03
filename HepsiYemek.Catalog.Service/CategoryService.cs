namespace HepsiYemek.Catalog.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MongoDB.Driver;
    using MongoDB.Bson;

    using HepsiYemek.Catalog.Data.Entities;
    using HepsiYemek.Catalog.Data.Interfaces;
    using HepsiYemek.Catalog.Service.Interfaces;
    using HepsiYemek.Catalog.Service.DTO;

    /// <summary>
    /// Product repository service
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICatalogContext _context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"><see cref="ICatalogContext"/></param>
        public CategoryService(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>IEnumarable list of <see cref="Category"/></returns>
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context
                            .Categories
                            .Find(p => true)
                            .ToListAsync();
        }

        /// <summary>
        /// Gets the category by id
        /// </summary>
        /// <param name="id">ObjectId of category</param>
        /// <returns><see cref="Category"/></returns>
        public async Task<Category> GetCategory(string id)
        {
            return await _context
                           .Categories
                           .Find(p => p._id == new ObjectId(id))
                           .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the categories by name
        /// </summary>
        /// <param name="name">name of the category</param>
        /// <returns>IEnumarable list of <see cref="Category"/></returns>
        public async Task<IEnumerable<Category>> GetCategoryByName(string name)
        {
            FilterDefinition<Category> filter = Builders<Category>.Filter.Where(c => c.Name == name);

            return await _context
                            .Categories
                            .Find(filter)
                            .ToListAsync();
        }

        /// <summary>
        /// Creates a new category into collection
        /// </summary>
        /// <param name="categoryDto"><see cref="CategoryDto"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task<Category> CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            await _context.Categories.InsertOneAsync(category);

            return category;
        }

        /// <summary>
        /// Updates a category in collection
        /// </summary>
        /// <param name="categoryDto"><see cref="CategoryDto"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> UpdateCategory(CategoryDto categoryDto, string id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x._id, new ObjectId(id));

            var update = Builders<Category>.Update
                .Set(x => x.Name, categoryDto.Name)
                .Set(x => x.Description, categoryDto.Description);

            var updateResult = await _context.Categories.UpdateOneAsync(filter, update);

            var updateSucceeded = updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;

            if (!updateSucceeded)
            {
                return updateSucceeded;
            }

            var productFilter = Builders<Product>.Filter.Eq(p => p.Category._id, new ObjectId(id));

            var updateProduct = Builders<Product>.Update
                .Set(x => x.Category.Description, categoryDto.Description)
                .Set(x => x.Category.Name, categoryDto.Name);

            var productUpdateResult = await _context.Products.UpdateOneAsync(productFilter, updateProduct);

            return productUpdateResult.IsAcknowledged
                    && productUpdateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes category from collection
        /// </summary>
        /// <param name="id">Id(ObjectId) of <see cref="Category"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> DeleteCategory(string id)
        {
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq(c => c._id, new ObjectId(id));

            DeleteResult deleteResult = await _context
                                                .Categories
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
