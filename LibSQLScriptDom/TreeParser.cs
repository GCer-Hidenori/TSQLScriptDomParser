using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace LibSQLScriptDom
{
    public class TreeParser
    {
        public TreeParser()
        {
        }

        public Node Parse(TSqlFragment tsqlnode, Node parent_node = null,string parent_key_name = null)
        {
            Node node = new Node
            {
                class_name = tsqlnode.GetType().Name
            };

            if (tsqlnode.FirstTokenIndex >= 0)
            {
                StringBuilder sb = new StringBuilder();
                for (var i = tsqlnode.FirstTokenIndex; i <= tsqlnode.LastTokenIndex; i++)
                {
                    sb.Append(tsqlnode.ScriptTokenStream[i].Text);
                }
                node.token = sb.ToString();
            }

            if (parent_node != null) parent_node.SetListDic(parent_key_name, node);

            foreach (var prop in tsqlnode.GetType().GetProperties())
            {
                if((new List<string> { "ScriptTokenStream","StartOffset", "FragmentLength", "StartLine", "StartColumn", "FirstTokenIndex", "LastTokenIndex" })
.Contains(prop.Name))
                {
                    //NOP
                } else if (prop.PropertyType.IsEnum == true & prop.PropertyType.Namespace == "Microsoft.SqlServer.TransactSql.ScriptDom") {
                    node.SetDic(prop.Name,prop.GetValue(tsqlnode).ToString());
                }
                else
                {
                    System.Reflection.ParameterInfo[] param_infos = prop.GetIndexParameters();
                    var index_length = prop.GetIndexParameters().Length;
                    if (index_length > 0)
                    {
                        //index parameter
                    }
                    else
                    {
                        var prop_value = prop.GetValue(tsqlnode);
                        if (prop_value == null)
                        {
                            //NOP
                        }else{
                            var prop_value_type = prop_value.GetType();
	                        if (prop_value_type.IsGenericType)
	                        {
	                            if (typeof(List<>).IsAssignableFrom(prop_value.GetType().GetGenericTypeDefinition()))
	                            {
	                                IEnumerable<object> list = (IEnumerable<object>)prop_value;
	
	                                foreach (var v in list)
	                                {
	                                    if (v.GetType().IsSubclassOf(typeof(TSqlFragment)))
	                                    {
	                                        Parse((TSqlFragment)v, node,prop.Name);
	                                    }
	                                }
	                            }
	                            else
	                            {
	                                Console.Error.WriteLine($"not support!\t{prop.Name}\t{prop_value_type.Name}\t{prop_value}");
	                            }
	                        }
	                        else if (prop_value_type.IsSubclassOf(typeof(TSqlFragment)))
	                        {
	                            Parse((TSqlFragment)prop_value, node,prop.Name);
	                        }
	                        else
	                        {
                    			node.SetDic(prop.Name,prop.GetValue(tsqlnode).ToString());
                            }
                        }
                    }
                }
               
            }
            return node;
        }
    }
}
