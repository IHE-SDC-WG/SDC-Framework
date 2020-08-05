using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Sources;
using Newtonsoft.Json;

//using SDC;
namespace SDC.Schema
{
    public interface INewTopLevel //TODO:
    {
        //FormDesignType CreateForm(bool addHeader, bool addFooter, string formID, string lineage, string version, string fullURI);
        //FormDesignType CreateFormFromTemplatePath(string path, string formID, string lineage, string version, string fullURI);
        //FormDesignType CreateFormFromTemplateXML(string xml, string formID, string lineage, string version, string fullURI);
        //bool RemoveFormFromPackage(RetrieveFormPackageType pkg, FormDesignType form);
    }
    public interface IPackage : ITopNode //TODO:
    { }
    public interface IDataElement : ITopNode //TODO:
    { }
    public interface IDemogForm : ITopNode //TODO:
    { }
    public interface IMap : ITopNode //TODO:
    { }
    public interface IFormDesign : ITopNode, IMoveRemove //TODO:
    {
        //Default Implementations
        SectionItemType AddHeader()
        {
            var fd = (this as FormDesignType);
            if (fd.Header == null)
            {
                fd.Header = new SectionItemType(fd, fd.ID + "_Header");  //Set a default ID, in case the database template does not have a body
                fd.Header.name = "Header";
            }
            return fd.Header;
        }
        SectionItemType AddBody()
        {
            var fd = (this as FormDesignType);
            if (fd.Body == null)
            {
                fd.Body = new SectionItemType(fd, fd.ID + "_Body");  //Set a default ID, in case the database template does not have a body
                fd.Body.name = "Body";
            }
            return fd.Body;
        }
        SectionItemType AddFooter()
        {
            var fd = (this as FormDesignType);
            if (fd.Footer == null)
            {
                fd.Footer = new SectionItemType(fd, fd.ID + "_Footer");  //Set a default ID, in case the database template does not have a body
                fd.Footer.name = "Footer";
            }
            return fd.Footer;
        }
        bool RemoveHeader()
        {
           return (this as FormDesignType).Header.Remove();

        }
        bool RemoveFooter()
        {
            return (this as FormDesignType).Footer.Remove();
        }
        bool RemoveBody()
        {
            return (this as FormDesignType).Body.Remove();
        }
    }
    public interface IRetrieveFormPackage
    {
        LinkType AddFormURL();
        HTMLPackageType AddHTMLPackage();
        XMLPackageType AddXMLPackage();
    }
    /// <summary>
    /// This interface is applied to the partial classes that can have a ChildItems element.
    /// These are Section, Question and ListItem.  
    /// This interface is require to support generic classes that must handle the creation ofthe 
    /// ChildItems element, which holds List of type IdentifiedItemType
    /// </summary>
    public interface IChildItemsParent<T> : IQuestionBuilder //implemented by items that can have a ChildItems node.
        where T : BaseType, IChildItemsParent<T>
    {
        ChildItemsType ChildItemsNode { get; set; }
        SectionItemType AddChildSection(string id, int insertPosition)
        {
            var childItems = AddChildItemsNode(this as T);
            var childItemsList = childItems.ChildItemsList;
            var sNew = new SectionItemType(childItems, id);
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, sNew);

            return sNew;
        }
        QuestionItemType AddChildQuestion(QuestionEnum qType, string id, int insertPosition = -1)
        {
            var childItems = AddChildItemsNode(this as T);
            var childItemsList = childItems.ChildItemsList;
            var qNew = new QuestionItemType(childItems, id);
            ListFieldType lf;
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, qNew);

            switch (qType)
            {
                case QuestionEnum.QuestionSingle:
                    AddListToListField(AddListFieldToQuestion(qNew));
                    break;
                case QuestionEnum.QuestionMultiple:
                    AddListToListField(AddListFieldToQuestion(qNew));
                    qNew.ListField_Item.maxSelections = 0;
                    break;
                case QuestionEnum.QuestionFill:
                    AddQuestionResponseField(qNew);
                    break;
                case QuestionEnum.QuestionLookupSingle:
                    lf = AddListFieldToQuestion(qNew);
                    AddEndpointToListField(lf);
                    break;
                case QuestionEnum.QuestionLookupMultiple:
                    lf = AddListFieldToQuestion(qNew);
                    AddEndpointToListField(lf);
                    break;
                default:
                    throw new NotSupportedException($"{qType} is not supported");
            }

