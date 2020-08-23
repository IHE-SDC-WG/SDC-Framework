using System;
using System.Collections.Generic;
using System.Data;
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
    } //Empty 
    public interface IPackage : ITopNode //TODO:
    { } //Empty 
    public interface IDataElement : ITopNode //TODO:
    { } //Empty 
    public interface IDemogForm : ITopNode //TODO:
    { } //Empty 
    public interface IMap : ITopNode //TODO:
    { } //Empty 
    public interface IFormDesign : ITopNode, IMoveRemove //TODO:
    {

    } //Empty 
    public interface IRetrieveFormPackage
    {

    } //Empty 
    /// <summary>
    /// This interface is applied to the partial classes that can have a ChildItems element.
    /// These are Section, Question and ListItem.  
    /// This interface is require to support generic classes that must handle the creation ofthe 
    /// ChildItems element, which holds List of type IdentifiedItemType
    /// </summary>
    /// 

    public interface IChildItemsParent<T> //implemented by items that can have a ChildItems node.
        where T : BaseType, IChildItemsParent<T>
    {
        public ChildItemsType ChildItemsNode { get; set; }
        //public SectionItemType AddChildSection(string id, int insertPosition);
        //internal static SectionItemType AddChildSectionI(T T_Parent, string id, int insertPosition)
        //{
        //    var childItems = AddChildItemsNode(T_Parent as T);
        //    var childItemsList = childItems.ChildItemsList;
        //    var sNew = new SectionItemType(childItems, id);
        //    var count = childItemsList.Count;
        //    if (insertPosition < 0 || insertPosition > count) insertPosition = count;
        //    childItemsList.Insert(insertPosition, sNew);

        //    return sNew;
        //}
        //public QuestionItemType AddChildQuestion(QuestionEnum qType, string id, int insertPosition = -1);
        //internal static QuestionItemType AddChildQuestionI(T T_Parent, QuestionEnum qType, string id, int insertPosition = -1)
        //{
        //    var childItems = AddChildItemsNode(T_Parent);
        //    var childItemsList = childItems.ChildItemsList;
        //    var qNew = new QuestionItemType(childItems, id);
        //    ListFieldType lf;
        //    var count = childItemsList.Count;
        //    if (insertPosition < 0 || insertPosition > count) insertPosition = count;
        //    childItemsList.Insert(insertPosition, qNew);

        //    switch (qType)
        //    {
        //        case QuestionEnum.QuestionSingle:
        //            AddListToListField(AddListFieldToQuestion(qNew));
        //            break;
        //        case QuestionEnum.QuestionMultiple:
        //            AddListToListField(AddListFieldToQuestion(qNew));
        //            qNew.ListField_Item.maxSelections = 0;
        //            break;
        //        case QuestionEnum.QuestionFill:
        //            AddQuestionResponseField(qNew);
        //            break;
        //        case QuestionEnum.QuestionLookupSingle:
        //            lf = AddListFieldToQuestion(qNew);
        //            AddEndpointToListField(lf);
        //            break;
        //        case QuestionEnum.QuestionLookupMultiple:
        //            lf = AddListFieldToQuestion(qNew);
        //            AddEndpointToListField(lf);
        //            break;
        //        default:
        //            throw new NotSupportedException($"{qType} is not supported");
        //    }

        //    return qNew;
        //}
        //public DisplayedType AddChildDisplayedItem(string id, int insertPosition = -1);
        //internal static DisplayedType AddChildDisplayedItemI(T T_Parent, string id, int insertPosition = -1)
        //{
        //    var childItems = AddChildItemsNode(T_Parent);
        //    var childItemsList = childItems.ChildItemsList;
        //    var dNew = new DisplayedType(childItems, id);
        //    var count = childItemsList.Count;
        //    if (insertPosition < 0 || insertPosition > count) insertPosition = count;
        //    childItemsList.Insert(insertPosition, dNew);

        //    return dNew;
        //}
        //public ButtonItemType AddChildButtonAction(string id, int insertPosition = -1);
        //internal static ButtonItemType AddChildButtonActionI(T T_Parent, string id, int insertPosition = -1)
        //{
        //    var childItems = AddChildItemsNode(T_Parent);
        //    var childItemsList = childItems.ChildItemsList;
        //    var btnNew = new ButtonItemType(childItems, id);
        //    var count = childItemsList.Count;
        //    if (insertPosition < 0 || insertPosition > count) insertPosition = count;
        //    childItemsList.Insert(insertPosition, btnNew);

        //    // TODO: Add AddButtonActionTypeItems(btnNew);
        //    return btnNew;
        //}
        //public InjectFormType AddChildInjectedForm(string id, int insertPosition = -1);
        //internal static InjectFormType AddChildInjectedFormI(T T_Parent, string id, int insertPosition = -1)
        //{
        //    var childItems = AddChildItemsNode(T_Parent);
        //    var childItemsList = childItems.ChildItemsList;
        //    var injForm = new InjectFormType(childItems, id);
        //    var count = childItemsList.Count;
        //    if (insertPosition < 0 || insertPosition > count) insertPosition = count;
        //    childItemsList.Insert(insertPosition, injForm);
        //    //TODO: init this InjectForm object

        //    return injForm;
        //}
        //internal static bool HasChildItems(T T_Parent)
        //{
        //    {
        //        if (T_Parent?.ChildItemsNode?.ChildItemsList != null)
        //        {
        //            foreach (var n in T_Parent.ChildItemsNode.ChildItemsList)
        //            { if (n != null) return true; }
        //        }
        //    }
        //    return false;
        //}
        //internal static ChildItemsType AddChildItemsNode(T T_Parent)
        //{
        //    ChildItemsType childItems = null;  //this class contains an "Items" list
        //    if (T_Parent == null)
        //        throw new ArgumentNullException("The T_Parent object was null");
        //    //return childItems; 
        //    else if (T_Parent.ChildItemsNode == null)
        //    {
        //        childItems = new ChildItemsType(T_Parent);
        //        T_Parent.ChildItemsNode = childItems;  //This may be null for the Header, Body and Footer  - need to check this
        //    }
        //    else //(T_Parent.ChildItemsNode != null)
        //        childItems = T_Parent.ChildItemsNode;

        //    if (childItems.ChildItemsList == null)
        //        childItems.ChildItemsList = new List<IdentifiedExtensionType>();

        //    return childItems;
        //}

        //internal static ResponseFieldType AddQuestionResponseField(QuestionItemType qParent)
        //{
        //    var rf = new ResponseFieldType(qParent);
        //    qParent.ResponseField_Item = rf;

        //    return rf;
        //}
        //internal static LookupEndPointType AddEndpointToListField(ListFieldType listFieldParent)
        //{
        //    if (listFieldParent.List == null)
        //    {
        //        var lep = new LookupEndPointType(listFieldParent);
        //        listFieldParent.LookupEndpoint = lep;
        //        return lep;
        //    }
        //    else throw new InvalidOperationException("Can only add LookupEndpoint to ListField if List object is not present");
        //}
        //internal static ListType AddListToListField(ListFieldType listFieldParent)
        //{
        //    ListType list;  //this is not the .NET List class; It's an answer list
        //    if (listFieldParent.List == null)
        //    {
        //        list = new ListType(listFieldParent);
        //        listFieldParent.List = list;
        //    }
        //    else list = listFieldParent.List;

        //    //The "list" item contains a list<DisplayedType>, to which the ListItems and ListNotes (DisplayedItems) are added.
        //    if (list.QuestionListMembers == null)

        //        list.QuestionListMembers = new List<DisplayedType>();

        //    return list;
        //}
        //internal static ListFieldType AddListFieldToQuestion(QuestionItemType qParent)
        //{
        //    if (qParent.ListField_Item == null)
        //    {
        //        var listField = new ListFieldType(qParent);
        //        qParent.ListField_Item = listField;
        //    }

        //    return qParent.ListField_Item;
        //}


        //Remove all child nodes 


        //QR AddChildQR(string id = "", int insertPosition = -1);
        //QS AddChildQS(string id = "", int insertPosition = -1);
        //QM AddChildQM(string id = "", int insertPosition = -1);
        //QL AddChildQL(string id = "", int insertPosition = -1);

    } 
    public interface IChildItemsMember<Tchild>  //Marks SectionItemType, QuestionItemType, DisplayedType, ButtonItemType, InjectFormType
            where Tchild : IdentifiedExtensionType, IChildItemsMember<Tchild>
    {    } //Empty 
    public interface IQuestionItem : IQuestionList
    { } //Empty 

    //bool ConvertToButton(); //abort if LIs or children present

    //convert type to QR
    //Convert to S, DI    (must first delete List or ResponseField present)
    //Convert QR to QS    (delete ResponseField, add List, set maxSelections)
    //Convert QR to QM    (delete ResponseField, add List, set maxSelections)
    //LookupEndPointType AddLookupEndpoint(ListFieldType lfParent);  //should be part of AddChildQL code;
    //CanConvert (to Type)


    public interface IQuestionList //may be implemented by Q, List, LI and perhaps DI (if inside a list)
    { } //Empty 

