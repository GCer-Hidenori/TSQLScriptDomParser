using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

namespace LibSQLScriptDom
{
    public class Node
    {
        static Hashtable members = new Hashtable();

        public string token;
        public string class_name;
        private Dictionary<string,object> dic = new Dictionary<string,object>();
        private Dictionary<string, List<Node>> listdic = new Dictionary<string, List<Node>>();

        public Node()
        {

        }
        public void setDic(string key,object value)
        {
            dic[key] = value;
        }
        public object getDic(string key)
        {
            return dic[key];
        }

        public void setListDic(string key, Node value)
        {
            if (listdic.ContainsKey(key))
            {
                listdic[key].Add(value);
            }
            else
            {
                listdic[key] = new List<Node>();
                listdic[key].Add(value);
            }
            
        }
        public List<Node> getListDic(string key)
        {
            return listdic[key];
        }

        public Hashtable to_Hashtable()
        {
            var hash = new Hashtable();
            hash.Add("token", token);
            hash.Add("class", class_name);
           
            foreach(var key in dic.Keys)
            {
                hash.Add(key, dic[key]);
            }

            foreach(var key in listdic.Keys)
            {
                var ary = new ArrayList();
                foreach (var child in listdic[key])
                {
                    ary.Add(child.to_Hashtable());
                }
                hash.Add(key, ary);
            }
            return hash;
        }
        
        public XmlDocument to_xml()
        {
            var doc = new XmlDocument();
            doc.AppendChild(to_xml_element(doc));
            return doc;
        }
        
        public XmlElement to_xml_element(XmlDocument doc)
        {
            var element = doc.CreateElement("node");
            element.SetAttribute("token", token);
            element.SetAttribute("class", class_name);

            foreach (var key in dic.Keys)
            {
                element.SetAttribute(key, dic[key].ToString());
            }

            foreach(var key in listdic.Keys)
            {
                var childrenelement = doc.CreateElement("children");
                childrenelement.SetAttribute("type", key);
 
                foreach(var child in listdic[key])
                {
                    childrenelement.AppendChild(child.to_xml_element(doc));
                }
                element.AppendChild(childrenelement);
            }
            return element;
        }
        

    }
}