            return qNew;
        } 
        DisplayedType AddChildDisplayedItem(string id, int insertPosition = -1)
        {
            var childItems = AddChildItemsNode(this as T);
            var childItemsList = childItems.ChildItemsList;
            var dNew = new DisplayedType(childItems, id);
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, dNew);

            return dNew;
        }
        ButtonItemType AddChildButtonAction(string id, int insertPosition = -1)
        {
            var childItems = AddChildItemsNode(this as T);
            var childItemsList = childItems.ChildItemsList;
            var btnNew = new ButtonItemType(childItems, id);
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, btnNew);

            // TODO: Add AddButtonActionTypeItems(btnNew);
            return btnNew;
        }
        InjectFormType AddChildInjectedForm(string id, int insertPosition = -1)
        {
            var childItems = AddChildItemsNode(this as T);
            var childItemsList = childItems.ChildItemsList;
            var injForm = new InjectFormType(childItems, id);
            var count = childItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.Insert(insertPosition, injForm);
            //TODO: init this InjectForm object

            return injForm;
        }
        bool HasChildItems()
        {
            {
                if (this?.ChildItemsNode?.ChildItemsList != null)
                {
                    foreach (var n in this.ChildItemsNode.ChildItemsList)
                    { if (n != null) return true; }
                }
            }
            return false;
        }
        ChildItemsType AddChildItemsNode(T T_Parent)
        {
            ChildItemsType childItems = null;  //this class contains an "Items" list
            if (T_Parent == null)
                throw new ArgumentNullException("The T_Parent object was null");
            //return childItems; 
            else if (T_Parent.ChildItemsNode == null)
            {
                childItems = new ChildItemsType(T_Parent);
                T_Parent.ChildItemsNode = childItems;  //This may be null for the Header, Body and Footer  - need to check this
            }
            else //(T_Parent.ChildItemsNode != null)
                childItems = T_Parent.ChildItemsNode;

            if (childItems.ChildItemsList == null)
                childItems.ChildItemsList = new List<IdentifiedExtensionType>();

            return childItems;
        }

        //Remove all child nodes 


        //QR AddChildQR(string id = "", int insertPosition = -1);
        //QS AddChildQS(string id = "", int insertPosition = -1);
        //QM AddChildQM(string id = "", int insertPosition = -1);
        //QL AddChildQL(string id = "", int insertPosition = -1);

    }
    public interface IChildItemsMember<Tchild>  //Marks SectionItemType, QuestionItemType, DisplayedType, ButtonItemType, InjectFormType
            where Tchild : IdentifiedExtensionType, IChildItemsMember<Tchild>
    {
        bool IsMoveAllowedToChild<U>(U Utarget, out string error)
            where U : notnull, IdentifiedExtensionType
            //where T : notnull, IdentifiedExtensionType
        {
            Tchild Tsource = this as Tchild;
            var errorSource = "";
            var errorTarget = "";
            error = "";
            bool sourceOK = false;
            bool targetOK = false;

            if (Tsource is null) { error = "source is null"; return false; }
            if (Utarget is null) { error = "target is null"; return false; }
            if (Utarget is ButtonItemType) { error = "ButtonItemType is not allowed as a target"; return false; }
            if (Utarget is InjectFormType) { error = "InjectFormType is not allowed as a target"; return false; }
            if (Utarget is DisplayedType) { error = "DisplayedItem is not allowed as a target"; return false; }

            if (Tsource is ListItemType && !(Utarget is QuestionItemType) && !(Utarget is ListItemType)) { error = "A ListItem can only be moved into a Question List"; return false; };

            //special case to allow LI to drop on a Q and be added to the Q's List, rather than under ChildItem (which would be illegal)
            if (Tsource is ListItemType &&
                Utarget is QuestionItemType &&
                !((Utarget as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionSingle) &&
                !((Utarget as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionMultiple))
            { error = "A Question target must be a QuestionSingle or QuestionMultiple"; return false; }


            if (Tsource is DisplayedType || Tsource is InjectFormType) sourceOK = true;
            if (Utarget is QuestionItemType || Utarget is SectionItemType || Utarget is ListItemType) targetOK = true;

            if (!sourceOK || !targetOK)
            {
                if (!sourceOK) errorSource = "Illegal source object";
                if (!targetOK) errorTarget = "Illegal target object";
                if (errorTarget.Length > 0) errorTarget += " and ";
                error = errorSource + errorTarget;
            }


            return sourceOK & targetOK;
        }
        bool MoveAsChild<S, T>(S source, T target, int newListIndex)
            where S : notnull, IdentifiedExtensionType    //, IChildItemMember
            where T : DisplayedType, IChildItemsParent<T>
        {
            if (source is null) return false;
            if (source.ParentNode is null) return false;
            if (source is ListItemType && !(target is QuestionItemType)) return false;  //ListItem can only be moved to a question.

            List<BaseType> sourceList;
            BaseType newParent = target;

            switch (source)  //get the sourceList from the parent node
            {
                case QuestionItemType _:
                case SectionItemType _:
                case InjectFormType _:
                case ButtonItemType _:
                    sourceList = (source.ParentNode as ChildItemsType)?.Items.ToList<BaseType>();
                    //sourceList = (source.ParentNode as ChildItemsType).Items.Cast<BaseType>().ToList(); //alternate method
                    break;
                case ListItemType _:
                    sourceList = (source.ParentNode as ListType)?.Items.ToList<BaseType>();
                    break;
                case DisplayedType _:
                    sourceList = (source.ParentNode as ChildItemsType)?.Items.ToList<BaseType>();
                    if (sourceList is null)
                        sourceList = (source.ParentNode as ListType)?.Items.ToList<BaseType>();
                    else return false;
                    break;
                default:
                    return false; //error in source type
            }

            if (sourceList is null) return false;

            List<BaseType> targetList = null;

            if (target != null)
            {
                switch (target)  //get the targetList from the child node
                {
                    case QuestionItemType q:
                        //This is an exception - if we drop a source LI on a QS/QM, we will want to add it ant the end of the Q's List object
                        if (source is ListItemType)
                        {
                            if (q.GetQuestionSubtype() != QuestionEnum.QuestionSingle &&
                                q.GetQuestionSubtype() != QuestionEnum.QuestionMultiple &&
                                q.GetQuestionSubtype() != QuestionEnum.QuestionRaw) return false;  //QR, and QL cannot have child LI nodes
                            if (q.ListField_Item is null)  //create new targetList
                            {
                                targetList = IQuestionBuilder.AddListToListField(IQuestionBuilder.AddListFieldToQuestion(q)).Items.ToList<BaseType>();
                                if (targetList is null) return false;
                                break;
                            }
                            newParent = q.ListField_Item.List;
                            targetList = q.ListField_Item.List.Items.ToList<BaseType>();
                        }
                        else //use the ChildItems node instead as the targetList
                        {
                            (q as IChildItemsParent<QuestionItemType>).AddChildItemsNode(q);
                            targetList = q.ChildItemsNode.Items.ToList<BaseType>();
                        }
                        break;
                    case SectionItemType s:
                        (s as IChildItemsParent<SectionItemType>).AddChildItemsNode(s);
                        targetList = s.ChildItemsNode.Items.ToList<BaseType>();
                        break;
                    case ListItemType l:
                        (l as IChildItemsParent<ListItemType>).AddChildItemsNode(l);
                        targetList = l.ChildItemsNode.Items.ToList<BaseType>();
                        break;
                    default:
                        return false; //error in source type
                }
            }
            else targetList = sourceList;
            if (targetList is null) return false;


            var count = targetList.Count;
            if (newListIndex < 0 || newListIndex > count) newListIndex = count; //add to end  of list

            var indexSource = sourceList.IndexOf(source);  //save the original source index in case we need to replace the source node back to its origin
            bool b = sourceList.Remove(source); if (!b) return false;
            targetList.Insert(newListIndex, source);
            if (targetList[newListIndex] == source) //check for success
            {
                source.TopNode.ParentNodes[source.ObjectGUID] = newParent;
                return true;
            }
            //Error - the source item is now disconnected from the list.  Lets add it back to the end of the list.
            sourceList.Insert(indexSource, source); //put source back where it came from; the move failed
            return false;
        }
        bool MoveAfterSib<S, T>(S source, T target, int newListIndex, bool moveAbove)
            where S : notnull, IdentifiedExtensionType
            where T : notnull, IdentifiedExtensionType
        {
            //iupdate TopNode.ParentNodes
            throw new Exception(String.Format("Not Implemented"));
        }
    }
    public interface IQuestionItem : IQuestionList
    {
        QuestionEnum GetQuestionSubtype();

        //testOnly return a bool indicating if the conversion will succeed.
        QuestionItemType ConvertToQR(bool testOnly = false); //abort if children present
        QuestionItemType ConvertToQS(bool testOnly = false);
        QuestionItemType ConvertToQM(int maxSelections = 0, bool testOnly = false);
        DisplayedType ConvertToDI(bool testOnly = false); //abort if LIs or children present
        QuestionItemType ConvertToSection(bool testOnly = false);
        QuestionItemType ConvertToLookup(bool testOnly = false);//abort if LIs present

        QuestionEnum GetQuestionSubtypeI()
        {
            var q = this as QuestionItemType;
            if (q.ResponseField_Item != null) return QuestionEnum.QuestionFill;
            if (q.ListField_Item is null) return QuestionEnum.QuestionRaw;
            if (q.ListField_Item.LookupEndpoint == null && q.ListField_Item?.maxSelections == 1) return QuestionEnum.QuestionSingle;
            if (q.ListField_Item.LookupEndpoint == null && q.ListField_Item?.maxSelections != 1) return QuestionEnum.QuestionMultiple;
            if (q.ListField_Item.LookupEndpoint != null && q.ListField_Item.maxSelections == 1) return QuestionEnum.QuestionLookupSingle;
            if (q.ListField_Item.LookupEndpoint != null && q.ListField_Item.maxSelections != 1) return QuestionEnum.QuestionLookupMultiple;
            if (q.ListField_Item.LookupEndpoint != null) return QuestionEnum.QuestionLookup;

            return QuestionEnum.QuestionGroup;
        }
        ListItemType AddListItemI(string id, int insertPosition)
        {  //Check for QS/QM first!
            var q = this as QuestionItemType;
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionMultiple ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionSingle)
            {
                if (q.ListField_Item is null) AddListFieldToQuestionI();
                ListType list = q.ListField_Item.List;
                if (list is null) AddListToListFieldI(q.ListField_Item);
                return AddListItemI(list, id, insertPosition);
            }
            else throw new InvalidOperationException("Can only add ListItem to QuestionSingle or QuestionMultiple");
        }
        ListItemType AddListItemResponseI(string id, int insertPosition)
        {
            var q = this as QuestionItemType;
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionMultiple ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionSingle)
            {
                if (q.ListField_Item is null) AddListFieldToQuestionI();
                ListType list = q.ListField_Item.List;
                if (list is null) AddListToListFieldI(q.ListField_Item);
                return AddListItemResponseI(list, id, insertPosition);
            }
            else throw new InvalidOperationException("Can only add ListItem to QuestionSingle or QuestionMultiple");
        }
        DisplayedType AddDisplayedTypeToListI(string id, int insertPosition)
        {
            var q = this as QuestionItemType;
            if (q.GetQuestionSubtype() == QuestionEnum.QuestionMultiple ||
                q.GetQuestionSubtype() == QuestionEnum.QuestionSingle)
            {
                if (q.ListField_Item is null) AddListFieldToQuestionI();
                ListType list = q.ListField_Item.List;
                if (list is null) AddListToListFieldI(q.ListField_Item);
                return AddDisplayedItemToListI(list, id, insertPosition);
            }
            else throw new InvalidOperationException("Can only add DisplayedItem to QuestionSingle or QuestionMultiple");
        }
        QuestionItemType ConvertToQR_I(bool testOnly)
        {
            throw new NotImplementedException();
        }
        QuestionItemType ConvertToQS_I(bool testOnly)
        {
            throw new NotImplementedException();
        }
        QuestionItemType ConvertToQM_I(int maxSelections, bool testOnly)
        {
            throw new NotImplementedException();
        }
        DisplayedType ConvertToDI_I(bool testOnly)
        {
            throw new NotImplementedException();
        }
        QuestionItemType ConvertToSectionI(bool testOnly)
        {
            throw new NotImplementedException();
        }
        QuestionItemType ConvertToLookupI(bool testOnly)
        {
            throw new NotImplementedException();
        }
        protected virtual ResponseFieldType AddQuestionResponseFieldI()
        {
            var qParent = this as QuestionItemType;
            var rf = new ResponseFieldType(qParent);
            qParent.ResponseField_Item = rf;

            return rf;
        }
        protected virtual LookupEndPointType AddEndpointToListFieldI(ListFieldType listFieldParent)
        {
            if (listFieldParent.List == null)
            {
                var lep = new LookupEndPointType(listFieldParent);
                listFieldParent.LookupEndpoint = lep;
                return lep;
            }
            else throw new InvalidOperationException("Can only add LookupEndpoint to ListField if List object is not present");
        }
        protected virtual ListType AddListToListFieldI(ListFieldType listFieldParent)
        {
            ListType list;  //this is not the .NET List class; It's an answer list
            if (listFieldParent.List == null)
            {
                list = new ListType(listFieldParent);
                listFieldParent.List = list;
            }
            else list = listFieldParent.List;

            //The "list" item contains a list<DisplayedType>, to which the ListItems and ListNotes (DisplayedItems) are added.
            if (list.QuestionListMembers == null)

                list.QuestionListMembers = new List<DisplayedType>();

            return list;
        }
        protected virtual ListFieldType AddListFieldToQuestionI()
        {
            var qParent = this as QuestionItemType;
            if (qParent.ListField_Item == null)
            {
                var listField = new ListFieldType(qParent);
                qParent.ListField_Item = listField;
            }

            return qParent.ListField_Item;
        }






        //bool ConvertToButton(); //abort if LIs or children present

        //convert type to QR
        //Convert to S, DI    (must first delete List or ResponseField present)
        //Convert QR to QS    (delete ResponseField, add List, set maxSelections)
        //Convert QR to QM    (delete ResponseField, add List, set maxSelections)
        //LookupEndPointType AddLookupEndpoint(ListFieldType lfParent);  //should be part of AddChildQL code;
        //CanConvert (to Type)

    }
    public interface IQuestionList //may be implemented by Q, List, LI and perhaps DI (if inside a list)
    {
        ListItemType AddListItem(string id = "", int insertPosition = -1); //check that no ListItemResponseField object is present
        ListItemType AddListItemResponse(string id = "", int insertPosition = -1); //check that no ListFieldType object is present
        DisplayedType AddDisplayedTypeToList(string id = "", int insertPosition = -1);


        ListItemType AddListItemI(ListType lt, string id, int insertPosition) //check that no ListItemResponseField object is present
        {
            ListItemType li = new ListItemType(lt, id);
            var count = lt.QuestionListMembers.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            lt.QuestionListMembers.Insert(insertPosition, li);
            return li;

        }
        ListItemType AddListItemResponseI(ListType lt, string id, int insertPosition) //check that no ListFieldType object is present
        { throw new NotImplementedException(); }
        DisplayedType AddDisplayedItemToListI(ListType lt, string id, int insertPosition)
        { throw new NotImplementedException(); }


    }
    public interface IQuestionListMember : IQuestionBuilder //decorates LI/LIR and DI
    {
        //for DI, make sure parent is a ListType object
        //bool Remove(bool removeDecendants = false);
        //bool Move(QuestionItemType target = null, bool moveAbove = false, bool testOnly = false);
        //bool Move(ListItemType target = null, bool moveAbove = false, bool testOnly = false);
        //ListItemType ConvertToLI(bool testOnly = false);
        //DisplayedType ConvertToDI(bool testOnly = false); //abort if children of LI are present
        //ListItemType ConvertToLIR(bool testOnly = false);


        //for DI, make sure parent is a ListType object
        bool Remove(bool removeDecendants) { throw new NotImplementedException(); }
        //bool Move(QuestionItemType target, bool moveAbove, bool testOnly) { throw new NotImplementedException(); }
        //bool Move(ListItemType target, bool moveAbove, bool testOnly) { throw new NotImplementedException(); }
        /// <summary>
        /// Move a ListItem or DisplayedItem within the same List
        /// </summary>
        /// <returns>returns true if operation succeeded    
        /// </returns>
        bool MoveinList(out List<string> errList, int newListIndex = -1)
        {
            var qlm = this as BaseType;  //"this" is either a DI or LI
            var list = (ListType)(qlm)?.ParentNode;
            return qlm.Move(list, out errList, 6);
        }
        //bool Move(List<BaseType> targetProperty, out List<string> errList, int newListIndex = -1) //DI and LI must have a parent property of ListType (DI or LI) of ChildItems (DI)
        //{
        //    var qlm = this as BaseType;  //"this" is either a DI or LI
        //    var list = (ListType)(qlm)?.ParentNode;
        //    return qlm.Move(targetProperty, qlm, out errList, newListIndex);
        //}

        /// <summary>
        /// Move a ListItem or DisplayedItem to another List
        /// </summary>
        /// <param name="targetList"></param>
        /// <param name="errList"></param>
        /// <param name="newListIndex"></param>
        /// <returns>returns true if operation succeeded    </returns>
        bool MoveToList(ListType targetList, out List<string> errList, int newListIndex = -1) 
        {
            var qlm = this as BaseType;  //"this" is either a DI or LI
            return qlm.Move(targetList, out errList, newListIndex);
        }

        ListItemType ConvertToLI(bool testOnly) { throw new NotImplementedException(); }
        DisplayedType ConvertToDI(bool testOnly) { throw new NotImplementedException(); } //abort if children of LI are present
        ListItemType ConvertToLIR(bool testOnly) { throw new NotImplementedException(); }
        bool IsMoveAllowedToList<T>(T target, out string error)
            where T : notnull, IQuestionListMember
        {
            error = "";

            //if (source is null) { error = "source is null"; return false; }
            //if (target is null) { error = "target is null"; return false; }
            //if (source is SectionItemType) return false; //S is illegal in the list
            //if (target is SectionItemType) return false; //S is illegal in the list
            //if (source is ButtonItemType) return false; //B is illegal in the list
            //if (target is ButtonItemType) return false; //B is illegal in the list

            //if (source is QuestionItemType) return false; //Q is illegal in the list


            //if (!(source is ListItemType) && !(source is DisplayedType)) { error = "The source must be a ListItem or DisplayedItem"; return false; };
            //if (!(target is ListItemType) && !(target is QuestionItemType)) { error = "The target must be a ListItem or Question"; return false; };

            if (target is QuestionItemType &&
                !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionSingle) &&
                !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionMultiple) &&
                !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionRaw))
            { error = "A Question target must be a QuestionSingle, QuestionMultiple or of unassigned (QuestionRaw) type"; return false; }

            return true;

        }
        bool MoveInList(DisplayedType source, QuestionItemType target, bool moveAbove)
        {
            if (source is null) return false;
            if (source is RepeatingType) return false; //S, Q are illegal in the list
            if (source is ButtonItemType) return false; //B is illegal in the list

            if (target is null) target = source.ParentNode?.ParentNode?.ParentNode as QuestionItemType; //LI-->List-->ListField-->Q  - see if we can capture a Question from the source node
            if (target is null) return false;

            List<BaseType> sourceList = (source.ParentNode as ListType)?.Items?.ToList<BaseType>();//guess that the sourrce node is inside a List
            if (sourceList is null) sourceList = (source.ParentNode as ChildItemsType)?.ChildItemsList?.ToList<BaseType>();//try again - guess that source node is inside a ChildItems node
            if (sourceList is null) return false;//both guesses failed - this is probably a disconnected node, and we can't work with that.

            if (target.ListField_Item is null) AddListToListField(AddListFieldToQuestion(target));  //make sure there is a ist instantiated on this target Question; if not, then create it.          
            var targetList = target.ListField_Item.List.Items;
            if (targetList is null) return false;  //unkown problem getting Question-->ListField-->List

            var indexSource = sourceList.IndexOf(source);
            int index = targetList.Count;
            sourceList.Remove(source);
            targetList.Insert(index, source);
            if (targetList[index] == source)
            {
                source.TopNode.ParentNodes[source.ObjectGUID] = target;
                return true;
            }

            //Error - the source item is now disconnected from the list.  Lets add it back to the end of the list.
            sourceList.Insert(indexSource, source); //put source back where it came from; the move failed
            return false;

        }
        bool MoveInList(DisplayedType source, DisplayedType target, bool moveAbove) //target must be a LI or DI (not a RepeatingType); need to address Nodes dictionary updates
        {
            //this function allows dropping items inside a QS o QM list to rearrange the list
            //prevent illegal operationss
            if (source is null) return false;
            if (source is RepeatingType) return false; //S, Q (RepeatingTypes) are illegal in the Q's list
            if (target is RepeatingType) return false; //S, Q (RepeatingTypes) are illegal in the Q's list
            if (source is ButtonItemType) return false; //B is illegal in the Q's list
            if (target is ButtonItemType) return false; //B is illegal in the Q's list

            List<BaseType> sourceList = (source.ParentNode?.ParentNode as ListType)?.Items?.ToList<BaseType>();
            if (sourceList is null) sourceList = (source.ParentNode as ChildItemsType)?.ChildItemsList?.ToList<BaseType>();
            if (sourceList is null) return false;

            var qTarget = target.ParentNode?.ParentNode?.ParentNode as QuestionItemType;
            if (qTarget is null) return false;  //we did not get a Q object, so we are not moving  a node into a Q List

            var targetList = qTarget.ListField_Item?.List?.Items;


            if (targetList is null) return false;

            var indexSource = sourceList.IndexOf(source);
            sourceList.Remove(source);

            //Determine where to insert the node in the list, based on the location of the existing Lis
            var index = targetList.IndexOf(target);
            if (index < 0) index = targetList.Count; //target node not found in list, so insert source at the end of the list; this should never execute
            if (moveAbove) index--;

            targetList.Insert(index, source);
            if (targetList[index] == source)
            {
                source.TopNode.ParentNodes[source.ObjectGUID] = target;
                return true;
            }

            //Error - the source item is now disconnected from the list.  Lets add it back to the end of the list.
            sourceList.Insert(indexSource, source); //put source back where it came from; the move failed
            return false;
        }



    } //Implemented on ListItem and DisplayedItem
    public interface IListField
    {
        ListType List { get; set; }
        LookupEndPointType LookupEndpoint { get; set; }

    }
    public interface IQuestionBase
    {
        //ListFieldType ListField_Item { get; set; }
        ResponseFieldType ResponseField_Item { get; set; }
    }
    public interface IListItem
    {
        ListItemResponseFieldType AddListItemResponseField()
        {
            var liParent = this as ListItemType;
            var liRF = new ListItemResponseFieldType(liParent);
            liParent.ListItemResponseField = liRF;

            return liRF;
        }
        EventType AddOnDeselect()
        {
            var li = (ListItemType)this;
            var ods = new EventType(li);
            li.OnDeselect.Add(ods);
            return ods;
        }
        EventType AddOnSelect()
        {
            var li = (ListItemType)this;
            var n = new EventType(li);
            li.OnSelect.Add(n);
            return n;
        }
        PredGuardType AddSelectIf()
        {
            var li = (ListItemType)this;
            var n = new PredGuardType(li);
            li.SelectIf = n;
            return n;
        }
        PredGuardType AddDeSelectIf()
        {
            var li = (ListItemType)this;
            var n = new PredGuardType(li);
            li.DeselectIf = n;
            return n;
        }

    }
    public interface IQuestionBuilder
    {
        internal static ResponseFieldType AddQuestionResponseField(QuestionItemType qParent)
        {
            var rf = new ResponseFieldType(qParent);
            qParent.ResponseField_Item = rf;

            return rf;
        }
        internal static LookupEndPointType AddEndpointToListField(ListFieldType listFieldParent)
        {
            if (listFieldParent.List == null)
            {
                var lep = new LookupEndPointType(listFieldParent);
                listFieldParent.LookupEndpoint = lep;
                return lep;
            }
            else throw new InvalidOperationException("Can only add LookupEndpoint to ListField if List object is not present");
        }
        internal static ListType AddListToListField(ListFieldType listFieldParent)
        {
            ListType list;  //this is not the .NET List class; It's an answer list
            if (listFieldParent.List == null)
            {
                list = new ListType(listFieldParent);
                listFieldParent.List = list;
            }
            else list = listFieldParent.List;

            //The "list" item contains a list<DisplayedType>, to which the ListItems and ListNotes (DisplayedItems) are added.
            if (list.QuestionListMembers == null)

                list.QuestionListMembers = new List<DisplayedType>();

            return list;
        }
        internal static ListFieldType AddListFieldToQuestion(QuestionItemType qParent)
        {
            if (qParent.ListField_Item == null)
            {
                var listField = new ListFieldType(qParent);
                qParent.ListField_Item = listField;
            }

            return qParent.ListField_Item;
        }

    }
    public interface ISection
    {
        QuestionItemType ChangeToQuestionMultiple();
        QuestionItemType ChangeToQuestionSingle();
        QuestionItemType ChangeToQuestionResponse();
        QuestionItemType ChangeToQuestionLookup();
        DisplayedType ChangeToDisplayedItem();


    }
    public interface IDisplayedTypeChanges
    {
        // use these as part of a static DisplayedTypeChanges utility class; Do not add them to DisplayedType, since they would be inherited by Section, Question andd InjectForm
        QuestionItemType ChangeToQuestionMultiple(DisplayedType source);
        QuestionItemType ChangeToQuestionSingle(DisplayedType source);
        QuestionItemType ChangeToQuestionResponse(DisplayedType source);
        QuestionItemType ChangeToQuestionLookup(DisplayedType source);
        SectionItemType ChangeToSection(DisplayedType source);
        ButtonItemType ChangeToButtonAction(DisplayedType source);
        InjectFormType ChangeToInjectForm(DisplayedType source);

        DisplayedType ChangeToDisplayedItem(SectionItemType source);
        QuestionItemType ChangeToQuestionMultiple(SectionItemType source);
        QuestionItemType ChangeToQuestionSingle(SectionItemType source);
        QuestionItemType ChangeToQuestionResponse(SectionItemType source);
        QuestionItemType ChangeToQuestionLookup(SectionItemType source);
        ButtonItemType ChangeToButtonAction(SectionItemType source);
        InjectFormType ChangeToInjectForm(SectionItemType source);


        DisplayedType ChangeToDisplayedItem(ListItemType source);

        //ListItemType ChangeToListItem
        //ListItemType ChangeToListItemResponse
        //SectionItemType ChangeToSection()
        //ChangeToButtonAction
        //ChangeToInjectForm
        //etc.


        //Question
        SectionItemType ChangeToSection(QuestionItemType source);
        DisplayedType ChangeToDisplayedItem(QuestionItemType source);


    }
    public interface IExtensionBase
    {
        bool HasExtensionBaseMembers() //Has Extension, Property or Comment sub-elements
        {
            var item = this as ExtensionBaseType;
            if (item?.Property?.Count() > 0)
            {
                foreach (var n in item.Property)
                { if (n != null) return true; }
            }
            if (item?.Comment?.Count() > 0)
            {
                foreach (var n in item.Comment)
                { if (n != null) return true; }
            }
            if (item?.Extension?.Count() > 0)
            {
                foreach (var n in item.Extension)
                { if (n != null) return true; }
            }
            return false;
        }
        ExtensionType AddExtension(int insertPosition = -1)
        {
            var ebtParent = this as ExtensionBaseType;
            var e = new ExtensionType(ebtParent);
            if (ebtParent.Extension == null) ebtParent.Extension = new List<ExtensionType>();
            var count = ebtParent.Extension.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            ebtParent.Extension.Insert(insertPosition, e);
            return e;
        }
        CommentType AddComment(int insertPosition = -1)
        {
            var ebtParent = this as ExtensionBaseType;
            if (ebtParent.Comment == null) ebtParent.Comment = new List<CommentType>();
            CommentType ct = null;
            var count = ebtParent.Comment.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            ebtParent.Comment.Insert(insertPosition, ct);  //return new empty Comment object for caller to fill
            return ct;
        }
        PropertyType AddProperty(int insertPosition = -1)
        {
            var ebtParent = this as ExtensionBaseType;
            var prop = new PropertyType(ebtParent);
            if (ebtParent.Property == null) ebtParent.Property = new List<PropertyType>();
            var count = ebtParent.Property.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            ebtParent.Property.Insert(insertPosition, prop);

            return prop;
        }


    }
    /// <summary>
    /// Move and Remove methods for Comment, Extension and Property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMoveRemove  //Used on BaseType only; IExtensionBaseTypeMember has some custom methods but they do not handle Node dictionaries or is-move-allowed testing
    {
        public bool Remove();
        public bool Move(BaseType targetProperty, out List<string> errList, int newListIndex = -1);
        public bool IsMoveAllowed(BaseType targetProperty, out List<string> errList, int newListIndex = -1);


    }
    public interface IExtensionBaseTypeMember : IMoveRemove //Used on Extension, Property, Comment
    {
        //!+TODO: Handle Dictionary updates
        bool MoveI(ExtensionType extension, ExtensionBaseType ebtTarget, int newListIndex = -1)
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
        bool MoveI(CommentType comment, ExtensionBaseType ebtTarget, int newListIndex)
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
        bool MoveI(PropertyType property, ExtensionBaseType ebtTarget, int newListIndex)
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
    public interface IDisplayedTypeMember { } //LinkType, BlobType, ContactType, CodingType, EventType, OnEventType, PredGuardType
    public interface IResponse: IVal //marks LIR and QR
    {
        //UnitsType AddUnits(ResponseFieldType rfParent);
        UnitsType AddUnits(ResponseFieldType rfParent)
        {
            UnitsType u = new UnitsType(rfParent);
            rfParent.ResponseUnits = u;
            return u;
        }

        void RemoveUnits(ResponseFieldType rfParent) => rfParent.ResponseUnits = null;
        BaseType DataTypeObject { get; set; }
        RichTextType AddTextAfterResponse { get; set; }
    }
    public interface IResponseField: IVal
    {
        CallFuncActionType AddCallSetValue();
        ScriptCodeAnyType AddSetValue();
        
        
    }
    public interface IVal 
    {
        //Implemented by data types, which have a strongly-typed val attribute.  Not implemented by anyType, XML, or HTML  
        object Val { get; set; }
        string ValString { get; }

    } 
    public interface IValNumeric : IVal { decimal ValDec { get; set; } } //Implemented by numeric data types, which have a strongly-type val attribute.
    public interface IValDateTime : IVal { } //Implemented by DateTime data types, which have a strongly-type val attribute.
    public interface IValInteger : IVal { long ValLong { get; set; } } //Implemented by Integer data types, which have a strongly-type val attribute.  Includes byte, short, long, positive, no-positive, negative and non-negative types
    public interface IIdentifiers
    {
        string GetNewCkey() { throw new NotImplementedException(); }

    }
    public interface IAddCoding
    {
        CodingType AddCodedValue(DisplayedType dt, int insertPosition)
        {
            throw new NotImplementedException();
        }

        CodingType AddCodedValue(LookupEndPointType lep, int insertPosition)
        {
            throw new NotImplementedException();
        }
        UnitsType AddUnits(CodingType ctParent)
        {
            UnitsType u = new UnitsType(ctParent);
            ctParent.Units = u;
            return u;
        }
    }
    public interface IAddContact //(File)
    {
        ContactType AddContact(FileType ftParent, int insertPosition)
        {
            ContactsType c;
            if (ftParent.Contacts == null)
                c = AddContactsListToFileType(ftParent);
            else
                c = ftParent.Contacts;
            var ct = new ContactType(c);
            var count = c.Contact.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            c.Contact.Insert(insertPosition, ct);
            //TODO: Need to be able to add multiple people/orgs by reading the data source or ORM
            var p = (this as IAddPerson).AddPersonI(ct);
            var org = (this as IAddOrganization).AddOrganizationI(ct);

            return ct;
        }
        private ContactsType AddContactsListToFileType(FileType ftParent)
        {
            if (ftParent.Contacts == null)
                ftParent.Contacts = new ContactsType(ftParent);

            return ftParent.Contacts; //returns a .NET List<ContactType>

        }
    }
    public interface IAddOrganization {
        OrganizationType AddOganization();

        internal OrganizationType AddOrganizationI(ContactType contactParent)
        {
            var ot = new OrganizationType(contactParent);
            contactParent.Organization = ot;

            return ot;
        }
        internal OrganizationType AddOrganizationI(JobType jobParent)
        {
            var ot = new OrganizationType(jobParent);
            jobParent.Organization = ot;

            return ot;
        }
        internal OrganizationType AddOrganizationItemsI(OrganizationType ot)
        {
            throw new NotImplementedException();
        }

    }
    public interface IAddPerson
    {
        PersonType AddPerson();

        internal PersonType AddPersonI(ContactType contactParent)
        {

            var newPerson = new PersonType(contactParent);
            contactParent.Person = newPerson;

            AddPersonItems(newPerson);  //AddFillPersonItems?

            return newPerson;
        }
        internal PersonType AddPersonI(DisplayedType dtParent, int insertPosition)
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

            var newPerson = AddPersonI(newContact);

            return newPerson;
        }
        internal PersonType AddContactPersonI(OrganizationType otParent, int insertPosition)
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
            AddPersonItems(newPerson);

            var count = contactPersonList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            contactPersonList.Insert(insertPosition, newPerson);

            return newPerson;
        }
        protected virtual PersonType AddPersonItems(PersonType pt)  //AddFillPersonItems, make this abstract and move to subclass?
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
    public interface IEvent : IHasActionElseGroup  //Used for events (PredActionType)
    {
        PredEvalAttribValuesType AddAttributeVal()
        {
            var pgt = (PredActionType)this;
            var av = new PredEvalAttribValuesType(pgt);
            pgt.Items.Add(av);
            return av;
        }
        ScriptBoolFuncActionType AddScriptBoolFunc();
        CallFuncBoolActionType AddCallBoolFunction();
        MultiSelectionsActionType AddMultiSelections();
        SelectionSetsActionType AddSelectionSets();
        PredSelectionTestType AddSelectionTest();
        //PredAlternativesType AddItemAlternatives();
        RuleSelectMatchingListItemsType SelectMatchingListItems();
        PredGuardType AddGroup();
    }
    /// <summary>
    /// //Used for guards, e.g., SelectIf, DeselectIf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPredGuard //used by Guards on ListItem, Button
    {
        PredEvalAttribValuesType AddAttributeVal()
        {
            var pgt = (PredGuardType)this;
            var av = new PredEvalAttribValuesType(pgt);
            pgt.Items.Add(av);
            return av;
        }
        ScriptBoolFuncActionType AddScriptBoolFunc();
        CallFuncBoolActionType AddCallBoolFunction();
        MultiSelectionsActionType AddMultiSelections();
        PredSelectionTestType AddSelectionTest();
        PredGuardTypeSelectionSets AddSelectionSets();
        PredAlternativesType AddItemAlternatives();
        PredGuardType AddGroup();


    }
    public interface IRules 
    {
        //List<ExtensionBaseType> Items 
        RuleAutoActivateType AddAutoActivation();
         RuleAutoSelectType AddAutoSelection();
         PredActionType AddConditionalActions();
         CallFuncActionType AddExternalRule();
         ScriptCodeAnyType AddScriptedRule();
         RuleSelectMatchingListItemsType AddSelectMatchingListItems();
         ValidationType AddValidation();
    }
    public interface IHasConditionalActionsNode
    {
        PredActionType AddConditionalActionsNode();
    }
    public interface IHasParameterGroup
    {
        ParameterItemType AddParameterRefNode();
        ListItemParameterType AddListItemParameterRefNode();
        ParameterValueType AddParameterValueNode();
    }
    public interface IHasDataType_SType
    {
        DataTypes_SType AddDataTypes_SType();
    }
    public interface IHasDataType_DEType
    {
        DataTypes_DEType AddDataTypes_DEType();
    }
    public interface IHasActionsNode
    {
        ActionsType AddActionsNode()
        {
            var actions = new ActionsType((ExtensionBaseType)this);
            var p = this as PredActionType;
            if (p != null)
            {
                p.Actions = actions;
                return p.Actions;
            }
            else
            {
                var pe = this as EventType;
                if (pe != null)
                {
                    pe.Actions = actions;
                    return pe.Actions;
                }
            }
            throw new InvalidCastException("The parent node must be of type EventType or PredActionType");
        }
    }
    public interface IHasActionElseGroup : IActions, IHasElseNode
    {

    }
    public interface IHasElseNode
    {
        PredActionType AddElseNode()
        {
            if (this is null) return null;
            var elseNode = new PredActionType((BaseType)this);

            switch (this)
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
    public interface IActions
    {
        //ExtensionBaseType[] Items
        public ActActionType AddActAction(int insertPosition = -1)
        {
            //var p = (ActionsType)this;
            //var act = new ActActionType(p);
            //return (ActActionType)ArrayAddReturnItem(p.Items, act);
            return AddAction(new ActActionType((ActionsType)this), insertPosition);
        }
        public RuleSelectMatchingListItemsType AddActSelectMatchingListItems(int insertPosition = -1)
        {
            return AddAction(new RuleSelectMatchingListItemsType((ActionsType)this), insertPosition);
        }
        //public abstract ActSetPropertyType AddSetProperty(ActionsType at);
        public ActAddCodeType AddActAddCode(int insertPosition = -1)
        {
            return AddAction(new ActAddCodeType((ActionsType)this), insertPosition);
        }
        //public abstract ActSetValueType AddSetValue(ActionsType at);
        public ActInjectType AddActInject(int insertPosition = -1)
        {
            return AddAction(new ActInjectType((ActionsType)this), insertPosition);
        }
        public CallFuncActionType AddActShowURL(int insertPosition = -1)
        {
            return AddAction(new CallFuncActionType((ActionsType)this), insertPosition);
        }
        public ActSaveResponsesType AddActSaveResponses(int insertPosition = -1)
        {
            return AddAction(new ActSaveResponsesType((ActionsType)this), insertPosition);
        }
        public ActSendReportType AddActSendReport(int insertPosition = -1)
        {
            return AddAction(new ActSendReportType((ActionsType)this), insertPosition);
        }
        public ActSendMessageType AddActSendMessage(int insertPosition = -1)
        {
            return AddAction(new ActSendMessageType((ActionsType)this), insertPosition);
        }
        public ActSetAttributeType AddActSetAttributeValue(int insertPosition = -1)
        {
            return AddAction(new ActSetAttributeType((ActionsType)this), insertPosition);
        }
        public ActSetAttrValueScriptType AddActSetAttributeValueScript(int insertPosition = -1)
        {
            return AddAction(new ActSetAttrValueScriptType((ActionsType)this), insertPosition);
        }
        public ActSetBoolAttributeValueCodeType AddActSetBoolAttributeValueCode(int insertPosition = -1)
        {
            return AddAction(new ActSetBoolAttributeValueCodeType((ActionsType)this), insertPosition);
        }
        public ActShowFormType AddActShowForm(int insertPosition = -1)
        {
            return AddAction(new ActShowFormType((ActionsType)this), insertPosition);
        }
        public ActShowMessageType AddActShowMessage(int insertPosition = -1)
        {
            return AddAction(new ActShowMessageType((ActionsType)this), insertPosition);
        }
        public ActShowReportType AddActShowReport(int insertPosition = -1)
        {
            return AddAction(new ActShowReportType((ActionsType)this), insertPosition);
        }
        public ActPreviewReportType AddActPreviewReport(int insertPosition = -1)
        {
            return AddAction(new ActPreviewReportType((ActionsType)this), insertPosition);
        }
        public ActValidateFormType AddActValidateForm(int insertPosition = -1)
        {
            return AddAction(new ActValidateFormType((ActionsType)this), insertPosition);
        }
        public ScriptCodeAnyType AddActRunCode(int insertPosition = -1)
        {
            return AddAction(new ScriptCodeAnyType((ActionsType)this), insertPosition);
        }
        public CallFuncActionType AddActCallFunction(int insertPosition = -1)
        {
            return AddAction(new CallFuncActionType((ActionsType)this), insertPosition);
        }
        public PredActionType AddActConditionalGroup(int insertPosition = -1)
        {
            return AddAction(new PredActionType((ActionsType)this), insertPosition);
        }

        private T AddAction<T>(T action, int insertPosition = -1) where T : ExtensionBaseType
        {
            var p = (ActionsType)this;
            var lst = (IList<BaseType>)p.Items;
            int c = lst.Count();
            if (insertPosition > -1 && (insertPosition < c)) lst.Insert(insertPosition, action);
            else lst.Insert(c, action);
            return action;
        }

    }
    public interface IActionsMember : IMoveRemove
        //used from within an individual action object; "this" refers to the action object itself.  Its parent is the Actions element (ActionsType)
    {

        /// <summary>
        /// Helper function to change the order of actions (e.g., ActSendMessage) inside the Actions element
        /// All movements are within the same parent array (ExtensionBaseType[] Items)
        /// The Nodes, ParentNodes and ChildNodes Dictionaries will be updated as well.
        /// </summary>
        /// <param name="action">ActionsType object that becomes the Actions element</param>
        /// <param name="errList">If the retirn bool value is false, errList contains the errors encountered.</param>
        /// <param name="newListIndex">Optional list location for the current action.  
        /// If not provided or < 0, or > than the size of the Items list,the action will be placeed last in the Items list.
        /// </param>
        /// <returns></returns>
        public bool Move(ExtensionBaseType action, out List<string> errList, int newListIndex = -1)
        {
            var par = ((BaseType)this).ParentNode;
            var items = ((ActionsType)this).Items;
            return Move(this as BaseType, out errList, newListIndex);
        }
    }
    public interface ISendMessage_Report
    {
        //List<ExtensionBaseType> Items
        //Supports ActSendMessageType and ActSendReportType
        EmailAddressType AddEmail();
        PhoneNumberType AddFax();
        CallFuncActionType AddWebService();
    }
    public interface IButtonItem
    {
        //List<EventType> Items
        EventType AddOnClick();
    }
    public interface ICallFuncBase
    {
        //anyURI_Stype Item (choice)
        anyURI_Stype AddFunctionURI();
        anyURI_Stype AddLocalFunctionName();
        

        //List<ExtensionBaseType> Items
        ListItemParameterType AddListItemParameterRef();
        ParameterItemType AddParameterRef();
        ParameterValueType AddParameterValue();

    }
    public interface IScriptBoolFuncAction
    {
        //ExtensionBaseType[] Items 
         ActionsType AddActions();
         PredActionType AddConditionalActions();
         PredActionType AddElse();
    }
    public interface ICallFuncBoolAction: ICallFuncBase, IScriptBoolFuncAction
    {
        //ExtensionBaseType[] Items1
        //see IScriptBoolFuncAction, which is identical except that this interface implementation must use "Item1", not "Item"
        //Implementations using Item1:
        ActionsType  AddActionsI();
        PredActionType AddConditionalActionsI();
        PredActionType AddElseI();

    }
    public interface IValidationTests
    {
        //List<FuncBoolBaseType> Items        
         PredAlternativesType AddItemAlternatives();
         ValidationTypeSelectionSets AddSelectionSets();
         ValidationTypeSelectionTest AddSelectionTest();
    }
    public interface INavigate //apply only to BaseType
    {
        BaseType GetPreviousSib() => SdcUtil.GetPrevSib((BaseType)this);
        BaseType GetNextSib() => SdcUtil.GetNextSib((BaseType)this);
        BaseType GetPrevious() => SdcUtil.PrevElement((BaseType)this);
        BaseType GetNext() => SdcUtil.NextElement((BaseType)this);
        BaseType GetFirstChild() => SdcUtil.GetFirstChild((BaseType)this);
        BaseType GetLastChild() => SdcUtil.GetLastChild((BaseType)this);
        BaseType GetLastDescendant() => SdcUtil.GetLastDescendant((BaseType)this);

        PropertyInfoMetadata GetPropertyInfo() => SdcUtil.GetPropertyInfo((BaseType)this);
        bool IsItemChangeAllowed<T>(T target) where T : notnull, IdentifiedExtensionType
            => SdcUtil.IsItemChangeAllowed((IdentifiedExtensionType)this, target);
        bool ReOrder() { throw new NotImplementedException(); }

    }
    public interface IClone
    {
        BaseType CloneSubtree();
        BaseType CloneSubtree(BaseType top)
        { throw new NotImplementedException(); }
    }
    public interface IBlob
    {   //DisplayedItem.BlobType
        //Uses Items types choice
        bool AddBinaryMedia();
        bool AddBlobURI();
    }
    public interface IInjectForm
    {   //ChildItems.InjectForm - this is mainly useful for a DEF injecting items based on the InjectForm URL
        //Item types choice under ChildItems
        FormDesignType AddFormDesign();
        QuestionItemType AddQuestion();
        SectionItemType AddSection();
    }
    public interface IHTMLPackage
    {//On SDCPackage.HTMLPackage
        base64Binary_Stype AddHTMLbase64();
    }
    public interface IRegistrySummary
    {
        //BaseType[] Items
        //Attach to Admin.RegistryData as OriginalRegistry and/or CurrentRegistry
        
         ContactType AddContact();
         FileType AddManual();
         string_Stype AddReferenceStandardIdentifier();
         InterfaceType AddRegistryInterfaceType();
         string_Stype AddRegistryName();
         FileType AddRegistryPurpose();
         FileType AddServiceLevelAgreement();

    }
}
