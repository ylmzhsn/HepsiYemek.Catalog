﻿namespace HepsiYemek.Catalog.Data.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Product of hepsiyemek in mongodb collection
    /// </summary>
    public class Product
    {
        /// <summary>
        /// ObjectId of the product in mongodb collection
        /// </summary>
        [BsonId]
        public ObjectId _id { get; set; }

        /// <summary>
        /// Name of of the product
        /// </summary>
        [BsonElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary> 
        /// Category of the product
        /// </summary>
        public Category Category { get; set; }

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
