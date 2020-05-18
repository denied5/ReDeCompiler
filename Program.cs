using DenysRedkoParser.Interpreter;
using DenysRedkoParser.Interpreter.SemanticAnalyzer;
using System;
using System.IO;
using System.Text.Json;

namespace DenysRedkoParser
{
    class Program
    {
        static void Main(string[] args)
        {
            TokenStream tokenStream = new TokenStream("../../../output.json");
            Parser parser = new Parser(tokenStream);
            var tree = parser.BuildTree();

            SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer();
            semanticAnalyzer.Process(tree.root);

            semanticAnalyzer.DeclareListHandler.IdentifiersTable.ToString();
            semanticAnalyzer.ConstantsTableHandler.ConstantsTable.ToString();
            semanticAnalyzer.PrintRPNCodes();

            LangInterpreter interpreter = new LangInterpreter(semanticAnalyzer.DeclareListHandler.IdentifiersTable, semanticAnalyzer.ConstantsTableHandler.ConstantsTable, semanticAnalyzer.RPNCode);
            interpreter.Process();
            semanticAnalyzer.DeclareListHandler.IdentifiersTable.ToString();
            semanticAnalyzer.ConstantsTableHandler.ConstantsTable.ToString();
            using (StreamWriter file = File.CreateText("../../../outputTree.json"))
            {
                
                var serializedTree = JsonSerializer.Serialize(tree);
                file.Write(serializedTree);
            }
        }
    }
}
