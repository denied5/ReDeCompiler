using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Operators
{
    public class BinaryOperator: Operator
    {
        public BinaryOperator(string operatorName, string type, string line) : base(operatorName, type, line)
        {
        }

        public bool isMultOp()
        {
            return Type == "Star" || Type == "Slash" || Type == "Caret";
        }
        public bool IsAddOp()
        {
            return Type == "Plus" || Type == "Minus";
        }
        public bool isAssignOp()
        {
            return Type == "AssignOp";
        }
        public bool isRelOp()
        {
            return Type == "RelOp";
        }
    }
}
