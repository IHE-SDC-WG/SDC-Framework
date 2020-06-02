using System;
using System.IO;

namespace SDC.Schema
{
    /// <summary>
    /// Top-level public methods used to build SDC tree in SDC.Schema.PartialClasses
    /// </summary>
    public interface ITreeBuilder
    {
        #region New
        //FormDesignType CreateForm(bool addHeader, bool addFooter, string formID, string lineage, string version, string fullURI);
        //FormDesignType CreateFormFromTemplatePath(string path, string formID, string lineage, string version, string fullURI);
        //FormDesignType CreateFormFromTemplateXML(string xml, string formID, string lineage, string version, string fullURI);
        //bool RemoveFormFromPackage(RetrieveFormPackageType pkg, FormDesignType form);
        #endregion

        #region Package

        #endregion

        #region DataElement

        #endregion

        #region Demog Form

        #endregion

        #region Map

        #endregion

        #region IFormDesign
        SectionItemType AddHeader(FormDesignType fd);
        SectionItemType AddBody(FormDesignType fd);
        SectionItemType AddFooter(FormDesignType fd);
        //bool RemoveHeader(FormDesignType fd);
        //bool RemoveFooter(FormDesignType fd);

        #endregion

        #region IExtensionBase
        bool HasExtensionBaseMembers(ExtensionBaseType item); //Has Extension, Property or Comment sub-elements
        ExtensionType AddExtension(ExtensionBaseType ebtParent, int insertPosition = -1);
        CommentType AddComment(ExtensionBaseType ebtParent, int insertPosition = -1);
        PropertyType AddProperty(ExtensionBaseType dtParent, int insertPosition = -1);
        #endregion

        #region IExtensionBaseTypeMember (Extension, Comment, Property)
        //ExtensionBaseType AddExtensionBaseTypeItems(ExtensionBaseType ebt);        
        #region Extension        
        bool Remove(ExtensionType extension);
        bool Move(ExtensionType extension, ExtensionBaseType ebtTarget, int newListIndex);
        #endregion

        #region Comment
        bool Remove(CommentType comment);
        bool Move(CommentType comment, ExtensionBaseType ebtTarget, int newListIndex);
        #endregion

        #region Property        
        bool Remove(PropertyType property);
        bool Move(PropertyType property, ExtensionBaseType ebtTarget, int newListIndex);

        #endregion
        #endregion

        #region IDisplayedType

        LinkType AddLink(DisplayedType dtParent, int insertPosition = -1);
        BlobType AddBlob(DisplayedType dtParent, int insertPosition = -1);
        ContactType AddContact(DisplayedType dtParent, int insertPosition = -1);

        //bool RemoveLink(DisplayedType dtParent, int removePosition = -1);
        //bool RemoveBlob(DisplayedType dtParent, int removePosition = -1);
        //allow move to new parents?  Moving allowed from ebt to ebt
        //void MoveAllowedEBT(string movingItemObjectID, string targetItemObjectID, int insertPosition);
        #endregion

        #region IDisplayedTypeMember (empty)
        //LinkType, BlobType, ContactType, CodingType, EventType, OnEventType, PredGuardType
        #endregion

        #region IChildItemsParent
        SectionItemType AddChildSection<T>(T T_Parent, string id = "", int insertPosition = -1) where T : BaseType, IChildItemsParent; //, new();
        QuestionItemType AddChildQuestion<T>(T T_Parent, QuestionEnum qType, string id = "", int insertPosition = -1) where T : BaseType, IChildItemsParent; //, new();
        DisplayedType AddChildDisplayedItem<T>(T T_Parent, string id = "", int insertPosition = -1) where T : BaseType, IChildItemsParent; //, new();
        ButtonItemType AddChildButtonAction<T>(T T_Parent, string id = "", int insertPosition = -1) where T : BaseType, IChildItemsParent; //, new();
        InjectFormType AddChildInjectedForm<T>(T T_Parent, string id = "", int insertPosition = -1) where T : BaseType, IChildItemsParent; //, new();
        bool HasChildItems(IChildItemsParent parent);
        //C AddChildItem<P, C>(P parent, string childID = "", int insertPosition = -1) 
        //    where P : IdentifiedExtensionType, IParent
        //    where C : IdentifiedExtensionType, IChildItem, new();
        //bool RemoveChildItem<C>(C childItem) where C: IChildItem;
        //bool MoveChildItem<P, C>(C childItem, int newPosition, P targetParent) where P: IParent where C: IChildItem ; 

        //allow move to new parents?  The above check will also work for iet-derived parent classes lile DIs
        #endregion

