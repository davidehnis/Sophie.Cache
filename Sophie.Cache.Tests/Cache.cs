using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baxter.Domain;
using Sophie.Domain.Applied;
using System.Linq;
using CacheManager.Core;

namespace Sophie.Cache.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void DoesCacherProperlyHandleSubsequentInsertCallsOnTheSameKey()
        {
            // Arrange
            var cache = new Cacher("getStartedCache", "handleName");

            // Act
            cache.Insert("keyA", "valueA");
            cache.Insert("keyA", "valueB");

            // Assert
            Assert.IsTrue(cache.Fetch("keyA").ToString() == "valueB");
        }

        [TestMethod]
        public void DoesCacherStringProperlyHandleSubsequentInsertCallsOnTheSameKey()
        {
            // Arrange
            var cache = new Cacher<string>("getStartedCache", "handleName");

            // Act
            cache.Insert("keyA", "valueA");
            cache.Insert("keyA", "valueB");

            // Assert
            Assert.IsTrue(cache.Fetch("keyA") == "valueB");
        }

        [TestMethod]
        public void DoseReserveProperlyStoreChangedItems()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy();
            var b = new Toy();

            a.Id = Guid.NewGuid();
            a.Name = "Bob";
            b.Id = Guid.NewGuid();
            b.Name = "George";

            // Act
            cache.Insert(a.Id, a);
            cache.Insert(b.Id, b);
            a.Name = "Phil";
            cache.Insert(a.Id, a);
            var result = cache.Fetch(a.Id);

            // Assert
            Assert.IsTrue(result.Name == "Phil");
        }

        [TestMethod]
        [Ignore]
        public void ErrorsOccurredInTheExpectedAmount()
        {
            // Arrange
            //var cache = Cache.Instance;
            //var node = new Node("Bob");

            //// Act
            //cache.Insert(node);
            //var retrieved = cache.Retrieve(node.Key);

            //// Assert
            //Assert.IsTrue(retrieved.Errors.Occurrences("method was invalid") == 4);
        }

        [TestMethod]
        public void InsertAndRetreiveNodeWithoutException()
        {
            // Arrange
            var cache = CacheFactory.Build("getStartedCache", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("handleName");
            });

            // Act
            cache.Add("keyA", "valueA");
            cache.Put("keyB", 23);
            cache.Update("keyB", v => 42);

            // Assert
            Assert.IsTrue(cache.Get("keyA").ToString() == "valueA");
            Assert.IsTrue((cache.Get("keyB")).ToString() == "42");
        }

        [TestMethod]
        public void InsertAndRetreiveNodeWithoutExceptionWithCacher()
        {
            // Arrange
            var cache = new Cacher("getStartedCache", "handleName");

            // Act
            cache.Insert("keyA", "valueA");

            // Assert
            Assert.IsTrue(cache.Fetch("keyA").ToString() == "valueA");
        }

        [TestMethod]
        public void InsertAndRetreiveNodeWithoutExceptionWithCacherNoParams()
        {
            // Arrange
            var cache = new Cacher();

            // Act
            cache.Insert("key", "valueA");

            // Assert
            Assert.IsTrue(cache.Fetch("key").ToString() == "valueA");
        }

        [TestMethod]
        [Ignore]
        public void InsertANodeWithoutException()
        {
            // Arrange
            //var cache = Cache.Instance;
            //var node = new Node("Bob");

            //// Act
            //var response = cache.Insert(node);

            //// Assert
            //Assert.IsNotNull(response);
        }

        [TestMethod]
        [Ignore]
        public void ProperErrorsWereRegisteredAndReturned()
        {
            // Arrange
            //var cache = Cache.Instance;
            //var node = new Node("Bob");

            //// Act
            //cache.Insert(node);
            //var retrieved = cache.Retrieve(node.Key);

            //// Assert
            //Assert.IsNotNull(retrieved.Errors);
            //Assert.IsTrue(retrieved.Errors.Any());
            //Assert.IsTrue(retrieved.Errors.Contains("method was invalid"));
        }

        [TestMethod]
        public void RemoveNodeWithoutException()
        {
            // Arrange
            var cache = CacheFactory.Build("getStartedCache", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("handleName");
            });

            // Act
            cache.Add("keyA", "valueA");
            cache.Put("keyB", 23);
            cache.Update("keyB", v => 42);
            cache.Remove("keyA");

            // Assert
            Assert.IsTrue(cache.Get("keyA") == null);
        }
    }

    internal class Toy : IItem<Toy>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Stamp { get; set; }

        public Toy Value { get; set; }
    }
}