//public interface IQuestionListMember : IQuestionBuilder //decorates LI/LIR and DI
//{
//    //for DI, make sure parent is a ListType object
//    //bool Remove(bool removeDecendants = false);
//    //bool Move(QuestionItemType target = null, bool moveAbove = false, bool testOnly = false);
//    //bool Move(ListItemType target = null, bool moveAbove = false, bool testOnly = false);
//    //ListItemType ConvertToLI(bool testOnly = false);
//    //DisplayedType ConvertToDI(bool testOnly = false); //abort if children of LI are present
//    //ListItemType ConvertToLIR(bool testOnly = false);


//    //for DI, make sure parent is a ListType object
//    bool Remove(bool removeDecendants); 
//    bool RemoveI(bool removeDecendants) 
//    { throw new NotImplementedException(); }
//    //bool Move(QuestionItemType target, bool moveAbove, bool testOnly) { throw new NotImplementedException(); }
//    //bool Move(ListItemType target, bool moveAbove, bool testOnly) { throw new NotImplementedException(); }
//    /// <summary>
//    /// Move a ListItem or DisplayedItem within the same List
//    /// </summary>
//    /// <returns>returns true if operation succeeded    
//    /// </returns>
//    bool MoveInList(out string err, int newListIndex = -1);
//    bool MoveInListI(out string err, int newListIndex = -1)
//    {
//        var qlm = this as BaseType;  //"this" is either a DI or LI
//        var list = (ListType)(qlm)?.ParentNode;
//        return qlm.Move(list, out err, 6);
//    }
//    //bool Move(List<BaseType> targetProperty, out List<string> errList, int newListIndex = -1) //DI and LI must have a parent property of ListType (DI or LI) of ChildItems (DI)
//    //{
//    //    var qlm = this as BaseType;  //"this" is either a DI or LI
//    //    var list = (ListType)(qlm)?.ParentNode;
//    //    return qlm.Move(targetProperty, qlm, out errList, newListIndex);
//    //}

