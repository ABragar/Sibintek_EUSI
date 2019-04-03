using System;
using System.Linq;
using Base.Attributes;
using EUSI.Entities.Estate;
using EUSI.Services.Estate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EUSI.Tests
{
    [TestClass]
    public class BPTest
    {
        [TestMethod]
        public void NodeCreationTest()
        {
            Graph<string> web = new Graph<string>();

            var node1 = new GraphNode<string>("Направлена на проверку");
            var node2 = new GraphNode<string>("Требуется уточнения");
            var node3 = new GraphNode<string>("Заявка отклонена");

            web.AddDirectedEdge(node1, node2, s => true);
            web.AddDirectedEdge(node2, node3, s => true);
            web.AddDirectedEdge(node3, node1, s => true);



        }
    }
}
