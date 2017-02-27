using CacheManager.Core;
using System;

namespace Sophie.Cache
{
    /// <summary>A simple set of Cacher extensions</summary>
    public static class CacherExtensions
    {
        public static ExpirationMode ConvertMode(this Expiration mode)
        {
            switch (mode)
            {
                case Expiration.Default:
                    return ExpirationMode.Default;

                case Expiration.None:
                    return ExpirationMode.None;

                case Expiration.Sliding:
                    return ExpirationMode.Sliding;

                case Expiration.Absolute:
                    return ExpirationMode.Absolute;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }

    /// <summary></summary>
    /// <typeparam name="TItemType"></typeparam>
    public class Cacher<TItemType>
    {
        protected ICacheManager<TItemType> _cache;

        /// <summary>A constructor that allows the user to select name and handle of cache</summary>
        /// <param name="name">  Moniker for the cache</param>
        /// <param name="handle">The handle for the cache</param>
        public Cacher(string name, string handle)
        {
            _cache = CacheFactory.Build<TItemType>(name, settings =>
            {
                settings.WithSystemRuntimeCacheHandle(handle);
            });
        }

        /// <summary>Default constructor for the proxy class</summary>
        public Cacher()
        {
            const string name = "cache_name";
            const string handle = "cache_handle";

            _cache = CacheFactory.Build<TItemType>(name, settings =>
            {
                settings.WithSystemRuntimeCacheHandle(handle);
            });
        }

        /// <summary>Retrieves the item from the cache given the specified key</summary>
        /// <param name="key">The key in GUID form by which to retrieve the instance of type TItemType</param>
        /// <returns>The instance of type TItemType</returns>
        public virtual TItemType Fetch(Guid key)
        {
            var stringKey = key.ToString();
            return Fetch(stringKey);
        }

        /// <summary>Retrieves the item from the cache given the specified key</summary>
        /// <param name="key">
        /// The key in string form by which to retrieve the instance of type TItemType
        /// </param>
        /// <returns>The instance of type TItemType</returns>
        public virtual TItemType Fetch(string key)
        {
            return _cache.Get(key);
        }

        /// <summary>
        /// Adds (or updates) an instance of type TItemType, indexed by the value of key
        /// </summary>
        /// <param name="key">  The key value in GUID form</param>
        /// <param name="value">The instance of type TItemType to insert into the cache.</param>
        public virtual void Insert(Guid key, TItemType value)
        {
            var stringKey = key.ToString();
            Insert(stringKey, value);
        }

        /// <summary>
        /// Adds (or updates) an instance of type TItemType, indexed by the value of key
        /// </summary>
        /// <param name="key">  The key value in string form</param>
        /// <param name="value">The instance of type TItemType to insert into the cache.</param>
        public virtual void Insert(string key, TItemType value)
        {
            var item = _cache.GetCacheItem(key);
            if (item != null)
            {
                _cache.Update(key, v => value);
            }
            else
            {
                AddItem(key, value, ExpirationMode.None);
            }
        }

        /// <summary>
        /// Adds (or updates) an instance of type TItemType, indexed by the value of key
        /// </summary>
        /// <param name="key">  The key value in GUID form</param>
        /// <param name="value">The instance of type TItemType to insert into the cache.</param>
        /// <param name="mode"> Allows the user to specify lifespan of cached instance</param>
        public virtual void Insert(Guid key, TItemType value, Expiration mode)
        {
            var stringKey = key.ToString();
            Insert(stringKey, value, mode);
        }

        /// <summary>
        /// Adds (or updates) an instance of type TItemType, indexed by the value of key
        /// </summary>
        /// <param name="key">  The key value in GUID form</param>
        /// <param name="value">The instance of type TItemType to insert into the cache.</param>
        /// <param name="mode"> Allows the user to specify lifespan of cached instance</param>
        public virtual void Insert(string key, TItemType value, Expiration mode)
        {
            var item = _cache.Get(key);
            if (item != null)
            {
                _cache.Update(key, v => value);
            }
            else
            {
                AddItem(key, value, mode.ConvertMode());
            }
        }

        /// <summary>A simple helper function to add a value to the cache</summary>
        /// <param name="key">  The key value in string form</param>
        /// <param name="value">The instance of type TItemType to insert into the cache.</param>
        /// <param name="mode"> Allows the user to specify lifespan of cached instance</param>
        protected virtual void AddItem(string key, TItemType value, ExpirationMode mode)
        {
            _cache.Add(key, value);
        }
    }

    /// <summary>
    /// A simplified version of Cacher that allows the user to create a cache without specifying the
    /// cached item type.
    /// </summary>
    public class Cacher : Cacher<object>
    {
        public Cacher(string name, string handle) : base(name, handle)
        {
        }

        public Cacher() : base()
        {
        }
    }
}