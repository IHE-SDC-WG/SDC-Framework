using System;
using System.Linq;
using System.Xml;

namespace SDC.Schema
{
    public static class SDCHelpers
    {

        /// <summary>
        /// Converts the string expression of an enum value to the desired type. Example: var qType= reeBuilder.ConvertStringToEnum&lt;ItemType&gt;("answer");
        /// </summary>
        /// <typeparam name="Tenum">The enum type that the inputString will be converted into.</typeparam>
        /// <param name="inputString">The string that must represent one of the Tenum enumerated values; not case sensitive</param>
        /// <returns></returns>
        public static Tenum ConvertStringToEnum<Tenum>(string inputString) where Tenum : struct
        {
            //T newEnum = (T)Enum.Parse(typeof(T), inputString, true);

            Tenum newEnum;
            if (Enum.TryParse<Tenum>(inputString, true, out newEnum))
            {
                return newEnum;
            }
            else
            { //throw new Exception("Failure to create enum");

            }
            return newEnum;
        }
        
        public static void NZ<T>(T nullTestObject, T ObjectToSet)
        {
            if (nullTestObject == null) return;
            if (nullTestObject.GetType() == typeof(string) && nullTestObject.ToString() == "") return;
                ObjectToSet = nullTestObject;
        }

        public static string XmlReorder (string Xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(Xml);
            int j = 0;
            foreach (XmlNode node in doc.SelectNodes("//*"))
            {   //renumber the XML elements in Node order
                if (node.NodeType == XmlNodeType.Element)
                {
                    //if (node.Attributes.GetNamedItem("order") !=null)
                    node.Attributes["order"].Value = j++.ToString();
                }
            }
            return doc.OuterXml;
        }

        public static string XmlFormat (string Xml)
        {
            return System.Xml.Linq.XDocument.Parse(Xml).ToString();  //prettify the minified doc XML 
        }

    }
}
