using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DenysRedkoParser.Interpreter.Tables
{
    public class IdentifiersTable
    {
        public Dictionary<int, Identifier> Identifiers { get; set; }
        private int id = 0;

        public IdentifiersTable(List<Identifier> identifiers):base()
        {
            foreach (var item in identifiers)
            {
                AddIdentifierIfNotExsist(item.Content, item.Type, item.Value);
            }
        }

        public IdentifiersTable()
        {
            Identifiers = new Dictionary<int, Identifier>();
        }

        public Identifier AddIdentifier(string name, string type = null, string value = null)
        {
            var identifier = Find(name, type);
            if (identifier != null)
            {
                throw new Exception($"Identifier {identifier.Content} already exist in table.");
            }

            return Add(name, type, value);
        }

        public void ChangeValue(int id, string value)
        {
            Identifiers[id].Value = value;
        }

        public Identifier AddIdentifierIfNotExsist(string name, string type = null, string value = null)
        {
            var identifier = Find(name, type);
            if (identifier != null)
                return identifier;

            return Add(name, type, value);
        }

        private Identifier Find(string name, string type)
        {
            var identifier = FindByName(name);
            if (identifier != null)
            {
                if (!string.IsNullOrEmpty(identifier.Type) && identifier.Type != type)
                {
                    throw new Exception("Identifier exsist but have dif type");
                }

                return identifier;
            }
            return null;
        }

        public Identifier FindByName(string name)
        {
            var identifiersWithName = Identifiers.Where(x => x.Value.Content == name);
            if (identifiersWithName.Count() > 0)
            {
                return identifiersWithName.FirstOrDefault().Value;
            }
            return null;
        }

        private Identifier Add(string name, string type = null, string value = null)
        {
            var id = this.id++;
            var identifier = new Identifier(id, name, type, value);
            Identifiers[id] = identifier;
            return identifier;
        }

        public override string ToString()
        {
            var s = "";
            var formatString = string.Format("{{0, -{0}}}|", 10);
            Console.WriteLine("Identifiers Table");
            Console.Write(formatString, "id");
            Console.Write(formatString, "Value");
            Console.WriteLine(formatString, "Content");
            foreach (var item in Identifiers)
            {
                Console.Write(formatString, item.Value.id);
                Console.Write(formatString, item.Value.Value);
                Console.WriteLine(formatString, item.Value.Content);
            }
            Console.WriteLine("");
            return s;
        }
    }
}
