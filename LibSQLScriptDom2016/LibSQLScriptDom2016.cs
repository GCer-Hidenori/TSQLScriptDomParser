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
Install-Package Newtonsoft.Json -Version 12.0.3
*/

namespace LibSQLScriptDom2016
{
    public class LibSQLScriptDom2016
    {
        dynamic root;

    	public LibSQLScriptDom2016()
    	{
    	}
        public void load_string(string str)
        {
            var parser = new TSql110Parser(false);
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

                    sb.AppendLine("error {++i}\nLine { e.Line}\nColumn { e.Column}\nOffset {e.Offset}\nNumber {e.Number}\n{e.Message}");
                }
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