//    /// <summary>
//    /// Move a ListItem or DisplayedItem to another List
//    /// </summary>
//    /// <param name="targetList"></param>
//    /// <param name="errList"></param>
//    /// <param name="newListIndex"></param>
//    /// <returns>returns true if operation succeeded    </returns>
//    bool MoveToList(ListType targetList, out string err, int newListIndex = -1);
//    bool MoveToListI(ListType targetList, out string err, int newListIndex = -1)
//    {
//        var qlm = this as BaseType;  //"this" is either a DI or LI
//        return qlm.Move(targetList, out err, newListIndex);
//    }

//    ListItemType ConvertToLI(bool testOnly);
//    ListItemType ConvertToLI_I(bool testOnly) 
//    { throw new NotImplementedException(); }
//    DisplayedType ConvertToDI(bool testOnly);
//    DisplayedType ConvertToDI_I(bool testOnly)
//    { throw new NotImplementedException(); } //abort if children of LI are present
//    ListItemType ConvertToLIR(bool testOnly);
//    ListItemType ConvertToLIR_I(bool testOnly)
//    { throw new NotImplementedException(); }
//    bool IsMoveAllowedToList(QuestionItemType target, out string error);
//    bool IsMoveAllowedToListI(QuestionItemType target, out string error)
//    {
//        error = "";

