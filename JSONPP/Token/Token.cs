using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPP.Token
{
    public class Token
    {
        public readonly TokenType type;
        public readonly String value;
        public Token(TokenType type, String value)
        {
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            if (type == TokenType.STRING)
            {
                return '"' + value + '"';
            }
            return value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Token))
            {
                return false;
            }
            Token tok = (Token)obj;
            return (this.type == tok.type && this.value == tok.value);
        }
    }

   public enum TokenType
    {
        SYMBOL,
        NUMBER,
        STRING,
        WORD

    }
}
