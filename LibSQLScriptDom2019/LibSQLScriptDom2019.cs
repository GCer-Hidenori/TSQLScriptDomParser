using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;

/*
  
 Install-Package Newtonsoft.Json
 
 */

namespace LibSQLScriptDom2019
{
    public class LibSQLScriptDom2019  : LibSQLScriptDom2016.LibSQLScriptDom2016
    {

        public LibSQLScriptDom2019()
        {
        }
        public override void setParser(int parserversion, bool initialQuotedIdentifiers)
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
                case 0:
                case 150:
                    parser = new TSql150Parser(initialQuotedIdentifiers);
                    break;
                default:
                    Console.Error.WriteLine("SqlServer.TransactSql.ScriptDom version 15 supports only 80,90,100,110,120,130,140,150 parser.");
                    throw new ArgumentException();
            }
        }
    }
}
