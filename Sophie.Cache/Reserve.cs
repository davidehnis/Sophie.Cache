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
        /// <param name="instanceId">instance id to fetch</param>
        /// <returns>Returns the instance</returns>
        public T Fetch(Guid instanceId)
        {
            return _cache.Fetch(instanceId);
        }

        /// <summary>Retruns cached instance of type T, based on key value</summary>
        /// <param name="instance">fetch the most recent copy of T</param>
        /// <returns>Returns the instance</returns>
        public T Fetch(T instance)
        {
            return _cache.Fetch(instance.Instance);
        }

        /// <summary>Retruns cached instance of type T, based on key value</summary>
        /// <param name="instanceId">string key of index of instance</param>
        /// <returns>instance of type T</returns>
        public T Fetch(string instanceId)
        {
            return _cache.Fetch(instanceId);
        }

        /// <summary>Inserts a new instance of type T, indexes it based on key value</summary>
        /// <param name="key">  Guid type key</param>
        /// <param name="value">The instance to insert</param>
        public void Insert(T value)
        {
            _cache.Insert(value.Instance, value);

            var copyof = value.Copy(value);
            _revisions.Insert(copyof);
        }

        /// <summary>Returns all copies/revisions of a single instance</summary>
        /// <returns>A list of revisions of an instance</returns>
        public IEnumerable<T> Revisions(T value)
        {
            return _revisions.FetchAll(value.Instance);
        }
    }
}