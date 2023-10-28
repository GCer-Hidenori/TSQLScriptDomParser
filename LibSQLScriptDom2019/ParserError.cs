using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSQLScriptDom0150
{
    public class ParserError : Exception
    {
        
        public ParserError(string message, Exception e) : base(message, e)
        {

        }
        public ParserError(string message) : base(message)
        {

        }
        

    }

}
