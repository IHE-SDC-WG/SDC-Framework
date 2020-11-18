using MessagePack.Formatters;
using MsgPack.Serialization.CollectionSerializers;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.CodeDom;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.IO;
using System.Runtime.InteropServices;



//using SDC;
namespace SDC.Schema
{

    public struct PropertyInfoOrdered
    {
        public PropertyInfoOrdered(PropertyInfo propertyInfo, int xmlOrder)

        {
            PropertyInfo = propertyInfo;
            this.XmlOrder = xmlOrder;
        }
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// xmlOrder is the Order found in the XmlElementAttribute's Order property.  
        /// xmlOrder represents the element order in the SDC XML.  
        /// However, all inherited properties of the requested object occur as SDC XML elements that precede the current object's XML element, 
        /// even when the inherited properties have a higher xmlOrder value.
        /// </summary>
        public int XmlOrder { get; }
    }

    public struct PropertyInfoMetadata
    {
        public PropertyInfoMetadata(
            PropertyInfo propertyInfo,
            string propName,
            int itemIndex,
            IEnumerable<BaseType> ieItems,
            int xmlOrder,
            int maxXmlOrder,
            string xmlElementName
            )

        {
            PropertyInfo = propertyInfo;
            this.PropName = propName;
            this.ItemIndex = itemIndex;
            this.IeItems = ieItems;
            this.XmlOrder = xmlOrder;
            this.MaxXmlOrder = maxXmlOrder;
            this.XmlElementName = xmlElementName;


        }
        public PropertyInfo PropertyInfo { get; }
        /// <summary>
        /// Name of the class property that contains the requested object
        /// </summary>
        public string PropName { get; }
        /// <summary>
        /// If the requested object is held by a parent IEnumerable (usually an array of List), itemIndex contains the index of the requested object inside ieItems
        /// If the requested object is represented by a non-IEnumerable property, then itemIndex = -1 and ieItems is null.
        /// </summary>
        public int ItemIndex { get; }
        /// <summary>
        /// If the requested object is held by a parent IEnumerable (usually an array of List), the IEnumerable is retuurned as ieItems.
        /// If the requested object is represented by a non-IEnumerable property, then itemIndex = -1 and ieItems is null.
        /// </summary>
        public IEnumerable<BaseType> IeItems { get; }
        /// <summary>
        /// xmlOrder is the Order found in the XmlElementAttribute's Order property.  
        /// xmlOrder represents the element order in the SDC XML.  
        /// However, all inherited properties of the requested object occur as SDC XML elements that precede the current object's XML element, 
        /// even when the inherited properties have a higher xmlOrder value.
        /// </summary>
        public int XmlOrder { get; }
        /// <summary>
        /// maxXmlOrder is the maximum xmlOrder value found on properties in the XmlElementAttribute's Order field of the requested object's parent class 
        /// The parent class codes for the property member that represents the requested object. 
        /// Property members in the parent class may be decorated with XmlElementAttribute attributes, and these attributes have an "Order" field
        /// </summary>
        public int MaxXmlOrder { get; }
        /// <summary>
        /// Name of the XML element that is used to represent the requested object
        /// </summary>
        public string XmlElementName { get; }

        public override string ToString()
        {
            return @$"PropertyInfoMetadata:
---------------------------------------
IeItems.Count   {IeItems?.Count() ?? 0}
ItemIndex:      {ItemIndex}
PropName:       {PropName}
XmlOrder:       {XmlOrder}
MaxXmlOrder:    {MaxXmlOrder}
XmlElementName: {XmlElementName}
---------------------------------------";
        }

    }

    public class PropertyInfoOrderedComparer : Comparer<PropertyInfoOrdered>
    {
        public override int Compare(PropertyInfoOrdered pioA, PropertyInfoOrdered pioB)
        {
            //In XML Schemas, it appears that base class (Schema base type) xml elements always come before subclass elements, regardless of the XmlElementAttribute Order value.
            if (pioA.PropertyInfo.DeclaringType.IsSubclassOf(pioB.PropertyInfo.DeclaringType))
                return 1;  //base class xml orders come before subclasses; ancNodeA is the base type here
            if (pioB.PropertyInfo.DeclaringType.IsSubclassOf(pioA.PropertyInfo.DeclaringType))
                return -1; //base class xml orders come before subclasses; ancNodeB is the base type here

            //Determine the comparison based on the xmlOrder in the XmlElementAttributes
            if (pioA.XmlOrder < pioB.XmlOrder)
                return -1;
            if (pioB.XmlOrder < pioA.XmlOrder)
                return 1;
            else return 0;
        }
    }
    public class AttributeComparer : Comparer<PropertyInfo>
    {
        public override int Compare(PropertyInfo piA, PropertyInfo piB)
        {
            //In XML Schemas, it appears that base class (Schema base type) xml elements always come before subclass elements, regardless of the XmlElementAttribute Order value.
            if (piA.DeclaringType.IsSubclassOf(piB.DeclaringType))
                return 1;  //base class xml orders come before subclasses
            if (piB.DeclaringType.IsSubclassOf(piA.DeclaringType))
                return -1; //base class xml orders come before subclasses

            return piA.Name.CompareTo(piB.Name);
        }
    }
    public class TreeComparer : Comparer<BaseType>
    {
        public override int Compare(BaseType nodeA, BaseType nodeB)
        {
            //if nodeA is higher in the tree, return -1;
            //if nodeB is higher in the tree, return 1;

            int ord;
            if (nodeA.ObjectID < nodeB.ObjectID)
                ord = -1;
            else if (nodeA.ObjectID > nodeB.ObjectID)
                ord = 1;
            else ord = 0;

            //Debug.Print($" {ord},   A:{nodeA.ObjectID},   B:{nodeB.ObjectID}");


            if (nodeA == nodeB)
            { Result(0); return 0; }
            if (nodeA.TopNode != nodeB.TopNode) throw new Exception("nodeA and nodeB are derived from different SDC templates");
            if (nodeA == nodeA.TopNode)
            { Result(-1); return -1; }
            if (nodeB == nodeB.TopNode)
            { Result(1); return 1; }
            if (nodeB.ParentNode == nodeA)
            { Result(-1); return -1; }
            if (nodeA.ParentNode == nodeB)
            { Result(1); return 1; }
            //if (nodeA.ParentNode is null && nodeB.ParentNode != null) return -1; //nodeA is the top node
            //if (nodeB.ParentNode is null && nodeA.ParentNode != null) return 1;  //nodeB is the top node

            //create ascending ancestor ("anc") set ("ancSet") for nodeA branch, with nodeA as the first element in the ancester set, i.e., ancSetA[0] == nodeA
            BaseType prevPar = null;
            int count = nodeA.TopNode.Nodes.Count();
            var ancSetA = new BaseType[count];
            var ancSetB = new BaseType[count];
            int indexA = 0;
            ancSetA[indexA] = nodeA;
            prevPar = ancSetA[indexA]?.ParentNode ?? null;
            while (prevPar != null)
            {
                ancSetA[indexA + 1] = prevPar;
                indexA++;
                prevPar = ancSetA[indexA]?.ParentNode ?? null;
            }

            //Find the first intersection of the 2 arrays (lowest common ancestor) - the common node furthest from the tree's top node
            //If they share an ancestor, get the common ancestor object, to find which ancester is first.
            //If they have different ancesters, move down one node in both ancSetA and ancSetB.
            //The set with the highest level (first) parent indicates that the that all nodes in that set come before all nodes in the other set.

            indexA = -1; //we reuse indexA to refer to the common ancester in AncSetA
            indexA = SdcUtil.IndexOf(ancSetA, nodeB);

            if (indexA > -1)
            { Result(1); return 1; }//nodeB is an ancester of NodeA,and thus comes first in order

            int indexB = 0;
            ancSetB[indexB] = nodeB;
            bool failed = true;//failed indicates that the nodeA and nodeB trees branches never converge on a common ancestor.              
                               //This will generate an exception unless it is set to false below.
                               //!Look for nodeB ancestors in ancSetA.  Loop through nodeB ancesters (we build ancSetB as we loop here) until we find a common ancester in nodeA's ancesters (already assembled in ancSetB)
            prevPar = ancSetB[indexB]?.ParentNode ?? null;

            while (prevPar != null)
            {

                ancSetB[indexB + 1] = prevPar;  //note that we create the ancSetB only as needed.  No need to walk all the way up to the root node if we don't need to.  Thus it's slightly more efficient to place the deeper-on-tree node in nodeB.
                indexA = SdcUtil.IndexOf(ancSetA, prevPar); //Find the lowest common parent node; later we'll see we can determine which parent node comes first in the tree
                indexB++;
                if (indexA > -1)
                {//we found the lowest common parent node at ancSetB[IndexA] and ancSetB[IndexB], at nde prevPar
                    failed = false;
                    break;
                }
                prevPar = ancSetB[indexB]?.ParentNode ?? null;
            }
            if (failed)
                throw new Exception("the compared nodes cannot be compared because they do not have a common ancester node");

            //We have found the lowest common ancester ("ANC") located at index indexA in ancSetA and at IndexB in ancSetB
            //We now move one parent node further from the root on each tree branch (ancSetA and ancSetA), closer to nodeA and nodeB
            //and determine which of these ancesters has an XML Element sequence that is closer to the root node.
            //Both of these ancester nodes have ANC as a common SDC ParentNode.
            if (indexA == 0 && indexB > 0)
            { Result(-1); return -1; } //nodeA (located at index 0) is a direct ancestor of nodeB, so it must come first
            if (indexB == 0 && indexA > 0)
            { Result(1); return 1; }  //nodeB (located at index 0) is a direct ancester of nodeA, so it must come first
            BaseType ancNodeA = null;
            BaseType ancNodeB = null;

            if (indexA > 0 && indexB > 0)
            {
                ancNodeA = ancSetA[indexA - 1]; //first child of common ancester node on nodeA branch; this is still an ancester of NodeA, or NodeA itself
                ancNodeB = ancSetB[indexB - 1]; //first child of common ancester node on nodeB branch; this is still an ancester of NodeB, or NodeB itself
            }
            else
            {
                ancNodeA = ancSetA[indexA]; //first child of common ancester node on nodeA branch; this is still an ancester of NodeA, or NodeA itself
                ancNodeB = ancSetB[indexB]; //first child of common ancester node on nodeB branch; this is still an ancester of NodeB, or NodeB itself

            }
            //Retrieve customized Property Metadata for the class properties that hold our nodes.
            var piAncNodeA = SdcUtil.GetPropertyInfo(ancNodeA, false);
            var piAncNodeB = SdcUtil.GetPropertyInfo(ancNodeB, false);

            //Let's see if both items come from the same IEnumerable (ieItems) in ANC, and then see which one has the lower itemIndex
            if (piAncNodeA.IeItems != null && piAncNodeB.IeItems != null &&
                piAncNodeA.IeItems == piAncNodeB.IeItems &&
                piAncNodeA.ItemIndex > -1 && piAncNodeB.ItemIndex > -1)
            {
                if (piAncNodeA.ItemIndex == piAncNodeB.ItemIndex)
                    throw new Exception("Unknown error - the compared nodes share a common ParentNode and appear to be identical");
                if (piAncNodeA.ItemIndex < piAncNodeB.ItemIndex)
                { Result(-1); return -1; }
                if (piAncNodeB.ItemIndex < piAncNodeA.ItemIndex)
                { Result(1); return 1; }
            }

            //In XML Schemas, it appears that base class (Schema base type) xml elements always come before subclass elements, regardless of the XmlElementAttribute Order value.
            if (piAncNodeA.PropertyInfo.DeclaringType.IsSubclassOf(piAncNodeB.PropertyInfo.DeclaringType))
            { Result(1); return 1; } //base class xml orders come before subclasses; ancNodeA is the base type here
            if (piAncNodeB.PropertyInfo.DeclaringType.IsSubclassOf(piAncNodeA.PropertyInfo.DeclaringType))
            { Result(-1); return -1; } //base class xml orders come before subclasses; ancNodeB is the base type here

            //Determine the comparison based on the xmlOrder in the XmlElementAttributes
            if (piAncNodeA.XmlOrder < piAncNodeB.XmlOrder)
            { Result(-1); return -1; }
            if (piAncNodeB.XmlOrder < piAncNodeA.XmlOrder)
            { Result(1); return 1; }
            throw new Exception("the compare nodes algorithm could not determine the node order");


            void Result(int i)
            {
                //For debugging only:
                //Debug.Print($" {i}:ord:{ord},   A:{nodeA.ObjectID},   B:{nodeB.ObjectID}");
                //if (i != ord) Debugger.Break();
            }


        }
    }

    public static class SdcSerialization
    {         //TODO: why are these internal static methods in BaseType?  Should they be in SdcUtil or another helper class?
        //!+XML
        internal static ITopNode GetSdcObjectFromXmlPath<T>(string path) where T : ITopNode
        {
            string sdcXml = System.IO.File.ReadAllText(path);  // System.Text.Encoding.UTF8);
            return GetSdcObjectFromXml<T>(sdcXml);
        }
        internal static T GetSdcObjectFromXml<T>(string sdcXml) where T : ITopNode
        {
            T obj = SdcSerializer<T>.Deserialize(sdcXml);
            return InitParentNodesFromXml<T>(sdcXml, obj); ;
        }
        //!+JSON
        internal static ITopNode GetSdcObjectFromJsonPath<T>(string path) where T : ITopNode
        {
            string sdcJson = System.IO.File.ReadAllText(path);
            return GetSdcObjectFromJson<T>(sdcJson);
        }
        internal static ITopNode GetSdcObjectFromJson<T>(string sdcJson) where T : ITopNode
        {
            T obj = SdcSerializerJson<T>.DeserializeJson<T>(sdcJson);
            //InitParentNodesFromXml<T>(obj.GetXml(), obj);
            return InitParentNodesFromXml<T>(obj.GetXml(), obj); ;
        }
        //!+MsgPack
        internal static ITopNode GetSdcObjectFromMsgPackPath<T>(string path) where T : ITopNode
        {
            byte[] sdcMsgPack = System.IO.File.ReadAllBytes(path);
            return GetSdcObjectFromMsgPack<T>(sdcMsgPack);
        }
        internal static ITopNode GetSdcObjectFromMsgPack<T>(byte[] sdcMsgPack) where T : ITopNode
        {
            T obj = SdcSerializerMsgPack<T>.DeserializeMsgPack(sdcMsgPack);
            return InitParentNodesFromXml<T>(obj.GetXml(), obj);
        }

        private static T InitParentNodesFromXml<T>(string sdcXml, T obj) where T : ITopNode
        {
            //read as XMLDocument to walk tree
            var x = new System.Xml.XmlDocument();
            x.LoadXml(sdcXml);
            XmlNodeList xmlNodeList = x.SelectNodes("//*");

            var dX_obj = new Dictionary<int, Guid>(); //the index is iXmlNode, value is FD ObjectGUID
            int iXmlNode = 0;
            XmlNode xmlNode;

            foreach (BaseType bt in obj.Nodes.Values)
            {   //As we interate through the nodes, we will need code to skip over any non-element node, 
                //and still stay in sync with FD (using iFD). For now, we assume that every nodeList node is an element.
                //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?view=netframework-4.8
                //https://docs.microsoft.com/en-us/dotnet/standard/data/xml/types-of-xml-nodes
                xmlNode = xmlNodeList[iXmlNode];
                while (xmlNode.NodeType.ToString() != "Element")
                {
                    iXmlNode++;
                    xmlNode = xmlNodeList[iXmlNode];
                }
                //Create a new attribute node to hold the node's index in xmlNodeList
                XmlAttribute a = x.CreateAttribute("index");
                a.Value = iXmlNode.ToString();
                var e = (XmlElement)xmlNode;
                e.SetAttributeNode(a);

                //Set the correct Element Name, in case we have errors in the SDC object tree logic
                bt.ElementName = e.LocalName;

                //Create  dictionary to track the matched indexes of the XML and FD node collections
                dX_obj[iXmlNode] = bt.ObjectGUID;
                //Debug.Print("iXmlNode: " + iXmlNode + ", ObjectID: " + bt.ObjectID);

                //Search for parents:
                int parIndexXml = -1;
                Guid parObjectGUID = default;
                bool parExists = false;
                BaseType btPar;
                XmlNode parNode;
                btPar = null;

                parNode = xmlNode.ParentNode;
                parExists = int.TryParse(parNode?.Attributes?.GetNamedItem("index")?.Value, out parIndexXml);//The index of the parent XML node
                if (parExists)
                {
                    parExists = dX_obj.TryGetValue(parIndexXml, out parObjectGUID);// find the matching parent SDC node Object ID
                    if (parExists) { parExists = obj.Nodes.TryGetValue(parObjectGUID, out btPar); } //Find the parent node in FD
                    if (parExists)
                    {
                        //bt.IsLeafNode = true;
                        bt.RegisterParent(btPar);
                        //Debug.WriteLine($"The node with ObjectID: {bt.ObjectID} is leaving InitializeNodesFromSdcXml. Item type is {bt.GetType().Name}.  " +
                        //            $"Parent ObjectID is {bt?.ParentID}, ParentIETypeID: {bt?.ParentIETypeID}, ParentType: {btPar.GetType().Name}");
                    }
                    else { throw new KeyNotFoundException("No parent object was returned from the SDC tree"); }
                }
                else
                {
                    //bt.IsLeafNode = false;
                    //Debug.WriteLine($"The node with ObjectID: {bt.ObjectID} is leaving InitializeNodesFromSdcXml. Item type is {bt.GetType()}.  " +
                    //                $", No Parent object exists");
                }

                iXmlNode++;
            }
            return obj;

        }
    }

