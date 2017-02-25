using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;

namespace Sophie.Cache
{
    /// <summary>
    /// A Reserve stores instances of type T in a cache and also stores changes made to instance of
    /// type T
    /// </summary>
    /// <typeparam name="T">The type of instance. One Reserve per class/type</typeparam>
    public class Reserve<T> where T : IItem<T>
    {
        private readonly Cacher<T> _cache = new Cacher<T>();
        private readonly Revisions<T> _revisions = new Revisions<T>();

        /// <summary>Retruns cached instance of type T, based on key value</summary>
        /// <param name="key">instance id to fetch</param>
        /// <returns>Returns the instance</returns>
        public T Fetch(Guid key)
        {
            return _cache.Fetch(key);
        }

        /// <summary>Retruns cached instance of type T, based on key value</summary>
        /// <param name="key">string key of index of instance</param>
        /// <returns>instance of type T</returns>
        public T Fetch(string key)
        {
            return _cache.Fetch(key);
        }

        /// <summary>Inserts a new instance of type T, indexes it based on key value</summary>
        /// <param name="key">  Guid type key</param>
        /// <param name="value">The instance to insert</param>
        public void Insert(Guid key, T value)
        {
            _cache.Insert(key, value);
            _revisions.Insert(value);
        }

        /// <summary>Inserts a new instance of type T, indexes it based on key value</summary>
        /// <param name="key">  string type key</param>
        /// <param name="value">The instance to insert</param>
        public void Insert(string key, T value)
        {
            _cache.Insert(key, value);
            _revisions.Insert(value);
        }

        /// <summary>Returns all copies/revisions of a single instance</summary>
        /// <param name="instanceId">The instance id</param>
        /// <returns>A list of revisions of an instance</returns>
        public IEnumerable<T> Revisions(Guid instanceId)
        {
            return _revisions.FetchAll(instanceId);
        }
    }
}