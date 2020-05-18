using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Tables
{
    public class Identifier: IToken
    {
        public int id { get; }
        public string Content { get; set; }
        public string Type { get ; set ; }
        public string Value { get; set; }

        public Identifier(int id, string name, string type, string value)
        {
            this.id = id;
            this.Content = name;
            this.Type = type;
            this.Value = value;
        }
    }
}
