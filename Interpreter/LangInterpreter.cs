using DenysRedkoParser.Interpreter.Operators;
using DenysRedkoParser.Interpreter.Tables;
using DenysRedkoParser.Interpreter.Treits;
using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter
{
    public class LangInterpreter
    {
        private Stack<IToken> stack;
        private IdentifiersTable identifiers;
        private ConstantsTable constants;
        private List<IToken> RPNCode;
        private ArithmeticTrait _arithmeticTrait;
        private BoolExpressionTrait _boolExpressionTrait;
        private LabelsTable _labels;
        private int current = 0;

        public LangInterpreter(IdentifiersTable identifiers, ConstantsTable constants, List<IToken> rPNCode, LabelsTable labelsTable)
        {
            this.identifiers = identifiers;
            this.constants = constants;
            RPNCode = rPNCode;
            stack = new Stack<IToken>();
            _arithmeticTrait = new ArithmeticTrait();
            _boolExpressionTrait = new BoolExpressionTrait();
            _labels = labelsTable; 
        }

        public void Process()
        {
            var size = RPNCode.Count;

            while (current < size)
            {
                var item = RPNCode[current];
                if (item.GetType() == typeof(Constant) || item.GetType() == typeof(Identifier))
                {
                    stack.Push(item);
                }
                else if (item.GetType() == typeof(BinaryOperator))
                {
                    BinaryOperator(item);
                }
                else
                {
                    throw new Exception("Unknown item");
                }
                current++;
            }
        }

        private void BinaryOperator(IToken op)
        {
            var left = StackPop();
            var right = StackPop();
            var realOperator = op as BinaryOperator;
            if (realOperator.IsAddOp() || realOperator.isMultOp())
            {
                var result = _arithmeticTrait.Calculate(realOperator, left, right);
                var constant = constants.AddConstant(result.Content, result.Type);
                stack.Push(constant);
                return;
            }

            if (realOperator.isRelOp())
            {
                var result = _boolExpressionTrait.Calculate(realOperator, left, right);
                var constant = constants.AddConstant(result.Content, result.Type);
                stack.Push(constant);
                return;
            }

            if (realOperator.isAssignOp())
            {
                var leftType = left.Type;
                if (leftType != "real" && (leftType != right.Type))
                {
                    throw new Exception($"Cannot set variable {leftType} to {right.Type} in line 7");
                }
                var d = left as Identifier;
                identifiers.ChangeValue(d.id, right.Content);
                return;
            }
            throw new Exception("Unknown binary operator");

        }

        private IToken StackPop()
        {
            var item = stack.Pop();
            if (item.GetType() == typeof(Identifier))
            {
                return identifiers.FindByName(item.Content);
            }
            return item;
        }

        private void JumpIf(Constant expresion, Label label)
        {
            bool value;
            if (expresion.Value == "true")
            {
                value = true;
            }
            else if (expresion.Value == "false")
            {
                value = false;
            }
            else
            {
                value = !string.IsNullOrEmpty(expresion.Value);
            }

            if (!value)
            {
                JumpTo(_labels.GetAdress(label));
            }
        }

        private void JumpTo(int? adresss)
        {
            if (adresss == null)
            {
                throw new Exception("Fail with adress");
            }
            current = adresss.GetValueOrDefault() - 1;
        }
    }
}
