using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DenysRedkoParser
{
    [Serializable]
    public class Node<T> : ISerializable where T : IToken  
    {
        public T Data { get; set; }


        public List<Node<T>> nodesLinks { get; set; }

        public Node(T data)
        {
            nodesLinks = new List<Node<T>>();
            Data = data;
        }

        public Node()
        {
        }

        public void Add (Node<T> node)
        {
            nodesLinks.Add(node);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("nodes", nodesLinks);
            info.AddValue("type", Data.Type);
            info.AddValue("Content", Data.Content);
        }

        public bool HasChild(string name)
        {
            foreach (var item in nodesLinks)
            {
                if (item.Data.Content == name || item.Data.Type == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
