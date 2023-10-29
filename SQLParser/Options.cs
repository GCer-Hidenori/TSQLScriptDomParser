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
        public string Filename { get; set; }

        [Option('s',"string", Required = false, HelpText = "Input string.")]
        public string Str { get; set; }

        [Option('e',"encode",Required =false,HelpText = "Encoding of input file")]
        public string Encoding { get; set; }

        [Option('o',"format", Required = true, HelpText = "Output format.json or xml.")]
        public string Format { get; set; }

        [Option('i', "indentxml", Required = false, HelpText = "Indent xml.")]
        public bool Indentxml { get; set; }

        [Option('u', "outputfilename", Required = false, HelpText = "Output filename.")]
        public string Outputfilename { get; set; }


        [Option('p',"parserversion",Required =false,HelpText= "Parser version.default 160.One of 80,90,100,110,120,130,140,150,160.")]
        public int Parserversion { get; set; }

        [Option('q', "no-quotedIdentifier",Required=false,HelpText = "Not support quotedIdentifier.")]
        public bool No_quotedIdentifier { get; set; }
        /*
                [Option("noerrorlistener",Required =false,HelpText ="No use error listener")]
                public bool noerrorlistener { get; set; }
            */
    }
}
