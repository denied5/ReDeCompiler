using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Tables
{
    public class Constant : IToken
    {
        public int Id { get; }
        public string Type { get; set; }
        public string Content { get ; set ; }
        public string Value { get ; set ; }

        public Constant(int id, string value, string type)
        {
            this.Id = id;
            this.Content = value;
            this.Type = type;
        }
    }
}
