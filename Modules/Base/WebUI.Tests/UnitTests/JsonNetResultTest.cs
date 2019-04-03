using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUI.Helpers;

namespace WebUI.Tests.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        ///     Проверка ограничения глубины вложенности при сериализации
        /// </summary>
        [TestMethod]
        public void DepthTest()
        {
            for (var levels = 0; levels < 8; levels++)
            {
                var multilevelObject = new MultilevelObject();
                AddSubLevel(multilevelObject, 0, levels);

                var jsonNetResult = new JsonNetResult(multilevelObject);
                var context = new ControllerContext
                {
                    HttpContext =
                        new HttpContextWrapper(new HttpContext(new HttpRequest("", "http://localhost", ""),
                            new HttpResponse(new StringWriter())))
                };
                jsonNetResult.ExecuteResult(context);

                var expectedResult = ExpectedResult(levels + 1);

                Assert.AreEqual(expectedResult, context.HttpContext.Response.Output.ToString(),
                    "Failed at depth " + levels + " with limit ");// + JsonNetResult.DepthLimit);
            }
        }

        private static void AddSubLevel(MultilevelObject upperLevel, int depth, int limit)
        {
            upperLevel.Data = depth;
            if (depth >= limit) return;
            upperLevel.Sublevel = new MultilevelObject();
            AddSubLevel(upperLevel.Sublevel, ++depth, limit);
        }

        private static string ExpectedResult(int depth)
        {
            var nullIsRequired = true;
            if (depth > 4)//JsonNetResult.DepthLimit)
            {
                depth = 4;// JsonNetResult.DepthLimit;
                nullIsRequired = false;
            }

            var result = new StringBuilder();
            for (var level = 0; level < depth; level++)
                result.Append(
                    "{\"Data\":" + level + ",\"Sublevel\":");

            result.Append(nullIsRequired ? "null" : "{");
            for (var level = nullIsRequired ? 0 : -1; level < depth; level++)
                result.Append("}");

            return result.ToString();
        }

        private class MultilevelObject
        {
            public int Data { get; set; }

            public MultilevelObject Sublevel { get; set; }
        }
    }
}