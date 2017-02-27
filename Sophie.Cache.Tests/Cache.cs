using CacheManager.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;

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
        public void DoseReserveProperlyCountStoreRevisedItems()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy(Guid.NewGuid()) { Name = "Bob" };
            cache.Insert(a);

            // Act
            a.Name = "Phil";
            cache.Insert(a);
            var revisions = cache.Revisions(a);

            // Assert
            Assert.IsNotNull(revisions);
            Assert.IsTrue(revisions.Any());
        }

        [TestMethod]
        public void DoseReserveProperlyStoreChangedItems()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy(Guid.NewGuid())
            {
                Id = Guid.NewGuid(),
                Name = "Bob"
            };
            cache.Insert(a);

            // Act
            a.Name = "Phil";
            cache.Insert(a);
            var result = cache.Fetch(a.Instance);

            // Assert
            Assert.IsTrue(result.Name == "Phil");
        }

        [TestMethod]
        public void DoseReserveProperlyStoreRevisedItems()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy(Guid.NewGuid());
            var b = new Toy(Guid.NewGuid());

            a.Name = "Bob";
            b.Name = "George";

            // Act
            cache.Insert(a);
            cache.Insert(b);
            a.Name = "Phil";
            cache.Insert(a);
            var elementZero = cache.Revisions(a).ElementAt(0);
            var elementOne = cache.Revisions(a).ElementAt(1);

            // Assert
            Assert.IsTrue(elementZero.Name != elementOne.Name);
        }

        [TestMethod]
        public void DoseReserveRetrievedCorrectCountOfRevisedItems()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy(Guid.NewGuid());
            var b = new Toy(Guid.NewGuid());

            a.Name = "Bob";
            b.Name = "George";

            // Act
            cache.Insert(a);
            cache.Insert(b);
            a.Name = "Phil";
            cache.Insert(a);
            var result = cache.Revisions(a);

            // Assert
            Assert.IsTrue(result.Count() == 2);
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
        public Toy(Guid instance)
        {
            Instance = instance;
        }

        public Guid Id { get; set; }

        public Guid Instance { get; protected set; }

        public string Name { get; set; }

        public DateTime Stamp { get; set; }

        public Toy Copy(Toy instance)
        {
            var result = new Toy(instance.Instance)
            {
                Id = Guid.NewGuid(),
                Name = string.Copy(instance.Name),
                Stamp = DateTime.UtcNow
            };

            return result;
        }
    }
}