    public static class SdcUtil
    {
        #region ArrayHelpers
        public static bool IsGenericList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                o.GetType().IsGenericType &&
                o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        public static int GetFirstNullArrayIndex<T>(T[] array, int growthIncrement = 3)
        {
            int i = 0;
            if (array is null) array = new T[growthIncrement];
            foreach (var n in array)
            {
                if (n is null) return i;
                i++;
            }
            Array.Resize(ref array, array.Length + growthIncrement);
            return i;
        }
        public static object GetObjectFromIEnumerableObject(IEnumerable ie, object obj)
        {
            foreach (object o in ie)
                if (ReferenceEquals(o, obj)) return o;
            return null;
        }
        public static object GetObjectFromIEnumerableIndex(IEnumerable ie, int index)
        {
            int i = -1;
            foreach (object o in ie)
            {
                i++;
                if (index == i) return o;
            }
            return null;
        }
        public static int GetListIndex<T>(List<T> list, T node) where T : notnull //TODO: could make this an interface feature of all list children
        {
            int i = 0;
            foreach (T n in list)
            {
                if ((object)n == (object)node) return i;
                i++;
            }
            return -1; //object was not found in list
        }
        public static int IndexOf(IEnumerable array, object item)
        {
            int i = 0;
            if (array is null || item is null) return -1;
            foreach (object n in array)
            {
                if (ReferenceEquals(n, item)) return i;
                i++;
            }
            return -1;
        }
        public static object ObjectAtIndex(IEnumerable ie, int index)
        {
            //get Dictionary and similar object by index.  
            //This is not a reliable or effficient algorithm, since there is no defined sort order.  
            //Need to use an indexed Dictionary/Collection
            if (index < 0) return null;
            int i = 0;
            foreach (var n in ie)
            {
                if (i == index) return n;
                i++;
            }
            return null;
        }
        public static IEnumerable<T> IEnumerableCopy<T>(IEnumerable<T> source, IEnumerable<T> emptyTarget)
        {
            if (emptyTarget.Count() > 0) throw new Exception(String.Format("", "emptyTarget was not empty"));
            foreach (var n in source)
            {
                emptyTarget.Append(n);
            }
            return emptyTarget;
        }
        public static T[] ArrayAddItemReturnArray<T>(T[] array, T itemToAdd, int growthIncrement = 3)
        {
            int i = GetFirstNullArrayIndex(array, growthIncrement);
            array[i] = itemToAdd;
            return array;

        }
        public static T ArrayAddReturnItem<T>(T[] array, T itemToAdd, int growthIncrement = 3)
        {
            ArrayAddItemReturnArray(array, itemToAdd, growthIncrement);
            return itemToAdd;

        }
        public static T[] RemoveArrayNullsNew<T>(T[] array)
        {
            int i = 0;
            var newarray = new T[array.Length - 1];

            foreach (var n in array)
            {
                if (n != null) newarray[i] = n;
                i++;
            }
            return newarray;
        }

        #endregion

        #region SDC Helpers
        public static BaseType GetPrevElement(BaseType item)
        {
            if (item is null) return null;
            BaseType par = item.ParentNode;
            if (par is null) return null; //We have the top node here

            var prevSib = SdcUtil.GetPrevSib(item);
            if (prevSib != null && prevSib != item)
            {
                var lastDesc1 = GetLastDescendant(prevSib);
                if (lastDesc1 != null && lastDesc1 != item) return lastDesc1;
            }

            var lastDesc = GetLastDescendant(par, item);// move up one node and drill down to bottom of tree until we hit item, then back to prevNode
            if (lastDesc != null && lastDesc != item) return lastDesc;


            return par; //par has no descendants, so just return par          
        }

        public static BaseType GetNextElement(BaseType item)
        {
            if (item is null) return null;

            var firstKid = GetFirstChild(item);
            if (firstKid != null) return firstKid;

            var n = item;
            do
            {
                var nextSib = GetNextSib(n);
                if (nextSib != null) return nextSib;

                n = n.ParentNode;
            } while (n != null);

            return null;
        }

        public static string ReflectNodeDictionariesOrdered(ITopNode topNode, bool print = false)
        {
            int counter = 0;
            int indent = 0;
            var treeText = new StringBuilder();
            BaseType node = topNode as BaseType;
            topNode.ParentNodes.Clear();
            topNode.ChildNodes.Clear();
            if (print) treeText.Append($"({node.DotLevel})#{counter}; OID: {node.ObjectID}; name: {node.name}{content(node)}");

            DoTree(topNode as BaseType);

            void DoTree(BaseType node)
            {
                indent++;  //indentation level of the node for output formatting
                counter++; //simple integer counter, incremented with each node; should match the ObjectID assigned during XML deserialization
                BaseType btProp = null;  //holds the current property
                if (print) treeText.Append("\r\n");

                //Create a LIFO stack of the targetNode inheritance hierarchy.  The stack's top level type will always be BaseType
                //For most non-datatype SDC objects, it could be a bit more efficient to use ExtensionBaseType - we can test this another time
                Type t = node.GetType();
                var s = new Stack<Type>();
                s.Push(t);

                do
                {//build the stack of inherited types
                    t = t.BaseType;
                    if (t.IsSubclassOf(typeof(BaseType))) s.Push(t);
                    else break; //quit when we hit a non-BaseType type
                } while (true);

                while (s.Count > 0)
                {
                    var props = s.Pop().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(p => p.IsDefined(typeof(XmlElementAttribute)));
                    //.OrderBy(p => p.GetCustomAttributes<XmlElementAttribute>().First().Order);
                    foreach (var p in props)
                    {                        
                        var prop = p.GetValue(node);
                        if (prop != null)
                        {
                            if (prop is BaseType)
                            {
                                btProp = prop as BaseType;
                                btProp.RegisterParent(node);
                                AssignSdcProperties(p);
                                if (print) treeText.Append($"{"".PadRight(indent, '.')}({btProp.DotLevel})#{counter}; OID: {btProp.ObjectID}; name: {btProp.name}{content(btProp)}");
                                //Debug.Assert(btProp.ObjectID == counter);                                
                                DoTree(btProp);
                            }
                            else if (prop is IEnumerable<BaseType> ieProp)
                            {
                                foreach (BaseType btItem in ieProp)
                                {
                                    btProp = btItem;
                                    btProp.RegisterParent(node);
                                    AssignSdcProperties(p);
                                    if (print) treeText.Append($"{"".PadRight(indent, '.')}({btProp.DotLevel})#{counter}; OID: {btProp.ObjectID}; name: {btProp.name}{content(btItem)}");
                                    //Debug.Assert(btItem.ObjectID == counter);                                    
                                    DoTree(btProp);
                                }
                            }
                        }
                    }
                }
                indent--;
                //-------------------------------------------------
                void AssignSdcProperties(PropertyInfo p)
                {
                    string elementName;
                    int elementOrder = -1;
                    //Fill some useful properties, while it's efficient to do so, 
                    //because we have the PropertyInfo object (p) and the actual property object (btProp) already available.
                    elementName = SdcUtil.ReflectSdcElement(p, btProp, out elementOrder);
                    btProp.ElementName = elementName;
                    btProp.ElementOrder = elementOrder;
                }

            }


 


            //This is a temporary kludge to generate printable output.  
            //It should be easy to create a tree walker to create any desired output by visiting each node.
            string content(BaseType n)
                {
                string s;
                if (n is DisplayedType) s = "; title: " + (n as DisplayedType).title;
                else if (n is PropertyType) s = "; " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else s = $"; type: {n.GetType().Name}";
                return s;
            }

            if (print) return treeText.ToString();
            return "";
        }
        public static BaseType ReflectNextElement(BaseType item)
        {
            int xmlOrder = -1;
            if (item is null) return null;
            BaseType par = item.ParentNode;
            BaseType nextNode = null;
            bool doDescendants = true;
            if (par is null) par = item; //We have the top node here

            while (par != null)
            {
                nextNode = null;
                xmlOrder = -1;

                //Does item have any children of its own?  If yes, find the first non-null property in the XML element order
                if (doDescendants) if (GetNextNode(item) != null)
                        return nextNode;

                //No child items contained the next node, so let's look at other properties inside the parent object
                var piMeta = GetPropertyInfo(item, true);
                xmlOrder = piMeta.XmlOrder;

                //Is next item is contained inside the same IEnumerable parent?
                if (piMeta.ItemIndex > -1 && piMeta.ItemIndex < piMeta.IeItems.Count() - 1)
                    return ObjectAtIndex(piMeta.IeItems, piMeta.ItemIndex + 1) as BaseType;

                //Is next item part of a parent property that follows our item?
                if (GetNextNode(par, item) != null)
                    return nextNode;

                //We did not find a next item, so let's move up one parent level in this while loop and check the properties under the parent.
                //We keep climbing upwards in the tree until we find a parent with a next item, or we hit the top ancester node and return null.
                item = par;
                par = item.ParentNode;
                doDescendants = false; //this will prevent infinite loops by preventing a useless search through the new parent's direct descendants - we already searched these.
            }
            return null;
            //!+--------------Local Function--------------------
            BaseType GetNextNode(BaseType targetNode, BaseType startAfterNode = null)
            {
                int lowestOrder = 10000;  //Order in XmlElementAttribute, for finding the  next property to return; start with a huge value.
                IEnumerable<PropertyInfo> piIE = null;
                Type startType = null;

                //the following flag improves efficency by delaying object tree assessment until startAfterNode has been passed
                bool startAfterNodeWasHit = false;
                if (startAfterNode is null)
                { //the following flag improves effciency by delaying object tree assessment until startAfterNode has been passed
                    startAfterNodeWasHit = true;
                }
                else startType = startAfterNode.GetType();

                //Create a LIFO stack of the targetNode inheritance hierarchy.  The stack's top level type will always be BaseType
                //For most non-datatype SDC objects, it could be a bit more efficient to use ExtensionBaseType - we can test this another time
                Type t = targetNode.GetType();
                var s = new Stack<Type>();
                s.Push(t);

                do
                {//build the stack of inherited types
                    t = t.BaseType;
                    if (t.IsSubclassOf(typeof(BaseType))) s.Push(t);
                    else break; //quit when we hit a non-BaseType type
                } while (true);

                //starting with the least-derived inherited type (BaseType), look for any non-null properties of targetNode
                while (s.Count() > 0)
                {
                    lowestOrder = 10000;
                    piIE = s.Pop().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(p =>
                    {
                        var atts = (XmlElementAttribute[])p.GetCustomAttributes<XmlElementAttribute>();
                        if (atts.Count() == 0) return false;
                        XmlElementAttribute a = atts[0];
                        object o = null;
                        if (a.Order < lowestOrder)
                        {
                            if (startAfterNodeWasHit)
                            {
                                o = p.GetValue(targetNode);

                                if (o is null) return false;
                                if (o is BaseType)
                                {
                                    lowestOrder = a.Order;
                                    nextNode = o as BaseType;
                                    return true;
                                }
                                if (o is IEnumerable<BaseType> && (o as IEnumerable<BaseType>).Count() > 0)
                                {
                                    lowestOrder = a.Order;
                                    nextNode = (o as IList)[0] as BaseType;
                                    return true;
                                }
                            }

                            if (!startAfterNodeWasHit)
                            {
                                o = p.GetValue(targetNode);

                                if (o is IEnumerable<BaseType> &&
                                    (o as IEnumerable<BaseType>).Count() > 0
                                    && IndexOf(o as IEnumerable<BaseType>, startAfterNode) > -1)
                                    startAfterNodeWasHit = true; //start looking for nextNode now
                                if (!startAfterNodeWasHit && ReferenceEquals(o, startAfterNode))
                                    startAfterNodeWasHit = true; //start looking for nextNode now
                            }
                        }
                        return false;
                    }).ToList();

                    //var piIeOrdered = piIE?.OrderBy(p => p.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault()?.Order); //sort pi list by XmlElementAttribute.Order
                    //PropertyInfo piFirst = GetObjectFromIEnumerableIndex(piIeOrdered, 0) as PropertyInfo; //Get the property whose XmlElementAttribute has the smallest order

                    if (nextNode != null) return nextNode;
                }
                return null;
            }
        }

        public static BaseType ReflectNextElement2(BaseType item)
        {
            if (item is null) return null;
            BaseType par = item.ParentNode;
            BaseType nextNode = null;
            bool doDescendants = true;
            if (par is null) par = item; //We have the top node here

            while (par != null)
            {
                //Does item have any children of its own?  If yes, return the first child
                if (doDescendants)
                {
                    nextNode = ReflectFirstChild(item);
                    if (nextNode != null)
                        return nextNode;
                }
                nextNode = ReflectNextSib(item);
                if (nextNode != null)
                    return nextNode;

                //We did not find a next item, so let's move up one parent level in this while loop and check the properties under the parent.
                //We keep climbing upwards in the tree until we find a parent with a next item, or we hit the top ancester node and return null.
                item = par;
                par = item.ParentNode;
                doDescendants = false;
            }
            return null;

        }
        public static BaseType GetLastSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;
            item.TopNode.ChildNodes.TryGetValue(par.ObjectGUID, out List<BaseType> sibs);
            sibs?.Sort(new TreeComparer());
            return sibs?[sibs.Count() - 1];
        }
        public static BaseType ReflectLastSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;