//        if (
//            !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionSingle) &&
//            !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionMultiple) 
//            )
//        { error = "A Question target must be a QuestionSingle or QuestionMultiple"; return false; }

//        return true;
//    }
//    bool MoveInList(DisplayedType source, QuestionItemType target, bool moveAbove);
//    bool MoveInListI(DisplayedType source, QuestionItemType target, bool moveAbove)
//    {
//        if (source is null) return false;
//        if (source is RepeatingType) return false; //S, Q are illegal in the list
//        if (source is ButtonItemType) return false; //B is illegal in the list

//        if (target is null) target = source.ParentNode?.ParentNode?.ParentNode as QuestionItemType; //LI-->List-->ListField-->Q  - see if we can capture a Question from the source node
//        if (target is null) return false;

//        List<BaseType> sourceList = (source.ParentNode as ListType)?.Items?.ToList<BaseType>();//guess that the sourrce node is inside a List
//        if (sourceList is null) sourceList = (source.ParentNode as ChildItemsType)?.ChildItemsList?.ToList<BaseType>();//try again - guess that source node is inside a ChildItems node
//        if (sourceList is null) return false;//both guesses failed - this is probably a disconnected node, and we can't work with that.

//        if (target.ListField_Item is null) AddListToListField(AddListFieldToQuestion(target));  //make sure there is a ist instantiated on this target Question; if not, then create it.          
//        var targetList = target.ListField_Item.List.Items;
//        if (targetList is null) return false;  //unkown problem getting Question-->ListField-->List

//        var indexSource = sourceList.IndexOf(source);
//        int index = targetList.Count;
//        sourceList.Remove(source);
//        targetList.Insert(index, source);
//        if (targetList[index] == source)
//        {
//            source.TopNode.ParentNodes[source.ObjectGUID] = target;
//            return true;
//        }

//        //Error - the source item is now disconnected from the list.  Lets add it back to the end of the list.
//        sourceList.Insert(indexSource, source); //put source back where it came from; the move failed
//        return false;

//    }
//    bool MoveInList(DisplayedType source, DisplayedType target, bool moveAbove);
//    bool MoveInListI(DisplayedType source, DisplayedType target, bool moveAbove) //target must be a LI or DI (not a RepeatingType); need to address Nodes dictionary updates
//    {
//        //this function allows dropping items inside a QS o QM list to rearrange the list
//        //prevent illegal operationss
//        if (source is null) return false;
//        if (source is RepeatingType) return false; //S, Q (RepeatingTypes) are illegal in the Q's list
//        if (target is RepeatingType) return false; //S, Q (RepeatingTypes) are illegal in the Q's list
//        if (source is ButtonItemType) return false; //B is illegal in the Q's list
//        if (target is ButtonItemType) return false; //B is illegal in the Q's list

//        List<BaseType> sourceList = (source.ParentNode?.ParentNode as ListType)?.Items?.ToList<BaseType>();
//        if (sourceList is null) sourceList = (source.ParentNode as ChildItemsType)?.ChildItemsList?.ToList<BaseType>();
//        if (sourceList is null) return false;

//        var qTarget = target.ParentNode?.ParentNode?.ParentNode as QuestionItemType;
//        if (qTarget is null) return false;  //we did not get a Q object, so we are not moving  a node into a Q List

//        var targetList = qTarget.ListField_Item?.List?.Items;


//        if (targetList is null) return false;

//        var indexSource = sourceList.IndexOf(source);
//        sourceList.Remove(source);

//        //Determine where to insert the node in the list, based on the location of the existing Lis
//        var index = targetList.IndexOf(target);
//        if (index < 0) index = targetList.Count; //target node not found in list, so insert source at the end of the list; this should never execute
//        if (moveAbove) index--;

