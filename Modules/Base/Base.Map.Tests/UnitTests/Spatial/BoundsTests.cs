using Base.Map.Spatial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base.Map.Tests.UnitTests.Spatial
{
    [TestClass]
    public class BoundsTests
    {
        [TestMethod]
        public void TestMinMaxPoints()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(2, 3);
            var bounds = new Bounds(point1, point2);

            // Act and Assert
            Assert.IsTrue(bounds.Min.Equals(new Point(1, 2)));
            Assert.IsTrue(bounds.Max.Equals(new Point(2, 3)));
        }

        [TestMethod]
        public void TestMinMaxPointsAsCollection()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(2, 3);
            var point3 = new Point(3, 1);
            var bounds = new Bounds(new[] { point1, point2, point3 });

            // Act and Assert
            Assert.IsTrue(bounds.Min.Equals(new Point(1, 1)));
            Assert.IsTrue(bounds.Max.Equals(new Point(3, 3)));
        }

        [TestMethod]
        public void TestEmpty()
        {
            // Arrange
            var zeroPoint = new Point(0, 0);
            var emptyBounds1 = new Bounds();
            var emptyBounds2 = Bounds.Empty;

            // Act and Assert
            Assert.IsTrue(emptyBounds1.Min.Equals(zeroPoint));
            Assert.IsTrue(emptyBounds1.Max.Equals(zeroPoint));

            Assert.IsTrue(emptyBounds2.Min.Equals(zeroPoint));
            Assert.IsTrue(emptyBounds2.Max.Equals(zeroPoint));
        }

        [TestMethod]
        public void TestCenter()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(2, 3);
            var bounds = new Bounds(point1, point2);

            // Act
            var center = bounds.Center;

            // Assert
            Assert.AreEqual(1.5, center.X);
            Assert.AreEqual(2.5, center.Y);
        }

        [TestMethod]
        public void TestContains()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(2, 3);
            var bounds = new Bounds(point1, point2);

            var containPoint = new Point(1, 3);
            var notContainPoint = new Point(1, 4);

            // Act and Assert
            Assert.IsTrue(bounds.Contains(containPoint));
            Assert.IsFalse(bounds.Contains(notContainPoint));
            Assert.IsTrue(bounds.Contains(point1));
            Assert.IsTrue(bounds.Contains(point2));
        }

        [TestMethod]
        public void TestExtendEmptyBounds()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(2, 3);
            var point3 = new Point(3, 1);
            var bounds = new Bounds();

            // Act
            bounds.Extend(point1);
            var extend1 = bounds;

            bounds.Extend(point2);
            var extend2 = bounds;

            bounds.Extend(point3);
            var extend3 = bounds;

            // Assert
            Assert.IsTrue(extend1.Min.Equals(new Point(1, 2)));
            Assert.IsTrue(extend1.Max.Equals(new Point(1, 2)));

            Assert.IsTrue(extend2.Min.Equals(new Point(1, 2)));
            Assert.IsTrue(extend2.Max.Equals(new Point(2, 3)));

            Assert.IsTrue(extend3.Min.Equals(new Point(1, 1)));
            Assert.IsTrue(extend3.Max.Equals(new Point(3, 3)));
        }

        [TestMethod]
        public void TestExtendNotEmptyBounds()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(2, 3);
            var point3 = new Point(3, 1);
            var bounds = new Bounds(point1, point2);

            // Act
            var beforeExtend = bounds;

            bounds.Extend(point3);
            var extendedBounds = bounds;

            // Assert
            Assert.IsTrue(beforeExtend.Min.Equals(new Point(1, 2)));
            Assert.IsTrue(beforeExtend.Max.Equals(new Point(2, 3)));

            Assert.IsTrue(extendedBounds.Min.Equals(new Point(1, 1)));
            Assert.IsTrue(extendedBounds.Max.Equals(new Point(3, 3)));
        }

        [TestMethod]
        public void TestEquals()
        {
            // Arrange
            var bounds1 = new Bounds(new Point(1, 2), new Point(2, 3));
            var bounds2 = new Bounds(new Point(1, 2), new Point(2, 3));
            var bounds3 = new Bounds(new Point(1, 2), new Point(2, 3));

            // Act and Assert
            Assert.IsTrue(bounds1.Equals(bounds1));

            Assert.IsTrue(bounds1.Equals(bounds2));
            Assert.IsTrue(bounds2.Equals(bounds1));

            Assert.IsTrue(bounds1.Equals(bounds2));
            Assert.IsTrue(bounds2.Equals(bounds3));
            Assert.IsTrue(bounds1.Equals(bounds3));

            bounds3.Extend(new Point(1, 4));
            Assert.IsFalse(bounds1.Equals(bounds3));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            // Arrange
            var bounds1 = new Bounds(new Point(1, 2), new Point(2, 3));
            var bounds2 = new Bounds(new Point(1, 2), new Point(2, 3));
            var bounds3 = new Bounds(new Point(1, 3), new Point(2, 4));

            // Act and Assert
            Assert.IsTrue(bounds1.GetHashCode() == bounds2.GetHashCode());
            Assert.IsTrue(bounds1.GetHashCode() != bounds3.GetHashCode());
        }
    }
}