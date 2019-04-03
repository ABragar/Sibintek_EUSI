using FankySheet.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FankySheetTests.Internal
{

    [TestClass()]
    public class SheetUtilsTests
    {

        [TestMethod]

        public void CellReferenceTest0()
        {


            Assert.AreEqual(SheetUtils.GetColumnLetter(0), "A");
            
        }


        [TestMethod]

        public void CellReferenceTest25()
        {



            Assert.AreEqual(SheetUtils.GetColumnLetter(25), "Z");

        }
        [TestMethod]

        public void CellReferenceTest26()
        {



            
            Assert.AreEqual(SheetUtils.GetColumnLetter(26), "AA");
            

        }
        [TestMethod]

        public void CellReferenceTest27()
        {



            Assert.AreEqual(SheetUtils.GetColumnLetter(27), "AB");

        }
        [TestMethod]

        public void CellReferenceTest702()
        {



            Assert.AreEqual(SheetUtils.GetColumnLetter(702), "AAA");
            
        }

        [TestMethod]

        public void CellReferenceTest1234()
        {



            Assert.AreEqual(SheetUtils.GetColumnLetter(1234), "AUM");

        }
        [TestMethod]

        public void CellReferenceTest701()
        {



            Assert.AreEqual(SheetUtils.GetColumnLetter(701), "ZZ");

        }
        [TestMethod]

        public void CellReferenceTest26x()
        {


            Assert.AreEqual(SheetUtils.GetColumnLetter(26, "1"), "AA1");

        }
    }
}