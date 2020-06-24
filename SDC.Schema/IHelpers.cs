using MsgPack.Serialization.CollectionSerializers;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
            if (nodeA.TopNode != nodeB.TopNode) throw new Exception("nodeA and nodeB are derived frm different SDC templates");
            if (nodeA == nodeA.TopNode) return -1;
            if (nodeB == nodeB.TopNode) return 1;
            //if (nodeA.ParentNode is null && nodeB.ParentNode != null) return -1; //nodeA is the top node
            //if (nodeB.ParentNode is null && nodeA.ParentNode != null) return 1;  //nodeB is the top node

            //create ascending ancestor set for nodeA branch, with nodeA as the first element in the ancester set, i.e., ancSetA[0] == nodeA
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
                indexA = IHelpers.IndexOf(ancSetA, prevPar); //Find the lowest common parent node; later we'll see we can determine which parent node comes first in the tree
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
            var piAncNodeA = IHelpers.GetPropertyInfo(ancNodeA, false);
            var piAncNodeB = IHelpers.GetPropertyInfo(ancNodeB, false);

            //Let's see if both items come from the same IEnumerable (ieItems) in ANC, and then see which one has the lower itemIndex
            if (piAncNodeA.ieItems != null && piAncNodeB.ieItems!=null &&
                piAncNodeA.ieItems == piAncNodeA.ieItems &&
                piAncNodeA.itemIndex > -1 && piAncNodeB.itemIndex > -1)
            {
                if (piAncNodeA.itemIndex == piAncNodeB.itemIndex) 
                    throw new Exception("Unknown error - the compared nodes share a common ParentNode and appear to be identical");
                if (piAncNodeA.itemIndex < piAncNodeB.itemIndex) return -1;
                if (piAncNodeB.itemIndex < piAncNodeA.itemIndex) return 1;
            }

            //Let's see if we can read the xmlOrder from the XmlElementAttributes on the two PropertyInfo objects
            //If both xmlOrder results are the same, then one of the nodes (ancNodeA or ancNodeB) is probably a subclass of the other (the base class). 
            //In XML Schemas, it appears that base class (Schema base type) xml elements always come before subclass elements.
            if (piAncNodeB.xmlOrder == piAncNodeA.xmlOrder)
            {
                if (ancNodeB.GetType().IsSubclassOf(ancNodeA.GetType())) return -1; //base class xml orders come before subclasses; ancNodeA is the base type here
                if (ancNodeA.GetType().IsSubclassOf(ancNodeB.GetType())) return 1;  //base class xml orders come before subclasses; ancNodeB is the base type here
                throw new Exception("Unknown error - the compared nodes share a common ParentNode and the same xmlOrder");
            }

            //Determine the comparison based on the xmlOrder in the XmlElementAttributes
            if (piAncNodeA.xmlOrder < piAncNodeB.xmlOrder) return -1;
            if (piAncNodeB.xmlOrder < piAncNodeA.xmlOrder) return 1;
            throw new Exception("the compare nodes algorithm could not determine the node order");
        }      
    }

    public interface IHelpers
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
        public static BaseType PreviousItem(BaseType item)
        {
            int xmlOrder = -1;
            if (item is null) return null;
            BaseType par = item.ParentNode;
            BaseType prevNode = null;
            bool doDescendants = true;
            if (par is null) par = item; //We have the top node here

            while (true)
            {
                prevNode = null;
                xmlOrder = -1;

                //!Does item have any children of its own?  If yes, find the first non-null property in the XML element order
                if (doDescendants) if (PrevNode(item) != null) return prevNode;

                //!No child items contained the next node, so let's look at other properties inside the parent object
                var piMeta = GetPropertyInfo(item, true);
                xmlOrder = piMeta.xmlOrder;

                //!Is next item is contained inside the same IEnumerable parent?
                if (piMeta.itemIndex > 0)
                    return ObjectAtIndex(piMeta.ieItems, piMeta.itemIndex - 1) as BaseType;

                //!Is next item part of a parent property that follows our item?
                if (PrevNode(par) != null) return prevNode;

                //!We did not found a next item, so let's move up one parent level in this while loop and check the properties under the parent.
                //We keep climbing upwards in the tree until we find a parent with a next item, or we hit the top ancester node and return null.
                if (par is null) return item;
                item = par;
                par = item.ParentNode;
                doDescendants = false; //this will prevent infinite loops by preventing a useless search through the new parent's direct descendants - we already searched these.
            }
            //return null;
            //!+--------------Local Function--------------------
            BaseType PrevNode(BaseType targetNode)
            {
                foreach (var p in targetNode.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    int highestOrder = 10000;  //Order in XmlElementAttribute, for finding the  next property to return; start with a huge value.
                    XmlElementAttribute att = p.GetCustomAttributes<XmlElementAttribute>()
                        .FirstOrDefault(a =>
                        a.Order < xmlOrder &&
                        a.Order > highestOrder &&
                        p.GetValue(targetNode) != null);

                    if (att != null)
                    {//assign nextNode whenever we find a higher Order in an XmlElementAttribute                                
                        object temp = p.GetValue(targetNode);
                        if (!(temp is Enum))
                        {
                            var ie = temp as IList<BaseType>;
                            if (ie !=null && ie[ie.Count()-1] != null)
                            { //If we find an IEnumerable property, get its last member.
                                highestOrder = att.Order;
                                prevNode = ie[ie.Count() - 1];
                            }
                            if (temp is BaseType)
                            {
                                highestOrder = att.Order;
                                prevNode = temp as BaseType;
                            }
                        }
                    }
                }
                return prevNode;
            }
        }
        public static BaseType NextNode(BaseType item)
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
                //!Does item have any children of its own?  If yes, find the first non-null property in the XML element order
                if (doDescendants) if (NextKid(item) != null) return nextNode;                    

                //!No child items contained the next node, so let's look at other properties inside the parent object
                var piMeta = GetPropertyInfo(item, true);
                xmlOrder = piMeta.xmlOrder;

                //!Is next item is contained inside the same IEnumerable parent?
                if (piMeta.itemIndex > -1 && piMeta.itemIndex < piMeta.ieItems.Count() - 1)
                    return ObjectAtIndex(piMeta.ieItems, piMeta.itemIndex + 1) as BaseType;

                //!Is next item part of a parent property that follows our item?
                if (NextKid(par) != null) return nextNode;

                //!We did not found a next item, so let's move up one parent level in this while loop and check the properties under the parent.
                //We keep climbing upwards in the tree until we find a parent with a next item, or we hit the top ancester node and return null.
                item = par;
                par = item.ParentNode;
                doDescendants = false; //this will prevent infinite loops by preventing a useless search through the new parent's direct descendants - we already searched these.
            }
            return null;
            //!+--------------Local Function--------------------
            BaseType NextKid(BaseType targetNode)
            {
                int lowestOrder = 10000;  //Order in XmlElementAttribute, for finding the  next property to return; start with a huge value.
                foreach (var p in targetNode.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    XmlElementAttribute att = p.GetCustomAttributes<XmlElementAttribute>()
                        .FirstOrDefault(a =>
                        a.Order > xmlOrder &&
                        a.Order < lowestOrder &&
                        p.GetValue(targetNode) != null);

                    if (att != null)
                    {//assign nextNode whenever we find a lower Order in an XmlElementAttribute                                
                        object temp = p.GetValue(targetNode);
                        if (!(temp is Enum))
                        {
                            if (temp is IEnumerable && ObjectAtIndex(temp as IEnumerable, 0) != null)
                            { //If we find an IEnumerable property, get it's first member.
                                lowestOrder = att.Order;
                                nextNode = ObjectAtIndex(temp as IEnumerable, 0) as BaseType;
                            }
                            if (temp is BaseType)
                            {
                                lowestOrder = att.Order;
                                nextNode = temp as BaseType;
                            }
                        }
                    }
                }
                return nextNode;
            }
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

            //If there is more than one reference match to item, then we have created a wrapper property in the par partial class.
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
        
        internal static XmlChoiceIdentifierAttribute X_GetXmlChoiceIdentifierAttribute(BaseType item)
        {
            var xci = (XmlChoiceIdentifierAttribute)Attribute.GetCustomAttribute(
                GetPropertyInfo(item, out string _, out int _, out IEnumerable<BaseType> _, out int _, out int _, out string _),
                typeof(XmlChoiceIdentifierAttribute));
            return xci;
        }
        internal static XmlElementAttribute[] X_GetXmlElementAttributes(BaseType item)
        {
            XmlElementAttribute[] xea = (XmlElementAttribute[])Attribute.GetCustomAttributes(
                GetPropertyInfo(item, out string _, out int _, out IEnumerable<BaseType> _, out int _, out int _, out string _),
                typeof(XmlElementAttribute));
            return xea;
        }
        public static IEnumerable<BaseType> X_GetParentIEnumerable(BaseType item, out int itemIndex)
        {
            var par = item.ParentNode;
            itemIndex = -1;
            IEnumerable<BaseType> ieItems = null;
            IEnumerable<BaseType> tempIEitems = null;

            if (par != null)
            {
                var ieProps = par.GetType().GetProperties()
                    .Where(n => n.GetValue(par) is IEnumerable<BaseType>);

                foreach (var propInfo in ieProps)
                {
                    if (propInfo != null)
                    {
                        ieItems = (IEnumerable<BaseType>)propInfo.GetValue(par, null); //Reflect each list to see if our item parameter lives in it
                        itemIndex = IndexOf(ieItems, item); //search for item
                    }
                    //if (itemIndex > -1) return ieItems; //we found item 

                    if (itemIndex > -1) //we found item inside property iePI
                    {
                        var xmlAtts = Attribute.GetCustomAttributes(propInfo, typeof(XmlElementAttribute));
                        if (xmlAtts.Count() > 0)
                            return ieItems;  //returns the property name of the IEnumerable parent of item
                        else tempIEitems = ieItems;  //save in case we can't find a better match in ieItems
                    }
                }
                if (tempIEitems != null) return tempIEitems;  //even though tempIEitems has no XmlElementAttribute, it still contains our item
            }
            return null;

        }
        public static (IEnumerable<BaseType> parentList, int parentListIndex, string itemElementName, int itemPropertyOrder) X_ReflectSdcElement(BaseType item)
        {
            //return parent list object, index in list, ElementName, Property order
            //create a class (SdcItemTreeMetadata) to hold the return value

            string itemElementName = "";
            int itemOrder = -1;
            var par = item.ParentNode;
            int itemIndex = -1;
            IEnumerable<BaseType> lst = X_GetParentIEnumerable(item, out itemIndex);


            XmlChoiceIdentifierAttribute itemXci = X_GetXmlChoiceIdentifierAttribute(item);
            XmlElementAttribute[] xea = X_GetXmlElementAttributes(item);

            //Can we get the property Order quickly from an item XmlElementAttribute?
            if (xea != null && xea.Count() > 0 && xea[0] != null) itemOrder = xea[0].Order;  //all the XmlElementAttributes share the same Order.

            //Simple, common case: we are not in a list (list items don't have their own XmlElementAttribute), and we have a single attribute to describe our ElementName and property order (XML Element order).
            if (itemXci is null && xea != null) //Since itemXci is null, there is no enum to check for the Element Name.
            {
                XmlElementAttribute[] n = null;
                if (xea.Count() == 1) 
                    n = xea;  //if there's only one XmlElementAttribute in xea, let's use it
                else 
                    n = xea.Where(n => n.DataType == item.GetType().Name).ToArray();  //For this to work, there should be only a single match to the property type.

                if (n?.Count() == 1)
                //There is exactly one type match, directly on our item, or else there is only one XmlElementAttribute on item.  
                //This implies that we cannot be in a list, since list members do not have their own XmlElementAttributes.  Is this strictly true? Or only true for ElementName?
                {
                    itemElementName = n[0].ElementName;
                    itemOrder = n[0].Order;
                    if (string.IsNullOrEmpty(itemElementName)) itemElementName = X_GetPropertyName(item, out IEnumerable<BaseType> IEnumerableProperty, out int indexInProperty); //the XmlElementAttribute did not give us an ElementName,, so we have to reflect the ElementName from the item property itself.
                    return (lst, itemIndex, itemElementName, itemOrder);
                }
            }
            /*The simple method did not work because:
             * there are zero or multiple matches to XmlElementAttribute.DataType or
             * XmlElementAttribute.DataType has no value or
             * item is contained in a list or array, and thus there are no XmlElementAttribute directly on item
             */

            if (lst == null && itemXci != null) //We are NOT in a List
            {
                // We are NOT in a List, but we need to check the XmlChoiceIdentifierAttribute (itemXci) for correct ElementName
                var piEnum = par.GetType().GetProperty(itemXci.MemberName);  //get a PropertyInfo for the XmlChoiceIdentifier property that describes an Enum with the Element Names enumerated
                if (piEnum is null)
                    throw new TypeAccessException("Could not reflect Enum from XmlChoiceIdentifierAttribute");
                var myEnum = piEnum.GetValue(par, null) as Enum; //Note: NOT an Enum array
                if (myEnum is null || !(myEnum.GetType().IsSubclassOf(typeof(Enum))))
                    throw new TypeAccessException("Could not obtain Enum property by reflection of XmlChoiceIdentifierAttribute");
                itemElementName = myEnum.ToString();
                if (string.IsNullOrEmpty(itemElementName)) itemElementName = item.ElementName;
                return (null, -1, itemElementName, itemOrder);

            }
            else if(lst!= null) //we ARE in a list on the parent object
            {
                //Look for XmlChoiceIdentifierAttribute on the parent list - 
                XmlChoiceIdentifierAttribute lstXci = Attribute.GetCustomAttribute(lst.GetType(), typeof(XmlChoiceIdentifierAttribute)) as XmlChoiceIdentifierAttribute;

                if (lstXci is null)
                {//Try to find a matching XmlElementAttribute on the parent list, first based on a Data Type match
                    var xmlAtts = lst.GetType().GetCustomAttributes(typeof(XmlElementAttribute)).Where(n =>
                         {
                             string dt = (n as XmlElementAttribute)?.DataType;
                             if(dt != null) return dt == item.GetType().Name;
                             return false;                         
                         }) as XmlElementAttribute[];

                    if (xmlAtts != null && xmlAtts.Count() == 1)
                    {
                        itemElementName = xmlAtts[0].ElementName;
                        if (string.IsNullOrEmpty(itemElementName)) itemElementName = item.ElementName;
                        return (lst, itemIndex, itemElementName, xmlAtts[0].Order);
                    }
                }
                //Data Type match with XmlElementAttribute did not work
                else //let's look at the Enum array referenced by XmlChoiceIdentifierAttribute on the parent list
                {
                    var piEnum = par.GetType().GetProperty(lstXci.MemberName);  //get a PropertyInfo for the XmlChoiceIdentifier property that describes an Enum with the Element Names enumerated
                    if (piEnum is null)
                        throw new TypeAccessException("Could not reflect Enum from XmlChoiceIdentifierAttribute");
                    var myEnumArray = piEnum.GetValue(par, null) as Enum[]; //Note: this is an Enum Array
                    if (itemIndex < 0 || itemIndex > myEnumArray.Length - 1)
                        throw new TypeAccessException("Could not locate property by reflection of XmlChoiceIdentifierAttribute because item's list index is out of range: " + itemIndex.ToString());
                    var myEnum = myEnumArray[itemIndex];  //should return an Enum from a list or array of enums
                    if (myEnum is null || !(myEnum.GetType().IsSubclassOf(typeof(Enum))))
                        throw new TypeAccessException("Could not obtain Enum property by reflection of XmlChoiceIdentifierAttribute");
                    itemElementName = myEnum.ToString();
                    if (string.IsNullOrEmpty(itemElementName)) itemElementName = item.ElementName;
                    return (lst, itemIndex, itemElementName, itemOrder);
                }
            }

            throw new TypeAccessException("Could not reflect anything from the item Parameter");
        }
        /// <summary>
        /// Returns name of the property in the parent object that contains the supplied item parameter.
        /// Does not use XMLElementAttribute to retrieve the element name
        /// </summary>
        public static string X_GetPropertyName(BaseType item)//change to private after testing?
        { return X_GetPropertyName(item, out IEnumerable<BaseType> _, out int _); }
        /// <summary>
        /// Returns name of the property in the parent object that contains the supplied item parameter.
        /// Does not use XMLElementAttribute to retrieve the element name
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// <paramref name="IEnumerableProperty">If the host property supports IEnumerable, it will be returned as an out parameter</paramref>
        /// <paramref name="index">If the host property supports IEnumerable, the item index will be returned as an out parameter</paramref>
        /// </returns>
        public static string X_GetPropertyName(BaseType item, out IEnumerable<BaseType> IEnumerableProperty, out int index) //change to private after testing?
        {
            //Gets the non-attribute property name directly from the property.  
            //This will be used as the serialized element name if the element name is not specified inn an attribute.
            index = -1;
            IEnumerableProperty = null;

            var par = item.ParentNode;            

            if (par != null && !(par is IEnumerable))  //this only works properly if the parent is not a list or array - we do not have IEnumerable parent classes definedd in SDC
            {
                IEnumerable<PropertyInfo> piSet = par.GetType().GetProperties()?.Where(
                    pi => ReferenceEquals(pi.GetValue(par), item));
                if (piSet != null && piSet.Count() == 1)
                {
                    return piSet.ToArray()[0].Name;
                }
                else if(piSet != null && piSet.Count() == 2) //check for a custom property0 wrapping an inner property1 - use the inner property, if we can tell the difference.
                {
                    var piaSet = piSet.ToArray();
                    var prop0 = piaSet[0].GetValue(par);
                    var prop1 = piaSet[1].GetValue(par);

                    if (ReferenceEquals(prop0, prop1))
                    {
                        //The true inner property will have XmlElementAttributes on it 
                        //piaSet[1] is more likely to be the inner property, so we try it first.
                        var a1 = Attribute.GetCustomAttributes(piaSet[1], typeof(XmlElementAttribute));
                        if (a1?.Count() > 0) return piaSet[1].Name;

                        var a0 = Attribute.GetCustomAttributes(piaSet[0], typeof(XmlElementAttribute));
                        if (a0?.Count() > 0) return piaSet[0].Name;
                        
                        return piaSet[1].Name; //the second member is more likely to be the inner IEnumerable if neither prop0 or prop1 has XmlElementAttribute 
                    }
                }
            }

            //We did not find the item in the stated non-IEnumerable properties, so let's look in par's IEnumerables (Lists and Arrays)
            //If we find item in an IEnumerable property, (and that IEnumerable should have at least one XmlAttribute - as an optional check), 
            //we can get the IEnumerable property's name and apply it to all IEnumerable member objects
            var ieProps = par.GetType().GetProperties()?.Where(pi => pi.GetValue(par) is IEnumerable);
            IEnumerable ieItems = null;
            int itemIndex = -1;
            PropertyInfo tempPropInfo = null;
            foreach (var propInfo in ieProps)
            {
                if (propInfo != null)
                {
                    ieItems = (IEnumerable)propInfo.GetValue(par, null); //Reflect each IEnumerable object to see if our item parameter lives in it
                    itemIndex = IndexOf(ieItems, item); //search for item
                }
                if (itemIndex > -1) //we found item inside property iePI
                {
                    var xmlAtts = Attribute.GetCustomAttributes(propInfo, typeof(XmlElementAttribute));
                    IEnumerableProperty = ieItems as IEnumerable<BaseType>;
                    index = itemIndex;
                    if (xmlAtts.Count() > 0)                        
                        return propInfo.Name;  //returns the property name of the IEnumerable parent of item
                    else tempPropInfo = propInfo;  //save in case we can't find a better match in ieItems
                }
            }
            if (tempPropInfo != null) return tempPropInfo.Name;  //even though pi has no XmlElementAttribute, it still contains our item
            


            throw new Exception("Could not reflect property name for item parameter");
        }
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

        #endregion



    }

}
