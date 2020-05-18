using DenysRedkoParser.Interpreter.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.SemanticAnalyzer.Handlers
{
    public class DeclareListHandler
    {
        public DeclareListHandler()
        {
            IdentifiersTable = new IdentifiersTable();
        }

        public IdentifiersTable IdentifiersTable { get; set; }

        public void Handle(Node<Token> node)
        {
            DeclareList(node);
        }

        private void DeclareList(Node<Token> node) 
        {
            var childrens = node.nodesLinks;
            if (childrens.Count == 0)
                throw new Exception("Fail to find declare List");

            foreach (var item in childrens)
            {
                if (item.Data.Content == "declaration")
                {
                    Declaration(item);
                }
            }

        }

        private void Declaration(Node<Token> node)
        {
            var childrens = node.nodesLinks;
            if (childrens.Count == 0)
                throw new Exception("Fail to find declare List");

            var type = "";
            var identifiers = new List<string>();

            foreach (var child  in childrens)
            {
                if (child.Data.Content == "identList")
                {
                    identifiers = IdentList(child);
                }
                else if(child.Data.Type == "Type")
                {
                    type = child.Data.Content;
                }
            }

            foreach (var item in identifiers)
            {
                IdentifiersTable.AddIdentifier(item, type);
            }
        }

        private List<string> IdentList(Node<Token> node)
        {
            var childrens = node.nodesLinks;
            if (childrens.Count == 0)
                throw new Exception("Fail to find declare List");

            var identifiers = new List<string>();

            foreach (var item in childrens)
            {
                if (item.Data.Type == "Ident")
                {
                    identifiers.Add(item.Data.Content);
                }
            }

            return identifiers;
        }
    }
}
