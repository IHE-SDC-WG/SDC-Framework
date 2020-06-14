using MsgPack.Serialization.CollectionSerializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


//using SDC;
namespace SDC.Schema
{


    public interface IHelpers
    {
        #region ArrayHelpers
        int GetFirstNullArrayIndex<T>(T[] array, int growthIncrement = 3)
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
        T[] ArrayAddItemReturnArray<T>(T[] array, T itemToAdd, int growthIncrement = 3)
        {
            int i = GetFirstNullArrayIndex(array, growthIncrement);
            array[i] = itemToAdd;
            return array;

        }
        T ArrayAddReturnItem<T>(T[] array, T itemToAdd, int growthIncrement = 3)
        {
            ArrayAddItemReturnArray(array, itemToAdd, growthIncrement);
            return itemToAdd;

        }
        internal T[] RemoveArrayNullsNew<T>(T[] array)  //TODO: could make this an interface feature of all list children
        {
            int i = 0;
            foreach (var n in array) if (n != null) i++;
            var newarray = new T[i - 1];
            foreach (var n in array)
            {
                if (n != null) newarray[i] = array[i];
                i++;
            }
            return newarray;
        }

        #endregion

        #region SDC Helpers
        internal List<BaseType> GetBaseTypeListParent(BaseType item, string elementName)
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
        internal T PreviousSib<T>(T item) where T : notnull, BaseType
        {
            var p = GetListParent(item);
            if (p != null && p is IEnumerable)
            {
                var pos = p.IndexOf(item);
                if (pos > 0) return p[pos - 1];
            }
            return null;
        }
        internal BaseType NextSib<T>(T item, string elementName = "") where T : notnull, BaseType
        {
            //Reflecting SDC objects to move to previous or next sib in the SDC tree

            //First, locate the current item in the object tree.
            //extract the item's XML order from any XmlElementAttribute on that property
            //  if only one XmlElementAttribute is present in GetProperties, extract ElementName from it.
            //      if ElementName is not present, then extract Name property from the PropertyInfo object
            //  if XmlChoiceIdentifierAttribute("ItemElementName") is NOT present, 
            //      then extract the correct ElementName by matching the XmlElementAttribute on the object Type, 
            //      and selecting the ElementName field from the attribute.
            //      if XmlChoiceIdentifierAttribute("value") is present, see XmlChoiceIdentifierAttribute section below to discover the ElementName
            //      If there are multiple XmlElementAttribute on the parent List object, match the XmlElementAttribute with the same type field


            //if XmlChoiceIdentifierAttribute("value") is present
            //Extract the value string from the attribute, and then look in the enum property named "value" and extract the ElementName from the enum property's value. 

            //if no XmlElementAttribute is present, obtain the parentNode's List or Array, and locate item in the List/array
            //  use GetListParent() for this because it requires knowing about SDC child-->parent structure to locate the correct parent object
            //extract the XmlElementAttribute(s) from the located parent item (it should be a list or array) and use the above technique to locate the ElementName

            //Determine if the SDC ParentNode is a List or Array
            //  if the item is inside a List or Array, find its position in the List/Array using .IndexAt().  
            //  
            //  Determine if it's at the start or end of the array.
            //  If at the end, find the next node by getting the parent object (holding the List/Array as a property) 
            //  and moving  up or down the XmlElementAttribute order property.  
            //  If we're at the end of the properties in that parent object, we need to move up one  more parent SDC level to move forward or back
            //  The parent object must be queried on all its properties (incuding lists/array) to find the previous or next node.
            //  


            var par = item.ParentNode;
            bool isParList = IsList(par);

            var pList = GetListParent(item);
            if (string.IsNullOrWhiteSpace(elementName)) elementName = item.ElementName;

            if (pList != null && pList is IEnumerable)
            {
                var pos = pList.IndexOf(item);
                if (pos > 0 && pos < pList.Count - 1) return pList[pos + 1];
            }
            //could not find item in pList, so move up to the parent node level (the object holding the list) and check where item is attached.
            //then check the parent object's other XmlElement properties
            int? parOrder = null;
            par = item.ParentNode;
            if (par is null) return null;
            var props = X_GetXmlElementAttributesPIs(par);
            //Get the max Order among all the XmlElementAttributes.  This will be an upper bound for later searching
            int maxOrder = GetMaxOrderFromXmlElementAttributes(par);

            var tup = GetXmlElementNameOrder(item);
            foreach (var pi in props)
            {
                if (pi.Name == par.ElementName) //find our elementName in the XmlElementAttribute metadata, then get the XML element's order
                {
                    parOrder = pi
                        .GetCustomAttributes<XmlElementAttribute>()
                        .First().Order;
                    break;
                }
            }
            if (parOrder is null) throw new InvalidOperationException("Could not find order of the XML element " + elementName);
            //Find the next non-null property, using the next closest XmlElementAttribute Order property on a non-null object property.
            //start looking from parOrder; we are done when a non-null property with the smallest Order that is > parOrder is located
            object testProp;
            string nextXmlElementName;

            for (int nextXmlElOrder = parOrder ?? 0 + 1; nextXmlElOrder < maxOrder + 1; nextXmlElOrder++)
            {
                foreach (var pi in props) //Loop through the properties having XmlElementAttribute Order > parOrder
                {
                    var a = pi
                        .GetCustomAttributes<XmlElementAttribute>()
                        .Where(a => a.Order == nextXmlElOrder)
                        .First();
                    if (a != null)
                    {
                        //see if there is a non-null property object of pi's type in par:
                        var type = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                        testProp = pi.GetValue(par); //use property information metadata object (pi) to retrieve the actual property reference inside the instantiated par object;
                        if (testProp != null)
                        {
                            nextXmlElementName = a.ElementName; //we could provide this in an out parameter
                            return testProp as BaseType;
                        }

                    }
                }
            }
            return null;
        }
        internal PropertyInfo[] X_GetXmlElementAttributesPIs<T>(T item) where T : class
        {
            var props = item.GetType().GetProperties();
            if (props is null) return null;

            var xmlEls = props
                .Where(
                        n => n.GetCustomAttributes<XmlElementAttribute>()
                        .Count() > 0
                        )
                .ToArray();

            return xmlEls;
        }
        internal int GetMaxOrderFromXmlElementAttributes(BaseType item)
        {
            var props = X_GetXmlElementAttributesPIs(item);

            //Get the max Order among all the XmlElementAttributes in props.  This will be an upper bound for later searching
            int maxOrder = -1;
            int tempMax = -1;
            foreach (var pi in props)
            {
                tempMax = pi
                    .GetCustomAttributes<XmlElementAttribute>()
                        .Max(a => a.Order);
                if (tempMax > maxOrder) maxOrder = tempMax;
            }
            return maxOrder;

        }
        internal XmlChoiceIdentifierAttribute GetXmlChoiceIdentifierAttribute<T>(T item) where T : BaseType
        {
            var xci = (XmlChoiceIdentifierAttribute) Attribute.GetCustomAttribute(item.GetType(), typeof(XmlChoiceIdentifierAttribute));
            return xci;           
        }
        internal XmlElementAttribute[] GetXmlElementAttributes<T>(T item) where T : BaseType
        {
            XmlElementAttribute[] xea = (XmlElementAttribute[])Attribute.GetCustomAttributes(item.GetType(), typeof(XmlElementAttribute));
            return xea;
        }

