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
        public void DoseCacheProperlyStoreRevisionsOfTwoDistinctInstances()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var firstInstance = new Toy(Guid.NewGuid()) { Name = "Bob" };
            var secondInstance = new Toy(Guid.NewGuid()) { Name = "George" };
            cache.Insert(firstInstance);
            cache.Insert(secondInstance);

            // Act
            firstInstance.Name = "Robert";
            secondInstance.Name = "G";
            cache.Insert(firstInstance);
            cache.Insert(secondInstance);

            // Assert
            Assert.IsTrue(cache.Fetch(firstInstance).Name == "Robert");
            Assert.IsTrue(cache.Fetch(secondInstance).Name == "G");
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
            var a = new Toy(Guid.NewGuid()) { Name = "Bob" };
            cache.Insert(a);

            // Act
            a.Name = "Phil";
            cache.Insert(a);

            // Assert
            Assert.IsTrue(cache.Revisions(a).ElementAt(0).Name != cache.Revisions(a).ElementAt(1).Name);
        }

        [TestMethod]
        public void DoseReserveProperlyStoreRevisionsOfTwoDistinctInstances()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var firstInstance = new Toy(Guid.NewGuid()) { Name = "Bob" };
            var secondInstance = new Toy(Guid.NewGuid()) { Name = "George" };
            cache.Insert(firstInstance);
            cache.Insert(secondInstance);

            // Act
            firstInstance.Name = "Robert";
            secondInstance.Name = "G";
            cache.Insert(firstInstance);
            cache.Insert(secondInstance);

            // Assert
            Assert.IsTrue(cache.Revisions(firstInstance).ElementAt(0).Name == "Robert");
            Assert.IsTrue(cache.Revisions(secondInstance).ElementAt(0).Name == "G");
        }

        [TestMethod]
        public void DoseReserveRetrievedCorrectCountOfRevisedItems()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy(Guid.NewGuid()) { Name = "Bob" };
            cache.Insert(a);

            // Act
            a.Name = "Phil";
            cache.Insert(a);

            // Assert
            Assert.IsTrue(cache.Revisions(a).Any());
            Assert.IsTrue(cache.Revisions(a).Count() == 2);
        }

        [TestMethod]
        public void DoseReserveReturnRevisionsSortedFromMostRecentToLeastRecent()
        {
            // Arrange
            var cache = new Reserve<Toy>();
            var a = new Toy(Guid.NewGuid()) { Name = "Bob" };
            cache.Insert(a);

            // Act
            a.Name = "Phil";
            cache.Insert(a);

            // Assert
            Assert.IsTrue(cache.Revisions(a).ElementAt(0).Name == "Phil");
            Assert.IsFalse(cache.Revisions(a).ElementAt(1).Name == "Phil");
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