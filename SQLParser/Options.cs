using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SQLParser
{
    class Options
    {
        [Option('f',"filename", Required = false, HelpText = "Input filename.")]
        public string filename { get; set; }

        [Option('s',"string", Required = false, HelpText = "Input string.")]
        public string str { get; set; }

        [Option('e',"encode",Required =false,HelpText = "Encoding of input file")]
        public string encoding { get; set; }

        [Option('o',"format", Required = true, HelpText = "Output format.json or xml.")]
        public string format { get; set; }

        [Option('i', "indentxml", Required = false, HelpText = "Indent xml.")]
        public bool indentxml { get; set; }

        [Option('u', "outputfilename", Required = false, HelpText = "Output filename.")]
        public string outputfilename { get; set; }

        [Option('d',"scriptdomversion",Default="2019",Required =false,HelpText ="SqlServer.TransactSql.ScriptDom version.13,15,2016,2019.")]
        public string scriptdomversion { get; set; }

        [Option('p',"parserversion",Required =false,HelpText="Parser version.ex)110,130")]
        public int parserversion { get; set; }

        [Option('q', "no-quotedIdentifier",Required=false,HelpText = "Not support quotedIdentifier.")]
        public bool no_quotedIdentifier { get; set; }
        /*
                [Option("noerrorlistener",Required =false,HelpText ="No use error listener")]
                public bool noerrorlistener { get; set; }
            */
    }
}