//        targetList.Insert(index, source);
//        if (targetList[index] == source)
//        {
//            source.TopNode.ParentNodes[source.ObjectGUID] = target;
//            return true;
//        }

//        //Error - the source item is now disconnected from the list.  Lets add it back to the end of the list.
//        sourceList.Insert(indexSource, source); //put source back where it came from; the move failed
//        return false;
//    }



//} //Implemented on ListItem and DisplayedItem
public interface IListField
{
    ListType List { get; set; }
    LookupEndPointType LookupEndpoint { get; set; }

}
public interface IQuestionBase
{
    ResponseFieldType ResponseField_Item { get; set; }
}
public interface IListItem
    { } //Empty 
public interface IQuestionBuilder
    {   } //Empty 
public interface ISection //may delete the change types
    {
    //QuestionItemType ChangeToQuestionMultiple();
    //QuestionItemType ChangeToQuestionSingle();
    //QuestionItemType ChangeToQuestionResponse();
    //QuestionItemType ChangeToQuestionLookup();
    //DisplayedType ChangeToDisplayedItem();
} //Empty 
public interface IButtonItem
{   }   //Empty 
public interface IInjectForm
{ }   //Empty 
public interface IDisplayedTypeMember
{ //LinkType, BlobType, ContactType, CodingType, EventType, OnEventType, PredGuardType
} //Empty
public interface IBlob
{ }//Empty
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


} //may delete this
public interface IExtensionBase
{ }//Empty
public interface IExtensionBaseTypeMember : IMoveRemove //Used on Extension, Property, Comment
{ }//Empty
public interface IIdentifiedExtensionType
{ }//Empty
public interface IMoveRemove
{//Used on BaseType only; IExtensionBaseTypeMember has some custom methods but they do not handle Node dictionaries or is-move-allowed testing 
}//Empty
public interface INavigate
{//apply only to BaseType 
}//Empty
public interface IResponse : IVal //marks LIR and QR
{ }//Empty
public interface IResponseField : IVal
{ }//Empty


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
{ }//Empty
public interface IAddCoding
{ }//Empty
public interface IAddContact //(File)
{ }//Empty
public interface IAddOrganization
{
}//Empty
public interface IAddPerson
{ }//Empty
public interface IEvent : IHasActionElseGroup  //Used for events (PredActionType)
{ }//Empty
public interface IPredGuard
{   //used by Guards on ListItem, Button 
}//Empty
public interface IRule
{ }//Empty
public interface IHasConditionalActionsNode
{ }//Empty
public interface IHasParameterGroup
{ }//Empty
public interface IHasDataType_SType
{ }//Empty
public interface IHasDataType_DEType
{ }//Empty
public interface IHasActionsNode
{
}//Empty
public interface IHasActionElseGroup : IActions, IHasElseNode
{

}//Empty
public interface IHasElseNode
{ }//Empty
public interface IActions
{ } //Empty
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
    //public bool Move(ExtensionBaseType action, int newListIndex = -1)
    //{
    //    var par = ((BaseType)this).ParentNode;
    //    var items = ((ActionsType)this).Items;
    //    return Move(this as BaseType, newListIndex);
    //}
}//Empty
public interface ISendMessage_Report
{

}  //Empty  
public interface ICallFuncBase
{


}//Empty
public interface IScriptBoolFuncAction
{

}//Empty
public interface ICallFuncBoolAction : ICallFuncBase, IScriptBoolFuncAction
{
    //ExtensionBaseType[] Items1
    //see IScriptBoolFuncAction, which is identical except that this interface implementation must use "Item1", not "Item"
    //Implementations using Item1:

}//Empty
public interface IValidationTests
{
    //List<FuncBoolBaseType> Items        

}//Empty
public interface IClone
{

}//Empty
public interface IHtmlPackage
{//On SDCPackage.HTMLPackage

}
public interface IRegistrySummary
{
    //BaseType[] Items
    //Attach to Admin.RegistryData as OriginalRegistry and/or CurrentRegistry     
}//Empty

}
