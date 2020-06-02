using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks.Sources;
using Newtonsoft.Json;

//using SDC;
namespace SDC.Schema
{
    /// <summary>
    /// This interface is applied to the partial classes that can have a ChildItems element.
    /// These are Section, Question and ListItem.  
    /// This interface is require to support generic classes that must handle the creation ofthe 
    /// ChildItems element, which holds List of type IdentifiedItemType
    /// </summary>
    public interface IChildItemsParent  //implemented by items that can have a ChildItems node.
    {
        ChildItemsType ChildItemsNode { get; set; }
        //C AddChildItem<P, C>(P parent, string childID = "", int insertPosition = -1) where P : IParent where C : IChildItem;
        //bool RemoveChildItem<C>(C childItem) where C : IChildItem;
        //bool MoveChildItem<P, C>(C childItem, int newPosition, P targetParent) where P : IParent where C : IChildItem;

        SectionItemType AddChildSection(string childID = "", int insertPosition = -1);
        QuestionItemType AddChildQuestion(QuestionEnum qType, string childID = "", int insertPosition = -1);
        DisplayedType AddChildDisplayedItem(string childID = "", int insertPosition = -1);
        InjectFormType AddChildInjectedForm(string childID = "", int insertPosition = -1);
        ButtonItemType AddChildButtonAction(string childID = "", int insertPosition = -1);
        bool HasChildItems();

        //Remove all child nodes 


        //QR AddChildQR(string id = "", int insertPosition = -1);
        //QS AddChildQS(string id = "", int insertPosition = -1);
        //QM AddChildQM(string id = "", int insertPosition = -1);
        //QL AddChildQL(string id = "", int insertPosition = -1);

    }
    public interface IChildItemsMember //Marks SectionItemType, QuestionItemType, DisplayedType, ButtonItemType, InjectFormType
    {
        //bool Remove<T>() where T : IdentifiedExtensionType, IUnderChildItem;
        bool Remove();
        bool Move<T>(T target = null, int newListIndex = -1) where T : DisplayedType, IChildItemsParent;
        //CanRemove
        //CanMove (target ChildItems)
        //ChildCount

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
    }
    public interface IQuestionListMember
    {
        //for DI, make sure parent is a ListType object
        bool Remove(bool removeDecendants = false);
        bool Move(QuestionItemType target = null, bool moveAbove = false, bool testOnly = false);
        bool Move(ListItemType target = null, bool moveAbove = false, bool testOnly = false);
        ListItemType ConvertToLI(bool testOnly = false);
        DisplayedType ConvertToDI(bool testOnly = false); //abort if children of LI are present
        ListItemType ConvertToLIR(bool testOnly = false);

    } //Implemented on ListItem and DisplayedItem

    public interface IListField
    {
        ListType List { get; set; }
        LookupEndPointType LookupEndpoint { get; set; }

    }
    public interface IQuestionBase
    {
        ListFieldType ListField_Item { get; set; }
        ResponseFieldType ResponseField_Item { get; set; }
    }
    public interface IListItem
    {
        ListItemResponseFieldType AddListItemResponseField();
    }
    public interface IExtensionBase
    { 
        bool HasExtensionBaseMembers(); //Has Extension, Property or Comment sub-elements
        CommentType AddComment(int insertPosition = -1);
        ExtensionType AddExtension(int insertPosition = -1);
        PropertyType AddProperty(int insertPosition = -1);
    }
    public interface IExtensionBaseTypeMember : IMoveRemove //Used on Extension, Property, Comment
    { }
    public interface IDisplayedTypeMember { } //LinkType, BlobType, ContactType, CodingType, EventType, OnEventType, PredGuardType
    //public interface IDisplayedItem
    //{
    //    bool ConvertToLI(bool testOnly = false);
    //}
    public interface IResponse //marks LIR and QR
    {
        UnitsType AddUnits(ResponseFieldType rfParent);
        void RemoveUnits(ResponseFieldType rfParent) => rfParent.ResponseUnits = null;
        BaseType DataTypeObject { get; set; }
        RichTextType AddTextAfterResponse { get; set; }
    }
    public interface IMoveRemove
    {
        public bool Remove();
        public bool Move(ExtensionBaseType ebtTarget = null, int newListIndex = -1);
    }
    public interface ICoding
    {    }
    public interface IEvents
    { }

    public interface IContacts
    {  }
    public interface IClone
    {
        BaseType CloneSubtree();
    }
    public interface IRules
    {    }
    public interface IAction    
    {
        
    }
    public interface IOrganization
    {
        
    }
    public interface IPerson
    {
        
    }
    public interface INavigate
    {
        BaseType GetParent(BaseType Item);
        int GetIndex(BaseType Item);
        List<T> GetList<T>(T Item) where T:notnull, BaseType; //get the parent List type; some lists are not present in ParentNodes - this requires knowing the SDC object structure well.
        BaseType GetPreviousInList(BaseType Item);
        BaseType GetNextInList(BaseType Item);
        BaseType GetPrevious(BaseType Item);
        BaseType GetNext(BaseType Item);
    }
    public interface IVal { object Val { get; set; } } //Implemented by data types, which have a strongly-type val attribute.  Not implemented by anyType, XML, or HTML
    public interface IValNumeric: IVal { decimal ValDec{ get; set; } } //Implemented by numeric data types, which have a strongly-type val attribute.
    public interface IValDateTime: IVal {} //Implemented by DateTime data types, which have a strongly-type val attribute.
    public interface IValInteger : IVal { long ValLong { get; set; }} //Implemented by Integer data types, which have a strongly-type val attribute.  Includes byte, short, long, positive, no-positive, negative and non-negative types
    public interface IIdentifiers
    {
        string GetNewCkey();

    }

}