        #region IChildItemMember
        bool Remove<T>(T source) where T :notnull, IdentifiedExtensionType, IChildItemsMember;
        bool IsMoveAllowedToChild<S, T>(S source, T target, out string error)
            where S : notnull, IdentifiedExtensionType
            where T: notnull, IdentifiedExtensionType;
        bool MoveAsChild<S, T>(S source, T target, int newListIndex)
            where S : notnull, IdentifiedExtensionType    //, IChildItemMember
            where T : DisplayedType, IChildItemsParent;
        bool MoveAfterSib<S, T>(S source, T target, int newListIndex, bool moveBefore = false)
            where S : notnull, IdentifiedExtensionType
            where T : notnull, IdentifiedExtensionType;
        bool IsDisplayedItem(BaseType target);

        #endregion
       
        #region IQuestionItem

        QuestionEnum GetQuestionSubtype(QuestionItemBaseType q);

        ListItemType AddListItem(QuestionItemType q, string id = "", int insertPosition = -1); // where T : DisplayedType, IQuestionItem;
        ListItemType AddListItemResponse(QuestionItemType q, string id = "", int insertPosition = -1); // where T : DisplayedType, IQuestionItem;
        DisplayedType AddDisplayedTypeToList(QuestionItemType q, string id = "", int insertPosition = -1); // where T : DisplayedType, IQuestionItem;
        QuestionItemType ConvertToQR(QuestionItemType q, bool testOnly = false); //where T : DisplayedType, IQuestionItem;
        QuestionItemType ConvertToQS(QuestionItemType q, bool testOnly = false); //where T : DisplayedType, IQuestionItem;
        QuestionItemType ConvertToQM(QuestionItemType q, int maxSelections = 0, bool testOnly = false); // where T : DisplayedType, IQuestionItem;
        DisplayedType ConvertToDI(QuestionItemType q, bool testOnly = false); // where T : DisplayedType, IQuestionItem;
        QuestionItemType ConvertToSection(QuestionItemType q, bool testOnly = false); // where T : DisplayedType, IQuestionItem;
        QuestionItemType ConvertToLookup(QuestionItemType q, bool testOnly = false); // where T : DisplayedType, IQuestionItem;

        #endregion

        #region IQuestionList
        ListItemType AddListItem(ListType lt,string id = "", int insertPosition = -1);//check that no ListItemResponseField object is present
        ListItemType AddListItemResponse(ListType lt, string id = "", int insertPosition = -1); //check that no ListFieldType object is present
        DisplayedType AddDisplayedItemToList(ListType lt, string id = "", int insertPosition = -1);
        #endregion

        #region IListField (empty)
        //nothing to support here
        #endregion

        #region IQuestionBase (empty)
        //nothing to support here
        #endregion

        #region IListItem

        ListItemResponseFieldType AddListItemResponseField(ListItemBaseType liParent);
        //ListItemType AddListItemToQuestion(QuestionItemType qParent, string id = "", int insertPosition = -1);
        //ListItemType AddListItemToList(QuestionItemType qParent, string id = "", int insertPosition = -1);
        //ListItemType AddListItemResponseToQuestion(QuestionItemType qParent, string id = "", int listIndex = -1);        
        //LookupEndPointType AddLookupEndpoint(ListFieldType lfParent);


        #endregion

        #region IQuestionListMember
        //for DI, make sure parent is a ListType object
        //remove LI/LIR or DI
        bool Remove<T>(T item, bool removeDecendants = false) 
            where T : notnull, DisplayedType, IQuestionListMember;            
        bool Move(IQuestionListMember source, QuestionItemType target = null, bool moveAbove = false, bool testOnly = false);
        bool Move(IQuestionListMember source, ListItemType target = null, bool moveAbove = false, bool testOnly = false);
        ListItemType ConvertToLI(bool testOnly = false);
        DisplayedType ConvertToDI(bool testOnly = false); //abort if children of LI are present
        ListItemType ConvertToLIR(bool testOnly = false);

        bool IsMoveAllowedToList<S,T>(S source, T target, out string error)
            where S : notnull, DisplayedType
            where T: notnull, DisplayedType;
        bool MoveInList(DisplayedType source, QuestionItemType target, bool moveAbove = false);
        bool MoveInList(DisplayedType source, DisplayedType target, bool moveAbove = false);
        #endregion

        #region IResponse
        UnitsType AddUnits(ResponseFieldType rfParent);
        #endregion

        #region IMoveRemove
        public bool Remove(); 
        public bool Move(ExtensionBaseType ebtTarget = null, int newListIndex = -1);
        #endregion

