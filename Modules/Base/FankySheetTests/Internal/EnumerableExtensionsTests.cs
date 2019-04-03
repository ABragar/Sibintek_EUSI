using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace FankySheet.Internal.Tests
{
    [TestClass()]
    public class EnumerableExtensionsTests
    {





        [TestMethod()]
        public void PartByCountTest()
        {


            Assert.AreEqual(Enumerable.Range(0, 6).PartByCount(2).SelectMany(x => x).Count(), 6);

        }
        [TestMethod()]
        public void PartByCountTest2()
        {

            Assert.AreEqual(Enumerable.Range(0, 6).PartByCount(2).Select(x => x.First()).Count(), 3);

        }

        [TestMethod()]
        public void PartByCountTest2x()
        {


            Assert.AreEqual(Enumerable.Range(0, 6).PartByCount(2).Skip(1).First().Count(), 2);


        }

        [TestMethod()]
        public void PartByCountTest3()
        {
            var parts = Enumerable.Range(0, 6).PartByCount(2);

            WriteDebug(parts);
        }



        [TestMethod()]
        public void PartByCountTest4()
        {




            Assert.AreEqual(Enumerable.Range(0, 6).PartByCount(2).Count(), 3);


        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PartByCountTest5()
        {
            var parts = Enumerable.Range(0, 6).PartByCount(2).ToArray();

            WriteDebug(parts);

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PartByCountTest5x()
        {
            var parts = Enumerable.Range(0, 10).PartByCount(2).Take(3).ToArray().Skip(1);

            WriteDebug(parts);

        }

        private void WriteDebug<TElement>(IEnumerable<IEnumerable<TElement>> elements)
        {
            var str = string.Join(" ", elements.Select(x => "<" + string.Join(" ", x) + ">"));

            Debug.WriteLine(str);
        }


        [TestMethod()]

        public void PartByCountTest6()
        {
            Assert.AreEqual(Enumerable.Range(0, 5).Concat(Throw<int>()).PartByCount(2).Take(3).Count(), 3);
        }

        private IEnumerable<T> Throw<T>()
        {
            throw new Exception();
            yield break;
        }
    }
}