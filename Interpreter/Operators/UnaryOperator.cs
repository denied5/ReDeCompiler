using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Operators
{
    class UnaryOperator: Operator
    {
        public UnaryOperator(string operatorName, string type, string line) : base(operatorName, type, line)
        {
        }

        public int MyProperty { get; set; }
    }
}
