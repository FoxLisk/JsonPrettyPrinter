using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPP
{
    class InvalidLexemeException : Exception
    {
        private string msg;
        public InvalidLexemeException() : base() { }
        public InvalidLexemeException(string p)
        {
            msg = p;
        }
    }
    class InvalidSyntaxException : Exception
    {
        private string msg;
        public InvalidSyntaxException() : base() { }
        public InvalidSyntaxException(string p)
        {
            msg = p;
        }
    }
}
