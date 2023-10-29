using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;
using System.Xml;

/*
Install-Package Newtonsoft.Json
*/

namespace LibSQLScriptDom
{
    public class LibSQLScriptDom
    {
        public dynamic root;
        public TSqlParser parser = null;

        public LibSQLScriptDom()
    	{
    	}
        public virtual void setParser(int parserversion,bool initialQuotedIdentifiers)
        { 
            switch (parserversion)
            {
                case 80:
                    parser = new TSql80Parser(initialQuotedIdentifiers);
                    break;
                case 90:
                    parser = new TSql90Parser(initialQuotedIdentifiers);
                    break;
                case 100:
                    parser = new TSql100Parser(initialQuotedIdentifiers);
                    break;
                case 110:
                    parser = new TSql110Parser(initialQuotedIdentifiers);
                    break;
                case 120:
                    parser = new TSql120Parser(initialQuotedIdentifiers);
                    break;
                case 0:
                case 130:
                    parser = new TSql130Parser(initialQuotedIdentifiers);
                    break;
                default:
                    Console.Error.WriteLine("SqlServer.TransactSql.ScriptDom version 13 supports only 80,90,100,110,120,130 parser.");
                    throw new ArgumentException();
            }
        }
        public void load_string(string str)
        {
            IList<ParseError> errors;
            TSqlFragment f;
            using (var reader = new StringReader(str))
            {
                f = parser.Parse(reader, out errors);
            }
            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"There are {errors.Count} errors.");

                var i = 0;
                foreach (var e in errors)
                {

                    sb.AppendLine($"error {++i}\nLine { e.Line}\nColumn { e.Column}\nOffset {e.Offset}\nNumber {e.Number}\n{e.Message}");
                }
                throw new ParserError(sb.ToString());
            }
            var tree_parser = new TreeParser();
            root = tree_parser.parse(f);
        }
        public void load_file(string filepath, string encoding_name = "Shift_JIS")
        {
            string text;
            var encoding = Encoding.GetEncoding(encoding_name);
            using (StreamReader sr = new StreamReader(filepath, encoding))
            {
                text = sr.ReadToEnd();
            }
            load_string(text);
        }
        public string to_json()
        {
            return JsonConvert.SerializeObject(root.to_Hashtable(), Newtonsoft.Json.Formatting.Indented);
        }
        public XmlDocument to_xml()
        {
            return root.to_xml();
        }

    }
}
