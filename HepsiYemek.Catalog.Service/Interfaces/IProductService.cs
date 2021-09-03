namespace HepsiYemek.Catalog.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HepsiYemek.Catalog.Data.Entities;
    using HepsiYemek.Catalog.Service.DTO;

    /// <summary>
    /// Interface of product repository service
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>IEnumarable list of <see cref="Product"/></returns>
        Task<IEnumerable<Product>> GetProducts();

        /// <summary>
        /// Gets the product by id
        /// </summary>
        /// <param name="id">ObjectId of product</param>
        /// <returns><see cref="Product"/></returns>
        Task<Product> GetProduct(string id);

        /// <summary>
        /// Gets the products by name
        /// </summary>
        /// <param name="name">name of the product</param>
        /// <returns>IEnumarable list of <see cref="Product"/></returns>
        Task<IEnumerable<Product>> GetProductByName(string name);

        /// <summary>
        /// Gets products by categoryId
        /// </summary>
        /// <param name="categoryId">categoryId of product</param>
        /// <returns>IEnumarable list of <see cref="Product"/></returns>
        Task<IEnumerable<Product>> GetProductByCategory(string categoryId);

        /// <summary>
        /// Creates a new product into collection
        /// </summary>
        /// <param name="productDto"><see cref="ProductDto"/></param>
        /// <returns><see cref="Task"/></returns>
        Task<Product> CreateProduct(ProductDto productDto);

        /// <summary>
        /// Updates a product in collection
        /// </summary>
        /// <param name="product"><see cref="Product"/></param>
        /// /// <param name="id"><see cref="string"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        Task<bool> UpdateProduct(ProductDto productDto, string id);

        /// <summary>
        /// Deletes product from collection
        /// </summary>
        /// <param name="id">Id(ObjectId) of <see cref="Product"/></param>
        /// <returns>Task TResult of <see cref="bool"/></returns>
        Task<bool> DeleteProduct(string id);
    }
}
