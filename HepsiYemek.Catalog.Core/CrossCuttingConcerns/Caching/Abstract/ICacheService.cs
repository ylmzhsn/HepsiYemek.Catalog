using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Abstract
{
    /// <summary>
    /// Cache Services interface
    /// </summary>
    public interface ICacheService
    {
        T Get<T>(string key);
        void Add(string key, object data);
        public bool SetTTL(string key, TimeSpan ttl);
        void Remove(string key);
        void Clear();
        bool Any(string key);
    }
}
