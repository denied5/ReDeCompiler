using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DenysRedkoParser.Interpreter.Tables
{
    public class ConstantsTable
    {
        public Dictionary<int, Constant> Constants { get; set; }
        private int id = 0;

        public ConstantsTable(List<Constant> constants): base()
        {
            foreach (var item in constants)
            {
                AddConstant(item.Content, item.Type);
            }
        }

        public ConstantsTable()
        {
            Constants = new Dictionary<int, Constant>();
        }

        public Constant AddConstant (string value, string type)
        {
            var constant = Find(value);
            if (constant != null)
            {
                return constant;
            }
            
            return Add(value, type);
        }


        public Constant Find(string value)
        {
            var sequence = Constants.Where(x => x.Value.Content == value);
            if (sequence.Count() > 0)
            {
                return sequence.First().Value;
            }
            return null;
        }

        private Constant Add(string value, string type)
        {
            var id = this.id++;
            var constant = new Constant(id, value, type);
            Constants[id] = constant;
            return constant;
        }

        public override string ToString()
        {
            var s = "";
            var formatString = string.Format("{{0, -{0}}}|", 10);
            Console.WriteLine("Constants Table");
            Console.Write(formatString, "Id");
            Console.Write(formatString, "Type");
            Console.WriteLine(formatString, "Content");
            foreach (var item in Constants)
            {
                Console.Write(formatString, item.Value.Id);
                Console.Write(formatString, item.Value.Type);
                Console.WriteLine(formatString, item.Value.Content);
            }
            Console.WriteLine("");
            return s;
        }
    }
}
