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
        public virtual void SetParser(int parserversion,bool initialQuotedIdentifiers)
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
                case 130:
                    parser = new TSql130Parser(initialQuotedIdentifiers);
                    break;
                case 140:
                    parser = new TSql140Parser(initialQuotedIdentifiers);
                    break;
                case 150:
                    parser = new TSql150Parser(initialQuotedIdentifiers);
                    break;
                case 0:
                case 160:
                    parser = new TSql160Parser(initialQuotedIdentifiers);
                    break;
                default:
                    Console.Error.WriteLine("SqlServer.TransactSql.ScriptDom version 161 supports only 80,90,100,110,120,130,140,150,160 parser.");
                    throw new ArgumentException();
            }
        }
        public void LoadString(string str)
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
            root = tree_parser.Parse(f);
        }
        public void LoadFile(string filepath, string encoding_name = "Shift_JIS")
        {
            string text;
            var encoding = Encoding.GetEncoding(encoding_name);
            using (StreamReader sr = new StreamReader(filepath, encoding))
            {
                text = sr.ReadToEnd();
            }
            LoadString(text);
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(root.ToHashtable(), Newtonsoft.Json.Formatting.Indented);
        }
        public XmlDocument ToXml()
        {
            return root.ToXml();
        }

    }
}
