using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Treits
{
    public class BoolExpressionTrait
    {
        public IToken Calculate(IToken op, IToken left, IToken right)
        {
            if (String.IsNullOrEmpty(left.Value) || String.IsNullOrEmpty(right.Value))
            {
                throw new Exception("Unable to process bool Expr of undef ident in line 7");
            }
            var leftV = left.Value;
            var rightV = right.Value;
            bool value;

            switch (op.Content)
            {
                case "<":
                    value = Convert.ToDouble(leftV) < Convert.ToDouble(rightV);
                    break;
                case "<=":
                    value = Convert.ToDouble(leftV) <= Convert.ToDouble(rightV);
                    break;
                case "==":
                    value = Convert.ToDouble(leftV) == Convert.ToDouble(rightV);
                    break;
                case ">":
                    value = Convert.ToDouble(leftV) > Convert.ToDouble(rightV);
                    break;
                case ">=":
                    value = Convert.ToDouble(leftV) >= Convert.ToDouble(rightV);
                    break;
                case "!=":
                    value = Convert.ToDouble(leftV) != Convert.ToDouble(rightV);
                    break;
                default:
                    throw new Exception("Unknown arithmetic operator");
            }

            return new Token() { Content = value.ToString(), Type = "bool" };
        }

    }
}
