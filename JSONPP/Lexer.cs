using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPP
{
    public class Lexer
    {
        public static char EOF = '\0';
        private TextReader source;
        private char lookahead;

        public Lexer(TextReader source)
        {
            this.source = source;
            Advance();

        }

        public Token.Token NextToken()
        {
            SkipWhiteSpace();
            if (EndOfSource())
            {
                return null;
            }
            if ("{}}[]:,".Contains(lookahead))
            {
                Token.Token ret = new Token.Token(Token.TokenType.SYMBOL, lookahead.ToString());
                Advance();
                return ret;
            }
            else if (lookahead == '"')
            {
                return String();
            }
            else if (Char.IsDigit(lookahead))
            {
                return Number();
            }
            else
            {
                return Word();
            }
        }

        private Token.Token Word()
        {
            if (lookahead == 't')
            {
                ExpectWord("true");
                return new Token.Token(Token.TokenType.WORD, "true");
            }
            else if (lookahead == 'f')
            {
                ExpectWord("false");
                return new Token.Token(Token.TokenType.WORD, "false");
            }
            else if (lookahead == 'n')
            {
                ExpectWord("null");
                return new Token.Token(Token.TokenType.WORD, "null");
            }
            else
            {
                throw new InvalidLexemeException();
            }
        }

        private void ExpectWord(string word)
        {
            char[] expected = word.ToArray();
            foreach (char c in expected)
            {
                if (lookahead != c)
                {
                    throw new InvalidLexemeException();
                }
                Advance();
            }
        }

        private Token.Token Number()
        {
            if (lookahead == '0')
            {
                throw new InvalidLexemeException("Numbers may not begin with leading zeroes.");
            }
            String val = "";
            do
            {
                val += lookahead;
                Advance();
            } while (Char.IsDigit(lookahead));
            if (lookahead == '.')
            {
                Advance();
                do
                {
                    val += lookahead;
                    Advance();
                } while (Char.IsDigit(lookahead));
            }
            if ("eE".Contains(lookahead))
            {
                val += lookahead;
                Advance();
                if ("+-".Contains(lookahead))
                {
                    val += lookahead;
                    Advance();
                }
                if (!Char.IsDigit(lookahead))
                {
                    throw new InvalidLexemeException("When specifying an exponent for a number, you must have at least one digit after the e");
                }
                do
                {
                    val += lookahead;
                    Advance();
                } while (Char.IsDigit(lookahead));
            }
            return new Token.Token(Token.TokenType.NUMBER, val);
        }

        private Token.Token String()
        {
            String val = "";
            char prev = lookahead;
            while (true)
            {
                Advance();
                if (prev == '\\')
                {
                    switch (lookahead)
                    {
                        case 'n':
                            val += '\n';
                            break;
                        case 'r':
                            val += '\r';
                            break;
                        case '\\':
                            val += '\\';
                            break;
                        case '"':
                            val += '"';
                            break;
                        default:
                            throw new InvalidLexemeException("Invalid escape sequence \\" + lookahead);
                    }
                }
                else
                {
                    if (lookahead == '"')
                    {
                        Token.Token ret = new Token.Token(Token.TokenType.STRING, val);
                        Advance();
                        return ret;
                    }
                    val += lookahead;
                }
                prev = lookahead;
            }
        }


        private void SkipWhiteSpace()
        {
            while (Char.IsWhiteSpace(lookahead))
            {
                Advance();
            }
        }

        private void Advance()
        {
            int next = source.Read();
            if (next == -1)
            {
                lookahead = EOF;
            }
            else
            {
                lookahead = (char)next;
            }
        }
        private bool EndOfSource()
        {
            return lookahead == EOF;
        }
    }
}
