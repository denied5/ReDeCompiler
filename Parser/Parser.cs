using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser
{
    class Parser
    {
        private TokenStream _tokenStream;
        public Tree<Token> _tree;

        public Parser(TokenStream tokenStream)
        {
            _tokenStream = tokenStream;
            _tree = new Tree<Token>();
        }

        public Tree<Token> BuildTree()
        {

            Node<Token> root = new Node<Token>(new Token { Content = "Program" });
            _tree.AddRoot(root);

            _tree.root.Add(MatchOrFail("Program"));
            _tree.root.Add(MatchOrFail("Ident"));
            _tree.root.Add(MatchOrFail("Var"));
            _tree.root.Add(DeclareList());
            _tree.root.Add(MatchOrFail("Semi"));
            _tree.root.Add(MatchOrFail("Begin"));
            _tree.root.Add(StatementList());
            _tree.root.Add(MatchOrFail("Semi"));
            _tree.root.Add(MatchOrFail("End"));
            _tree.root.Add(MatchOrFail("EOF"));
            return _tree;
        }

        private Node<Token> Match(string lexem)
        {
            if(_tokenStream.LookAhead().Type == lexem)
            {
                return new Node<Token>(_tokenStream.Next());
            }
            return null;
        }

        private Node<Token> MatchOrFail(string lexem)
        {
            var result = Match(lexem);
            if (result != null)
            {
                return result;
            }
            throw new Exception($"Expect {lexem} but {_tokenStream.LookAhead().Type} in line 4");
        }

        private Node<Token> MatchOneOfLexeme(string[] lexems)
        {
            foreach (var lexem in lexems)
            {
                var result = Match(lexem);
                if (result != null)
                {
                    return result;
                }
            }

            throw new Exception($"MatchOneOfLexeme error");
        }

        private Node<Token> MatchOneOfRules(Func<Node<Token>>[] rules)
        {
            foreach (var rule in rules)
            {
                var result = MatchRule(rule);
                if (result != null)
                {
                    return result;
                }
            }

            throw new Exception("We dont't found any rules");
        }

        private Node<Token> MatchRule(Func<Node<Token>> rule)
        {
            var position = _tokenStream.Remember();
            object[] userParams = new object[0];
            try
            {
                return rule();
            }
            catch (Exception e)
            {
                _tokenStream.GoTo(position);
            }

            return null;
        }

        private Node<Token> DeclareList()
        {
            var node = new Node<Token>(new Token { Content = "declareList" });
            node.Add(Declaration());

            RepeatedMatch(node, xNode =>
            {
                var semiNode = MatchOrFail("Semi");
                var declarationNode = Declaration();
                node.Add(semiNode);
                node.Add(declarationNode);
            });
            return node;
        }

        private Node<Token> Declaration()
        {
            var node = new Node<Token>(new Token { Content = "declaration" });
            node.Add(IdentList());
            node.Add(MatchOrFail("Colon"));
            node.Add(MatchOrFail("Type"));
            return node;
        }

        private Node<Token> IdentList()
        {
            var node = new Node<Token>(new Token { Content = "identList" });
            node.Add(MatchOrFail("Ident"));

            RepeatedMatch(node, xNode =>
            {
                var comaNode = MatchOrFail("Comma");
                var identNode = MatchOrFail("Ident");
                node.Add(comaNode);
                node.Add(identNode);
            });

            return node;
        }

        private void RepeatedMatch(Node<Token> node, Action<Node<Token>> action)
        {
            while (true)
            {
                var position = _tokenStream.Remember();
                try
                {
                    action(node);
                }
                catch (Exception e)
                {
                    _tokenStream.GoTo(position);
                    break;
                }
            }
        }

        private Node<Token> StatementList()
        {
            var node = new Node<Token>(new Token { Content = "statementList" });
            node.Add(Statement());
            RepeatedMatch(node, xNode =>
                {
                    var semiNode = MatchOrFail("Semi");
                    var statmentNode = Statement();
                    node.Add(semiNode);
                    node.Add(statmentNode);
                }
            );

            return node;
        }

        private Node<Token> Statement()
        {
            var node = new Node<Token>(new Token { Content = "statement" });
            node.Add(MatchOneOfRules(new Func<Node<Token>>[] {Input, Output, Assign, ForStatement, BranchStatement }));
            return node;
        }

        private Node<Token> BranchStatement()
        {
            var node = new Node<Token>(new Token { Content = "BranchStatement" });
            node.Add(MatchOrFail("If"));
            node.Add(Expression());
            node.Add(MatchOrFail("Then"));
            node.Add(StatementList());
            node.Add(MatchOrFail("Semi"));
            node.Add(MatchOrFail("Fi"));
            return node;
        }

        private Node<Token> ForStatement()
        {
            var node = new Node<Token>(new Token { Content = "ForStatement" });
            node.Add(MatchOrFail("For"));
            node.Add(MatchOrFail("Ident"));
            node.Add(MatchOrFail("By"));
            node.Add(ArithmExpression());
            node.Add(MatchOrFail("To"));
            node.Add(ArithmExpression());
            node.Add(MatchOrFail("Do"));
            node.Add(StatementList());
            node.Add(MatchOrFail("Semi"));
            node.Add(MatchOrFail("Rof"));
            return node;
        }

        private Node<Token> Assign()
        {
            var node = new Node<Token>(new Token { Content = "Assign" });
            node.Add(MatchOrFail("Ident"));
            node.Add(MatchOrFail("AssignOp"));
            node.Add(Expression());
            return node;
        }

        private Node<Token> Expression()
        {
            var node = new Node<Token>(new Token { Content = "Expression" });
            node.Add(MatchOneOfRules(new Func<Node<Token>>[] { BoolExpr, ArithmExpression}));
            return node;
        }

        private Node<Token> BoolExpr()
        {
            var node = new Node<Token>(new Token { Content = "BoolExpr" });
            node.Add(ArithmExpression());
            node.Add(MatchOrFail("RelOp"));
            node.Add(ArithmExpression());
            return node;
        }

        private Node<Token> ArithmExpression()
        {
            var node = new Node<Token>(new Token { Content = "ArithmExpression" });
            var position = _tokenStream.Remember();
            try
            {
                node.Add(Term());
                node.Add(AddOp());
                node.Add(ArithmExpression());
            }
            catch (Exception e)
            {
                _tokenStream.GoTo(position);
                node.nodesLinks = new List<Node<Token>>();
                node.Add(Term());
            }

            return node;
        }

       

        private Node<Token> Term()
        {
            var node = new Node<Token>(new Token { Content = "Term" });
            var position = _tokenStream.Remember();
            try
            {
                node.Add(Factor());
                node.Add(MultOp());
                node.Add(Term());
            }
            catch (Exception e)
            {
                _tokenStream.GoTo(position);
                node.nodesLinks = new List<Node<Token>>();
                node.Add(Factor());
            }
            return node;
        }

       

        private Node<Token> Factor()
        {
            var node = new Node<Token>(new Token { Content = "Factor" });
            if (_tokenStream.LookAhead().Type == "Ident")
            {
                node.Add(Match("Ident"));
                return node;
            }

            var matchRule = MatchRule(Constant);
            if (matchRule!= null)
            {
                node.Add(matchRule);
                return node;
            }

            try
            {
                node.Add(MatchOrFail("LBracket"));
                node.Add(ArithmExpression());
                node.Add(MatchOrFail("RBracket"));
            }
            catch (Exception e)
            {

                throw new Exception("factor Exception");
            }
            return node;
        }

        private Node<Token> Constant()
        {
            var node = new Node<Token>(new Token { Content = "Constant" });
            node.Add(MatchOneOfLexeme(new string[] { "ExponNum", "IntNum", "RealNum", "BoolConst" }));
            return node;
        }

        private Node<Token> AddOp()
        {
            var node = new Node<Token>(new Token { Content = "AddOp" });
            node.Add(MatchOneOfLexeme(new string[] { "Plus", "Minus" }));
            return node;
        }

        private Node<Token> MultOp()
        {
            var node = new Node<Token>(new Token { Content = "MultOp" });
            node.Add(MatchOneOfLexeme(new string[] { "Star", "Slash", "Caret" }));
            return node;
        }

        private Node<Token> Input()
        {
            var node = new Node<Token>(new Token { Content = "Read" });
            node.Add(MatchOrFail("Read"));
            node.Add(MatchOrFail("LBracket"));
            node.Add(IdentList());
            node.Add(MatchOrFail("RBracket"));
            return node;
        }

        private Node<Token> Output()
        {
            var node = new Node<Token>(new Token { Content = "Write" });
            node.Add(MatchOrFail("Write"));
            node.Add(MatchOrFail("LBracket"));
            node.Add(IdentList());
            node.Add(MatchOrFail("RBracket"));
            return node;
        }
    }
}
