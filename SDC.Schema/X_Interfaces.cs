using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
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
    public interface IChildItemsParent<T>:IChildItemsMember  //implemented by items that can have a ChildItems node.
        where T:ExtensionBaseType, IChildItemsParent<T>
    {
        ChildItemsType ChildItemsNode { get; set; }
        //C AddChildItem<P, C>(P parent, string childID = "", int insertPosition = -1) where P : IParent where C : IChildItem;
        //bool RemoveChildItem<C>(C childItem) where C : IChildItem;
        //bool MoveChildItem<P, C>(C childItem, int newPosition, P targetParent) where P : IParent where C : IChildItem;

        SectionItemType AddChildSection(string childID = "", int insertPosition = -1)
        {

            var childItemsList = AddChildItemsNode((T)this);
            var sNew = new SectionItemType(childItemsList, childID);
            var count = childItemsList.ChildItemsList.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            childItemsList.ChildItemsList.Insert(insertPosition, sNew);

            return sNew;
        }

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
    public interface IChildItemsMember:IHelpers, IQuestionBuilder //Marks SectionItemType, QuestionItemType, DisplayedType, ButtonItemType, InjectFormType
    {
        bool Remove()
        {
            var n = this as IdentifiedExtensionType;
            var cil = (n.ParentNode as ChildItemsType).ChildItemsList;
            return cil.Remove(n);
        }
            bool Move<T>(T target = null, int newListIndex = -1) where T : ExtensionBaseType, IChildItemsParent<T>;
        //CanRemove
        //CanMove (target ChildItems)
        //ChildCount
        #region IChildItemMember
        protected virtual ChildItemsType AddChildItemsNode()
        {
            var parent = (IChildItemsParent<T>)this;
            ChildItemsType childItems = null;  //this class contains an "Items" list
            if (parent == null)
                throw new ArgumentNullException("The T_Parent object was null");
            //return childItems; 
            else if (((IChildItemsParent<IdentifiedExtensionType>)parent).ChildItemsNode == null)
            {
                childItems = new ChildItemsType(parent);
                parent.ChildItemsNode = childItems;  //This may be null for the Header, Body and Footer  - need to check this
            }
            else //(T_Parent.ChildItemsNode != null)
                childItems = parent.ChildItemsNode;

            if (childItems.ChildItemsList == null)
                childItems.ChildItemsList = new List<IdentifiedExtensionType>();

            return childItems;
        }

        bool IsMoveAllowedToChild<S, T>(S source, T target, out string error)
            where S : notnull, IdentifiedExtensionType
            where T : notnull, IdentifiedExtensionType

        {
            var errorSource = "";
            var errorTarget = "";
            error = "";
            bool sourceOK = false;
            bool targetOK = false;

            if (source is null) { error = "source is null"; return false; }
            if (target is null) { error = "target is null"; return false; }
            if (target is ButtonItemType) { error = "ButtonItemType is not allowed as a target"; return false; }
            if (target is InjectFormType) { error = "InjectFormType is not allowed as a target"; return false; }
            if (((ITreeBuilder)this).IsDisplayedItem(target)) { error = "DisplayedItem is not allowed as a target"; return false; }

            if (source is ListItemType && !(target is QuestionItemType) && !(target is ListItemType)) { error = "A ListItem can only be moved into a Question List"; return false; };

            //special case to allow LI to drop on a Q and be added to the Q's List, rather than under ChildItem (which would be illegal)
            if (source is ListItemType &&
                target is QuestionItemType &&
                !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionSingle) &&
                !((target as QuestionItemType).GetQuestionSubtype() == QuestionEnum.QuestionMultiple))
            { error = "A Question target must be a QuestionSingle or QuestionMultiple"; return false; }


            if (source is DisplayedType || source is InjectFormType) sourceOK = true;
            if (target is QuestionItemType || target is SectionItemType || target is ListItemType) targetOK = true;

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
            where T : ExtensionBaseType, IChildItemsParent<T>
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
                                targetList = AddListToListField(AddListFieldToQuestion(q)).Items.ToList<BaseType>();
                                if (targetList is null) return false;
                                break;
                            }
                            newParent = q.ListField_Item.List;
                            targetList = q.ListField_Item.List.Items.ToList<BaseType>();
                        }
                        else //use the ChildItems node instead as the targetList
                        {
                            AddChildItemsNode(q as T);
                            targetList = q.ChildItemsNode.Items.ToList<BaseType>();
                        }
                        break;
                    case SectionItemType s:
                        AddChildItemsNode(s as T);
                        targetList = s.ChildItemsNode.Items.ToList<BaseType>();
                        break;
                    case ListItemType l:
                        AddChildItemsNode(l as T);
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

            var indexSource = GetListIndex(sourceList, source);  //save the original source index in case we need to replace the source node back to it's origin
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
        bool IsDisplayedItem(BaseType target)
        {  //checks for  exact type, not subtypes
            if (target.GetType() == typeof(DisplayedType)) return true;
            return false;
        }
        #endregion
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
            li.SelectIf=n;
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
        protected virtual ResponseFieldType AddQuestionResponseField(QuestionItemType qParent)
        {
            var rf = new ResponseFieldType(qParent);
            qParent.ResponseField_Item = rf;

            return rf;
        }
        protected virtual LookupEndPointType AddEndpointToListField(ListFieldType listFieldParent)
        {
            if (listFieldParent.List == null)
            {
                var lep = new LookupEndPointType(listFieldParent);
                listFieldParent.LookupEndpoint = lep;
                return lep;
            }
            else throw new InvalidOperationException("Can only add LookupEndpoint to ListField if List object is not present");
        }
        protected virtual ListType AddListToListField(ListFieldType listFieldParent)
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
        protected virtual ListFieldType AddListFieldToQuestion(QuestionItemType qParent)
        {
            if (qParent.ListField_Item == null)
            {
                var listField = new ListFieldType(qParent);
                qParent.ListField_Item = listField;
            }

            return qParent.ListField_Item;
        }

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
    public interface IVal { object Val { get; set; } } //Implemented by data types, which have a strongly-type val attribute.  Not implemented by anyType, XML, or HTML
    public interface IValNumeric: IVal { decimal ValDec{ get; set; } } //Implemented by numeric data types, which have a strongly-type val attribute.
    public interface IValDateTime: IVal {} //Implemented by DateTime data types, which have a strongly-type val attribute.
    public interface IValInteger : IVal { long ValLong { get; set; }} //Implemented by Integer data types, which have a strongly-type val attribute.  Includes byte, short, long, positive, no-positive, negative and non-negative types
    public interface IIdentifiers
    {
        string GetNewCkey() { throw new NotImplementedException(); }

    }
    public interface ICoding
    {    }
    public interface IContact
    {  }
    public interface IOrganization
    {
        OrganizationType AddOganization();
    }
    public interface IPerson
    {
        PersonType AddPerson();
    }
    public interface IEvent: IHasActionElseGroup, IHelpers  //Used for events (PredActionType)
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
    public interface IPredGuardType //used by Guards on ListItem, Button
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
    public interface IRules    {    }
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
    public interface IHasActionElseGroup: IAction, IHasElseNode
    {
        
    }
    public interface IHasElseNode: IHelpers
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
                    return (PredActionType)ArrayAddReturnItem(cfb.Items1, elseNode);
                case ScriptBoolFuncActionType sb:
                    return (PredActionType)ArrayAddReturnItem(sb.Items, elseNode);
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
    public interface IAction : IHelpers
    {
        public ActActionType AddActAction(int insertPosition = -1)
        {
            //var p = (ActionsType)this;
            //var act = new ActActionType(p);
            //return (ActActionType)ArrayAddReturnItem(p.Items, act);
            return AddAction(new ActActionType((ActionsType)this));
        }
        public RuleSelectMatchingListItemsType AddActSelectMatchingListItems(int insertPosition = -1)
        {
            return AddAction(new RuleSelectMatchingListItemsType((ActionsType)this));
        }
        //public abstract ActSetPropertyType AddSetProperty(ActionsType at);
        public ActAddCodeType AddActAddCode(int insertPosition = -1)
        {
            return AddAction(new ActAddCodeType((ActionsType)this));
        }
        //public abstract ActSetValueType AddSetValue(ActionsType at);
        public ActInjectType AddActInject(int insertPosition = -1)
        {
            return AddAction(new ActInjectType((ActionsType)this));
        }
        public CallFuncActionType AddActShowURL(int insertPosition = -1)
        {
            return AddAction(new CallFuncActionType((ActionsType)this));
        }
        public ActSaveResponsesType AddActSaveResponses(int insertPosition = -1)
        {
            return AddAction(new ActSaveResponsesType((ActionsType)this));
        }
        public ActSendReportType AddActSendReport(int insertPosition = -1)
        {
            return AddAction(new ActSendReportType((ActionsType)this));
        }
        public ActSendMessageType AddActSendMessage(int insertPosition = -1)
        {
            return AddAction(new ActSendMessageType((ActionsType)this));
        }
        public ActSetAttributeType AddActSetAttributeValue(int insertPosition = -1)
        {
            return AddAction(new ActSetAttributeType((ActionsType)this));
        }
        public ActSetAttrValueScriptType AddActSetAttributeValueScript(int insertPosition = -1)
        {
            return AddAction(new ActSetAttrValueScriptType((ActionsType)this));
        }
        public ActSetBoolAttributeValueCodeType AddActSetBoolAttributeValueCode(int insertPosition = -1)
        {
            return AddAction(new ActSetBoolAttributeValueCodeType((ActionsType)this));
        }
        public ActShowFormType AddActShowForm(int insertPosition = -1)
        {
            return AddAction(new ActShowFormType((ActionsType)this));
        }
        public ActShowMessageType AddActShowMessage(int insertPosition = -1)
        {
            return AddAction(new ActShowMessageType((ActionsType)this));
        }
        public ActShowReportType AddActShowReport(int insertPosition = -1)
        {
            return AddAction(new ActShowReportType((ActionsType)this));
        }
        public ActPreviewReportType AddActPreviewReport(int insertPosition = -1)
        {
            return AddAction(new ActPreviewReportType((ActionsType)this));
        }
        public ActValidateFormType AddActValidateForm(int insertPosition = -1)
        {
            return AddAction(new ActValidateFormType((ActionsType)this));
        }
        public ScriptCodeAnyType AddActRunCode(int insertPosition = -1)
        {
            return AddAction(new ScriptCodeAnyType((ActionsType)this));
        }
        public CallFuncActionType AddActCallFunction(int insertPosition = -1)
        {
            return AddAction(new CallFuncActionType((ActionsType)this));
        }
        public PredActionType AddActConditionalGroup(int insertPosition = -1)
        {
            return AddAction(new PredActionType((ActionsType)this));
        }

        T AddAction<T>(T action) where T:ExtensionBaseType
        {
            var p = (ActionsType)this;
            var act = new ActActionType(p);
            return (T)ArrayAddReturnItem(p.Items, act);
        }
    }
    public interface INavigate
    {
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
        bool IsItemChangeAllowed<S, T>(S source, T target)
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
        protected BaseType GetParent(BaseType item) { return item.ParentNode; }
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

        protected bool IsArray(object[] o) { throw new NotImplementedException(); }
        BaseType GetPreviousSib(BaseType item) { throw new NotImplementedException(); } //in list
        BaseType GetNextSib(BaseType item) { throw new NotImplementedException(); } //in list
        BaseType GetPrevious(BaseType item) { throw new NotImplementedException(); }
        BaseType GetNext(BaseType item) { throw new NotImplementedException(); }

    }
    public interface IClone
    {
        BaseType CloneSubtree();
    }

}
