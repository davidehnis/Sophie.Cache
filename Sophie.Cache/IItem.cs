using System;

namespace Sophie.Cache
{
    /// <summary>Represents an interface to an item that can be used by and contained in Cacher</summary>
    /// <typeparam name="T">The class that is being cached</typeparam>
    public interface IItem<T>
    {
        /// <summary>The unique copy id</summary>
        Guid Id { get; }

        /// <summary>
        /// The unique Instance id. A revisions list can have multiple copies of the instance
        /// </summary>
        Guid Instance { get; }

        /// <summary>The name for this copy</summary>
        string Name { get; }

        /// <summary>Creation date time stamp</summary>
        DateTime Stamp { get; }

        /// <summary>Create a value copy of the instance</summary>
        /// <param name="instance">The template</param>
        /// <returns>A new instance, copy, of instance</returns>
        T Copy(T instance);
    }
}