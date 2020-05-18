using DenysRedkoParser.Interpreter.Operators;
using DenysRedkoParser.Interpreter.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.SemanticAnalyzer.Handlers
{
    class AssignHandler
    {
        private List<IToken> result;
        private ConstantsTable _constantsTable;
        private IdentifiersTable _identifiersTable;

        public AssignHandler(ConstantsTable constantsTable, IdentifiersTable identifiersTable)
        {
            _constantsTable = constantsTable;
            _identifiersTable = identifiersTable;
        }

        public List<IToken> Handle(Node<Token> node)
        {
            result = new List<IToken>();
            Assign(node);
            return result;
        }

        private void Assign(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            Expression(childrens[2]);
            result.Add(Ident(childrens[0]));
            result.Add(AssignOp(childrens[1]));
        }

        private Operator AssignOp(Node<Token> node)
        {
            var op = node.Data.Content;
            var type = node.Data.Type;
            var line = node.Data.Line;
            return new BinaryOperator(op, type, line);
        }

        private void Expression(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            if (childrens[0].Data.Content == "BoolExpr")
            {
                BoolExpr(childrens[0]);
            }
            else if (childrens[0].Data.Content == "ArithmExpression")
            {
                ArithmEpression(childrens[0]);
            }
            else
            {
                throw new Exception("Invalid children");
            }
        }

        private void ArithmEpression(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            if (node.HasChild("AddOp"))
            {
                ArithmEpression(childrens[2]);
                Term(childrens[0]);
                result.Add(AddOp(childrens[1]));
                return;
            }

            Term(childrens[0]);
        }

        private Operator AddOp(Node<Token> node)
        {
            var op = node.nodesLinks[0].Data.Content;
            var type = node.nodesLinks[0].Data.Type;
            var line = node.nodesLinks[0].Data.Line;
            return new BinaryOperator(op, type, line);
        }

        private void Term(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            if (node.HasChild("MultOp"))
            {
                Term(childrens[2]);
                Factor(childrens[0]);
                result.Add(MultOp(childrens[1]));
                return;
            }

            Factor(childrens[0]);
        }

        private Operator MultOp(Node<Token> node)
        {
            var op = node.nodesLinks[0].Data.Content;
            var type = node.nodesLinks[0].Data.Type;
            var line = node.nodesLinks[0].Data.Line;
            return new BinaryOperator(op, type, line);
        }

        private void Factor(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            if (node.HasChild("ArithmExpression"))
            {
                ArithmEpression(childrens[1]);
            }
            else if(node.HasChild("Ident"))
            {
                result.Add(Ident(childrens[0]));
            }
            else
            {
                Constant(childrens[0]);
            }
        }
        private void Constant(Node<Token> node)
        {
            if (node.Data.Content == "BoolConst")
            {
                var value = node.Data.Content;
                result.Add(FindConstant(value));
            }

            IntOrRealNum(node);
        }

        private void IntOrRealNum(Node<Token> child)
        {
            var value = child.nodesLinks[0].Data.Content;
            result.Add(FindConstant(value));
        }

        private Constant FindConstant(string value)
        {
            var constant = _constantsTable.Find(value);
            if (constant != null)
            {
                return constant;
            }

            throw new Exception("Cannot find constant");
        }

        private IToken Ident(Node<Token> node)
        {
            var name = node.Data.Content;
            return FindIdentifier(name);
        }

        private IToken FindIdentifier(string name)
        {
            var identifier = _identifiersTable.FindByName(name);
            if (identifier != null)
            {
                return identifier;
            }
            throw new Exception($"Cannot find identifier {name} in line 7");
        }

        private void BoolExpr(Node<Token> node)
        {
            var childrens = node.nodesLinks;

            ArithmEpression(childrens[2]);
            ArithmEpression(childrens[0]);
            result.Add(RealOp(childrens[1]));
        }

        private IToken RealOp(Node<Token> node)
        {
            var name = node.Data.Content;
            var type = node.Data.Type;
            var line = node.Data.Line;
            return new BinaryOperator(name, type, line);
        }
    }
}
