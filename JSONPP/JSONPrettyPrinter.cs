using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPP
{
    public class JSONPrettyPrinter
    {
        private Lexer lexer;
        private Token.Token currentToken;
        private string pp;
        private int indentLevel = 0;
        private static string INDENT_INCREMENT = "    ";
        public JSONPrettyPrinter(TextReader source)
        {
            lexer = new Lexer(source);
            Advance();
        }
        public string Print()
        {
            Object();
            return pp;
        }

        private void Object()
        {
            Match(new Token.Token(Token.TokenType.SYMBOL, "{"));
            pp += "{\n";
            indentLevel++;
            Members();
            Match(new Token.Token(Token.TokenType.SYMBOL, "}"));
            indentLevel--;
            addIndent();
            pp += "}";
        }

        private void Members()
        {
            Pair();
            if (currentToken.Equals(new Token.Token(Token.TokenType.SYMBOL, ",")))
            {
                pp += ",\n";
                Match(new Token.Token(Token.TokenType.SYMBOL, ","));
                Members();
            }
            else
            {
                pp += "\n";
            }
        }
        private void Pair()
        {
            if (currentToken.type != Token.TokenType.STRING)
            {
                throw new InvalidSyntaxException("Expected a double-quoted string for the pair name");
            }
            addIndent();
            pp += '"' + currentToken.value + '"';
            Advance();
            Match(new Token.Token(Token.TokenType.SYMBOL, ":"));
            pp += ": ";
            Value();
        }

        private void Value()
        {
            if (currentToken.type == Token.TokenType.STRING)
            {
                pp += '"' + currentToken.value + '"';
                Advance();
            }
            else if (currentToken.type == Token.TokenType.NUMBER)
            {
                pp += currentToken.value;
                Advance();
            }
            else if (currentToken.Equals(new Token.Token(Token.TokenType.SYMBOL, "{")))
            {
                Object();
            }
            else if (currentToken.Equals(new Token.Token(Token.TokenType.SYMBOL, "[")))
            {
                Array();
            }
            else if (currentToken.type == Token.TokenType.WORD)
            {
                pp += currentToken.value;
                Advance();
            }
        }

        private void Array()
        {
            Match(new Token.Token(Token.TokenType.SYMBOL, "["));
            pp += "[\n";
            indentLevel++;
            Elements();
            Match(new Token.Token(Token.TokenType.SYMBOL, "]"));
            indentLevel--;
            addIndent();
            pp += "]";
        }

        private void Elements()
        {
            addIndent();
            Value();
            if (currentToken.Equals(new Token.Token(Token.TokenType.SYMBOL, ",")))
            {
                pp += ",\n";
                Match(new Token.Token(Token.TokenType.SYMBOL, ","));
                Elements();
            }
            else
            {
                pp += "\n";
            }
        }

        private void addIndent()
        {
            for (int i = 0; i < indentLevel; i++)
            {
                pp += INDENT_INCREMENT;
            }
        }

        private void Match(Token.Token expected)
        {
            if (!currentToken.Equals(expected))
            {
                throw new InvalidSyntaxException();
            }
            Advance();
        }
        private void Advance()
        {
            currentToken = lexer.NextToken();
        }
    }
}