        public string GetItemElementName<T>(T item, int? index = null) where T : BaseType
        {
            XmlChoiceIdentifierAttribute xci = GetXmlChoiceIdentifierAttribute(item);

            var par = item.ParentNode;
            string val = "";
            if (par is null) 

            if (xci != null) //We have to look up the name in a name enumeration
                {
                    var piEnum = par.GetType().GetProperty(xci.MemberName);  //should return a PropertyInfo for the XmlChoiceIdentifier property
                    if (piEnum is null) throw new TypeAccessException("Could not locate property by reflection of XmlChoiceIdentifierAttribute");

                    if (piEnum.ReflectedType is IEnumerable<BaseType>)
                    {//we have a list or array here 
                        //we need the list position of item to look up its enum value
                        if (index == null) throw new TypeAccessException("Could not locate property by reflection of XmlChoiceIdentifierAttribute because the list index was null ");
                        var e1 = piEnum.GetValue(par, new object[] { index });  //should return an Enum from a list or array of enums
                        if (e1 is null || !(e1.GetType().IsSubclassOf(typeof(Enum)))) throw new TypeAccessException("Could not obtain Enum property by reflection of XmlChoiceIdentifierAttribute");
                        val = (e1 as Enum).ToString();
                        return val;
                    }
                    else
                    { //the name is found in a simple enumeration, not an array of Enums
                        var e2 = piEnum.GetValue(par) as Enum;//get the enum object (not a List of Enum type)
                        if (e2 is null) throw new TypeAccessException("Could not obtain Enum from reflection of XmlChoiceIdentifierAttribute");
                        val = (e2 as Enum).ToString();
                        return val;
                    }
                    
                }
            //the item name is not found in an enum.  It's either the property name itself, or found in an XML element attribute.



            //the item name is not found in an enum or in a list of parent XmlElementAttributes.  It's either the property name itself, or found in an XML element attribute.
            //XmlChoiceIdentifierAttribute not present on item, but there may be a list of XmlElementAttributes, only one of which is the right type to match
            XmlElementAttribute[] xea = GetXmlElementAttributes(item);
            var n = xea.Where(n => n.Type == item.GetType()).ToArray();  //there should be only a singel match to the property type.
            if (n.Count() > 1) throw new TypeAccessException("Could not obtain a unique element name by matching the item type");
            if (n[0] != null || string.IsNullOrEmpty(n[0].ElementName) )
            {
                val = n[0].ElementName;
                return val;
            }
            else //no usable XmlElementAttribute attributes found to deteremine ElementName, so look for the actual property name on the parent object, if we can find it.
            {
                var props = par.GetType().GetProperties().Where(n => n.GetType() == item.GetType() && n.GetValue(item) == item).ToList();
                if (props.Count == 1)
                { val = props[0].Name; }  //name ot the matching property on the parent type
                else
                { throw new TypeAccessException("Could not obtain ElementName by reflecting parent properties"); }
            }
                 
            return null;

        }


