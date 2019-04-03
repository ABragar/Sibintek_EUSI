using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace EUSI.Services.Estate
{
    public class GraphNode<T>: Node<T>
    {
        private List<Predicate<T>> _costs;

        public GraphNode() : base() { }
        public GraphNode(T value) : base(value) { }
        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

        new public NodeList<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>();

                return base.Neighbors;
            }
        }

        public List<Predicate<T>> Costs
        {
            get
            {
                if (_costs == null)
                    _costs = new List<Predicate<T>>();

                return _costs;
            }
        }
    }

    public class Graph<T>
    {
        private NodeList<T> _nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this._nodeSet = new NodeList<T>();
            else
                this._nodeSet = nodeSet;
        }

        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            _nodeSet.Add(node);
        }


        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, Predicate<T> cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, Predicate<T> cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public bool Contains(T value)
        {
            return _nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)_nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            _nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (var node in _nodeSet)
            {
                var gnode = (GraphNode<T>) node;
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public NodeList<T> Nodes
        {
            get
            {
                return _nodeSet;
            }
        }

        public int Count
        {
            get { return _nodeSet.Count; }
        }
    }

    public class Node<T>
    {
        // Private member-variables
        private T _data;
        private NodeList<T> _neighbors = null;

        public Node() { }
        public Node(T data) : this(data, null) { }
        public Node(T data, NodeList<T> neighbors): this()
        {
            this._data = data;
            this._neighbors = neighbors;
        }

        public T Value
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        protected NodeList<T> Neighbors
        {
            get
            {
                return _neighbors;
            }
            set
            {
                _neighbors = value;
            }
        }
    }

    public class NodeList<T>: Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (var node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }

}
