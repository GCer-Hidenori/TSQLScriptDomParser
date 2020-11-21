using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CommandLine;

/*
 Install-Package CommandLineParser -Version 2.8.0
*/
namespace SQLParser
{
    class Program
    {
        static int Main(string[] args)
        {
            var option = new Options();
            var parseResult = Parser.Default.ParseArguments<Options>(args);
            string output;
            switch (parseResult.Tag)
            {
                case ParserResultType.Parsed:
                    var parsed = parseResult as Parsed<Options>;
                    option = parsed.Value;
                    LibSQLScriptDom2016.LibSQLScriptDom2016 lib = new LibSQLScriptDom2016.LibSQLScriptDom2016();
                    
                    try
                    {
                        if (option.str != null)
                        {
                            lib.load_string(option.str);
                        }
                        else if (option.filename != null)
                        {
                            if (option.encoding != null)
                            {
                                lib.load_file(option.filename, option.encoding);
                            }
                            else
                            {
                                lib.load_file(option.filename);
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("Filename or string is required.");
                            return 1;
                        }
                    }catch(LibSQLScriptDom2016.ParserError e)
                    {
                        Console.Error.WriteLine(e);
                        return 1;
                    }

                    switch(option.format.ToUpper())
                    {
                        case "JSON":
                            output = lib.to_json();
                            break;
                        case "XML":
                            output = lib.to_xml().OuterXml;
                            if (option.indentxml)
                            {
                                output = indentxml(output);
                            }
                            break;
                        default:
                            Console.Error.WriteLine("Invalid format '{0}'.Please specify JSON or XML.");
                            return 1;
                    }
                    if (option.outputfilename != null)
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(option.outputfilename))
                        {
                            sw.Write(output); // ファイルへテキストデータを出力する
                        }
                    }
                    else
                    {
                        Console.WriteLine(output);
                    }
                   
                    return 0;
                    
                case ParserResultType.NotParsed:
                    Console.Error.WriteLine("Commandline parse error.");
                    return 1;
                default:
                    return 1;
            }
        }
        static string indentxml(string xmlstring)
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlstring);
            var ws = new System.Xml.XmlWriterSettings();
            ws.Encoding = new System.Text.UTF8Encoding(false);
            ws.Indent = true;
            ws.IndentChars = "  ";

            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, ws))
                {
                    doc.Save(writer);
                }
                return (new System.Text.UTF8Encoding(false)).GetString(stream.ToArray());
                
            }
        }
    }
}
