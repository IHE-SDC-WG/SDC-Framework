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
using System.Xml.Serialization;


//using SDC;
namespace SDC.Schema
{
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
            this.propName = propName;
            this.itemIndex = itemIndex;
            this.ieItems = ieItems;
            this.xmlOrder = xmlOrder;
            this.maxXmlOrder = maxXmlOrder;
            this.xmlElementName = xmlElementName;


        }
        public PropertyInfo PropertyInfo { get; }
        public string propName { get; }
        public int itemIndex { get; }
        public IEnumerable<BaseType> ieItems { get; }
        public int xmlOrder { get; }
        public int maxXmlOrder { get; }
        public string xmlElementName { get; }

        public override string  ToString()
        {
            return @$"PropertyInfoMetadata:
---------------------------------------
ieItems.Count   {ieItems?.Count() ?? 0}
itemIndex:      {itemIndex}
propName:       {propName}
xmlOrder:       {xmlOrder}
maxXmlOrder:    {maxXmlOrder}
xmlElementName: {xmlElementName}
---------------------------------------";
        }

    }
    public class TreeComparer:Comparer<BaseType>
    {
        public override int Compare(BaseType nodeA, BaseType nodeB)
        {
            //if nodeA is higher in the tree, return -1;
            //if nodeB is higher in the tree, return 1;

            if (nodeA == nodeB) return 0;
            if (nodeA.TopNode != nodeB.TopNode) throw new Exception("nodeA and nodeB are derived from different SDC templates");
            if (nodeA == nodeA.TopNode) return -1;
            if (nodeB == nodeB.TopNode) return 1;
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
                ancSetA[indexA+1]= prevPar;
                indexA++;
                prevPar = ancSetA[indexA]?.ParentNode ?? null;
            }

            //Find the first intersection of the 2 arrays (lowest common ancestor) - the common node furthest from the tree's top node
            //If they share an ancestor, get the common ancestor object, to find which ancester is first.
            //If they have different ancesters, move down one node in both ancSetA and ancSetB.
            //The set with the highest level (first) parent indicates that the that all nodes in that set come before all nodes in the other set.

            int indexB = 0;
            indexA = -1; //we reuse indexA to refer to the common ancester in AncSetA
            ancSetB[indexB] = nodeB;
            //failed indicates that the nodeA and nodeB trees branches never converge on a common ancestor.  
            //This will generate an exception unless it is set to false below.
            bool failed = true; 
            //!Look for nodeB ancestors in ancSetA.  Loop through nodeB ancesters (we build ancSetB as we loop here) until we find a common ancester in nodeA's ancesters (already assembled in ancSetB)
            prevPar = ancSetB[indexB]?.ParentNode ?? null;
            while (prevPar != null)
            {

                ancSetB[indexB + 1] = prevPar;  //note that we create the ancSetB only as needed.  No need to walk all the way up to the root node if we don't need to.  Thus it's slightly more efficient to place the deeper-on-tree node in nodeB.
                indexA = ISdcUtil.IndexOf(ancSetA, prevPar); //Find the lowest common parent node; later we'll see we can determine which parent node comes first in the tree
                indexB++;
                if (indexA > -1)
                {//we found the lowest common parent node
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
            BaseType ancNodeA = ancSetA[indexA - 1]; //first child of common ancester node on nodeA branch; this is still an ancester of NodeA, or NodeA itself
            BaseType ancNodeB = ancSetB[indexB - 1]; //first child of common ancester node on nodeB branch; this is still an ancester of NodeB, or NodeB itself

            //Retrieve customized Property Metadata for the class properties that hold our nodes.
            var piAncNodeA = ISdcUtil.GetPropertyInfo(ancNodeA, false);
            var piAncNodeB = ISdcUtil.GetPropertyInfo(ancNodeB, false);

            //Let's see if both items come from the same IEnumerable (ieItems) in ANC, and then see which one has the lower itemIndex
            if (piAncNodeA.ieItems != null && piAncNodeB.ieItems!=null &&
                piAncNodeA.ieItems == piAncNodeB.ieItems &&
                piAncNodeA.itemIndex > -1 && piAncNodeB.itemIndex > -1)
            {
                if (piAncNodeA.itemIndex == piAncNodeB.itemIndex) 
                    throw new Exception("Unknown error - the compared nodes share a common ParentNode and appear to be identical");
                if (piAncNodeA.itemIndex < piAncNodeB.itemIndex) return -1;
                if (piAncNodeB.itemIndex < piAncNodeA.itemIndex) return 1;
            }

            //In XML Schemas, it appears that base class (Schema base type) xml elements always come before subclass elements, regardless of the XmlElementAttribute Order value.
            if (piAncNodeA.PropertyInfo.DeclaringType.IsSubclassOf(piAncNodeB.PropertyInfo.DeclaringType)) return 1; //base class xml orders come before subclasses; ancNodeA is the base type here
            if (piAncNodeB.PropertyInfo.DeclaringType.IsSubclassOf(piAncNodeA.PropertyInfo.DeclaringType)) return -1;  //base class xml orders come before subclasses; ancNodeB is the base type here

            //Determine the comparison based on the xmlOrder in the XmlElementAttributes
            if (piAncNodeA.xmlOrder < piAncNodeB.xmlOrder) return -1;
            if (piAncNodeB.xmlOrder < piAncNodeA.xmlOrder) return 1;
            throw new Exception("the compare nodes algorithm could not determine the node order");
        }      
    }

    public interface ISdcUtil
    {
        #region ArrayHelpers
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
            if (emptyTarget.Count() > 0) throw new Exception(String.Format("","emptyTarget was not empty" ));
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

            var prevSib = ISdcUtil.GetPrevSib(item);
            if (prevSib != null && prevSib != item)
            {
                var lastDesc1 = GetLastDescendant(prevSib);
                if (lastDesc1 != null && lastDesc1 != item) return lastDesc1;
            }

            var lastDesc = GetLastDescendant(par, item);// move up on node and drill down to bottom of tree until we hit item, then back to prevNode
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
            if(par is null) par = item; //We have the top node here

            while (par != null)
            {
                nextNode = null;
                xmlOrder = -1;

                //Does item have any children of its own?  If yes, find the first non-null property in the XML element order
                if (doDescendants) if (GetNextNode(item) != null) 
                        return nextNode;                    

                //No child items contained the next node, so let's look at other properties inside the parent object
                var piMeta = GetPropertyInfo(item, true);
                xmlOrder = piMeta.xmlOrder;

                //Is next item is contained inside the same IEnumerable parent?
                if (piMeta.itemIndex > -1 && piMeta.itemIndex < piMeta.ieItems.Count() - 1)
                    return ObjectAtIndex(piMeta.ieItems, piMeta.itemIndex + 1) as BaseType;

                //Is next item part of a parent property that follows our item?
                if (GetNextNode(par, item) != null) 
                    return nextNode;

                //We did not found a next item, so let's move up one parent level in this while loop and check the properties under the parent.
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
        public static BaseType GetFirstSib(BaseType item)
        {
            var par = item.ParentNode;
            if (par is null) return null;
            item.TopNode.ChildNodes.TryGetValue(par.ObjectGUID, out List<BaseType> sibs);
            sibs?.Sort(new TreeComparer());
            return sibs?[0];
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

        public static BaseType GetLastChild(BaseType item)
        {
            item.TopNode.ChildNodes.TryGetValue(item.ObjectGUID, out List<BaseType> kids);
            kids?.Sort(new TreeComparer());
            return kids?[kids.Count() - 1];
        }
        public static BaseType GetFirstChild(BaseType item)
        {
            item.TopNode.ChildNodes.TryGetValue(item.ObjectGUID, out List<BaseType> kids);
            kids?.Sort(new TreeComparer());
            return kids?[0];
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

                n = kids[kids.Count()-1];
                if (n != null) last = n;
            }
            return last;
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

            IEnumerable<PropertyInfo> piSet = par.GetType().GetProperties()?
                .Where(pi => ReferenceEquals(pi.GetValue(par), item));

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

            object ItemChoiceEnum (PropertyInfo pi)
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

                if (choiceIdentifierObject is IEnumerable && arrayIndex >-1)
                {
                    var ie = ((IEnumerable)choiceIdentifierObject);
                    return ((Enum)GetObjectFromIEnumerableIndex(ie, arrayIndex)).ToString();
                }
                return null;
            }

        }
        internal static bool IsGenericList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                o.GetType().IsGenericType &&
                o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        internal static List<T> GetStatedListParent<T>(T item)
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
                    return GetStatedEventParent(ev).Cast<T>().ToList();
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
        private static List<BaseType> GetStatedListParent(BaseType item, string elementName)
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
        private static List<T> GetStatedEventParent<T>(T item)
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
        public static bool IsItemChangeAllowedI<S, T>(S source, T target)
            where S : notnull, IdentifiedExtensionType
            where T : notnull, IdentifiedExtensionType
        {
            ChildItemsType ci;
            switch (source)
            {
                case SectionItemType _:
                case QuestionItemType _:
                case ListItemType _:
                    ci = (source as ChildItemsType);
                    switch (target)
                    {
                        case SectionItemType _:
                        case QuestionItemType _:
                        case ListItemType _:
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
        internal static XmlElement StringToXMLElement(string rawXML)
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

        #endregion



    }

}
