using System;
using System.Collections.Generic;

namespace Sophie.Cache
{
    /// <summary>
    /// Revisions contains a cache that holds multiple instances of the same type that represent
    /// copies that very by modifications made
    /// </summary>
    /// <typeparam name="T">The class type of the revision. One set of revisions per type.</typeparam>
    public class Revisions<T> where T : IItem<T>
    {
        private readonly List<T> _keep = new List<T>();
        private readonly List<string> _keys = new List<string>();
        private readonly Cacher<List<T>> _revisions = new Cacher<List<T>>();

        /// <summary>Retrieves all of the updates of an instance of type T</summary>
        /// <param name="id">The instance id to fetch revisions</param>
        /// <returns>A list of all revisions [copies] of instance identified by id</returns>
        public virtual IEnumerable<T> FetchAll(Guid id)
        {
            return _revisions.Fetch(id);
        }

        /// <summary>Adds a revision to the cache</summary>
        /// <param name="value">The value [copy] to insert</param>
        public virtual void Insert(T value)
        {
            var revisions = _revisions.Fetch(value.Instance);

            if (revisions == null)
            {
                revisions = new List<T>();
                _revisions.Insert(value.Instance, revisions);
            }

            revisions.Add(value);
            _revisions.Insert(value.Instance, revisions);
        }
    }
}