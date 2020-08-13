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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.CodeDom;


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
                throw new Exception("the compared nodes cannot be  compared because they do not have a common ancester node");

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
                //Debug.Print($" {i}:ord:{ord},   A:{nodeA.ObjectID},   B:{nodeB.ObjectID}");
                if (i != ord) Debugger.Break();
            }


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
        public static BaseType PrevElement(BaseType item)
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

        public static BaseType NextElement2(BaseType item)
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

        public static BaseType NextElement(BaseType item)
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
                { //the following flag improves effciency by delaying objct tree assessment until startAfterNode has been passed
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
                    piIE = s.Pop().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    piIE = piIE.Where(p =>
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
                    });

                    var piIeOrdered = piIE?.OrderBy(p => p.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault()?.Order); //sort pi list by XmlElementAttribute.Order
                    PropertyInfo piFirst = GetObjectFromIEnumerableIndex(piIeOrdered, 0) as PropertyInfo; //Get the property whose XmlElementAttribute has the smallest order

                    if (nextNode != null) return nextNode;
                }
                return null;
            }
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
            if (myIndex < 0 || myIndex == lst?.Count()-1) return null;
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
            BaseType lastKid =  null;

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
        /// The returned PropertyInfo object may refer to a BaseType or the IEnumerables List<BaseType> and Array<BaseType> 
        /// If a wrapper property was created in an SDC parrtial class, only the inner property (i.e., the one with XML attributes) is returned
        /// </summarout i
        /// <param name="item"></param>
        /// <param name="getNames">if true, element names will be determined</param>
        /// <returns>
        /// propName: name of the property is returned as an out parameter
        /// ieItems: if the property is IEnumerable<BaseType>, the IEnumerable property object is returned as an out parameter, otherwise it is null
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
                var pi = t.GetProperties();
                xmlElementName = t.GetCustomAttribute<XmlRootAttribute>()?.ElementName;
                maxXmlOrder = GetMaxOrderFromXmlElementAttributes(item);
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
                        xmlElementName = GetElementName(propInfo, out xmlOrder);
                        return propInfo;
                    }
                }
            }

            //Look in generic arrays and Lists of the par properties, and match to item reference

            PropertyInfo tempPI = null;
            //Find all IEnumerable PropertyInfo objects in par
            var ieProps = par.GetType().GetProperties().Where(n => n.GetValue(par) is IEnumerable<BaseType>);

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
                        xmlElementName = GetElementName(propInfo, out xmlOrder, itemIndex);
                        return propInfo;
                    }
                    else tempPI = propInfo;  //save , just in case we can't find a better match in ieItems
                }
            }
            if (tempPI != null) //even though tempPI has no XmlElementAttribute, it still contains our item
                return tempPI;  //we already extracted (out) xmlElementName, (out) xmlOrder and (out) itemIndex, so we can just return now.

            return null;

            //!+---------------------Local Methods------------------------------

            string GetElementName(PropertyInfo pi, out int xmlOrder, int itemIndex = -1)
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

                if (dtAtts != null && dtAtts.Count() > 0)
                    xmlOrder = dtAtts.ToArray()[0].Order;

                if (getNames)
                {
                    string elementName = ElementNameFromEnum(item, ItemChoiceEnum(pi), itemIndex);
                    if (elementName != null)
                        return elementName;


                    //There was no ElementName to extract from an ItemChoiceEnum, so we get it directly from the attribute.
                    if (dtAtts.Count() == 1)
                    {
                        return dtAtts.ToArray()[0].ElementName;
                    }

                    dtAtts = dtAtts.Where(a => a.Type == item.GetType()).ToArray();
                    if (dtAtts.Count() == 1)
                    {
                        //return ElementName based on data type match in the XMLAttribute
                        return dtAtts.ToArray()[0].ElementName;
                    }
                    //There was no ElementName to extract from an XmlElementAttribute, so we get it directly from the propName.
                    return pi.Name;
                }
                return "";
            }

            string ItemChoiceEnumName(PropertyInfo pi)
            {
                XmlChoiceIdentifierAttribute xci = (XmlChoiceIdentifierAttribute)pi.GetCustomAttribute(typeof(XmlChoiceIdentifierAttribute));
                if (xci is null) return null;
                return xci.MemberName;
            }

            object ItemChoiceEnum(PropertyInfo pi)
            {
                string enumName = ItemChoiceEnumName(pi);
                if (enumName == null) return null;

                var enumObj = par.GetType().GetProperty(enumName).GetValue(par);
                if (enumObj is Enum) return (Enum)enumObj;
                if (enumObj is IEnumerable[]) return (IEnumerable<Enum>[])enumObj;
                return null;
            }

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

        }

        public static List<PropertyInfoOrdered> ReflectPropertyInfoList( BaseType bt)
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
                if (att !=null)

                    props.Add(new PropertyInfoOrdered(p, att.Order));
            }

            props.Sort(new PropertyInfoOrderedComparer());
            return props;

        }
        /// <summary>
        /// Uses reflection to determine XML attributes that are eligible to be serialized.
        /// </summary>
        /// <param name="ti"></param>
        /// <returns>List<PropertyInfo></returns>
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
        /// <returns>List<PropertyInfo></returns>
        public static List<PropertyInfo> ReflectAttributesFilled(BaseType bt)
        {
            TypeInfo ti = bt.GetType().GetTypeInfo();
            if (ti is null) return null;
            var attProps = new List<PropertyInfo>();
            foreach (var p in ReflectAttributesAll(bt))
            {
                    if ((bool)ti.GetMethod("ShouldSerialize" + p.Name).Invoke(bt, null))
                        attProps.Add(p);
            }
            return attProps;
        }
        public static List<PropertyInfo> ReflectAttributesAll(BaseType bt)
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

        static bool IsTargetForItemName(PropertyInfo targetProp, BaseType item, BaseType target)
        {
            //We want to know if the supplied itemName is allowed to be atttached at a hypothetical property defined by piTarget
            //piTarget can be an array or List of Type BaseType, or may be a subclass of BaseType
            var par = item.ParentNode;
            string itemName = GetPropertyInfo(item).XmlElementName;
            if (targetProp is null) return false;

            var dtAtts = (XmlElementAttribute[])Attribute.GetCustomAttributes(targetProp, typeof(XmlElementAttribute));


            bool success = ElementNameFromEnum(ItemChoiceEnum(targetProp));
            if (success) return true;


            //There was no ElementName to extract from an ItemChoiceEnum, so we get it directly from the attribute.
            if (dtAtts.Count() == 1)
            {
                return dtAtts.ToArray()[0].ElementName == itemName;
            }

            dtAtts = dtAtts.Where(a => a.Type == item.GetType()).ToArray();
            if (dtAtts.Count() == 1)
            {
                //return ElementName based on data type match in the XMLAttribute
                return dtAtts.ToArray()[0].ElementName == itemName;
            }
            //There was no ElementName to extract from an XmlElementAttribute
            return false;

            string ItemChoiceEnumName(PropertyInfo piTarget)
            {
                XmlChoiceIdentifierAttribute xci = (XmlChoiceIdentifierAttribute)piTarget.GetCustomAttribute(typeof(XmlChoiceIdentifierAttribute));
                if (xci is null) return null;
                return xci.MemberName;
            }

            object ItemChoiceEnum(PropertyInfo piTarget)
            {
                string enumName = ItemChoiceEnumName(piTarget);
                if (enumName == null) return null;

                var enumObj = target.GetType().GetProperty(enumName).GetValue(target);
                if (enumObj is Enum) return (Enum)enumObj;
                if (enumObj is IList[]) return (IList<Enum>[])enumObj;
                return null;
            }

            bool ElementNameFromEnum(object choiceIdentifierObject)
            {
                if (choiceIdentifierObject is Enum)
                {
                    string enumVal;
                    try //try to find out element name in the target enum
                    {
                        var et = choiceIdentifierObject.GetType();
                        enumVal = Enum.Parse(et, itemName).ToString();
                    }
                    catch { return false; }
                    return !string.IsNullOrEmpty(enumVal);
                }

                if (choiceIdentifierObject is IList)
                {
                    var lst = ((IList<string>)choiceIdentifierObject);
                    return lst.Contains(itemName);
                }
                return false;
            }


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
        public static bool IsItemChangeAllowed<S, T>(S source, T target)
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
        #endregion



        #region Retired

        #endregion

        #region Helpers
        internal static string CreateName(BaseType bt)
        {
            throw new NotImplementedException();
        }
        //internal static XmlElement StringToXMLElement(string rawXML)
        //{
        //    var xe = XElement.Parse(rawXML, LoadOptions.PreserveWhitespace);
        //    var doc = new XmlDocument();

        //    var xmlReader = xe.CreateReader();
        //    doc.Load(xmlReader);
        //    xmlReader.Dispose();

        //    return doc.DocumentElement;
        //}


        /// <summary>
        /// Converts the string expression of an enum value to the desired type. Example: var qType= reeBuilder.ConvertStringToEnum&lt;ItemType&gt;("answer");
        /// </summary>
        /// <typeparam name="Tenum">The enum type that the inputString will be converted into.</typeparam>
        /// <param name="inputString">The string that must represent one of the Tenum enumerated values; not case sensitive</param>
        /// <returns></returns>
        //public static Tenum ConvertStringToEnum<Tenum>(string inputString) where Tenum : struct
        //{
        //    //T newEnum = (T)Enum.Parse(typeof(T), inputString, true);

        //    Tenum newEnum;
        //    if (Enum.TryParse<Tenum>(inputString, true, out newEnum))
        //    {
        //        return newEnum;
        //    }
        //    else
        //    { //throw new Exception("Failure to create enum");

        //    }
        //    return newEnum;
        //}

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

        public static List<BaseType> GetChildren(this BaseType bt)
        {
            return SdcUtil.ReflectChildList(bt);
        }
        /// <summary>
        /// Provides PropertyInfo (PI) definitions for all bt attributes that will be serialized to XML
        /// Each PI can be used t  obtain the type, name and other features of each attribute
        /// Also, each PI can be used to create an instance of the object by calling PI.GetValue(parentObject)
        /// </summary>
        /// <param name="bt"></param>
        /// <returns>List<PropertyInfo></returns>
        public static List<PropertyInfo> GetAttributesFilled(this BaseType bt)
        {
            return SdcUtil.ReflectAttributesFilled (bt);
        }
        /// <summary>
        /// Provides PropertyInfo (PI) definitions for all attributes of bt
        /// </summary>
        /// <param name="bt"></param>
        /// <returns>List<PropertyInfo> </returns>
        public static List<PropertyInfo> GetAttributeAll(this BaseType bt)
        {
            return SdcUtil.ReflectAttributesAll(bt);             
        }
        public static List<PropertyInfoOrdered> GetPropertyInfoList(this BaseType bt)
        {
            return SdcUtil.ReflectPropertyInfoList(bt);
        }

        public static PropertyInfoMetadata GetPropertyInfoMetaData(this BaseType bt)
        {
            return SdcUtil.GetPropertyInfo(bt);
        }
        public static List<BaseType> GetDescendants(this BaseType bt)
        {
            return SdcUtil.ReflectSubtree(bt);
        }
        public static List<BaseType> GetSibs(this BaseType bt)
        {
            return bt.GetPropertyInfoMetaData().IeItems.ToList();
        }
        public static bool IsItemChangeAllowed(this IdentifiedExtensionType iet, IdentifiedExtensionType targetType)
        {
            return SdcUtil.IsItemChangeAllowed(iet, targetType);

        }

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
        {
            return (T)o;
        }
        catch (Exception ex)
        {
            return null;
        }
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