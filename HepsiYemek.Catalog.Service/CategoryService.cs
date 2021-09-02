namespace HepsiYemek.Catalog.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    using HepsiYemek.Catalog.Data.Entities;
    using HepsiYemek.Catalog.Data.Interfaces;
    using HepsiYemek.Catalog.Service.Interfaces;

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
                           .Find(p => p._id.ToString() == id)
                           .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the categories by name
        /// </summary>
        /// <param name="name">name of the category</param>
        /// <returns>IEnumarable list of <see cref="Category"/></returns>
        public async Task<IEnumerable<Category>> GetCategoryByName(string name)
        {
            FilterDefinition<Category> filter = Builders<Category>.Filter.ElemMatch(c => c.Name, name);

            return await _context
                            .Categories
                            .Find(filter)
                            .ToListAsync();
        }

        /// <summary>
        /// Creates a new category into collection
        /// </summary>
        /// <param name="category"><see cref="Category"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task CreateCategory(Category category)
        {
            await _context.Categories.InsertOneAsync(category);
        }

        /// <summary>
        /// Updates a category in collection
        /// </summary>
        /// <param name="category"><see cref="Category"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> UpdateCategory(Category category)
        {
            var updateResult = await _context
                                        .Categories
                                        .ReplaceOneAsync(filter: g => g._id.ToString() == category._id.ToString(), replacement: category);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes category from collection
        /// </summary>
        /// <param name="id">Id(ObjectId) of <see cref="Category"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        public async Task<bool> DeleteCategory(string id)
        {
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq(c => c._id.ToString(), id);

            DeleteResult deleteResult = await _context
                                                .Categories
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
