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
    {   //FormDesignType CreateForm(bool addHeader, bool addFooter, string formID, string lineage, string version, string fullURI);
        //FormDesignType CreateFormFromTemplatePath(string path, string formID, string lineage, string version, string fullURI);
        //FormDesignType CreateFormFromTemplateXML(string xml, string formID, string lineage, string version, string fullURI);
        //bool RemoveFormFromPackage(RetrieveFormPackageType pkg, FormDesignType form); 
    } //Empty 
    public interface IDataElement : ITopNode //TODO:
    { } //Empty 
    public interface IDemogForm : ITopNode //TODO:
    { } //Empty 
    public interface IMap : ITopNode //TODO:
    { } //Empty 
    public interface IFormDesign : ITopNode //TODO:
    { } //Empty 
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
       
        //Remove all child nodes 


        //QR AddChildQR(string id = "", int insertPosition = -1);
        //QS AddChildQS(string id = "", int insertPosition = -1);
        //QM AddChildQM(string id = "", int insertPosition = -1);
        //QL AddChildQL(string id = "", int insertPosition = -1);
    } 
    public interface IChildItemsMember<Tchild>  
            where Tchild : IdentifiedExtensionType, IChildItemsMember<Tchild>
    {//Marks SectionItemType, QuestionItemType, DisplayedType, ButtonItemType, InjectFormType    
    } //Empty 
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
public interface ISection 
    {//may delete the change types
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
{    //Use this as part of a static DisplayedTypeChanges utility class; 
    //Do NOT add them to DisplayedType, since they would be inherited by Section, Question andd InjectForm
//may delete this
} //Empty
public interface IExtensionBase
{ }//Empty
public interface IExtensionBaseTypeMember  
{//Used on Extension, Property, Comment 
    }//Empty
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

    #region Values    
    public interface IVal
    {
        //Implemented by data types, which have a strongly-typed val attribute.  Not implemented by anyType, XML, or HTML  
        object Val { get; set; }
        string ValString { get; }
    }
    public interface IValNumeric : IVal { decimal ValDec { get; set; } } //Implemented by numeric data types, which have a strongly-type val attribute.
    public interface IValDateTime : IVal { } //Implemented by DateTime data types, which have a strongly-type val attribute.
    public interface IValInteger : IVal
    { long ValLong { get; set; } } //Implemented by Integer data types, which have a strongly-type val attribute.  Includes byte, short, long, positive, no-positive, negative and non-negative types
    #endregion
    
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
public interface IEvent : IHasActionElseGroup  
{//Used for events (PredActionType) 
}//Empty
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
public interface IActionsMember

{//used from within an individual action object; "this" refers to the action object itself.  Its parent is the Actions element (ActionsType)

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

}//Empty
public interface IRegistrySummary
{
    //BaseType[] Items
    //Attach to Admin.RegistryData as OriginalRegistry and/or CurrentRegistry     
}//Empty

}
