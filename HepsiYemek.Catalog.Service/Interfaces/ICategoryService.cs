namespace HepsiYemek.Catalog.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HepsiYemek.Catalog.Data.Entities;

    /// <summary>
    /// Interface of category repository service
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>IEnumarable list of <see cref="Category"/></returns>
        Task<IEnumerable<Category>> GetCategories();

        /// <summary>
        /// Gets the category by id
        /// </summary>
        /// <param name="id">ObjectId of category</param>
        /// <returns><see cref="Category"/></returns>
        Task<Category> GetCategory(string id);

        /// <summary>
        /// Gets the categories by name
        /// </summary>
        /// <param name="name">name of the category</param>
        /// <returns>IEnumarable list of <see cref="Category"/></returns>
        Task<IEnumerable<Category>> GetCategoryByName(string name);

        /// <summary>
        /// Creates a new category into collection
        /// </summary>
        /// <param name="category"><see cref="Category"/></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateCategory(Category category);

        /// <summary>
        /// Updates a category in collection
        /// </summary>
        /// <param name="category"><see cref="Category"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        Task<bool> UpdateCategory(Category category);

        /// <summary>
        /// Deletes category from collection
        /// </summary>
        /// <param name="id">Id(ObjectId) of <see cref="Category"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        Task<bool> DeleteCategory(string id);
    }
}
