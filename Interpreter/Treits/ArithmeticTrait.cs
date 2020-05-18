using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Treits
{
    public class ArithmeticTrait
    {
        public IToken Calculate(IToken op, IToken left, IToken right)
        {
            if (String.IsNullOrEmpty(left.Content))
            {
                throw new Exception("Unable to process ");
            }
            if (String.IsNullOrEmpty(right.Content))
            {
                throw new Exception("Unable to process ");
            }

            if (op.Type == "Plus")
            {
                return Plus(left, right);
            }
            if (op.Type == "Minus")
            {
                return Minus(left, right);
            }
            if (op.Type == "Star")
            {
                return Star(left, right);
            }
            if (op.Type == "Slash")
            {
                return Slash(left, right);
            }
            if (op.Type == "Caret")
            {
                return Caret(left, right);
            }
            throw new Exception("Unknown arithmetic operator");
        }

        private IToken Minus(IToken left, IToken right)
        {
            var value = Convert.ToDouble(left.Content) - Convert.ToDouble(right.Content);
            var type = "";
            if (left.Type == "real" || right.Type == "real")
            {
                type = "real";
            }
            else
            {
                type = "int";
            }

            return new Token { Content = value.ToString(), Type = type };
        }

        private IToken Plus(IToken left, IToken right)
        {
            var value = Convert.ToDouble(left.Content) + Convert.ToDouble(right.Content);
            var type = "";
            if (left.Type == "real" || right.Type == "real")
            {
                type = "real";
            }
            else
            {
                type = "int";
            }
            
            return new Token { Content = value.ToString(), Type = type };
        }

        private IToken Star(IToken left, IToken right)
        {
            var value = Convert.ToDouble(left.Content) * Convert.ToDouble(right.Content);
            var type = "real";

            return new Token { Content = value.ToString(), Type = type };
        }

        private IToken Slash(IToken left, IToken right)
        {
            if (Convert.ToDouble(right.Content) == 0)
            {
                throw new Exception("Zero division");
            }
            var value = Convert.ToDouble(left.Content) / Convert.ToDouble(right.Content);
            var type = "real";

            return new Token { Content = value.ToString(), Type = type };
        }

        private IToken Caret(IToken left, IToken right)
        {
            var value =  Math.Pow(Convert.ToDouble(left.Content), Convert.ToDouble(right.Content));
            var type = "real";

            return new Token { Content = value.ToString(), Type = type };
        }
    }
}
