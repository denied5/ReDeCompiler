using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser
{
    public class Token:IToken
    {
        public string Line { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Value { get ; set ; }
    }
}
