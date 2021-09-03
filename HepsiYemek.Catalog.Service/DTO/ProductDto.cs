namespace HepsiYemek.Catalog.Service.DTO
{
    public class ProductDto
    {
        /// <summary>
        /// Name of of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary> 
        /// CategoryId of the product
        /// Represents relationship between categories and products
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency of the product
        /// </summary>
        public string Currency { get; set; }
    }
}