        internal (string, int?) GetXmlElementNameOrder(BaseType item)  //tuple return
        {
            //See if item has a single XML element attribute, so we can extract the name.
            if (item is null) return (null, -1);
            
            string elName = null;
            int? order;
            int countXmlEls = item.GetType().GetCustomAttributes<XmlElementAttribute>().Count();
            if (countXmlEls == 1)
            {
                order = item.GetType()
                    .GetCustomAttributes<XmlElementAttribute>()?
                    .First()?.Order;
                elName = item.GetType().GetCustomAttributes<XmlElementAttribute>()?
                    .First()?.ElementName;
                if (!string.IsNullOrEmpty(elName)) return (elName, order);
            }

            //let's back up a level to the parent SDC node, 
            //and see if our item is inside one of the parent's properties (i.e., in a List or Array parent object)
            var par = item.ParentNode;
            if (par is null) return (null, -2);
            //check to see if par is a List vs Array, vs neither?
            if (!(par is IEnumerable<BaseType>))
            {
                PropertyInfo parPI = par.GetType()
                    .GetProperties() //check the properties on par
                    .Where(p => (p.GetValue(p) as BaseType) == item)? //find a property of the ParentNode that refers to our item object
                    .First(); //Get the matching property - there can only be one match in SDC object trees
                if (parPI is null) return (null, -3); //so where is that item node??? Do we need to go higher in the object tree to find it?

                int? count = parPI.GetCustomAttributes<XmlElementAttribute>()?.Count();
                if (count == 1)
                {
                    elName = parPI.GetCustomAttributes<XmlElementAttribute>()?
                        .First()?.ElementName; //get elName from XmlElementAttribute, if present

                    if (string.IsNullOrEmpty(elName))
                        elName = parPI.Name; //get elName from the property Name itself, since it wasn't found in the XmlElementAttribute
                }
            }

            //Check for XMLChoiceIdentifierAttribute if we are on a parent Array--> look up name in the ItemTypeChoice enum

            //We should not need to look any further to find the object in the SDC tree

            




            //if (string.IsNullOrEmpty(elName)) elName = item.ElementName; //last resort: get from the Element Name we asigned when creating the tree

            order = item.GetType()
                .GetCustomAttributes<XmlElementAttribute>()
                .First().Order;

            return (elName, order);
            //return (null, -4);
        }
        internal List<T> GetListParent<T>(T item)
            where T:BaseType
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
                    return GetEventParent(ev).Cast<T>().ToList();
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
        internal List<T> GetEventParent<T>(T item)
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
        internal bool IsList(object o)
        {
            if (o == null) return false;
            return o is List<BaseType> 
                &&  // is Ilist &&
                o.GetType().IsGenericType &&
                o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        #endregion

        internal bool IsItemChangeAllowedI<S, T>(S source, T target)
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

        #region Helpers
        internal string CreateName(BaseType bt)
        {
            throw new NotImplementedException();
        }
        internal XmlElement StringToXMLElement(string rawXML)
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
