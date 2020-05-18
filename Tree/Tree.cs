using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser
{
    [Serializable]
    public class Tree<T> where T: IToken
    {
        public Node<T> root { get; set; }

        public Tree()
        {
        }

        public Tree(Node<T> root)
        {
            this.root = root;
        }

        public void AddRoot(Node<T> root)
        {
            this.root = root;
        }

        public static Node<T> FindFirstWithName(string name, Node<T> root)
        {
            if (root.Data.Content == name)
                return root;

            if (root.nodesLinks.Count > 0)
            {
                foreach (var child in root.nodesLinks)
                {
                    if (FindFirstWithName(name, child) != null)
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        internal static List<Node<T>> FindAllWithName(string name, Node<T> node)
        {
            var nodes = new List<Node<T>>();
            if (node.Data.Content == name)
                nodes.Add(node);

            if (node.nodesLinks.Count > 0)
            {
                foreach (var child in node.nodesLinks)
                {
                    foreach (var item in FindAllWithName(name, child))
                    {
                        nodes.Add(item);
                    }
                }
            }

            return nodes;
        }
    }
}