        #region IVal (ToDo)

        #endregion

        #region IValNumeric (ToDo)

        #endregion

        #region IValDateTime (ToDo)

        #endregion

        #region IValInteger (ToDo)

        #endregion

        #region IIdentifiers (ToDo)

        #endregion

        #region Data Helpers
        DataTypes_DEType AddDataTypesDE(
            ResponseFieldType rfParent,
            ItemChoiceType dataTypeEnum = ItemChoiceType.@string,
            dtQuantEnum quantifierEnum = dtQuantEnum.EQ,
            object value = null);
        dtQuantEnum AssignQuantifier(string quantifier);

        HTML_Stype AddHTML(RichTextType rt);
        #endregion

        #region ICoding
        CodingType AddCodedValue(DisplayedType dt, int insertPosition = -1);
        CodingType AddCodedValue(LookupEndPointType lep, int insertPosition = -1);
        UnitsType AddUnits(CodingType ctParent);
        #endregion

        #region IContact (File)
        ContactType AddContact(FileType ftParent, int insertPosition = -1);
        #endregion

        #region IOrganization
        OrganizationType AddOrganization(ContactType contactParent);
        OrganizationType AddOrganization(JobType jobParent);
        OrganizationType AddOrganizationItems(OrganizationType ot);

        #endregion

        #region IPerson
        PersonType AddPerson(ContactType contact);
        PersonType AddPerson(DisplayedType dt, int insertPosition = -1);
        PersonType AddContactPerson(OrganizationType otParent, int insertPosition = -1);

        #endregion

        #region IEventsAndGuards
        PredGuardType AddActivateIf(DisplayedType dt);
        PredGuardType AddDeActivateIf(DisplayedType dt);
        EventType AddOnEnterEvent(DisplayedType dt);
        OnEventType AddOnEventEvent(DisplayedType dt);
        EventType AddOnExitEvent(DisplayedType dt);
        #endregion

        #region IRules
        
        #endregion

        #region  IActions
        public ActActionType AddActAction(ActionsType at, int insertPosition = -1);
        public RuleSelectMatchingListItemsType AddActSelectMatchingListItems(ActionsType at, int insertPosition = -1);
        //public abstract ActSetPropertyType AddSetProperty(ActionsType at);
        public ActAddCodeType AddActAddCode(ActionsType at, int insertPosition = -1);
        //public abstract ActSetValueType AddSetValue(ActionsType at);
        public ActInjectType AddActInject(ActionsType at, int insertPosition = -1);
        public CallFuncActionType AddActShowURL(ActionsType at, int insertPosition = -1);
        public ActSaveResponsesType AddActSaveResponses(ActionsType at, int insertPosition = -1);
        public ActSendReportType AddActSendReport(ActionsType at, int insertPosition = -1);
        public ActSendMessageType AddActSendMessage(ActionsType at, int insertPosition = -1);
        public ActSetAttributeType AddActSetAttributeValue(ActionsType at, int insertPosition = -1);
        public ActSetAttrValueScriptType AddActSetAttributeValueScript(ActionsType at, int insertPosition = -1);
        public ActSetBoolAttributeValueCodeType AddActSetBoolAttributeValueCode(ActionsType at, int insertPosition = -1);
        public ActShowFormType AddActShowForm(ActionsType at, int insertPosition = -1);
        public ActShowMessageType AddActShowMessage(ActionsType at, int insertPosition = -1);
        public ActShowReportType AddActShowReport(ActionsType at, int insertPosition = -1);
        public ActPreviewReportType AddActPreviewReport(ActionsType at, int insertPosition = -1);
        public ActValidateFormType AddActValidateForm(ActionsType at, int insertPosition = -1);
        public ScriptCodeAnyType AddActRunCode(ActionsType at, int insertPosition = -1);
        public CallFuncActionType AddActCallFunctionction(ActionsType at, int insertPosition = -1);
        public PredActionType AddActConditionalGroupAction(ActionsType at, int insertPosition = -1);

        #endregion

        #region INavigate
        BaseType GetPreviousSib(BaseType item);
        BaseType GetNextSib(BaseType item);
        BaseType GetPrevious(BaseType item);
        BaseType GetNext(BaseType item);

        #endregion

        #region IClone (ToDo)
        BaseType CloneSubtree(BaseType top);
        #endregion

        #region Helpers
        string CreateName(BaseType bt);
        bool IsItemChangeAllowed<S, T>(S source, T target)
            where S : notnull, IdentifiedExtensionType
            where T : IdentifiedExtensionType;
        #endregion

    }
}
