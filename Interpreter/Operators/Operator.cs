using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Operators
{
    public class Operator: IToken
    {
        public Operator(string content, string type, string line)
        {
            Content = content;
            Type = type;
            Line = line;
        }

        public string Content { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Line { get; set; }
    }
}
