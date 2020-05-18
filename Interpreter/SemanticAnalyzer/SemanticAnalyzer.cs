using DenysRedkoParser.Interpreter.SemanticAnalyzer.Handlers;
using DenysRedkoParser.Interpreter.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.SemanticAnalyzer
{
    public class SemanticAnalyzer
    {
        public DeclareListHandler DeclareListHandler { get; }
        public ConstantsTableHandler ConstantsTableHandler { get; }
        private AssignHandler _assignHandler;
        public List<IToken> RPNCode { get; set; }

        public SemanticAnalyzer()
        {
            DeclareListHandler = new DeclareListHandler();
            ConstantsTableHandler = new ConstantsTableHandler();
            RPNCode = new List<IToken>();
        }

        public void Process(Node<Token> node)
        {
            var declareList = buildIdentifiersTable(node);
            var constantList = buildConstantsTable(node);
            _assignHandler = new AssignHandler(constantList, declareList);
            buildRPNCode(node);
        }

        private ConstantsTable buildConstantsTable(Node<Token> node)
        {
            var statementList = Tree<Token>.FindFirstWithName("statementList", node);
            if (statementList == null)
                throw new Exception("fail to find statmentList");

            ConstantsTableHandler.Handle(statementList);
            return ConstantsTableHandler.ConstantsTable;
        }

        private IdentifiersTable buildIdentifiersTable(Node<Token> node)
        {
            var declareList = Tree<Token>.FindFirstWithName("declareList", node);
            if (declareList == null)
                throw new Exception("fail to find declareList");

            DeclareListHandler.Handle(declareList);
            return DeclareListHandler.IdentifiersTable;
        }

        private void buildRPNCode(Node<Token> node)
        {
            var statments = Tree<Token>.FindAllWithName("statement", node);
            foreach (var item in statments)
            {
                if (item.nodesLinks[0].Data.Content == "Assign")
                {
                    RPNCode.Add(_assignHandler.Handle(item.nodesLinks[0]));
                }
            }
        }

        public void PrintRPNCodes()
        {
            var formatString = string.Format("{{0, -{0}}}", 10);
            Console.WriteLine("Polis Table");

            foreach (var code in RPNCode)
            {
                Console.Write($"({code.Content} : {code.Type})");
            }
            Console.WriteLine("");

        }
    }
}
