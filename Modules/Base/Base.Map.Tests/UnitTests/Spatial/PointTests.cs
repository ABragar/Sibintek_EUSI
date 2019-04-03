using Base.Map.Spatial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base.Map.Tests.UnitTests.Spatial
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void TestConstructor()
        {
            // Arrange
            const double x = 1D;
            const double y = 2D;
            var point = new Point(x, y);

            // Act and Assert
            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }

        [TestMethod]
        public void TestZeroPoint()
        {
            // Arrange
            var zeroPoint = new Point();

            // Act and Assert
            Assert.AreEqual(0, zeroPoint.X);
            Assert.AreEqual(0, zeroPoint.Y);
        }

        [TestMethod]
        public void TestEquals()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(1, 2);
            var point3 = new Point(1, 2);
            var point4 = new Point(2, 3);

            // Act and Assert
            Assert.IsTrue(point1.Equals(point1));

            Assert.IsTrue(point1.Equals(point2));
            Assert.IsTrue(point2.Equals(point1));

            Assert.IsTrue(point1.Equals(point2));
            Assert.IsTrue(point2.Equals(point3));
            Assert.IsTrue(point1.Equals(point3));

            Assert.IsFalse(point1.Equals(point4));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            // Arrange
            var point1 = new Point(1, 2);
            var point2 = new Point(1, 2);
            var point3 = new Point(2, 3);

            // Act and Assert
            Assert.IsTrue(point1.GetHashCode() == point2.GetHashCode());
            Assert.IsTrue(point1.GetHashCode() != point3.GetHashCode());
        }
    }
}