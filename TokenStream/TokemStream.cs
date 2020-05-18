using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DenysRedkoParser
{
    class TokenStream
    {
        public TokenStream(List<Token> tokens)
        {
            _tokens = tokens;
            position = -1;
            _length = tokens.Count;
        }

        public TokenStream(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                JsonSerializerOptions js = new JsonSerializerOptions();
                js.PropertyNameCaseInsensitive = true;

                var fileContent = sr.ReadToEnd();
                _tokens = JsonSerializer.Deserialize<List<Token>>(fileContent, js );
                position = -1;
                _length = _tokens.Count;
            }
        }

        private List<Token> _tokens;
        private int position;
        private int _length;

        public Token this[int key]
        {
            get
            {
                if (key > _length || key < 0)
                    throw new Exception("Range exception");
                return _tokens[key];
            }
            set 
            {
                if (key > _length || key < 0)
                    throw new Exception("Range exception");
                _tokens[key] = value;
            }
        }


        public Token Next()
        {
            position++;
            return this[position];
        }

        public Token LookAhead()
        {
            return this[position + 1];
        }

        public void Back()
        {
            position = position > 0 ? position - 1 : 0;
        }

        public Token Current()
        {
            return this[position];
        }

        public int Remember()
        {
            return position;
        }

        public void GoTo(int index)
        {
            position = index > 0 || index < _length ? index : position;
        }
    }
}