            var lst = ReflectChildList(par);
            return lst?.Last();
        }
        public static BaseType GetFirstSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;
            item.TopNode.ChildNodes.TryGetValue(par.ObjectGUID, out List<BaseType> sibs);
            sibs?.Sort(new TreeComparer());
            return sibs?[0];
        }
        public static BaseType ReflectFirstSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;

            var lst = ReflectChildList(par);
            return lst?.FirstOrDefault();
        }
        public static BaseType GetNextSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;
            var sibs = item.TopNode.ChildNodes[par.ObjectGUID];
            if (sibs is null) throw new Exception("Sibs were not found in TopNode.ChildNodes");
            sibs?.Sort(new TreeComparer());
            var index = sibs.IndexOf(item);
            if (index == sibs.Count() - 1) return null; //item is the last item
            return sibs?[index + 1];
        }
        public static BaseType ReflectNextSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;

            var lst = ReflectChildList(par);
            var myIndex = lst?.IndexOf(item) ?? -1;
            if (myIndex < 0 || myIndex == lst?.Count() - 1) return null;
            return lst[myIndex + 1];
        }
        public static BaseType GetPrevSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;
            item.TopNode.ChildNodes.TryGetValue(par.ObjectGUID, out List<BaseType> sibs);
            if (sibs is null) throw new Exception("Sibs were not found in TopNode.ChildNodes");
            sibs?.Sort(new TreeComparer());
            var index = sibs.IndexOf(item);
            if (index == 0) return null; //item is the first item
            return sibs?[index - 1];
        }
        public static BaseType ReflectPrevSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;

            var lst = ReflectChildList(par);
            var myIndex = lst?.IndexOf(item) ?? -1;
            if (myIndex < 1) return null;
            return lst[myIndex - 1];
        }

        public static BaseType GetLastChild(BaseType item)
        {
            item.TopNode.ChildNodes.TryGetValue(item.ObjectGUID, out List<BaseType> kids);
            kids?.Sort(new TreeComparer());
            return kids?[kids.Count() - 1];
        }
        public static BaseType ReflectLastChild(BaseType item)
        {
            if (item is null) return null;

            var lst = ReflectChildList(item);
            return lst?.Last();
        }
        public static BaseType GetFirstChild(BaseType item)
        {
            item.TopNode.ChildNodes.TryGetValue(item.ObjectGUID, out List<BaseType> kids);
            kids?.Sort(new TreeComparer());
            return kids?[0];
        }
        public static BaseType ReflectFirstChild(BaseType item)
        {
            if (item is null) return null;

            var lst = ReflectChildList(item);
            return lst?.FirstOrDefault();
        }
        public static List<BaseType> GetChildList(BaseType item)
        {
            item.TopNode.ChildNodes.TryGetValue(item.ObjectGUID, out List<BaseType> kids);
            kids?.Sort(new TreeComparer());
            return kids;
        }
        public static bool HasChild(BaseType item)
        {
            item.TopNode.ChildNodes.TryGetValue(item.ObjectGUID, out List<BaseType> kids);
            if (kids is null || kids.Count() == 0) return false;
            return true;
        }




        public static BaseType GetLastDescendant(BaseType item, BaseType stopNode = null)
        {
            BaseType last = null;
            var n = item;
            var tc = new TreeComparer();
            while (n != null)
            {
                item.TopNode.ChildNodes.TryGetValue(n.ObjectGUID, out List<BaseType> kids);
                kids?.Sort(tc);
                if (kids == null) break;
                //option to abort search just before stopNode: check for stopNode in sibling list.
                if (stopNode != null)
                {
                    var snIndex = kids.IndexOf(stopNode);
                    if (snIndex > 0) return kids[snIndex - 1];
                    if (snIndex == 0) return last;
                }

                n = kids[kids.Count() - 1];
                if (n != null) last = n;
            }
            return last;
        }

        public static BaseType ReflectLastDescendant(BaseType bt, BaseType stopNode = null)
        {
            if (bt is null) return null;
            BaseType lastKid = null;

            FindLastKid(bt);
            //!+-------Local Method--------------------------
            void FindLastKid(BaseType bt)
            {
                List<BaseType> kids = ReflectChildList(bt);
                var testLast = kids?.Last();
                if (testLast is null) return; //we ran out of kids to check, so lastKid is the last descendant                

                if (stopNode != null)
                {
                    var pos = kids.IndexOf(stopNode);
                    if (pos == 0) return;
                    if (pos > 0)
                    {
                        lastKid = kids[pos - 1];
                        return;
                    }
                }
                lastKid = testLast;

                FindLastKid(lastKid);
            }
            return lastKid;
        }


        internal static int GetMaxOrderFromXmlElementAttributes(BaseType item)
        {
            var props = item.GetType().GetProperties();

            //Get the max Order among all the XmlElementAttributes in props.  This will be an upper bound for later searching
            int maxOrder = -1;
            int? tempMax = -1;
            foreach (var pi in props)
            {
                if (pi.GetCustomAttributes<XmlElementAttribute>().Count() > 0)
                {
                    tempMax = pi.GetCustomAttributes<XmlElementAttribute>()?.Where(a => a.Order > -1)?.First()?.Order;
                    if (tempMax > maxOrder) maxOrder = tempMax ?? -1;
                }
            }
            return maxOrder;

        }

        public static PropertyInfoMetadata GetPropertyInfo(BaseType item, bool getNames = true)
        {
            var pi = GetPropertyInfo(
                item,
                out string propName,
                out int itemIndex,
                out IEnumerable<BaseType> ieItems,
                out int xmlOrder,
                out int maxXmlOrder,
                out string xmlElementName,
                getNames);

            return new PropertyInfoMetadata(pi, propName, itemIndex, ieItems, xmlOrder, maxXmlOrder, xmlElementName);

        }
        /// <summary>
        /// Get the PropertyInfo object that represents the "item" property in the item's ParentNode
        /// This PropertyInfo object may be decorated with important XML annnotations such as XmlElementAttribute
        /// The returned PropertyInfo object may refer to a BaseType or the IEnumerables List&lt;BaseType> and Array&lt;BaseType> 
        /// If a wrapper property was created in an SDC parrtial class, only the inner property (i.e., the one with XML attributes) is returned
        /// </summary>
        /// <param name="item"></param>
        /// <param name="getNames">if true, element names will be determined</param>
        /// <returns>
        /// propName: name of the property is returned as an out parameter
        /// ieItems: if the property is IEnumerable&lt;BaseType>, the IEnumerable property object is returned as an out parameter, otherwise it is null
        /// itemIndex: the index of "item" in "ieItems" is returned as an out parameter, otherwise it is -1
        /// </returns>
        private static PropertyInfo GetPropertyInfo(
            BaseType item,
            out string propName,
            out int itemIndex,
            out IEnumerable<BaseType> ieItems,
            out int xmlOrder,
            out int maxXmlOrder,
            out string xmlElementName,
            bool getNames = true)
        {
            xmlOrder = -2;
            maxXmlOrder = -1;
            propName = null;
            xmlElementName = null;
            itemIndex = -1;
            ieItems = null;
            if (item is null) return null;
            var par = item.ParentNode;
            if (par is null)
            {
                par = item;  //we are at the top node
                var t = item.GetType();
                //var pi = t.GetProperties();
                xmlElementName = t.GetCustomAttribute<XmlRootAttribute>()?.ElementName;
                //maxXmlOrder = GetMaxOrderFromXmlElementAttributes(item);
                xmlOrder = -1; //-1 is a special case indicating root node
            }

            maxXmlOrder = GetMaxOrderFromXmlElementAttributes(par);

            //Look for a direct reference match to item in the existing par properties
            //if we find a match, that means that item is not contained inside a list or array property 
            //      we know this because we do not have any parent classes defined in SDC that implement IEnumerable in the SDC class itself

            //If there is more than one reference match to item, then we have created a wrapper property in the par (parent) partial class.
            //  and both outer and inner PropertyInfo structs found in ieProps reference the same item object.
            //We want the inner property, because its PropertyInfo object contains the XmlElementAttributes we need.
            //Outer wrapper PropertyInfo objects do NOT have XmlElementAttributes.

            ///////////////////////////////////////////////////////////////
            IList<PropertyInfo> piSet = par.GetType().GetProperties()?.Where(pi => ReferenceEquals(pi.GetValue(par), item))?.ToList();

            if (piSet != null && piSet.Count() > 0)
            {
                foreach (PropertyInfo propInfo in piSet)
                {
                    //The true inner property will have XmlElementAttributes on it 
                    var a1 = (XmlElementAttribute[])Attribute.GetCustomAttributes(propInfo, typeof(XmlElementAttribute));
                    if (a1?.Count() > 0)
                    {
                        propName = propInfo.Name;
                        //xmlElementName = ReflectElementName(propInfo, out xmlOrder);
                        xmlElementName = ReflectSdcElement(propInfo, item, out xmlOrder, itemIndex);
                        return propInfo;
                    }
                }
            }

            //Look in generic arrays and Lists of the par properties, and match to item reference

            PropertyInfo tempPI = null;
            //Find all IEnumerable PropertyInfo objects in par
            //var ieProps = par.GetType().GetProperties().Where(n => n.GetValue(par) is IEnumerable<BaseType>);  //GetValue can cause infinite recursion if we are evaluating the calling object.
            var ieProps
                = par.GetType().GetProperties().Where(n => {
                return typeof(IEnumerable<BaseType>).IsAssignableFrom(n.PropertyType);

            });


            foreach (var propInfo in ieProps) //loop through IEnumerable PropertyInfo objects in par
            {
                if (propInfo != null)
                {
                    ieItems = (IEnumerable<BaseType>)propInfo.GetValue(par, null); //Reflect each list to see if our item parameter lives in it
                    itemIndex = IndexOf(ieItems, item); //search for item
                }

                if (itemIndex > -1) //We found an IEnumerable property object in par that contains a reference to item
                                    //Look for XmlElementAttributes on the IEnumerable property object in par 
                {
                    propName = propInfo.Name;  //property name ot the IEnumerable property object in par
                    var xmlAtts = (XmlElementAttribute[])Attribute.GetCustomAttributes(propInfo, typeof(XmlElementAttribute));

                    if (xmlAtts.Count() > 0) //we found the correct PropertyInfo object, which has XmlElementAttributes
                    {
                        //xmlElementName = ReflectElementName(propInfo, out xmlOrder, itemIndex);
                        xmlElementName = ReflectSdcElement(propInfo, item, out xmlOrder, itemIndex);

                        return propInfo;
                    }
                    else tempPI = propInfo;  //save , just in case we can't find a better match in ieItems
                }
            }
            if (tempPI != null) //even though tempPI has no XmlElementAttribute, it still contains our item
                return tempPI;  //we already extracted (out) xmlElementName, (out) xmlOrder and (out) itemIndex, so we can just return now.

            return null;

            //!+---------------------Local Methods------------------------------

            string ReflectElementName(PropertyInfo pi, out int xmlOrder, int itemIndex = -1)
            {
                //pi is the propertyInfo object on the parent object's class
                //it either holds a direct reference to the item or it is an IEnumerable that contains a reference to the item
                //in either case, pi holds XmlElementAttributes that we need to reflect to extract ElementName and Order.
                //ElementName is not always present, and DataType is not always present in the XmlElementAttribute
                //  so ElementName ultimately may need to be obtained from the pi.Name property, 
                //  which is the name of property (the one that references item) in the parent (par) class.
                xmlOrder = 0;
                if (pi is null) return null;
                //Debug.Print("item.GetType(): " + item.GetType().Name);

                var dtAtts = (XmlElementAttribute[])Attribute.GetCustomAttributes(pi, typeof(XmlElementAttribute));

                if (dtAtts != null && dtAtts.Count() > 0) xmlOrder = dtAtts.ToArray()[0].Order;

                if (getNames)
                {
                    string elementName = ElementNameFromEnum(item, ItemChoiceEnum(pi), itemIndex);
                    if (elementName != null) return elementName;

                    //There was no ElementName to extract from an ItemChoiceEnum, so we get it directly from the attribute.
                    if (dtAtts.Count() == 1) return dtAtts.ToArray()[0].ElementName;

                    dtAtts = dtAtts.Where(a => a.Type == item.GetType()).ToArray();
                    //return ElementName based on data type match in the XMLAttribute
                    if (dtAtts.Count() == 1) return dtAtts.ToArray()[0].ElementName;

                    //There was no ElementName to extract from an XmlElementAttribute, so we get it directly from the propName.
                    return pi.Name;
                }
                return "";
                //!+---------------------------------------

                string ElementNameFromEnum(BaseType item, object choiceIdentifierObject, int arrayIndex = -1)
                {
                    if (choiceIdentifierObject is Enum)
                        return ((Enum)choiceIdentifierObject).ToString();

                    if (choiceIdentifierObject is IEnumerable && arrayIndex > -1)
                    {
                        var ie = ((IEnumerable)choiceIdentifierObject);
                        return ((Enum)GetObjectFromIEnumerableIndex(ie, arrayIndex)).ToString();
                    }
                    return null;
                }
                //!+---------------------------------------
                object ItemChoiceEnum(PropertyInfo pi)
                {
                    string enumName = ItemChoiceEnumName(pi);
                    if (enumName == null) return null;

                    var enumObj = par.GetType().GetProperty(enumName).GetValue(par);
                    if (enumObj is Enum) return (Enum)enumObj;
                    if (enumObj is IEnumerable[]) return (IEnumerable<Enum>[])enumObj;
                    return null;
                    //!+---------------------------------------
                    string ItemChoiceEnumName(PropertyInfo pi)
                    {
                        XmlChoiceIdentifierAttribute xci = (XmlChoiceIdentifierAttribute)pi.GetCustomAttribute(typeof(XmlChoiceIdentifierAttribute));
                        if (xci is null) return null;
                        return xci.MemberName;
                    }
                }
            }
        }

        public static string ReflectSdcElement(PropertyInfo piItem, BaseType item, out int xmlOrder, int itemIndex = -1)
        {
            //pi is the propertyInfo object on the parent object's class
            //it either holds a direct reference to the item or it is an IEnumerable that contains a reference to the item
            //in either case, pi holds XmlElementAttributes that we need to reflect to extract ElementName and Order.
            //ElementName is not always present, and DataType is not always present in the XmlElementAttribute
            //  so ElementName ultimately may need to be obtained from the pi.Name property, 
            //  which is the name of property (the one that references item) in the parent (par) class.
            xmlOrder = 0;
            if (piItem is null) return null;
            //Debug.Print("item.GetType(): " + item.GetType().Name);

            var dtAtts = (XmlElementAttribute[])Attribute.GetCustomAttributes(piItem, typeof(XmlElementAttribute));

            if (dtAtts != null && dtAtts.Count() > 0) xmlOrder = dtAtts.ToArray()[0].Order;
            string elementName = ElementNameFromEnum(item, ItemChoiceEnum(piItem), itemIndex);
            if (elementName != null) return elementName;

            //There was no ElementName to extract from an ItemChoiceEnum, so we get it directly from the attribute.
            if (dtAtts.Count() == 1) return dtAtts.ToArray()[0].ElementName;
            //return ElementName based on data type match in the XMLAttribute
            dtAtts = dtAtts.Where(a => a.Type == item.GetType()).ToArray();
            
            if (dtAtts.Count() == 1) return dtAtts.ToArray()[0].ElementName;
            //There was no ElementName to extract from an XmlElementAttribute, so we get it directly from the propName.
            return piItem.Name;
            //!+---------Local Function------------------------------
            string ElementNameFromEnum(BaseType item, object choiceIdentifierObject, int arrayIndex = -1)
            {
                if (choiceIdentifierObject is Enum)
                    return ((Enum)choiceIdentifierObject).ToString();

                if (choiceIdentifierObject is IEnumerable && arrayIndex > -1)
                {
                    var ie = ((IEnumerable)choiceIdentifierObject);
                    return ((Enum)GetObjectFromIEnumerableIndex(ie, arrayIndex)).ToString();
                }
                return null;
            }
            //!+--------Local Function-------------------------------
            object ItemChoiceEnum(PropertyInfo pi)
            {
                string enumName = ItemChoiceEnumName(pi);
                if (enumName == null) return null;

                var enumObj = item.ParentNode.GetType().GetProperty(enumName).GetValue(item.ParentNode);
                if (enumObj is Enum) return (Enum)enumObj;
                if (enumObj is IEnumerable[]) return (IEnumerable<Enum>[])enumObj;
                return null;
                //!+---------Local Function------------------------------
                string ItemChoiceEnumName(PropertyInfo pi)
                {
                    XmlChoiceIdentifierAttribute xci = (XmlChoiceIdentifierAttribute)pi.GetCustomAttribute(typeof(XmlChoiceIdentifierAttribute));
                    if (xci is null) return null;
                    return xci.MemberName;
                }
            }
        }


        public static List<PropertyInfoOrdered> ReflectPropertyInfoList(BaseType bt)
        {
            return ReflectPropertyInfoElements(bt.GetType().GetTypeInfo());
        }
        public static List<PropertyInfoOrdered> ReflectPropertyInfoElements(TypeInfo ti)
        {
            if (ti is null) return null;
            var props = new List<PropertyInfoOrdered>();
            foreach (var p in ti.GetProperties())
            {
                var att = p.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault();
                if (att != null)

                    props.Add(new PropertyInfoOrdered(p, att.Order));
            }

            props.Sort(new PropertyInfoOrderedComparer());
            return props;

        }
        /// <summary>
        /// Uses reflection to determine XML attributes that are eligible to be serialized.
        /// </summary>
        /// <param name="ti"></param>
        /// <returns>List&lt;PropertyInfo></returns>
        public static List<PropertyInfo> ReflectPropertyInfoAttributes(TypeInfo ti)
        {
            if (ti is null) return null;
            var props = new List<PropertyInfo>();
            foreach (var p in ti.GetProperties())
            {
                var att = p.GetCustomAttributes<XmlAttributeAttribute>().FirstOrDefault();
                if (att != null)

                    props.Add(p);
            }
            props.Sort(new AttributeComparer());
            return props;
        }
        /// <summary>
        /// Uses reflection to determine XML attributes that will be serialized, based on the passed parameter object.
        /// These attributes are determined by innvoking the "ShouldSerialize[Attribute Name]" methods in the passed parameter
        /// </summary>
        /// <param name="bt">A non-null SDC object derrived from BaseType</param>
        /// <returns>List&lt;PropertyInfo></returns>
        public static List<PropertyInfo> ReflectXmlAttributesFilled(BaseType bt)
        {
            TypeInfo ti = bt.GetType().GetTypeInfo();
            if (ti is null) return null;
            var attProps = new List<PropertyInfo>();
            foreach (var p in ReflectXmlAttributesAll(bt))
            {
                if ((bool)ti.GetMethod("ShouldSerialize" + p.Name).Invoke(bt, null))
                    attProps.Add(p);
            }
            return attProps;
        }
        public static List<PropertyInfo> ReflectXmlAttributesAll(BaseType bt)
        {
            return ReflectPropertyInfoAttributes(bt.GetType().GetTypeInfo());
        }
        public static List<BaseType> ReflectChildList(BaseType bt)
        {
            if (bt is null) return null;
            var kids = new List<BaseType>();
            foreach (var p in bt.GetType().GetProperties())
            {
                var att = p.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault();
                if (att != null)
                {
                    var kid = p.GetValue(bt);
                    if (kid != null)
                    {
                        if (kid is BaseType btKid) kids.Add(btKid);
                        else if (kid is IList kidList)
                        {
                            var pList = kidList.OfType<BaseType>().Cast<BaseType>();
                            foreach (BaseType lkid in pList) { kids.Add(lkid); }
                        }
                    }
                }
            }

            var tc = new TreeComparer();
            kids.Sort(tc);
            return kids;
        }

        public static List<BaseType> ReflectSubtree(BaseType bt, bool reOrder = false, bool reRegisterNodes = false)
        {
            if (bt is null) return null;
            var i = 0;
            var kids = new List<BaseType>();
            kids.Add(bt); //root node
            if (reOrder) bt.order = i++;

            ReflectSubtree2(bt);
            void ReflectSubtree2(BaseType bt)
            {
                var cList = ReflectChildList(bt);
                foreach (BaseType c in cList)
                {
                    kids.Add(c);

                    if (reRegisterNodes)
                    {
                        c.UnRegisterParent();
                        c.RegisterParent(bt);
                    }
                    if (reOrder) c.order = i++;

                    ReflectSubtree2(c);
                }
            }
            return kids;

        }

        public static List<BaseType> GetSortedNodeList(ITopNode tn)
        {
            return GetSubtreeList(tn.TopNode as BaseType);
            //var n = tn.TopNode as BaseType;
            // var nodes = tn.Nodes;
            // var cn = tn.ChildNodes;
            // int i = 0;
            //
            // var sortedList = new List<BaseType>();
            // BaseType firstChild;
            // BaseType nextSib;

            // 
            // MoveNext(n);

            // void MoveNext(BaseType n)
            // {
            //     firstChild = null;
            //     nextSib = null;
            //     //n.order = i; 

            //     sortedList.Add(n);
            //     i++;


            //     if (cn.TryGetValue(n.ObjectGUID, out List<BaseType> childList))
            //     {
            //         firstChild = childList[0];
            //         if (firstChild != null)
            //             MoveNext(firstChild);
            //     }

            //     var par = n.ParentNode;
            //     if (par != null)
            //     {
            //         if (cn.TryGetValue(par.ObjectGUID, out List<BaseType> sibList))
            //         {
            //             var index = sibList.IndexOf(n);
            //             if (index < sibList.Count - 1)
            //             {
            //                 nextSib = sibList[index + 1];
            //                 if (nextSib != null)
            //                     MoveNext(nextSib);
            //             }
            //         }
            //     }
            // }
            // return sortedList;
        }

        public static List<BaseType> ReorderNodes(BaseType n)
        {
            return GetSubtreeList(n, 0, 1);
        }

        public static List<BaseType> GetSubtreeList(BaseType n, int startReorder = -1, int orderMultiplier = 1)
        {
            //var nodes = n.TopNode.Nodes;
            var cn = n.TopNode.ChildNodes;
            int i = 0;
            var sortedList = new List<BaseType>();

            MoveNext(n);

            void MoveNext(BaseType n)
            {
                sortedList.Add(n);
                if (startReorder >= 0)
                {
                    n.order = i * orderMultiplier;
                    i++;
                }

                if (cn.TryGetValue(n.ObjectGUID, out List<BaseType> childList))
                {
                    if (childList != null)
                        foreach (var child in childList)
                            MoveNext(child);
                }
            }
            return sortedList;
        }

        public static Dictionary<Guid, BaseType> GetSubtreeDictionary(BaseType n, int startReorder = -1, int orderMultiplier = 1)
        {
            //var nodes = n.TopNode.Nodes;
            var cn = n.TopNode.ChildNodes;
            int i = 0;
            var dict = new Dictionary<Guid, BaseType>();

            MoveNext(n);

            void MoveNext(BaseType n)
            {
                dict.Add(n.ObjectGUID, n);
                if (startReorder >= 0)
                {
                    n.order = i * orderMultiplier;
                    i++;
                }

                if (cn.TryGetValue(n.ObjectGUID, out List<BaseType> childList))
                {
                    if (childList != null)
                        foreach (var child in childList)
                            MoveNext(child);
                }
            }
            return dict;
        }

        public static BaseType GetLastDescendantNode(BaseType n)
        {
            //var nodes = n.TopNode.Nodes;
            var cn = n.TopNode.ChildNodes;

            BaseType lastNode = null;
            //bool doSibs = false;

            MoveNext(n);

            void MoveNext(BaseType n)
            {
                //if (doSibs)
                //{
                //    var par = n.ParentNode;
                //    if (par != null)
                //    {
                //        if (cn.TryGetValue(par.ObjectGUID, out List<BaseType> sibList))
                //        {                            
                //            if (sibList != null)
                //            {
                //                lastNode = sibList.Last();
                //                if(lastNode != n)
                //                    MoveNext(lastNode);
                //            }
                //        }
                //    }
                //}
                //else doSibs = true;

                if (cn.TryGetValue(n.ObjectGUID, out List<BaseType> childList))
                {
                    if (childList != null)
                    {
                        lastNode = childList.Last();
                        MoveNext(lastNode);
                    }
                }
            }
            return lastNode;
        }


        public static bool IsParentNodeAllowed(BaseType item, BaseType newParent, out object pObj, int newListIndex = -1)
        { //reflect the object tree to determine if "this" can be attached to the SDC XML element represented by teh targetProperty object.   
            //We must find an exact match for the element and the data type in the targetProperty to allow the move.

            pObj = null;  //the object to which new nodes are attached; it may be an array or List<> or a non-List object.

            if (newParent is null) return false;
            //make sure that item and target are not null and are part of the same tree
            if (item.TopNode.Nodes[newParent.ObjectGUID] is null) return false;

            Type itemType = item.GetType();
            var thisPi = SdcUtil.GetPropertyInfo(item);
            string itemName = thisPi.XmlElementName;

            foreach (var p in newParent.GetType().GetProperties())
            {
                var pAtts = p.GetCustomAttributes<XmlElementAttribute>();

                if (pAtts.Count() > 0)
                {
                    pObj = p.GetValue(newParent);  //object that can be assigned to "this"; it may be a List or Array to contain "this" as an element, or another BaseType object that can be set directly to "this"
                    foreach (var a in pAtts)
                    {
                        if (a.ElementName == itemName)
                        {
                            if (a.Type == itemType)
                                return true; //if type matches, then ElementName must also match.  This is the most common case.

                            if (a.Type is null &&
                                p.PropertyType == itemType
                                )
                                return true;

                            if (p.PropertyType.IsGenericType &&
                                p.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) &&
                                p.PropertyType.GetGenericArguments()[0] == itemType
                                )
                                return true;

                            if (p.PropertyType.IsArray &&
                                p.PropertyType.GetElementType() == itemType
                                )
                                return true;
                        }
                    }
                    //if none of the XmlElementAttributes had a matching Type an ElementName, perhaps the property Type will match directly
                    if (p.Name == itemName)
                    {
                        if (p.PropertyType == itemType)
                            return true;

                        if (p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) &&
                            p.PropertyType.GetGenericArguments()[0] == itemType
                            )
                            return true;

                        if (p.PropertyType.IsArray &&
                            p.PropertyType.GetElementType() == itemType
                            )
                            return true;
                    }
                }
            }
            pObj = null;
            return false;
        }

        static bool IsParentNodeAllowed(PropertyInfo piNewParentProperty, BaseType item, string itemName = null)
        {
            //We want to know if the itemName is allowed to be atttached at a hypothetical property defined by piNewParent
            //piNewParent 

            if (piNewParentProperty is null || item is null) return false;

            Type itemType = item.GetType();
            if (itemName.IsEmpty()) itemName = item.GetPropertyInfo().XmlElementName;

            var pAtts = piNewParentProperty.GetCustomAttributes<XmlElementAttribute>();

            if (pAtts.Count() > 0)
            {
                foreach (var a in pAtts)
                {
                    if (a.ElementName == itemName)
                    {
                        if (a.Type == itemType)
                            return true; //if type matches, then ElementName must also match.  This is the most common case.

                        if (a.Type is null &&
                            piNewParentProperty.PropertyType == itemType
                            )
                            return true;

                        if (piNewParentProperty.PropertyType.IsGenericType &&
                            piNewParentProperty.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) &&
                            piNewParentProperty.PropertyType.GetGenericArguments()[0] == itemType
                            )
                            return true;

                        if (piNewParentProperty.PropertyType.IsArray &&
                            piNewParentProperty.PropertyType.GetElementType() == itemType
                            )
                            return true;
                    }
                }
                //if none of the XmlElementAttributes had a matching Type an ElementName, perhaps the property Type will match directly
                if (piNewParentProperty.Name == itemName)
                {
                    if (piNewParentProperty.PropertyType == itemType)
                        return true;

                    if (piNewParentProperty.PropertyType.IsGenericType &&
                        piNewParentProperty.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) &&
                        piNewParentProperty.PropertyType.GetGenericArguments()[0] == itemType
                        )
                        return true;

                    if (piNewParentProperty.PropertyType.IsArray &&
                        piNewParentProperty.PropertyType.GetElementType() == itemType
                        )
                        return true;
                }
            }
            return false;
        }
        #endregion



        #region Retired

        public static bool X_IsItemChangeAllowed<S, T>(S source, T target)
            where S : notnull, IdentifiedExtensionType
            where T : notnull, IdentifiedExtensionType
        {
            ChildItemsType ci;
            switch (source)
            {
                case SectionItemType _:
                    ci = (source as ChildItemsType);
                    switch (target)
                    {
                        case SectionItemType _:
                        case QuestionItemType _:
                            return true;
                        case ButtonItemType _:
                        case DisplayedType _:
                            if (ci is null) return true;
                            if (ci.ChildItemsList is null) return true;
                            if (ci.ChildItemsList.Count == 0) return true;
                            return false;
                        case InjectFormType j:
                            return false;
                        default: return false;
                    }

                case QuestionItemType q:
                    ci = (source as ChildItemsType);
                    switch (target)
                    {
                        case QuestionItemType _:
                            //probably should not allow changing Q types
                            return false;
                        default: return false;
                    }

                case ListItemType _:
                    ci = (source as ChildItemsType);
                    switch (target)
                    {
                        case SectionItemType _:
                        case QuestionItemType _:
                        case ListItemType _:
                        case InjectFormType _:
                            return false;
                        case ButtonItemType _:
                        case DisplayedType _:
                            if (ci is null) return true;
                            if (ci.ChildItemsList is null) return true;
                            if (ci.ChildItemsList.Count == 0) return true;
                            return false;
                        default: return false;
                    }

                case ButtonItemType b:
                    return false;
                case DisplayedType d:
                    return true;
                case InjectFormType j:
                    return false;
                default:
                    break;
            }
            return false;
        }


        internal static List<T> X_GetStatedListParent<T>(T item)
    where T : BaseType
        {   //get the list object that points to the item node
            //Only works for SDC List<BaseType> derivitives.   Does not work e.g., for XML types, derived from XmlElement.
            //Work out how to return a list of the exact type <T>.

            var pn = item.ParentNode;
            if (pn is null) return null;

            switch (item)
            {
                case ExtensionType et:
                    return (pn as ExtensionBaseType).Extension as List<T>;
                case CommentType ct:
                    return (pn as ExtensionBaseType).Comment as List<T>;
                case PropertyType pt:
                    return (pn as ExtensionBaseType).Property as List<T>;
                case EventType ev:
                    return X_GetStatedEventParent(ev).Cast<T>().ToList();
                case SectionItemType s:
                case ListItemType li:
                case QuestionItemType q:
                    return (pn as ChildItemsType).Items as List<T>;
                case BlobType bt:
                    return (pn as DisplayedType).BlobContent as List<T>;
                case CodingType ct:
                    return (pn as DisplayedType).CodedValue as List<T>;
                case ContactType ctt:
                    return (pn as DisplayedType).Contact as List<T>;
                case LinkType lt:
                    return (pn as DisplayedType).Link as List<T>;
                case "xx":
                    return (pn as DisplayedType).BlobContent as List<T>;

                default:
                    throw new ArgumentException("Unknown input item:" + item.ElementName ?? "\"\"");
            }

            return null;
        }
        private static List<BaseType> X_GetStatedListParent(BaseType item, string elementName)
        {   //get the list object that points to the item node
            //Only works for SDC List<BaseType> derivitives.   Does not work e.g., for XML types, derived from XmlElement.
            //Work out how to return a list of the exact type <T>.

            //TODO: trap errors here: loook for null parent...
            var pn = item.ParentNode;
            List<BaseType> list;

            switch (item.GetType().Name)
            {
                case "Extension":
                    list = (pn as ExtensionBaseType).Extension.Cast<BaseType>().ToList();
                    return list;
                case "Comment":
                    list = (pn as ExtensionBaseType).Comment.Cast<BaseType>().ToList();
                    return list;
                case "Property":
                    list = (pn as ExtensionBaseType).Property.Cast<BaseType>().ToList();
                    return list;
                case "OnEvent":
                    list = (pn as FormDesignType).OnEvent.Cast<BaseType>().ToList();
                    if (list != null) return list;
                    list = (pn as DisplayedType).OnEvent.Cast<BaseType>().ToList();
                    if (list != null) return list;
                    break;
                case "Section":
                case "ListItem":
                case "Question":
                case "Header":
                case "Body":
                case "Footer":
                    list = (pn as ChildItemsType).Items.Cast<BaseType>().ToList();
                    return list;
                case "BlobContent":
                    list = (pn as DisplayedType).BlobContent.Cast<BaseType>().ToList();
                    return list;
                case "CodedValue":
                    list = (pn as DisplayedType).CodedValue.Cast<BaseType>().ToList();
                    return list;
                case "Contact":
                    list = (pn as DisplayedType).Contact.Cast<BaseType>().ToList();
                    return list;
                case "Link":
                    list = (pn as DisplayedType).Link.Cast<BaseType>().ToList();
                    return list;
                case "OnEnter":
                    list = (pn as DisplayedType).OnEnter.Cast<BaseType>().ToList();
                    return list;
                case "OnSelect":
                    list = (pn as ListItemBaseType).OnSelect.Cast<BaseType>().ToList();
                    return list;
                case "OnDeselect":
                    list = (pn as ListItemBaseType).OnDeselect.Cast<BaseType>().ToList();
                    return list;
                case "xx":
                    list = (pn as DisplayedType).BlobContent.Cast<BaseType>().ToList();
                    return list;



                default:
                    break;



            }


            return null;
        }
        private static List<T> X_GetStatedEventParent<T>(T item)
            where T : EventType
        {
            var elementName = item.ElementName;
            var pn = item.ParentNode;
            if (pn is null) return null;
            List<T> list;


            if (item is FormDesignType)
            {
                switch (elementName)
                {
                    case "OnEvent":
                        list = (pn as FormDesignType).OnEvent as List<T>;
                        return list;
                }
                if (item is ListItemBaseType)
                    switch (elementName)
                    {
                        case "OnSelect":
                            list = (pn as ListItemBaseType).OnSelect as List<T>;
                            if (list != null) return list;
                            break;
                        case "OnDeselect":
                            list = (pn as ListItemBaseType).OnDeselect as List<T>;
                            if (list != null) return list;
                            break;
                    }
                if (item is DisplayedType)
                {
                    switch (elementName)
                    {
                        case "OnEvent":
                            list = (pn as DisplayedType).OnEvent as List<T>;
                            if (list != null) return list;
                            break;
                        case "OnEnter":
                            list = (pn as DisplayedType).OnEnter as List<T>;
                            if (list != null) return list;
                            break;
                        case "OnExit":
                            list = (pn as DisplayedType).OnExit as List<T>;
                            if (list != null) return list;
                            break;
                        default:
                            throw new ArgumentException("Unknown ElementName:" + elementName ?? "\"\"");
                    }

                    if (item is ResponseFieldType)
                        switch (elementName)
                        {
                            case "OnEvent":
                                list = (pn as ResponseFieldType).OnEvent as List<T>;
                                if (list != null) return list;
                                break;
                            case "AfterChange":
                                list = (pn as ResponseFieldType).AfterChange as List<T>;
                                if (list != null) return list;
                                break;
                            default:
                                throw new ArgumentException("Unknown ElementName:" + elementName ?? "\"\"");
                        }
                }
            }
            return null;
        }

        #endregion

        #region Helpers
        internal static string CreateName_(BaseType bt)
        {
            throw new NotImplementedException();
        }

        public static string XmlReorder(string Xml)
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

        public static string XmlFormat(string Xml)
        {
            return System.Xml.Linq.XDocument.Parse(Xml).ToString();  //prettify the minified doc XML 
        }

        #endregion



    }

    public static class BaseTypeExtensions
    {

        public static List<BaseType> GetChildList(this BaseType bt)
        {
            var cn = bt?.TopNode?.ChildNodes;
            if (cn is null) return null;
            if (cn.TryGetValue(bt.ParentNode.ObjectGUID, out List<BaseType> childList))
                return childList;
            return null;
        }
        public static bool IsAncestorOf(this BaseType ancestorNode, BaseType descendantNode)
        {
            if (descendantNode is null || ancestorNode is null || descendantNode == ancestorNode) return false;

            var par = descendantNode.ParentNode;
            while (par != null)
            { if (par.Equals(ancestorNode)) return true; }
           
            return false;
        }
        public static bool IsParentOf(this BaseType parentNode, BaseType childNode)
        {
            if (childNode.ParentNode == parentNode) return true;
            return false;
        }
        public static bool IsChildOf(this BaseType childNode, BaseType parentNode)
        {
            if (childNode.ParentNode == parentNode) return true;
            return false;
        }


        public static bool IsDescendantOf(this BaseType descendantNode, BaseType ancestorNode)
        {
            if (ancestorNode is null || descendantNode is null || ancestorNode == descendantNode) return false;

            var par = descendantNode.ParentNode;
            while (par != null)
            { if (par.Equals(ancestorNode)) return true; }
            return false;
        }

        /// <summary>
        /// Provides PropertyInfo (PI) definitions for all bt attributes that will be serialized to XML
        /// Each PI can be used t  obtain the type, name and other features of each attribute
        /// Also, each PI can be used to create an instance of the object by calling PI.GetValue(parentObject)
        /// </summary>
        /// <param name="bt"></param>
        /// <returns>List&lt;PropertyInfo></returns>
        public static List<PropertyInfo> GetXmlAttributesFilled(this BaseType bt)
        {
            return SdcUtil.ReflectXmlAttributesFilled (bt);
        }
        /// <summary>
        /// Provides PropertyInfo (PI) definitions for all XML attributes of an SDC node
        /// </summary>
        /// <param name="bt"></param>
        /// <returns><b>List&lt;PropertyInfo></b> </returns>
        public static List<PropertyInfo> GetXmlAttributesAll(this BaseType bt)
        {
            return SdcUtil.ReflectXmlAttributesAll(bt);             
        }
        public static List<PropertyInfoOrdered> GetPropertyInfoList(this BaseType bt)
        {
            return SdcUtil.ReflectPropertyInfoList(bt);
        }

        public static PropertyInfoMetadata GetPropertyInfoMetaData(this BaseType bt)
        {
            return SdcUtil.GetPropertyInfo(bt);
        }
        public static List<BaseType> GetSubtreeList(this BaseType bt)
        {
            return SdcUtil.GetSubtreeList(bt);
        }
        public static List<BaseType> GetSibs(this BaseType bt)
        {
            var par = bt?.ParentNode;
            if (par is null) return null;
            if (bt.TopNode.ChildNodes.TryGetValue(par.ObjectGUID, out List<BaseType> sibs)) 
                return sibs;

            return null;
        }
        public static bool X_IsItemChangeAllowed_(this IdentifiedExtensionType iet, IdentifiedExtensionType targetType)
        {            
            throw new NotImplementedException();

        }

    }
    public static class ActionsTypeExtensions
    {
        public static ActActionType AddActAction(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActActionType(at), insertPosition);
        }
        public static RuleSelectMatchingListItemsType AddActSelectMatchingListItems(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new RuleSelectMatchingListItemsType(at), insertPosition);
        }
        //public abstract ActSetPropertyType AddSetProperty(ActionsType at);
        public static ActAddCodeType AddActAddCode(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActAddCodeType(at), insertPosition);
        }
        //public abstract ActSetValueType AddSetValue(ActionsType at);
        public static ActInjectType AddActInject(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActInjectType(at), insertPosition);
        }
        public static CallFuncActionType AddActShowURL(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new CallFuncActionType(at), insertPosition);
        }
        public static ActSaveResponsesType AddActSaveResponses(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActSaveResponsesType(at), insertPosition);
        }
        public static ActSendReportType AddActSendReport(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActSendReportType(at), insertPosition);
        }
        public static ActSendMessageType AddActSendMessage(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActSendMessageType(at), insertPosition);
        }
        public static ActSetAttributeType AddActSetAttributeValue(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActSetAttributeType(at), insertPosition);
        }
        public static ActSetAttrValueScriptType AddActSetAttributeValueScript(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActSetAttrValueScriptType(at), insertPosition);
        }
        public static ActSetBoolAttributeValueCodeType AddActSetBoolAttributeValueCode(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActSetBoolAttributeValueCodeType(at), insertPosition);
        }
        public static ActShowFormType AddActShowForm(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActShowFormType(at), insertPosition);
        }
        public static ActShowMessageType AddActShowMessage(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActShowMessageType(at), insertPosition);
        }
        public static ActShowReportType AddActShowReport(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActShowReportType(at), insertPosition);
        }
        public static ActPreviewReportType AddActPreviewReport(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActPreviewReportType(at), insertPosition);
        }
        public static ActValidateFormType AddActValidateForm(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ActValidateFormType(at), insertPosition);
        }
        public static ScriptCodeAnyType AddActRunCode(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new ScriptCodeAnyType(at), insertPosition);
        }
        public static CallFuncActionType AddActCallFunction(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new CallFuncActionType(at), insertPosition);
        }
        public static PredActionType AddActConditionalGroup(this ActionsType at, int insertPosition = -1)
        {
            return AddAction(at, new PredActionType(at), insertPosition);
        }

        private static T AddAction<T>(this ActionsType at, T action, int insertPosition = -1) where T : ExtensionBaseType
        {
            var p = at;
            var lst = (IList<BaseType>)p.Items;
            int c = lst.Count;
            if (insertPosition > -1 && (insertPosition < c)) lst.Insert(insertPosition, action);
            else lst.Insert(c, action);
            return action;
        }
    } 
    public static class Validate
    {

        public static string ValidateSdcObjectTree(this ITopNode itn)
        {
            //custom statements to enforce some things that the object model and/or XML Schema can't enforce by themselves.
            //complex nestings of choice and sequence
            //datatype metadata encoded in XML (i.e., no in the Schema per se)
            //references to element names inside of rules
            //uniqueness of BaseURI/ID pairs in FormDesign, DemogFormDesign, DataElement etc.
            //content consistency inside of SDCPackages

            throw new NotImplementedException();
        }
        public static string ValidateSdcXml(string xml, string sdcSchemaUri = null)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/data/xml/xmlschemaset-for-schema-compilation
            try
            {
                var sdcSchemas = new XmlSchemaSet();

                if (sdcSchemaUri is null)
                {
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCRetrieveForm.xsd"));

                    //unclear if the following Schemas will be automatically discovered by the validator
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCFormDesign.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCMapping.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCBase.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCDataTypes.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCExpressions.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCResources.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCTemplateAdmin.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "xhtml.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "xml.xsd"));
                    //Extras, not currently used.
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDC_IDR.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCRetrieveFormComplex.xsd"));
                    sdcSchemas.Add(null, Path.Combine(Directory.GetCurrentDirectory(), "SDCOverrides.xsd"));
                    sdcSchemas.Compile();
                }
                ValidationLastMessage = "no error";
                var doc = new XmlDocument();
                doc.Schemas = sdcSchemas;
                doc.LoadXml(xml);
                doc.Validate(ValidationEventHandler);
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                ValidationLastMessage = ex.Message;
                //TODO: Should create error list to deliver all messages to ValidationLastMessage
            }
            return ValidationLastMessage;

        }
        public static string ValidationLastMessage { get; private set; }
        public static string ValidateSdcJson(string json)
        {
            return ValidateSdcXml(GetXmlFromJson(json));
        }

        public static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
            ValidationLastMessage = e.Message;
            //Should create error list to deliver all messages to ValidationLastMessage
        }

        public static string GetXmlFromJson(string json)
        {
            var doc = JsonConvert.DeserializeXmlNode(json);
            return doc.OuterXml;
        }

    }
    public static class ITopNodeExtensions
    {
        public static List<BaseType> ReorderNodes(this ITopNode itn)
        {
            return SdcUtil.ReorderNodes(itn.TopNode as BaseType);
        }
        public static bool AssignElementNamesByReflection(this ITopNode itn)
        {
            foreach (var kvp in itn.Nodes)
            {
                BaseType bt;
                bt = kvp.Value;
                bt.ElementName = bt.GetPropertyInfo().XmlElementName;
            }
            return true;
        }

        public static void AssignElementNamesFromXmlDoc( this ITopNode itn, string sdcXml)
        {
            //read as XMLDocument to walk tree
            var x = new XmlDocument();
            x.LoadXml(sdcXml);
            XmlNodeList xmlNodeList = x.SelectNodes("//*");
            int iXmlNode = 0;
            XmlNode xmlNode;

            foreach (BaseType bt in itn.Nodes.Values)
            {   //As we interate through the nodes, we will need code to skip over any non-element node, 
                //and still stay in sync with FD (using iFD). For now, we assume that every nodeList node is an element.
                //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?view=netframework-4.8
                //https://docs.microsoft.com/en-us/dotnet/standard/data/xml/types-of-xml-nodes
                xmlNode = xmlNodeList[iXmlNode];
                while (xmlNode.NodeType.ToString() != "Element")
                {
                    iXmlNode++;
                    xmlNode = xmlNodeList[iXmlNode];
                }

                var e = (XmlElement)xmlNode;
                bt.ElementName = e.LocalName;
                iXmlNode++;
            }
        }
        public static List<BaseType> GetSortedNodesList(this ITopNode itn)
        {
            return SdcUtil.GetSortedNodeList(itn);
        }

        public static ObservableCollection<BaseType> GetSortedNodesObsCol(this ITopNode itn)
        => new ObservableCollection<BaseType>(GetSortedNodesList(itn));



        #region Utilities

        public static void TreeLoadReset(this ITopNode itn) => BaseType.ResetSdcImport();

        #endregion

        #region GetItems

        public static Dictionary<Guid, BaseType> GetDescendantDictionary(this ITopNode itn, BaseType topNode)
        {
            var d = new Dictionary<Guid, BaseType>();
            getKids(topNode);

            void getKids(BaseType node)
            {
                //For each child, get all their children recursively, then add to dictionary
                var vals = (Dictionary<Guid, BaseType>)itn.Nodes.Values.Where(n => n.ParentID == node.ParentID);
                foreach (var n in vals)
                {
                    getKids(n.Value); //recurse
                    d.Add(n.Key, n.Value);
                }
            }
            return d;
        }
        public static List<BaseType> GetDescendantList(this ITopNode itn, BaseType topNode)
        {
            var curNode = itn;
            var lst = new List<BaseType>();
            getKids((BaseType)itn);

            void getKids(BaseType node)
            {
                //For each child, get all their children recursively, then add to dictionary
                var vals = (Dictionary<int, BaseType>)itn.Nodes.Values.Where(n => n.ParentID == curNode.ParentID);
                foreach (var n in vals)
                {
                    getKids(n.Value); //recurse
                    lst.Add(n.Value);
                }
            }

            return lst;
        }
        public static IdentifiedExtensionType GetItemByID(this ITopNode itn, string id)
        {
            IdentifiedExtensionType iet;
            iet = (IdentifiedExtensionType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(IdentifiedExtensionType)).Where(
                    t => ((IdentifiedExtensionType)t).ID == id).FirstOrDefault();
            return iet;
        }
        public static BaseType GetItemByName(this ITopNode itn, string name)
        {
            BaseType bt;
            bt = (BaseType)itn.Nodes.Values.Where(
                n => n.name == name).FirstOrDefault();
            return bt;
        }
        public static QuestionItemType GetQuestionByID(this ITopNode itn, string id)
        {
            QuestionItemType q;
            q = (QuestionItemType)itn.Nodes.Values.Where(
                    n => (n as QuestionItemType)?.ID == id).FirstOrDefault();
            return q;
        }
        public static QuestionItemType GetQuestionByName(this ITopNode itn, string name)
        {
            QuestionItemType q;
            q = (QuestionItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(QuestionItemType)).Where(
                    t => ((QuestionItemType)t).name == name).FirstOrDefault();
            return q;
        }
        public static DisplayedType GetDisplayedTypeByID(this ITopNode itn, string id)
        {
            DisplayedType d;
            d = (DisplayedType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(DisplayedType)).Where(
                    t => ((DisplayedType)t).ID == id).FirstOrDefault();
            return d;
        }
        public static DisplayedType GetDisplayedTypeByName(this ITopNode itn, string name)
        {
            DisplayedType d;
            d = (DisplayedType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(DisplayedType)).Where(
                    t => ((DisplayedType)t).name == name).FirstOrDefault();
            return d;
        }
        public static SectionItemType GetSectionByID(this ITopNode itn, string id)
        {
            SectionItemType s;
            s = (SectionItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(SectionItemType)).Where(
                    t => ((SectionItemType)t).ID == id).FirstOrDefault();
            return s;
        }
        public static SectionItemType GetSectionByName(this ITopNode itn, string name)
        {
            SectionItemType s;
            s = (SectionItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(SectionItemType)).Where(
                    t => ((SectionItemType)t).name == name).FirstOrDefault();
            return s;
        }
        public static ListItemType GetListItemByID(this ITopNode itn, string id)
        {
            ListItemType li;
            li = (ListItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(ListItemType)).Where(
                    t => ((ListItemType)t).ID == id).FirstOrDefault();
            return li;
        }
        public static ListItemType GetListItemByName(this ITopNode itn, string name)
        {
            ListItemType li;
            li = (ListItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(ListItemType)).Where(
                    t => ((ListItemType)t).name == name).FirstOrDefault();
            return li;
        }

        public static ButtonItemType GetButtonByID(this ITopNode itn, string id)
        {
            ButtonItemType b;
            b = (ButtonItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(ButtonItemType)).Where(
                    t => ((ButtonItemType)t).ID == id).FirstOrDefault();
            return b;
        }
        public static ButtonItemType GetButtonByName(this ITopNode itn, string name)
        {
            ButtonItemType b;
            b = (ButtonItemType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(ButtonItemType)).Where(
                    t => ((ButtonItemType)t).name == name).FirstOrDefault();
            return b;
        }
        public static InjectFormType GetInjectFormByID(this ITopNode itn, string id)
        {
            InjectFormType inj;
            inj = (InjectFormType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(InjectFormType)).Where(
                    t => ((InjectFormType)t).ID == id).FirstOrDefault();
            return inj;
        }
        public static InjectFormType GetInjectFormByName(this ITopNode itn, string name)
        {
            InjectFormType inj;
            inj = (InjectFormType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(InjectFormType)).Where(
                    t => ((InjectFormType)t).name == name).FirstOrDefault();
            return inj;
        }
        public static ResponseFieldType GetResponseFieldByName(this ITopNode itn, string name)
        {
            ResponseFieldType rf;
            rf = (ResponseFieldType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(ResponseFieldType)).Where(
                    t => ((ResponseFieldType)t).name == name).FirstOrDefault();
            //rf.Response.Item.GetType().GetProperty("val").ToString();
            return rf;
        }
        //BaseType GetResponseValByQuestionID(string id)
        //{

        //    var Q = GetQuestion(id);
        //    return Q.ResponseField_Item.Response.Item;

        //}
        public static PropertyType GetPropertyByName(this ITopNode itn, string name)
        {
            PropertyType p;
            p = (PropertyType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(PropertyType)).Where(
                    t => ((PropertyType)t).name == name).FirstOrDefault();
            return p;
        }
        public static ExtensionType GetExtensionByName(this ITopNode itn, string name)
        {
            ExtensionType e;
            e = (ExtensionType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(ExtensionType)).Where(
                    t => ((ExtensionType)t).name == name).FirstOrDefault();
            return e;
        }
        public static CommentType GetCommentByName(this ITopNode itn, string name)
        {
            CommentType c;
            c = (CommentType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(CommentType)).Where(
                    t => ((CommentType)t).name == name).FirstOrDefault();
            return c;
        }
        public static ContactType GetContactByName(this ITopNode itn, string name)
        {
            ContactType c;
            c = (ContactType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(CommentType)).Where(
                    t => ((ContactType)t).name == name).FirstOrDefault();
            return c;
        }
        public static LinkType GetLinkByName(this ITopNode itn, string name)
        {
            LinkType l;
            l = (LinkType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(LinkType)).Where(
                    t => ((LinkType)t).name == name).FirstOrDefault();
            return l;
        }
        public static BlobType GetBlobByName(this ITopNode itn, string name)
        {
            BlobType b;
            b = (BlobType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(BlobType)).Where(
                    t => ((BlobType)t).name == name).FirstOrDefault();
            return b;
        }
        public static CodingType GetCodedValueByName(this ITopNode itn, string name)
        {
            CodingType c;
            c = (CodingType)itn.Nodes.Values.Where(
                t => t.GetType() == typeof(CodingType)).Where(
                    t => ((CodingType)t).name == name).FirstOrDefault();
            return c;
        }

        public static BaseType GetNodeFromName(this ITopNode itn, string name) => itn.Nodes.Values.Where(n => n.name == name).FirstOrDefault();
        public static BaseType GetNodeFromObjectGUID(this ITopNode itn, Guid objectGUID)
        {
            itn.Nodes.TryGetValue(objectGUID, out BaseType n);
            return n;
        }

        public static IdentifiedExtensionType GetNodeFromID(this ITopNode itn, string id) =>
            (IdentifiedExtensionType)itn.Nodes.Values
            .Where(v => v.GetType() == typeof(IdentifiedExtensionType))
            .Where(iet => ((IdentifiedExtensionType)iet).ID == id).FirstOrDefault();


        #endregion       
    }

    public static class INewTopLevelExtensions { } //Empty
    public static class IPackageExtensions { } //Empty
    public static class IDataElementExtensions { } //Empty
    public static class IDemogFormExtensions { } //Empty
    public static class IMapExtensions { } //Empty
    public static class FormDesignTypeExtensions
    {
        //Default Implementations
        public static SectionItemType AddHeader(this FormDesignType fd)
        {
           // var fd = (ifd as FormDesignType);
            if (fd.Header == null)
            {
                fd.Header = new SectionItemType(fd, fd.ID + "_Header");  //Set a default ID, in case the database template does not have a body
                fd.Header.name = "Header";
            }
            return fd.Header;
        }
        public static SectionItemType AddBody(this FormDesignType fd)
        {
            //var fd = (ifd as FormDesignType);
            if (fd.Body == null)
            {
                fd.Body = new SectionItemType(fd, fd.ID + "_Body");  //Set a default ID, in case the database template does not have a body
                fd.Body.name = "Body";
            }
            return fd.Body;
        }
        public static SectionItemType AddFooter(this FormDesignType fd)
        {
            //var fd = (ifd as FormDesignType);
            if (fd.Footer == null)
            {
                fd.Footer = new SectionItemType(fd, fd.ID + "_Footer");  //Set a default ID, in case the database template does not have a body
                fd.Footer.name = "Footer";
            }
            return fd.Footer;
        }

    } 
    public static class RetrieveFormPackageTypeExtensions
    {
        public static LinkType AddFormURL_(this RetrieveFormPackageType rfp)
        { throw new NotImplementedException();  }
        public static HTMLPackageType AddHTMLPackage_(this RetrieveFormPackageType rfp)
        { throw new NotImplementedException(); }
        public static XMLPackageType AddXMLPackage_(this RetrieveFormPackageType rfp)
        { throw new NotImplementedException(); }
    } 
    public static class IChildItemsParentExtensions
    {        
        public static SectionItemType AddChildSection<T>(this IChildItemsParent<T> T_Parent, string id, string defTitle = null, int insertPosition = -1) where T : BaseType, IChildItemsParent<T>
        {
            var childItems = AddChildItemsNode(T_Parent);
            var childItemsList = childItems.ChildItemsList;
            var sNew = new SectionItemType(childItems, id);
            sNew.title = defTitle;
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, sNew);

            return sNew;
        }
        public static QuestionItemType AddChildQuestion<T>(this IChildItemsParent<T> T_Parent, QuestionEnum qType, string id, string defTitle = null, int insertPosition = -1) where T : BaseType, IChildItemsParent<T>
        {
            var childItems = AddChildItemsNode(T_Parent);
            var childItemsList = childItems.ChildItemsList;
            var qNew = new QuestionItemType(childItems, id);
            //ListFieldType lf;
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, qNew);

            switch (qType)
            {
                case QuestionEnum.QuestionSingle:
                    qNew.AddListFieldToQuestion().AddList();
                    break;
                case QuestionEnum.QuestionMultiple:
                    qNew.AddListFieldToQuestion().AddList();
                    qNew.ListField_Item.maxSelections = 0;
                    break;
                case QuestionEnum.QuestionFill:
                    qNew.AddQuestionResponseField(out DataTypes_DEType _);
                    break;
                case QuestionEnum.QuestionLookupSingle:
                    qNew.AddListFieldToQuestion().AddEndpoint();
                    break;
                case QuestionEnum.QuestionLookupMultiple:
                    qNew.AddListFieldToQuestion().AddEndpoint();

                    break;
                default:
                    throw new NotSupportedException($"{qType} is not supported");
            }
            qNew.title = defTitle;
            return qNew;
        }

        public static QuestionItemType AddChildQuestionResponse<T>(this IChildItemsParent<T> T_Parent,
            string id,
            out DataTypes_DEType deType,
            string defTitle = null,
            int insertPosition = -1,
            ItemChoiceType dt = ItemChoiceType.@string,
            string textAfterResponse = null,
            string units = null,
            dtQuantEnum dtQuant = dtQuantEnum.EQ,
            object valDefault = null) where T : BaseType, IChildItemsParent<T>
        {
            var childItems = AddChildItemsNode(T_Parent);
            var childItemsList = childItems.ChildItemsList;
            var qNew = new QuestionItemType(childItems, id);
            qNew.title = defTitle;

            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, qNew);
            var rf = qNew.AddQuestionResponseField(out deType, dt, dtQuant, valDefault);
            rf.AddResponseUnits(units);
            rf.AddTextAfterResponse(textAfterResponse);

            return qNew;

        }
        public static DisplayedType AddChildDisplayedItem<T>(this IChildItemsParent<T> T_Parent, string id, string defTitle = null, int insertPosition = -1) where T : BaseType, IChildItemsParent<T>
        {
            var childItems = AddChildItemsNode(T_Parent);
            var childItemsList = childItems.ChildItemsList;
            var dNew = new DisplayedType(childItems, id);
            dNew.title = defTitle;
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, dNew);

            return dNew;
        }
        public static ButtonItemType AddChildButtonAction<T>(this IChildItemsParent<T> T_Parent, string id, string defTitle = null, int insertPosition = -1) where T : BaseType, IChildItemsParent<T>
        {
            var childItems = AddChildItemsNode(T_Parent);
            var childItemsList = childItems.ChildItemsList;
            var btnNew = new ButtonItemType(childItems, id);
            btnNew.title = defTitle;
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, btnNew);

            // TODO: Add AddButtonActionTypeItems(btnNew);
            return btnNew;
        }
        public static InjectFormType AddChildInjectedForm<T>(this IChildItemsParent<T> T_Parent, string id, int insertPosition = -1) where T : BaseType, IChildItemsParent<T>
        {
            var childItems = AddChildItemsNode(T_Parent);
            var childItemsList = childItems.ChildItemsList;
            var injForm = new InjectFormType(childItems, id);
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, injForm);
            //TODO: init this InjectForm object

            return injForm;
        }
        public static bool HasChildItems<T>(this IChildItemsParent<T> T_Parent) where T : BaseType, IChildItemsParent<T>
        {
            {
                if (T_Parent?.ChildItemsNode?.ChildItemsList != null)
                {
                    foreach (var n in T_Parent.ChildItemsNode.ChildItemsList)
                    { if (n != null) return true; }
                }
            }
            return false;
        }
        public static ChildItemsType AddChildItemsNode<T>(this IChildItemsParent<T> T_Parent) where T : BaseType , IChildItemsParent<T>
        {
            ChildItemsType childItems = null;  //this class contains an "Items" list
            if (T_Parent == null)
                throw new ArgumentNullException("The T_Parent object was null");
            //return childItems; 
            else if (T_Parent.ChildItemsNode == null)
            {
                childItems = new ChildItemsType(T_Parent as BaseType);
                T_Parent.ChildItemsNode = childItems;  //This may be null for the Header, Body and Footer  - need to check this
            }
            else //(T_Parent.ChildItemsNode != null)
                childItems = T_Parent.ChildItemsNode;

            if (childItems.ChildItemsList == null)
                childItems.ChildItemsList = new List<IdentifiedExtensionType>();

            return childItems;
        }
    } 
    public static class IChildItemsMemberExtensions
    {
    //!    public static bool X_IsMoveAllowedToChild<U>(U Utarget, out string error)
    //where U : notnull, IdentifiedExtensionType
    //        //where T : notnull, IdentifiedExtensionType
    //    {
    //        Tchild Tsource = this as Tchild;
    //        var errorSource = "";
    //        var errorTarget = "";
    //        error = "";
    //        bool sourceOK = false;
    //        bool targetOK = false;

    //        if (Tsource is null) { error = "source is null"; return false; }
    //        if (Utarget is null) { error = "target is null"; return false; }
    //        if (Utarget is ButtonItemType) { error = "ButtonItemType is not allowed as a target"; return false; }
    //        if (Utarget is InjectFormType) { error = "InjectFormType is not allowed as a target"; return false; }
    //        if (Utarget is DisplayedType) { error = "DisplayedItem is not allowed as a target"; return false; }

    //        if (Tsource is ListItemType && !(Utarget is QuestionItemType) && !(Utarget is ListItemType)) { error = "A ListItem can only be moved into a Question List"; return false; };

    //        //special case to allow LI to drop on a Q and be added to the Q's List, rather than under ChildItem (which would be illegal)
    //        if (Tsource is ListItemType &&
    //            Utarget is QuestionItemType &&
    //            !((Utarget as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionSingle) &&
    //            !((Utarget as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionMultiple))
    //        { error = "A Question target must be a QuestionSingle or QuestionMultiple"; return false; }


    //        if (Tsource is DisplayedType || Tsource is InjectFormType) sourceOK = true;
    //        if (Utarget is QuestionItemType || Utarget is SectionItemType || Utarget is ListItemType) targetOK = true;

    //        if (!sourceOK || !targetOK)
    //        {
    //            if (!sourceOK) errorSource = "Illegal source object";
    //            if (!targetOK) errorTarget = "Illegal target object";
    //            if (errorTarget.Length > 0) errorTarget += " and ";
    //            error = errorSource + errorTarget;
    //        }


    //        return sourceOK & targetOK;
    //    }
    //!    public static bool X_MoveAsChild<S, T>(S source, T target, int newListIndex)
    //        where S : notnull, IdentifiedExtensionType    //, IChildItemMember
    //        where T : DisplayedType, IChildItemsParent<T>
    //    {
    //        if (source is null) return false;
    //        if (source.ParentNode is null) return false;
    //        if (source is ListItemType && !(target is QuestionItemType)) return false;  //ListItem can only be moved to a question.

    //        List<BaseType> sourceList;
    //        BaseType newParent = target;

    //        switch (source)  //get the sourceList from the parent node
    //        {
    //            case QuestionItemType _:
    //            case SectionItemType _:
    //            case InjectFormType _:
    //            case ButtonItemType _:
    //                sourceList = (source.ParentNode as ChildItemsType)?.Items.ToList<BaseType>();
    //                //sourceList = (source.ParentNode as ChildItemsType).Items.Cast<BaseType>().ToList(); //alternate method
    //                break;
    //            case ListItemType _:
    //                sourceList = (source.ParentNode as ListType)?.Items.ToList<BaseType>();
    //                break;
    //            case DisplayedType _:
    //                sourceList = (source.ParentNode as ChildItemsType)?.Items.ToList<BaseType>();
    //                if (sourceList is null)
    //                    sourceList = (source.ParentNode as ListType)?.Items.ToList<BaseType>();
    //                else return false;
    //                break;
    //            default:
    //                return false; //error in source type
    //        }

    //        if (sourceList is null) return false;

    //        List<BaseType> targetList = null;

    //        if (target != null)
    //        {
    //            switch (target)  //get the targetList from the child node
    //            {
    //                case QuestionItemType q:
    //                    //This is an exception - if we drop a source LI on a QS/QM, we will want to add it ant the end of the Q's List object
    //                    if (source is ListItemType)
    //                    {
    //                        if (q.GetQuestionSubtype() != QuestionEnum.QuestionSingle &&
    //                            q.GetQuestionSubtype() != QuestionEnum.QuestionMultiple &&
    //                            q.GetQuestionSubtype() != QuestionEnum.QuestionRaw) return false;  //QR, and QL cannot have child LI nodes
    //                        if (q.ListField_Item is null)  //create new targetList
    //                        {
    //                            targetList = IQuestionBuilder.AddListToListField(IQuestionBuilder.AddListFieldToQuestion(q)).Items.ToList<BaseType>();
    //                            if (targetList is null) return false;
    //                            break;
    //                        }
    //                        newParent = q.ListField_Item.List;
    //                        targetList = q.ListField_Item.List.Items.ToList<BaseType>();
    //                    }
    //                    else //use the ChildItems node instead as the targetList
    //                    {
    //                        (q as IChildItemsParent<QuestionItemType>).AddChildItemsNode(q);
    //                        targetList = q.ChildItemsNode.Items.ToList<BaseType>();
    //                    }
    //                    break;
    //                case SectionItemType s:
    //                    (s as IChildItemsParent<SectionItemType>).AddChildItemsNode(s);
    //                    targetList = s.ChildItemsNode.Items.ToList<BaseType>();
    //                    break;
    //                case ListItemType l:
    //                    (l as IChildItemsParent<ListItemType>).AddChildItemsNode(l);
    //                    targetList = l.ChildItemsNode.Items.ToList<BaseType>();
    //                    break;
    //                default:
    //                    return false; //error in source type
    //            }
    //        }
    //        else targetList = sourceList;
    //        if (targetList is null) return false;


    //        var count = targetList.Count;
    //        if (newListIndex < 0 || newListIndex > count) newListIndex = count; //add to end  of list

    //        var indexSource = sourceList.IndexOf(source);  //save the original source index in case we need to replace the source node back to its origin
    //        bool b = sourceList.Remove(source); if (!b) return false;
    //        targetList.Insert(newListIndex, source);
    //        if (targetList[newListIndex] == source) //check for success
    //        {
    //            source.TopNode.ParentNodes[source.ObjectGUID] = newParent;
    //            return true;
    //        }
    //        //Error - the source item is now disconnected from the list.  Lets add it back to the end of the list.
    //        sourceList.Insert(indexSource, source); //put source back where it came from; the move failed
    //        return false;
    //    }
    //!    public static bool X_MoveAfterSib<S, T>(S source, T target, int newListIndex, bool moveAbove)
    //        where S : notnull, IdentifiedExtensionType
    //        where T : notnull, IdentifiedExtensionType
    //    {
    //        //iupdate TopNode.ParentNodes
    //        throw new Exception(String.Format("Not Implemented"));
    //    }
    } //Empty
    public static class QuestionItemTypeExtensions
    {
        public static QuestionItemType ConvertToQR_(this QuestionItemType q, bool testOnly = false)
        { throw new NotImplementedException(); } //abort if children present
        public static QuestionItemType ConvertToQS_(this QuestionItemType q, bool testOnly = false)
        { throw new NotImplementedException(); }
        public static QuestionItemType ConvertToQM_(this QuestionItemType q, int maxSelections = 0, bool testOnly = false)
        { throw new NotImplementedException(); }
        public static DisplayedType ConvertToDI_(this QuestionItemType q, bool testOnly = false)
        { throw new NotImplementedException(); } //abort if LIs or children present
        public static QuestionItemType ConvertToSection_(this QuestionItemType q, bool testOnly = false)
        { throw new NotImplementedException(); }
        public static QuestionItemType ConvertToLookup_(this QuestionItemType q, bool testOnly = false)
        { throw new NotImplementedException(); }//abort if LIs present

        public static QuestionEnum GetQuestionSubtype(this QuestionItemType q)
        {
            if (q.ResponseField_Item != null) return QuestionEnum.QuestionFill;
            if (q.ListField_Item is null) return QuestionEnum.QuestionRaw;
            if (q.ListField_Item.LookupEndpoint == null && q.ListField_Item.maxSelections == 1) return QuestionEnum.QuestionSingle;
            if (q.ListField_Item.LookupEndpoint == null && q.ListField_Item.maxSelections != 1) return QuestionEnum.QuestionMultiple;
            if (q.ListField_Item.LookupEndpoint != null && q.ListField_Item.maxSelections == 1) return QuestionEnum.QuestionLookupSingle;
            if (q.ListField_Item.LookupEndpoint != null && q.ListField_Item.maxSelections != 1) return QuestionEnum.QuestionLookupMultiple;
            if (q.ListField_Item.LookupEndpoint != null) return QuestionEnum.QuestionLookup;

            return QuestionEnum.QuestionGroup;
        }
        public static ListItemType AddListItem(this QuestionItemType q, string id, string defTitle = null, int insertPosition = -1)
        {  //Check for QS/QM first!
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionMultiple ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionSingle ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionRaw)
            {
                if (q.ListField_Item is null) q.AddListFieldToQuestion();
                ListType list = q.ListField_Item.List;
                if (list is null) q.ListField_Item.AddList();

                ListItemType li = new ListItemType(list, id);
                li.title = defTitle;
                var count = list.QuestionListMembers.Count;
                if (insertPosition < 0 || insertPosition > count) insertPosition = count;
                list.QuestionListMembers.Insert(insertPosition, li);

                return li;
            }
            else throw new InvalidOperationException("Can only add ListItem to QuestionSingle or QuestionMultiple");
        }
        public static ListItemType AddListItemResponse(this QuestionItemType q,
            string id,
            out DataTypes_DEType deType,
            string defTitle = null,
            int insertPosition = - 1,
            ItemChoiceType dt = ItemChoiceType.@string, 
            bool responseRequired = false,
            string textAfterResponse = null,
            string units = null,
            dtQuantEnum dtQuant = dtQuantEnum.EQ, 
            object valDefault = null
            )
        {
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionMultiple ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionSingle ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionRaw)
            {
                var li = q.AddListItem(id, defTitle, insertPosition);
                var lirf = li.AddListItemResponseField();
                var rsp = lirf.AddDataType(dt, dtQuant, valDefault);

                lirf.responseRequired = responseRequired;
                lirf.AddResponseUnits(units);
                lirf.AddTextAfterResponse(textAfterResponse);

                deType = IDataHelpers.AddDataTypesDE(lirf, dt, dtQuant, valDefault);
                return li;

            }
            else throw new InvalidOperationException("Can only add ListItem to QuestionSingle or QuestionMultiple");
        }
        public static DisplayedType AddDisplayedTypeToList(this QuestionItemType q, 
            string id, 
            string defTitle = null,
            int insertPosition = -1)
        {
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionMultiple ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionSingle ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionRaw)
            {
                if (q.ListField_Item is null) q.AddListFieldToQuestion();
                ListType list = q.ListField_Item.List;
                if (list is null) list = q.ListField_Item.AddList();

                return list.AddDisplayedType(id, defTitle, insertPosition);
            }
            else throw new InvalidOperationException("Can only add DisplayedItem to QuestionSingle or QuestionMultiple");
        }

        public static ResponseFieldType AddQuestionResponseField(this QuestionItemType q, 
            out DataTypes_DEType deType, 
            ItemChoiceType dataType = ItemChoiceType.@string, 
            dtQuantEnum dtQuant = dtQuantEnum.EQ, 
            object valDefault = null)
        {
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionRaw)
            {
                var rf = new ResponseFieldType(q);
                q.ResponseField_Item = rf;
                deType = rf.AddDataType(dataType, dtQuant, valDefault);
                return rf;
            }else throw new Exception("A Question subtype has already been assigned to the Question.");
        }

        public static ListFieldType AddListFieldToQuestion(this QuestionItemType q)
        {
            if (q.ListField_Item == null)
            {
                var listField = new ListFieldType(q);
                q.ListField_Item = listField;
            }
            return q.ListField_Item;
        } 
    }
    public static class ListTypeExtensions

    {
        public static ListItemType AddListItem(this ListType lt, string id, string defTitle = null, int insertPosition = -1) //check that no ListItemResponseField object is present
        {
            ListItemType li = new ListItemType(lt, id);
            li.title = defTitle;
            var count = lt.QuestionListMembers.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            lt.QuestionListMembers.Insert(insertPosition, li);
            return li;
        }

        public static ListItemType AddListItemResponse(this ListType lt,
            string id,
            string defTitle = null,
            int insertPosition = -1,
            ItemChoiceType dt = ItemChoiceType.@string,
            bool responseRequired = false,
            string textAfterResponse = null,
            string units = null,
            dtQuantEnum dtQuant = dtQuantEnum.EQ,
            object valDefault = null)
        {
            var li = lt.AddListItem(id, defTitle, insertPosition);
            var lirf = li.AddListItemResponseField();
            lirf.AddDataType(dt, dtQuant, valDefault);
            lirf.responseRequired = responseRequired;
            lirf.AddResponseUnits(units);
            lirf.AddTextAfterResponse(textAfterResponse);

            return li;
        } //check that no ListFieldType object is present
        public static DisplayedType AddDisplayedType(this ListType list, string id, string defTitle = null, int insertPosition = -1)
        {
            var di = new DisplayedType(list, id) { title = defTitle };
            var count = list.QuestionListMembers.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            list.QuestionListMembers.Insert(insertPosition, di);

            return di;
        }
    } 

    public static class ListFieldTypeExtensions
    {
        public static LookupEndPointType AddEndpoint(this ListFieldType lf)
        {
            if (lf.List == null)
            {
                var lep = new LookupEndPointType(lf);
                lf.LookupEndpoint = lep;
                return lep;
            }
            else throw new InvalidOperationException("Can only add LookupEndpoint to ListField if List object is not present");
        }
        public static ListType AddList(this ListFieldType lf)
        {
            ListType list;  //this is not the .NET List class; It's an answer list
            if (lf.List == null)
            {
                list = new ListType(lf);
                lf.List = list;
            }
            else list = lf.List;

            //The "list" item contains a list<DisplayedType>, to which the ListItems and ListNotes (DisplayedItems) are added.
            if (list.QuestionListMembers == null)
                list.QuestionListMembers = new List<DisplayedType>();

            return list;
        }

    }
    public static class IQuestionBaseExtensions { } //Empty
    public static class ListItemTypeExtensions
    {
        public static ListItemResponseFieldType AddListItemResponseField(this ListItemType li)
        {
            var liRF = new ListItemResponseFieldType(li);
            li.ListItemResponseField = liRF;

            return liRF;
        }
        public static EventType AddOnDeselect(this ListItemType li)
        {
            var ods = new EventType(li);
            li.OnDeselect.Add(ods);
            return ods;
        }
        public static EventType AddOnSelect(this ListItemType li)
        {
            var n = new EventType(li);
            li.OnSelect.Add(n);
            return n;
        }
        public static PredGuardType AddSelectIf(this ListItemType li)
        {
            var n = new PredGuardType(li);
            li.SelectIf = n;
            return n;
        }
        public static PredGuardType AddDeSelectIf(this ListItemType li)
        {
            var n = new PredGuardType(li);
            li.DeselectIf = n;
            return n;
        }
    }
    //public static class ISectionExtensions { } //Empty
    public static class ButtonItemTypeExtensions
    {
        public static EventType AddOnClick_(this ButtonItemType bf)
        { throw new NotImplementedException(); }
    }
    public static class InjectFormTypeExtensions
    {  //ChildItems.InjectForm - this is mainly useful for a DEF injecting items based on the InjectForm URL
        //Item types choice under ChildItems
        public static FormDesignType AddFormDesign_(this InjectFormType ijt)
        { throw new NotImplementedException(); }
        public static QuestionItemType AddQuestion_(this InjectFormType ijt)
        { throw new NotImplementedException(); }
        public static SectionItemType AddSection_(this InjectFormType ijt)
        { throw new NotImplementedException(); }

    }
    public static class DisplayedTypeExtensions
    {//LinkType, BlobType, ContactType, CodingType, EventType, OnEventType, PredGuardType
        public static BlobType AddBlob(this DisplayedType dtParent, int insertPosition = -1)
        {
            var blob = new BlobType(dtParent);
            if (dtParent.BlobContent == null) dtParent.BlobContent = new List<BlobType>();
            var count = dtParent.BlobContent.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.BlobContent.Insert(insertPosition, blob);
            return blob;
        }
        public static LinkType AddLink(this DisplayedType dtParent, int insertPosition = -1)
        {
            var link = new LinkType(dtParent);

            if (dtParent.Link == null) dtParent.Link = new List<LinkType>();
            var count = dtParent.Link.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.Link.Insert(insertPosition, link);
            link.order = link.ObjectID;

            var rtf = new RichTextType(link);
            link.LinkText = rtf;
            return link;
        }
        public static ContactType AddContact(this DisplayedType dtParent, int insertPosition = -1)
        {
            if (dtParent.Contact == null) dtParent.Contact = new List<ContactType>();
            var ct = new ContactType(dtParent);
            var count = dtParent.Contact.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.Contact.Insert(insertPosition, ct);
            return ct;
        }
        public static CodingType AddCodedValue(this DisplayedType dtParent, int insertPosition = -1)
        {
            if (dtParent.CodedValue == null) dtParent.CodedValue = new List<CodingType>();
            var ct = new CodingType(dtParent);
            var count = dtParent.CodedValue.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.CodedValue.Insert(insertPosition, ct);
            return ct;
        }


        public static PredGuardType AddActivateIf(this DisplayedType dt)
        {
            var pg = new PredGuardType(dt, "ActivateIf", "acif");
            dt.ActivateIf = pg;
            return pg;
        }
        public static PredGuardType AddDeActivateIf(this DisplayedType dt)
        {
            var pg = new PredGuardType(dt, "DeActivateIf", "deif");
            dt.DeActivateIf = pg;
            return pg;
        }
        public static EventType AddOnEnter(this DisplayedType dt)
        {
            var ev = new EventType(dt, "OnEnter", "onen");
            dt.OnEnter.Add(ev);
            return ev;
        }
        public static OnEventType AddOnEvent(this DisplayedType dt)
        {
            var oe = new OnEventType(dt, "OnEvent", "onev");
            dt.OnEvent.Add(oe);
            return oe;
        }
        public static EventType AddOnExit(this DisplayedType dt)
        {
            var oe = new EventType(dt, "OnExit", "onex");
            dt.OnEnter.Add(oe);
            return oe;
        }
        public static bool MoveEvent_(this DisplayedType dt, EventType ev, List<EventType> targetList = null, int index = -1)
        {
            throw new NotImplementedException();
        }
    }
    public static class DisplayedTypeChangesExtensions
    {
        public static QuestionItemType ChangeToQuestionMultiple_(DisplayedType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionSingle_(DisplayedType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionResponse_(DisplayedType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionLookup_(DisplayedType source)
        { throw new NotImplementedException(); }
        public static SectionItemType ChangeToSection_(DisplayedType source)
        { throw new NotImplementedException(); }
        public static ButtonItemType ChangeToButtonAction_(DisplayedType source)
        { throw new NotImplementedException(); }
        public static InjectFormType ChangeToInjectForm_(DisplayedType source)
        { throw new NotImplementedException(); }

        public static DisplayedType ChangeToDisplayedItem_(SectionItemType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionMultiple_(SectionItemType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionSingle_(SectionItemType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionResponse_(SectionItemType source)
        { throw new NotImplementedException(); }
        public static QuestionItemType ChangeToQuestionLookup_(SectionItemType source)
        { throw new NotImplementedException(); }
        public static ButtonItemType ChangeToButtonAction_(SectionItemType source)
        { throw new NotImplementedException(); }
        public static InjectFormType ChangeToInjectForm_(SectionItemType source)
        { throw new NotImplementedException(); }


        public static DisplayedType ChangeToDisplayedItem_(ListItemType source)
        { throw new NotImplementedException(); }

        //ListItemType ChangeToListItem
        //ListItemType ChangeToListItemResponse
        //SectionItemType ChangeToSection()
        //ChangeToButtonAction
        //ChangeToInjectForm
        //etc.


        //Question
        public static SectionItemType ChangeToSection_(QuestionItemType source)
        { throw new NotImplementedException(); }
        public static DisplayedType ChangeToDisplayedType_(QuestionItemType source)
        { throw new NotImplementedException(); }
    }
    //public static class IDisplayedTypeMemberExtensions { }//Empty; for LinkType, BlobType, ContactType, CodingType, EventType, OnEventType, PredGuardType
    public static class BlobExtensions
    {
        //DisplayedItem.BlobType
        //Uses Items types choice
        public static bool AddBinaryMedia_(this BlobType b)
        { throw new NotImplementedException(); } //Empty

        public static bool AddBlobURI_(this BlobType b)
        { throw new NotImplementedException(); }
    }
    public static class ExtensionBaseTypeExtensions
    {
        public static bool HasExtensionBaseMembers(this ExtensionBaseType ebt) //Has Extension, Property or Comment sub-elements
        {
            //var ebt = ieb as ExtensionBaseType;
            if (ebt?.Property?.Count() > 0)
            {
                foreach (var n in ebt.Property)
                { if (n != null) return true; }
            }
            if (ebt?.Comment?.Count() > 0)
            {
                foreach (var n in ebt.Comment)
                { if (n != null) return true; }
            }
            if (ebt?.Extension?.Count() > 0)
            {
                foreach (var n in ebt.Extension)
                { if (n != null) return true; }
            }
            return false;
        }
        public static ExtensionType AddExtension(this ExtensionBaseType ebtParent, int insertPosition = -1)
        {
            //var ebtParent = ieb as ExtensionBaseType;
            var e = new ExtensionType(ebtParent);
            if (ebtParent.Extension == null) ebtParent.Extension = new List<ExtensionType>();
            var count = ebtParent.Extension.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            ebtParent.Extension.Insert(insertPosition, e);
            return e;
        }
        public static CommentType AddComment(this ExtensionBaseType ebtParent, int insertPosition = -1)
        {
            //var ebtParent = ieb as ExtensionBaseType;
            if (ebtParent.Comment == null) ebtParent.Comment = new List<CommentType>();
            CommentType ct = null;
            var count = ebtParent.Comment.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            ebtParent.Comment.Insert(insertPosition, ct);  //return new empty Comment object for caller to fill
            return ct;
        }
        public static PropertyType AddProperty(this ExtensionBaseType ebtParent, int insertPosition = -1)
        {
            //var ebtParent = ieb as ExtensionBaseType;
            var prop = new PropertyType(ebtParent);
            if (ebtParent.Property == null) ebtParent.Property = new List<PropertyType>();
            var count = ebtParent.Property.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            ebtParent.Property.Insert(insertPosition, prop);

            return prop;
        }
    }
    public static class IExtensionBaseTypeMemberExtensions
    {
        //!+TODO: Handle Dictionary updates
        public static bool Move(this IExtensionBaseTypeMember iebt, ExtensionType extension, ExtensionBaseType ebtTarget, int newListIndex = -1)
        {
            if (extension == null) return false;

            var ebt = (ExtensionBaseType)(extension.ParentNode);  //get the list that comment is attached to          
            if (ebtTarget == null) ebtTarget = ebt;  //attach to the original parent
            bool b = ebt.Extension.Remove(extension);
            if (b) ebtTarget.Extension.Insert(newListIndex, extension);
            var count = ebtTarget.Extension.Count;
            if (newListIndex < 0 || newListIndex > count) newListIndex = count;
            if (ebtTarget.Extension[newListIndex] == extension) return true; //success
            return false;
        }
        public static bool Move(this IExtensionBaseTypeMember iebt, CommentType comment, ExtensionBaseType ebtTarget, int newListIndex)
        {
            if (comment == null) return false;

            var ebt = (ExtensionBaseType)(comment.ParentNode);  //get the list that comment is attached to          
            if (ebtTarget == null) ebtTarget = ebt;  //attach to the original parent
            bool b = ebt.Comment.Remove(comment);
            var count = ebt.Comment.Count;
            if (newListIndex < 0 || newListIndex > count) newListIndex = count;
            if (b) ebtTarget.Comment.Insert(newListIndex, comment);
            if (ebtTarget.Comment[newListIndex] == comment) return true; //success
            return false;
        }
        public static bool Move(this IExtensionBaseTypeMember iebt, PropertyType property, ExtensionBaseType ebtTarget, int newListIndex)
        {
            if (property == null) return false;

            var ebt = (ExtensionBaseType)(property.ParentNode);  //get the list that comment is attached to          
            if (ebtTarget == null) ebtTarget = ebt;  //attach to the original parent
            bool b = ebt.Property.Remove(property);
            var count = ebt.Property.Count;
            if (newListIndex < 0 || newListIndex > count) newListIndex = count;
            if (b) ebtTarget.Property.Insert(newListIndex, property);
            if (ebtTarget.Property[newListIndex] == property) return true; //success
            return false;
        }
    } 
    public static class IdentifiedExtensionTypeExtensions
    {
        public static string GetNewCkey_(this IdentifiedExtensionType i) 
        { throw new NotImplementedException(); }
    }
    public static class IMoveRemoveExtensions
    {
        #region IMoveRemove //not tested
        private static void MoveInDictionaries(this BaseType btSource, BaseType targetParent = null)
        {
            //Remove from ParentNodes and ChildNodes as needed
            UnRegisterParent(btSource as BaseType);

            //Re-register item node under new parent
            RegisterParent(btSource, targetParent);
        }
        public static bool IsParentNodeAllowed(this BaseType btSource, BaseType newParent, out object pObj, int newListIndex = -1)
            => SdcUtil.IsParentNodeAllowed(btSource, newParent, out pObj, newListIndex);
        public static bool IsParentNodeAllowed(this BaseType btSource, BaseType newParentNode)
            => SdcUtil.IsParentNodeAllowed(btSource, newParentNode, out _);
        public static bool Remove(this BaseType btSource, bool cancelIfChildNodes = true)
        {
            var par = btSource.ParentNode;

            //!check recursively for descendants and remove them recursively from all dictionaries
            //!this will remove the entire tree branch, ending with this object
            foreach (var childNode in par.TopNode.ChildNodes[btSource.ObjectGUID])
                childNode.Remove();

            if (par != null)
            {
                //reflect the parent property that represents the "this" object, then set the property to null
                var prop = par.GetType().GetProperties().Where(p => p.GetValue(par) == btSource).FirstOrDefault();
                if (prop != null)
                {
                    var propObj = prop.GetValue(par);
                    if (propObj is IList propIL && propIL[0] != null)
                    {
                            if (cancelIfChildNodes ) return false; //abort if a child node is present
                            (propObj as IList<BaseType>)?.Remove(btSource); 
                    }
                    else prop.SetValue(par, null);

                    UnRegisterThis(btSource);

                    return true;
                }
            }
            return false;
        }
        public static bool Move(this BaseType btSource, BaseType newParent, int newListIndex = -1)
        {
            if (IsParentNodeAllowed(btSource, newParent, out object targetObj, newListIndex))
            {

                if (targetObj is BaseType)
                {
                    MoveInDictionaries(btSource, targetParent: newParent);
                    targetObj = btSource;
                    return true;
                }
                else if (targetObj is IList)
                {
                    //Remove this from current parent object
                    IsParentNodeAllowed(btSource, btSource.ParentNode, out object currentParentObj);
                    if (currentParentObj is BaseType) currentParentObj = null;
                    else if (currentParentObj is IList)
                    {
                        var objList = currentParentObj.As<IList>();
                        if (objList?.IndexOf(btSource) > -1) objList.Remove(btSource);
                        else throw new Exception("Could not find node in parent object list");
                    }
                    else throw new Exception("Could not parent object to remove node");

                    IList propList = (IList)targetObj;
                    MoveInDictionaries(btSource, targetParent: newParent);

                    if (newListIndex < 0 || newListIndex >= propList.Count) propList.Add(btSource);
                    else propList.Insert(newListIndex, btSource);

                    return true;
                }
                throw new Exception("Invalid targetProperty");

            }
            else return false; //invalid Move
        }


        #endregion
        #region Register-UnRegister
        internal static void RegisterParent<T>(this BaseType btSource, T inParentNode) where T : BaseType
        {
            btSource.ParentNode = inParentNode;

            try
            {
                if (inParentNode != null)
                {   //Register parent node
                    btSource.TopNode.ParentNodes.Add(btSource.ObjectGUID, inParentNode);

                    List<BaseType> kids;
                    btSource.TopNode.ChildNodes.TryGetValue(inParentNode.ObjectGUID, out kids);
                    if (kids is null)
                    {
                        kids = new List<BaseType>();
                        btSource.TopNode.ChildNodes.Add(inParentNode.ObjectGUID, kids);
                    }
                    kids.Add(btSource);
                    //inParentNode.IsLeafNode = false; //the parent node has a child node, so it can't be a leaf node
                }
            }
            catch (Exception ex)
            { Debug.WriteLine(ex.Message + "/n  ObjectID:" + btSource.ObjectID.ToString()); }
        }

        internal static void UnRegisterParent(this BaseType btSource)
        {
            var par = btSource.ParentNode;
            try
            {
                bool success = false;
                if (par != null)
                {
                    if (btSource.TopNode.ParentNodes.ContainsKey(btSource.ObjectGUID))
                        success = btSource.TopNode.ParentNodes.Remove(btSource.ObjectGUID);
                    // if (!success) throw new Exception($"Could not remove object from ParentNodes dictionary: name: {this.name ?? "(none)"} , ObjectID: {this.ObjectID}");

                    if (btSource.TopNode.ChildNodes.ContainsKey(par.ObjectGUID))
                        success = btSource.TopNode.ChildNodes[par.ObjectGUID].Remove(btSource); //Returns a List<BaseType> and removes "item" from that list
                                                                                                //if (!success) throw new Exception($"Could not remove object from ChildNodes dictionary: name: {this.name ?? "(none)"}, ObjectID: {this.ObjectID}");

                    //if (TopNode.ChildNodes.ContainsKey(this.ObjectGUID))
                    //    success = TopNode.ChildNodes[this.ObjectGUID].Remove(this);
                    //if (!success) throw new Exception($"Could not remove object from ChildNodes dictionary: name: {this.name ?? "(none)"}, ObjectID: {this.ObjectID}");

                    //if(TopNode.ChildNodes[par.ObjectGUID] is null || par.TopNode.ChildNodes[par.ObjectGUID].Count() == 0)
                    //par.IsLeafNode = true; //the parent node has no child nodes, so it is a leaf node
                }
            }
            catch (Exception ex)
            { Debug.WriteLine(ex.Message + "/n  ObjectID:" + btSource.ObjectID.ToString()); }

            btSource.ParentNode = null;

        } //!not tested
        private static void UnRegisterThis(this BaseType btSource)
        {
            bool success = btSource.TopNode.Nodes.Remove(btSource.ObjectGUID);
            if (!success) throw new Exception($"Could not remove object from Nodes dictionary: name: {btSource.name ?? "(none)"}, ObjectID: {btSource.ObjectID}");
            UnRegisterParent(btSource);
        } //!not tested
        #endregion



    }
    public static class INavigateExtensions
    {
        public static BaseType GetNodeFirstSib(this INavigate n)
        { return SdcUtil.GetFirstSib(n as BaseType); }
        public static BaseType GetNodeLastSib(this INavigate n)
        { return SdcUtil.GetLastSib(n as BaseType); }
        public static BaseType GetNodePreviousSib(this INavigate n)
        { return SdcUtil.GetPrevSib(n as BaseType); }
        public static BaseType GetNodeNextSib(this INavigate n)
        { return SdcUtil.GetNextSib(n as BaseType); }
        public static BaseType GetNodePrevious(this INavigate n)
        { return SdcUtil.GetPrevElement(n as BaseType); }
        public static BaseType GetNodeNext(this INavigate n)
        { return SdcUtil.ReflectNextElement(n as BaseType); }
        public static BaseType GetNodeFirstChild(this INavigate n)
        { return SdcUtil.GetFirstChild(n as BaseType); }
        public static BaseType GetNodeLastChild(this INavigate n)
        { return SdcUtil.GetLastChild(n as BaseType); }
        public static BaseType GetNodeLastDescendant(this INavigate n)
        { return SdcUtil.GetLastDescendantNode(n as BaseType); }
        public static bool HasChildren(this INavigate n)
        { return SdcUtil.HasChild(n as BaseType); }

        public static List<BaseType> GetChildList(this INavigate n)
        { return SdcUtil.GetChildList(n as BaseType); }

        public static List<BaseType> GetSubtreeList(this INavigate n)
        { return SdcUtil.GetSubtreeList(n as BaseType); }
        public static Dictionary<Guid, BaseType> GetSubtreeDictionary(this INavigate n)
        { return SdcUtil.GetSubtreeDictionary(n as BaseType); }
        public static PropertyInfoMetadata GetPropertyInfo(this INavigate n)
        { return SdcUtil.GetPropertyInfo(n as BaseType); }

    }
    public static class IResponseExtensions
    {
        //UnitsType AddUnits(ResponseFieldType rfParent);
        //public static UnitsType AddUnits(this IResponse _, ResponseFieldType rfParent)
        //{
        //    UnitsType u = new UnitsType(rfParent);
        //    rfParent.ResponseUnits = u;
        //    return u;
        //}
        public static RichTextType AddTextAfterResponse_()
        { throw new NotImplementedException(); }
        public static BaseType GetDataTypeObject_()
        { throw new NotImplementedException(); }
    }
    public static class IResponseFieldExtensions
    {
        public static DataTypes_DEType AddDataType(this ResponseFieldType rf,
            ItemChoiceType dataType = ItemChoiceType.@string,
            dtQuantEnum dtQuant = dtQuantEnum.EQ,
            object valDefault = null)
            => IDataHelpers.AddDataTypesDE(rf, dataType, dtQuant, valDefault);

        public static UnitsType AddResponseUnits(this ResponseFieldType rf, string units)
        {
            if (rf != null && units != null)
            {
                var u = new UnitsType(rf);
                rf.ResponseUnits = u;
                u.val = units;
                return u;
            }
            return null;
        }

        public static RichTextType AddTextAfterResponse(this ResponseFieldType rf, string taf)
        {
            if (rf != null && taf != null)
            {
                var rt = new RichTextType(rf);
                rf.TextAfterResponse = rt;
                return rt;
            }
            return null;
        }

        public static CallFuncActionType AddCallSetValue_(this ResponseFieldType rf)
        { throw new NotImplementedException(); }
        public static ScriptCodeAnyType AddSetValue_(this ResponseFieldType rf)
        { throw new NotImplementedException(); }
    }
    public static class IValExtensions
    {//Implemented by data types, which have a strongly-typed val attribute.  Not implemented by anyType, XML, or HTML  
    } //Empty
    public static class IValNumericExtensions
    {//Implemented by numeric data types, which have a strongly-type val attribute.

    } //Empty
    public static class IValDateTimeExtensions
    {//Implemented by DateTime data types, which have a strongly-type val attribute.

    } //Empty
    public static class IValIntegerExtensions
    {//Implemented by Integer data types, which have a strongly-type val attribute.  Includes byte, short, long, positive, no-positive, negative and non-negative types
    } //Empty
    public static class IAddCodingExtensions
    {
        public static CodingType AddCodedValue_(this IAddCoding ac, DisplayedType dt, int insertPosition)
        {
            throw new NotImplementedException();
        }

        public static CodingType AddCodedValue_(this IAddCoding ac, LookupEndPointType lep, int insertPosition)
        {
            throw new NotImplementedException();
        }
        public static UnitsType AddUnits(this IAddCoding ac, CodingType ctParent)
        {
            UnitsType u = new UnitsType(ctParent);
            ctParent.Units = u;
            return u;
        }
    }
    public static class IAddPersonExtensions    
    {
        internal static PersonType AddPerson(this IAddPerson ap, ContactType contactParent)
        {

            var newPerson = new PersonType(contactParent);
            contactParent.Person = newPerson;

            AddPersonItems(ap, newPerson);  //AddFillPersonItems?

            return newPerson;
        }
        internal static PersonType AddPerson(this IAddPerson ap, DisplayedType dtParent, int insertPosition)
        {
            List<ContactType> contactList;
            if (dtParent.Contact == null)
            {
                contactList = new List<ContactType>();
                dtParent.Contact = contactList;
            }
            else
                contactList = dtParent.Contact;
            var newContact = new ContactType(dtParent); //newContact will contain a person child
            var count = contactList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            contactList.Insert(insertPosition, newContact);

            var newPerson = AddPerson(ap, newContact);

            return newPerson;
        }
        internal static PersonType AddContactPerson(this IAddPerson ap, OrganizationType otParent, int insertPosition)
        {
            List<PersonType> contactPersonList;
            if (otParent.ContactPerson == null)
            {
                contactPersonList = new List<PersonType>();
                otParent.ContactPerson = contactPersonList;
            }
            else
                contactPersonList = otParent.ContactPerson;

            var newPerson = new PersonType(otParent);
            AddPersonItems(ap, newPerson);

            var count = contactPersonList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            contactPersonList.Insert(insertPosition, newPerson);

            return newPerson;
        }
        internal static PersonType AddPersonItems(this IAddPerson ap, PersonType pt)  //AddFillPersonItems, make this abstract and move to subclass?
        {
            pt.PersonName = new NameType(pt);//TODO: Need separate method(s) for this
            //pt.Alias = new NameType();
            //pt.PersonName.FirstName.val = (string)drFormDesign["FirstName"];  //TODO: replace with real data
            //pt.PersonName.LastName.val = (string)drFormDesign["LastName"];  //TODO: replace with real data

            pt.Email = new List<EmailType>();//TODO: Need separate method(s) for this
            var email = new EmailType(pt);//TODO: Need separate method(s) for this
            pt.Email.Add(email);

            pt.Phone = new List<PhoneType>();//TODO: Need separate method(s) for this
            pt.Job = new List<JobType>();//TODO: Need separate method(s) for this

            pt.Role = new string_Stype(pt, "Role");

            pt.StreetAddress = new List<AddressType>();//TODO: Need separate method(s) for this
            pt.Identifier = new List<IdentifierType>();

            pt.Usage = new string_Stype(pt, "Usage");

            pt.WebURL = new List<anyURI_Stype>();//TODO: Need separate method(s) for this

            return pt;
        }
    } 
    public static class IAddContactExtensions
    {
        public static ContactType AddContact(this IAddContact ac, FileType ftParent, int insertPosition)
        {
            ContactsType c;
            if (ftParent.Contacts == null)
                c = AddContactsListToFileType(ac, ftParent);
            else
                c = ftParent.Contacts;
            var ct = new ContactType(c);
            var count = c.Contact.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            c.Contact.Insert(insertPosition, ct);
            //TODO: Need to be able to add multiple people/orgs by reading the data source or ORM
            var p = (ac as IAddPerson).AddPerson(ct);
            var org = (ac as IAddOrganization).AddOrganization(ct);

            return ct;
        }
        private static ContactsType AddContactsListToFileType(this IAddContact ac, FileType ftParent)
        {
            if (ftParent.Contacts == null)
                ftParent.Contacts = new ContactsType(ftParent);

            return ftParent.Contacts; //returns a .NET List<ContactType>

        }
    }
    public static class IAddOrganizationExtension
    {
        public static OrganizationType AddOganization_(this IAddOrganization ao)
        { throw new NotImplementedException(); }

        public static  OrganizationType AddOrganization(this IAddOrganization ao, ContactType contactParent)
        {
            var ot = new OrganizationType(contactParent);
            contactParent.Organization = ot;

            return ot;
        }
        public static OrganizationType AddOrganization(this IAddOrganization ao, JobType jobParent)
        {
            var ot = new OrganizationType(jobParent);
            jobParent.Organization = ot;

            return ot;
        }
        public static OrganizationType AddOrganizationItems_(this IAddOrganization ao, OrganizationType ot)
        { throw new NotImplementedException(); }
    }
    public static class IEventExtension //Used for events (PredActionType)
    {
        public static PredEvalAttribValuesType AddAttributeVal(this IEvent ae)
        {
            var pgt = (PredActionType)ae;
            var av = new PredEvalAttribValuesType(pgt);
            pgt.Items.Add(av);
            return av;
        }
        public static ScriptBoolFuncActionType AddScriptBoolFunc_(this IEvent ae)
        { throw new NotImplementedException(); }
        public static CallFuncBoolActionType AddCallBoolFunction_(this IEvent ae)
        { throw new NotImplementedException(); }
        public static MultiSelectionsActionType AddMultiSelections_(this IEvent ae)
        { throw new NotImplementedException(); }
        public static SelectionSetsActionType AddSelectionSets_(this IEvent ae)
        { throw new NotImplementedException(); }
        public static PredSelectionTestType AddSelectionTest_(this IEvent ae)
        { throw new NotImplementedException(); }
        //PredAlternativesType AddItemAlternatives();
        public static RuleSelectMatchingListItemsType SelectMatchingListItems_(this IEvent ae)
        { throw new NotImplementedException(); }
        public static PredGuardType AddGroup_(this IEvent ae)
        { throw new NotImplementedException(); }
    }
    public static class IPredGuardExtensions //used by Guards on ListItem, Button, e.g., SelectIf, DeselectIf
    {
        public static PredEvalAttribValuesType AddAttributeVal(this IPredGuard ipg)
        {            
            var pgt = (PredGuardType)ipg;
            var av = new PredEvalAttribValuesType(pgt);
            pgt.Items.Add(av);
            return av;
        }
        public static ScriptBoolFuncActionType AddScriptBoolFunc_(this IPredGuard ipg)
        { throw new NotImplementedException(); }
        public static CallFuncBoolActionType AddCallBoolFunction_(this IPredGuard ipg) 
        { throw new NotImplementedException(); }
        public static MultiSelectionsActionType AddMultiSelections_(this IPredGuard ipg) 
        { throw new NotImplementedException(); }
        public static PredSelectionTestType AddSelectionTest_(this IPredGuard ipg) 
        { throw new NotImplementedException(); }
        public static PredGuardTypeSelectionSets AddSelectionSets_(this IPredGuard ipg) 
        { throw new NotImplementedException(); }
        public static PredAlternativesType AddItemAlternatives_(this IPredGuard ipg) 
        { throw new NotImplementedException(); }
        public static PredGuardType AddGroup_(this IPredGuard ipg) 
        { throw new NotImplementedException(); }


    }
    public static class IRuleExtensions
    {
        public static RuleAutoActivateType AddAutoActivation_(this IRule r)
        { throw new NotImplementedException(); }
        public static RuleAutoSelectType AddAutoSelection_(this IRule r)
        { throw new NotImplementedException(); }
        public static PredActionType AddConditionalActions_(this IRule r)
        { throw new NotImplementedException(); }
        public static CallFuncActionType AddExternalRule_(this IRule r)
        { throw new NotImplementedException(); }
        public static ScriptCodeAnyType AddScriptedRule_(this IRule r)
        { throw new NotImplementedException(); }
        public static RuleSelectMatchingListItemsType AddSelectMatchingListItems_(this IRule r)
        { throw new NotImplementedException(); }
        public static ValidationType AddValidation_(this IRule r)
        { throw new NotImplementedException(); }
    } 
    public static class IHasConditionalActionsNodeExtensions
    {
        public static PredActionType AddConditionalActionsNode_(this IHasConditionalActionsNode hcan)
        { throw new NotImplementedException();   }
    }
    public static class IHasParameterGroupExtensions
    {
        public static ParameterItemType AddParameterRefNode_(this IHasParameterGroup hpg)
        {throw new NotImplementedException();}
        public static ListItemParameterType AddListItemParameterRefNode_(this IHasParameterGroup hpg)
        { throw new NotImplementedException(); }
        public static ParameterValueType AddParameterValueNode_(IHasParameterGroup hpg)
        { throw new NotImplementedException(); }
    } 
    public static class IHasDataType_STypeExtensions
    {
        public static DataTypes_SType AddDataTypes_SType_(this DataTypes_SType S)
        { throw new NotImplementedException(); }
    } 
    public static class IHasDataType_DETypeExtensions
    {
        public static DataTypes_DEType AddDataTypes_DEType_(this DataTypes_DEType DE)
        { throw new NotImplementedException(); }
    }
    public static class IHasActionsNodeExtensions
    {
        public static ActionsType AddActionsNode(this IHasActionsNode han)
        {
            var actions = new ActionsType((ExtensionBaseType)han);
            var p = han as PredActionType;
            if (p != null)
            {
                p.Actions = actions;
                return p.Actions;
            }
            else
            {
                var pe = han as EventType;
                if (pe != null)
                {
                    pe.Actions = actions;
                    return pe.Actions;
                }
            }
            throw new InvalidCastException("The parent node must be of type EventType or PredActionType");
        }
    } 
    public static class IHasActionElseGroupExtensions { } //Empty
    public static class IHasElseNodeExtensions
    {
        public static PredActionType AddElseNode(this IHasElseNode hen)
        {
            if (hen is null) return null;
            var elseNode = new PredActionType((BaseType)hen);

            switch (hen)
            {
                case PredActionType pe:
                    pe.Else.Add(elseNode); return elseNode;
                case CallFuncBoolActionType cfb:
                    return (PredActionType)SdcUtil.ArrayAddReturnItem(cfb.Items1, elseNode);
                case ScriptBoolFuncActionType sb:
                    return (PredActionType)SdcUtil.ArrayAddReturnItem(sb.Items, elseNode);
                case AttributeEvalActionType ae:
                    ae.Else.Add(elseNode); return elseNode;
                case MultiSelectionsActionType ms:
                    ms.Else.Add(elseNode); return elseNode;
                case SelectionSetsActionType ss:
                    ss.Else.Add(elseNode); return elseNode;
                case SelectionTestActionType st:
                    st.Else.Add(elseNode); return elseNode;
                default:
                    break;
            }
            throw new InvalidCastException();
            //return new Els
        }
    }
    public static class IActionsMemberExtensions { } //Empty
    public static class ActSendMessageTypeExtensions
    {
        //List<ExtensionBaseType> Items
        //Supports ActSendMessageType and ActSendReportType
        public static EmailAddressType AddEmail_(this ActSendMessageType smr)
        { throw new NotImplementedException(); }
        public static PhoneNumberType AddFax_(this ActSendMessageType smr)
        { throw new NotImplementedException(); }
        public static CallFuncActionType AddWebService_(this ActSendMessageType smr)
        { throw new NotImplementedException(); }
    }
    public static class CallFuncBaseTypeExtensions
    {
        //anyURI_Stype Item (choice)
        public static anyURI_Stype AddFunctionURI_(this CallFuncBaseType cfb)
        { throw new NotImplementedException(); }
        public static anyURI_Stype AddLocalFunctionName_(this CallFuncBaseType cfb)
        { throw new NotImplementedException(); }

        //List<ExtensionBaseType> Items
        public static ListItemParameterType AddListItemParameterRef_(this CallFuncBaseType cfb)
        { throw new NotImplementedException(); }
        public static ParameterItemType AddParameterRef_(this CallFuncBaseType cfb)
        { throw new NotImplementedException(); }
        public static ParameterValueType AddParameterValue_(this CallFuncBaseType cfb)
        { throw new NotImplementedException(); }
    }


    public static class CallFuncActionTypeExtensions
    {
        public static anyURI_Stype AddConnditionalActions_(this CallFuncActionType cfat)
        { throw new NotImplementedException(); }
    }
    public static class ScriptBoolFuncActionTypeExtensions
    {
        //ExtensionBaseType[] Items 
        public static ActionsType AddActions_(this ScriptBoolFuncActionType sbfa)
        { throw new NotImplementedException(); }
        public static PredActionType AddConditionalActions_(this ScriptBoolFuncActionType sbfa)
        { throw new NotImplementedException(); }
        public static PredActionType AddElse_(this ScriptBoolFuncActionType sbfa)
        { throw new NotImplementedException(); }
    } 
    public static class CallFuncBoolActionTypeExtensions
    {
        //ExtensionBaseType[] Items1
        //see IScriptBoolFuncAction, which is identical except that this interface implementation must use "Item1", not "Item"
        //Implementations using Item1:
        public static ActionsType AddActions_(this CallFuncBoolActionType cfba)
        { throw new NotImplementedException(); }
        public static PredActionType AddConditionalActions_(this CallFuncBoolActionType cfba)
        { throw new NotImplementedException(); }
        public static PredActionType AddElse_(this CallFuncBoolActionType cfba)
        { throw new NotImplementedException(); }
    }
    public static class IValidationTestsExtensions
    {
        public static PredAlternativesType AddItemAlternatives_(this IValidationTests vt)
        { throw new NotImplementedException(); }
        public static ValidationTypeSelectionSets AddSelectionSets_(this IValidationTests vt)
        { throw new NotImplementedException(); }
        public static ValidationTypeSelectionTest AddSelectionTest_(this IValidationTests vt)
        { throw new NotImplementedException(); }
    }
    public static class ICloneExtensions// Probably belongs on IBaseType 
    {
        public static BaseType CloneSubtree_(this IClone c, BaseType top)
        { throw new NotImplementedException(); }
    }
    public static class IHtmlPackageExtensions
    {
        public static base64Binary_Stype AddHTMLbase64_(this IHtmlPackage hp)
        { throw new NotImplementedException(); }
    }
    public static class RegistrySummaryTypeExtensions
    {
        //BaseType[] Items
        //Attach to Admin.RegistryData as OriginalRegistry and/or CurrentRegistry

        public static ContactType AddContact_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
        public static FileType AddManual_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
        public static string_Stype AddReferenceStandardIdentifier_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
        public static InterfaceType AddRegistryInterfaceType_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
        public static string_Stype AddRegistryName_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
        public static FileType AddRegistryPurpose_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
        public static FileType AddServiceLevelAgreement_(this RegistrySummaryType rs)
        { throw new NotImplementedException(); }
    } 

}


public static class StringExtensions
{
    public static int WordCount(this String str)
    {
        return str.Split(new char[] { ' ', '.', '?' },
                         StringSplitOptions.RemoveEmptyEntries).Length;
    }
    public static bool IsNullOrEmpty(this String str)
    {
        return string.IsNullOrEmpty(str);
    }
    public static bool IsNullOrWhitespace(this String str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
    public static bool IsEmpty(this String str)
    {
        return str.IsNullOrEmpty() || str.IsNullOrWhitespace();
    }
    public static string TrimAndReduce(this string str)
    {
        return ReduceWhitespace(str).Trim();
    }

    public static string ReduceWhitespace(this string str)
    {
        return Regex.Replace(str, @"\s+", " ");
    }
    public static string ReduceWhitespaceRegex(this string str)
    {
        return Regex.Replace(str, "[\n\r\t]", " ");
    }
    public static XmlElement ToXmlElement(this string rawXML)
    {
        var xe = XElement.Parse(rawXML, LoadOptions.PreserveWhitespace);
        var doc = new XmlDocument();

        var xmlReader = xe.CreateReader();
        doc.Load(xmlReader);
        xmlReader.Dispose();

        return doc.DocumentElement;
    }

    /// <summary>
    /// Converts the string expression of an enum value to the desired type. Example: var qType= reeBuilder.ConvertStringToEnum&lt;ItemType&gt;("answer");
    /// </summary>
    /// <typeparam name="Tenum">The enum type that the inputString will be converted into.</typeparam>
    /// <param name="inputString">The string that must represent one of the Tenum enumerated values; not case sensitive</param>
    /// <returns></returns>
    public static Tenum ToEnum<Tenum>(this string inputString) where Tenum : struct
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

}

public static class ObjectExtensions
{
    public static bool IsGenericList(this object o)
    {
        try
        {
            return SDC.Schema.SdcUtil.IsGenericList(o);
        }
        catch {return false; }

    }
    /// <summary>
    /// Direct Cast as T, if possible
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static T As<T>(this object o) where T:class 
    {
        try
        { return (T)o; }
        catch
        { return null; }
    }
   /// <summary>
   /// Try to Cast as T
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="oIn"></param>
   /// <param name="oOut"></param>
   /// <returns></returns>
    public static bool TryAs<T>(this object oIn, out T oOut ) where T : class 
    {
        try
        {
            oOut = (T)oIn;
            return true;
        }
        catch
        {
            oOut = null;
            return false;
        }
    }


}