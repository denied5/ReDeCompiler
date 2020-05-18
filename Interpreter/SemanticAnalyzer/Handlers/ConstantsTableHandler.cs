using DenysRedkoParser.Interpreter.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.SemanticAnalyzer.Handlers
{
    public class ConstantsTableHandler
    {
        public ConstantsTable ConstantsTable { get; set; }

        public void Handle(Node<Token> node)
        {
            ConstantsTable = new ConstantsTable();
            StatementList(node);
        }

        private void StatementList(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            foreach (var item in childrens)
            {
                if (item.Data.Content == "statement")
                    Statement(item);
            }
        }

        private void Statement(Node<Token> node)
        {
            var childrens = FindConstants(node);

            foreach (var item in childrens)
            {
                Constant(item);
            }
        }

        private List<Node<Token>> FindConstants(Node<Token> node)
        {
            if (node.Data.Content == "Constant")
            {
                return new List<Node<Token>>() { node};
            }

            var constants = new List<Node<Token>>();
            var childrens = node.nodesLinks;
            if (childrens != null && childrens.Count > 0)
            {
                foreach (var item in childrens)
                {
                    constants.AddRange(FindConstants(item));
                }
            }
            return constants;
        }

        private void Constant(Node<Token> node)
        {
            var firstChild = node.nodesLinks[0];
            var name = firstChild.Data.Type;

            //if (name == "IntNum" || name == "RealNum")
            //{
            //    IntOrRealNum(firstChild);
            //    return;
            //}
            var type = NodeToType(firstChild);
            var value = firstChild.Data.Content;
            ConstantsTable.AddConstant(value, type);
        }

        private string NodeToType(Node<Token> node)
        {
            var type = node.Data.Type;

            if (type == "IntNum")
                return "int";

            if (type == "UnsignedReal")
                return "real";

            if (type == "RealNum")
                return "real";

            if (type == "BoolConst")
                return "bool";

            if (type == "ExponNum")
                return "exponent";

            throw new Exception("Unknown type");
        }

        //private void IntOrRealNum(Node<Token> node)
        //{
        //    Node<Token> num;
        //    var childrens = node.nodesLinks;
        //    if (childrens.Count == 2)
        //        num = childrens[1];
        //    else
        //        num = childrens[0];

        //    var type = NodeToType(num);
        //    var value = num.Data.Content;
        //    ConstantsTable.AddConstant(value, type);
        //}
    }
}
