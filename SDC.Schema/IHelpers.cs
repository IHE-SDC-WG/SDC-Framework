using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


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
        #endregion

        #region ListHelpers
        protected int GetListIndex<T>(List<T> list, T node) where T : notnull //TODO: could make this an interface feature of all list children
        {
            int i = 0;
            foreach (T n in list)
            {
                if ((object)n == (object)node) return i;
                i++;
            }
            return -1; //object was not found in list
        }
        protected int GetIndex(BaseType item) { throw new NotImplementedException(); }
        protected List<BaseType> GetList(BaseType item)
        {   //get the list object that points to the item node
            //Only works for SDC List<BaseType> derivitives.   Does not work e.g., for XML types, derived from XmlElement.
            //Work out how to return a list of the exact type <T>.

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
        protected bool IsList(object o)
        {
            if (o == null) return false;
            return o is List<BaseType> &&  // is Ilist &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        #endregion

        bool IsItemChangeAllowed<S, T>(S source, T target)
            where S : notnull, IdentifiedExtensionType
            where T : IdentifiedExtensionType
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
                    break;
                case DisplayedType d:
                    break;
                case InjectFormType j:
                    return false;
                default:
                    break;
            }


            return false;
        }

        #region Helpers
        string CreateName(BaseType bt)
        {
            throw new NotImplementedException();
        }
        XmlElement StringToXMLElement(string rawXML)
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
