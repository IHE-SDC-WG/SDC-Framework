﻿
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


//!Handling Item and Items generic types derived from the xsd2Code++ code generator
namespace SDC.Schema
{

    #region   ..Top SDC Elements
    public partial class FormDesignType : ITopNode, IFormDesign
    {
        #region ctor

        protected FormDesignType() : base()
        { }
        public FormDesignType(BaseType parentNode = null, string id = "")
        : base(parentNode, id)
        //TODO: add ID, lineage, baseURI, version, etc to this constructor? (only ID is required)
        { }
        public void Clear()
        {
            //reset and clean up some items that might hold references to this object, keeping it alive
            ResetSdcImport();
            Nodes = null;
            ParentNodes = null;
            //IdentExtNodes = null;
            //sdcTreeBuilder = null;
            ((ITopNode)this).MaxObjectID = 0;
            Body = null;
            Header = null;
            Footer = null;
            Property = null;
            Extension = null;
            Comment = null;
            Rules = null;
            OnEvent = null;

        }
        ~FormDesignType()
        { }
        #endregion

        #region IFormDesign
        //public SectionItemType AddBody()
        //{ return (this as IFormDesign).AddBody(); }
        //public SectionItemType AddFooter()
        //{ return (this as IFormDesign).AddFooter(); }
        //public SectionItemType AddHeader()
        //{ return (this as IFormDesign).AddHeader(); }
        public RulesType AddRules()
        { throw new NotImplementedException(); }
        #endregion

        #region ITopNode 

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int GetMaxObjectID { get => ((ITopNode)this).MaxObjectID; }  //save the highest object counter value for the current FormDesign tree
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        int ITopNode.MaxObjectID { get; set; } //internal
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> Nodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> ParentNodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, List<BaseType>> ChildNodes { get; private set; } = new Dictionary<Guid, List<BaseType>>();

        public List<BaseType> GetSortedNodesList()  => ((ITopNode)this).GetSortedNodesList(); 
        public ObservableCollection<BaseType> GetSortedNodesObsCol()  => ((ITopNode)this).GetSortedNodesObsCol(); 

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public bool GlobalAutoNameFlag { get; set; } = true;



        #region Serialization
        public static FormDesignType DeserializeFromXmlPath(string sdcPath)
            => (FormDesignType)GetSdcObjectFromXmlPath<FormDesignType>(sdcPath);
        public static FormDesignType DeserializeFromXml(string sdcXml)
            => (FormDesignType)GetSdcObjectFromXml<FormDesignType>(sdcXml);
        public string GetXml() => SdcSerializer<FormDesignType>.Serialize(this);
        public static FormDesignType DeserializeFromJsonPath(string sdcPath)
            => (FormDesignType)GetSdcObjectFromJsonPath<FormDesignType>(sdcPath);
        public static FormDesignType DeserializeFromJson(string sdcJson)
            => (FormDesignType)GetSdcObjectFromXml<FormDesignType>(sdcJson);
        public string GetJson() => SdcSerializerJson<FormDesignType>.SerializeJson(this);
        public static FormDesignType DeserializeFromMsgPackPath(string sdcPath)
            => (FormDesignType)GetSdcObjectFromMsgPackPath<FormDesignType>(sdcPath);
        public static FormDesignType DeserializeFromMsgPack(byte[] sdcMsgPack)
            => (FormDesignType)GetSdcObjectFromMsgPack<FormDesignType>(sdcMsgPack);
        public byte[] GetMsgPack() => (byte[])SdcSerializerMsgPack<FormDesignType>.SerializeMsgPack(this);
        public void SaveXmlToFile(string path, Exception ex = null)
            => SdcSerializer<FormDesignType>.SaveToFile(this, path, out ex);
        public void SaveJsonToFile(string path, Exception ex = null)
            => SdcSerializerJson<FormDesignType>.SaveToFileJson(path, this);
        public void SaveMsgPackToFile(string path, Exception ex = null)
            => SdcSerializerMsgPack<FormDesignType>.SaveToFileMsgPack(path, this);

        #endregion

        #endregion



        #region Dictionaries
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, IdentifiedExtensionType> IdentifiedTypes;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, SectionItemType> Sections;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, QuestionItemType> Questions;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, ListItemType> ListItemsAll;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, ListItemResponseFieldType> ListItemResponses;
        ////public static Dictionary<string, ResponseFieldType> Responses;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, InjectFormType> InjectedItems;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, DisplayedType> DisplayedItems;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, ButtonItemType> Buttons;
        //[System.Xml.Serialization.XmlIgnore]
        //[NonSerialized]
        //[JsonIgnore]
        //public Dictionary<string, BaseType> NamedNodes;

        #endregion

    }
    public partial class DemogFormDesignType : FormDesignType
    {
        protected DemogFormDesignType() : base()
        { }
        //public DemogFormDesignType(ITreeBuilder treeBuilder, BaseType parentNode = null, string id = "")
        //    : base(treeBuilder, parentNode, id)
        //{ }
        public DemogFormDesignType(BaseType parentNode = null, string id = "")
            : base(parentNode, id)
        { }

        #region ITopNode
        #region Serialization
        public new static DemogFormDesignType DeserializeFromXmlPath(string sdcPath)
            => (DemogFormDesignType)GetSdcObjectFromXmlPath<DemogFormDesignType>(sdcPath);
        public new static DemogFormDesignType DeserializeFromXml(string sdcXml)
            => (DemogFormDesignType)GetSdcObjectFromXml<DemogFormDesignType>(sdcXml);
        public new string GetXml() => SdcSerializer<DemogFormDesignType>.Serialize(this);
        public new static DemogFormDesignType DeserializeFromJsonPath(string sdcPath)
            => (DemogFormDesignType)GetSdcObjectFromJsonPath<DemogFormDesignType>(sdcPath);
        public new static DemogFormDesignType DeserializeFromJson(string sdcJson)
            => (DemogFormDesignType)GetSdcObjectFromXml<DemogFormDesignType>(sdcJson);
        public new string GetJson() => SdcSerializerJson<DemogFormDesignType>.SerializeJson(this);
        public new static DemogFormDesignType DeserializeFromMsgPackPath(string sdcPath)
            => (DemogFormDesignType)GetSdcObjectFromMsgPackPath<DemogFormDesignType>(sdcPath);
        public new static DemogFormDesignType DeserializeFromMsgPack(byte[] sdcMsgPack)
            => (DemogFormDesignType)GetSdcObjectFromMsgPack<DemogFormDesignType>(sdcMsgPack);
        public new byte[] GetMsgPack() => (byte[])SdcSerializerMsgPack<DemogFormDesignType>.SerializeMsgPack(this);
        public new void SaveXmlToFile(string path, Exception ex = null)
            => SdcSerializer<DemogFormDesignType>.SaveToFile(this, path, out ex);
        public new void SaveJsonToFile(string path, Exception ex = null)
            => SdcSerializerJson<DemogFormDesignType>.SaveToFileJson(path, this);
        public new void SaveMsgPackToFile(string path, Exception ex = null)
            => SdcSerializerMsgPack<DemogFormDesignType>.SaveToFileMsgPack(path, this);

        #endregion
        #endregion
    }
    public partial class DataElementType : ITopNode
    {
        protected DataElementType() : base()
        { }
        public DataElementType(string id = "") : base(null)
        { //TODO:Add dictionaries for nodes etc
            //TODO:Make sure BaseType constructor functions work
        }

        #region ITopNode
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int GetMaxObjectID { get => ((ITopNode)this).MaxObjectID; }  //save the highest object counter value for the current FormDesign tree
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        int ITopNode.MaxObjectID { get; set; } //internal
        /// <summary>
        /// Dictionary.  Given an Node ID (int), returns the Node's object reference.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> Nodes { get; private set; } = new Dictionary<Guid, BaseType>();
        /// <summary>
        /// Dictionary.  Given a NodeID, return the *parent* Node's object reference
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> ParentNodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, List<BaseType>> ChildNodes { get; private set; } = new Dictionary<Guid, List<BaseType>>();

        public List<BaseType> GetSortedNodesList() => ((ITopNode)this).GetSortedNodesList();
        public ObservableCollection<BaseType> GetSortedNodesObsCol() => ((ITopNode)this).GetSortedNodesObsCol();

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public bool GlobalAutoNameFlag { get; set; }

        #region Serialization
        public static DataElementType DeserializeFromXmlPath(string sdcPath)
            => (DataElementType)GetSdcObjectFromXmlPath<DataElementType>(sdcPath);
        public static DataElementType DeserializeFromXml(string sdcXml)
            => (DataElementType)GetSdcObjectFromXml<DataElementType>(sdcXml);
        public string GetXml() => SdcSerializer<DataElementType>.Serialize(this);
        public static DataElementType DeserializeFromJsonPath(string sdcPath)
            => (DataElementType)GetSdcObjectFromJsonPath<DataElementType>(sdcPath);
        public static DataElementType DeserializeFromJson(string sdcJson)
            => (DataElementType)GetSdcObjectFromXml<DataElementType>(sdcJson);
        public string GetJson() => SdcSerializerJson<DataElementType>.SerializeJson(this);
        public static DataElementType DeserializeFromMsgPackPath(string sdcPath)
            => (DataElementType)GetSdcObjectFromMsgPackPath<DataElementType>(sdcPath);
        public static DataElementType DeserializeFromMsgPack(byte[] sdcMsgPack)
            => (DataElementType)GetSdcObjectFromMsgPack<DataElementType>(sdcMsgPack);
        public byte[] GetMsgPack() => (byte[])SdcSerializerMsgPack<DataElementType>.SerializeMsgPack(this);
        public void SaveXmlToFile(string path, Exception ex = null)
            => SdcSerializer<DataElementType>.SaveToFile(this, path, out ex);
        public void SaveJsonToFile(string path, Exception ex = null)
            => SdcSerializerJson<DataElementType>.SaveToFileJson(path, this);
        public void SaveMsgPackToFile(string path, Exception ex = null)
            => SdcSerializerMsgPack<DataElementType>.SaveToFileMsgPack(path, this);

        #endregion      
        #endregion


    }
    public partial class RetrieveFormPackageType : ITopNode
    {
        protected RetrieveFormPackageType() : base()
        { }
        public RetrieveFormPackageType(string id = "") //: base(null, false)
        {
            //TODO:Make sure BaseType constructor functions work
        }

        #region ITopNode
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int GetMaxObjectID { get => ((ITopNode)this).MaxObjectID; }  //save the highest object counter value for the current FormDesign tree
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        int ITopNode.MaxObjectID { get; set; } //internal
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> Nodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> ParentNodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, List<BaseType>> ChildNodes { get; private set; } = new Dictionary<Guid, List<BaseType>>();

        public List<BaseType> GetSortedNodesList() => ((ITopNode)this).GetSortedNodesList();
        public ObservableCollection<BaseType> GetSortedNodesObsCol() => ((ITopNode)this).GetSortedNodesObsCol();

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public bool GlobalAutoNameFlag { get; set; } = true;

        #region Serialization
        public static RetrieveFormPackageType DeserializeFromXmlPath(string sdcPath)
            => (RetrieveFormPackageType)GetSdcObjectFromXmlPath<RetrieveFormPackageType>(sdcPath);
        public static RetrieveFormPackageType DeserializeFromXml(string sdcXml)
            => (RetrieveFormPackageType)GetSdcObjectFromXml<RetrieveFormPackageType>(sdcXml);
        public string GetXml() => SdcSerializer<RetrieveFormPackageType>.Serialize(this);
        public static RetrieveFormPackageType DeserializeFromJsonPath(string sdcPath)
            => (RetrieveFormPackageType)GetSdcObjectFromJsonPath<RetrieveFormPackageType>(sdcPath);
        public static RetrieveFormPackageType DeserializeFromJson(string sdcJson)
            => (RetrieveFormPackageType)GetSdcObjectFromXml<RetrieveFormPackageType>(sdcJson);
        public string GetJson() => SdcSerializerJson<RetrieveFormPackageType>.SerializeJson(this);
        public static RetrieveFormPackageType DeserializeFromMsgPackPath(string sdcPath)
            => (RetrieveFormPackageType)GetSdcObjectFromMsgPackPath<RetrieveFormPackageType>(sdcPath);
        public static RetrieveFormPackageType DeserializeFromMsgPack(byte[] sdcMsgPack)
            => (RetrieveFormPackageType)GetSdcObjectFromMsgPack<RetrieveFormPackageType>(sdcMsgPack);
        public byte[] GetMsgPack() => (byte[])SdcSerializerMsgPack<RetrieveFormPackageType>.SerializeMsgPack(this);
        public void SaveXmlToFile(string path, Exception ex = null)
            => SdcSerializer<RetrieveFormPackageType>.SaveToFile(this, path, out ex);
        public void SaveJsonToFile(string path, Exception ex = null)
            => SdcSerializerJson<RetrieveFormPackageType>.SaveToFileJson(path, this);
        public void SaveMsgPackToFile(string path, Exception ex = null)
            => SdcSerializerMsgPack<RetrieveFormPackageType>.SaveToFileMsgPack(path, this);

        #endregion   
        #endregion

    }
    public partial class PackageListType : ITopNode
    {
        protected PackageListType() : base()
        { }
        public PackageListType(string id = "") //: base( null, false)
        {
            //TODO:Make sure BaseType constructor functions work
        }
        #region ITopNode
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int GetMaxObjectID { get => ((ITopNode)this).MaxObjectID; }  //save the highest object counter value for the current FormDesign tree
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        int ITopNode.MaxObjectID { get; set; } //internal
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> Nodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> ParentNodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, List<BaseType>> ChildNodes { get; private set; } = new Dictionary<Guid, List<BaseType>>();

        public List<BaseType> GetSortedNodesList() => ((ITopNode)this).GetSortedNodesList();
        public ObservableCollection<BaseType> GetSortedNodesObsCol() => ((ITopNode)this).GetSortedNodesObsCol();

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public bool GlobalAutoNameFlag { get; set; } = true;

        #region Serialization
        public static PackageListType DeserializeFromXmlPath(string sdcPath)
            => (PackageListType)GetSdcObjectFromXmlPath<PackageListType>(sdcPath);
        public static PackageListType DeserializeFromXml(string sdcXml)
            => (PackageListType)GetSdcObjectFromXml<PackageListType>(sdcXml);
        public string GetXml() => SdcSerializer<PackageListType>.Serialize(this);
        public static PackageListType DeserializeFromJsonPath(string sdcPath)
            => (PackageListType)GetSdcObjectFromJsonPath<PackageListType>(sdcPath);
        public static PackageListType DeserializeFromJson(string sdcJson)
            => (PackageListType)GetSdcObjectFromXml<PackageListType>(sdcJson);
        public string GetJson() => SdcSerializerJson<PackageListType>.SerializeJson(this);
        public static PackageListType DeserializeFromMsgPackPath(string sdcPath)
            => (PackageListType)GetSdcObjectFromMsgPackPath<PackageListType>(sdcPath);
        public static PackageListType DeserializeFromMsgPack(byte[] sdcMsgPack)
            => (PackageListType)GetSdcObjectFromMsgPack<PackageListType>(sdcMsgPack);
        public byte[] GetMsgPack() => (byte[])SdcSerializerMsgPack<PackageListType>.SerializeMsgPack(this);
        public void SaveXmlToFile(string path, Exception ex = null)
            => SdcSerializer<PackageListType>.SaveToFile(this, path, out ex);
        public void SaveJsonToFile(string path, Exception ex = null)
            => SdcSerializerJson<PackageListType>.SaveToFileJson(path, this);
        public void SaveMsgPackToFile(string path, Exception ex = null)
            => SdcSerializerMsgPack<PackageListType>.SaveToFileMsgPack(path, this);

        #endregion     
        #endregion

    }
    public partial class MappingType : ITopNode
    {
        #region ITopNode
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int GetMaxObjectID { get => ((ITopNode)this).MaxObjectID; }  //save the highest object counter value for the current FormDesign tree
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        int ITopNode.MaxObjectID { get; set; } //internal
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> Nodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, BaseType> ParentNodes { get; private set; } = new Dictionary<Guid, BaseType>();
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Dictionary<Guid, List<BaseType>> ChildNodes { get; private set; } = new Dictionary<Guid, List<BaseType>>();

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public bool GlobalAutoNameFlag { get; set; } = true;

        #region Serialization
        public static MappingType DeserializeFromXmlPath(string sdcPath)
            => (MappingType)GetSdcObjectFromXmlPath<MappingType>(sdcPath);
        public static MappingType DeserializeFromXml(string sdcXml)
            => (MappingType)GetSdcObjectFromXml<MappingType>(sdcXml);
        public string GetXml() => SdcSerializer<MappingType>.Serialize(this);
        public static MappingType DeserializeFromJsonPath(string sdcPath)
            => (MappingType)GetSdcObjectFromJsonPath<MappingType>(sdcPath);
        public static MappingType DeserializeFromJson(string sdcJson)
            => (MappingType)GetSdcObjectFromXml<MappingType>(sdcJson);
        public string GetJson() => SdcSerializerJson<MappingType>.SerializeJson(this);
        public static MappingType DeserializeFromMsgPackPath(string sdcPath)
            => (MappingType)GetSdcObjectFromMsgPackPath<MappingType>(sdcPath);
        public static MappingType DeserializeFromMsgPack(byte[] sdcMsgPack)
            => (MappingType)GetSdcObjectFromMsgPack<MappingType>(sdcMsgPack);
        public byte[] GetMsgPack() => (byte[])SdcSerializerMsgPack<MappingType>.SerializeMsgPack(this);
        public void SaveXmlToFile(string path, Exception ex = null)
            => SdcSerializer<MappingType>.SaveToFile(this, path, out ex);
        public void SaveJsonToFile(string path, Exception ex = null)
            => SdcSerializerJson<MappingType>.SaveToFileJson(path, this);
        public void SaveMsgPackToFile(string path, Exception ex = null)
            => SdcSerializerMsgPack<MappingType>.SaveToFileMsgPack(path, this);

        #endregion     
        #endregion

    }
    #endregion


    #region ..Main Types
    public partial class ButtonItemType
        : IChildItemsMember<ButtonItemType>
    {
        protected ButtonItemType() { }
        public ButtonItemType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "ButtonAction";
            ElementPrefix = "B";
            //SetNames(elementName, elementPrefix);
        }

    }

    public partial class InjectFormType : IChildItemsMember<InjectFormType>
    {
        protected InjectFormType() { }
        public InjectFormType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode, id)
        {
            this._repeat = "0";
            ElementName = "InjectForm";
            ElementPrefix = "Inj";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class SectionBaseType
    {
        public SectionBaseType() { }
        internal SectionBaseType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode, id)
        {
            this._ordered = true;
            ElementName = "Section";
            ElementPrefix = "S";
            //SetNames(elementName, elementPrefix);
        }

        //!+Replaced in original class: protected SectionBaseType() { }
        //public void FillSectionBaseType()
        //{ sdcTreeBuilder.FillSectionBase(this); }
    }

    public partial class SectionItemType : IChildItemsParent<SectionItemType>, IChildItemsMember<SectionItemType>
    {
        protected SectionItemType() { } //change back to protected
        public SectionItemType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode, id)
        { }

        #region IChildItemsParent Implementation
        private IChildItemsParent<SectionItemType> ci => this as IChildItemsParent<SectionItemType>;
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ChildItemsType ChildItemsNode
        {
            get { return this.Item; }
            set { this.Item = value;}
        }        
        #endregion
    }
    #region QAS

    #region Question

    public partial class QuestionItemType : IChildItemsParent<QuestionItemType>, IChildItemsMember<QuestionItemType>, IQuestionItem, IQuestionList
    {
        protected QuestionItemType() { }  //need public parameterless constructor to support generics
        public QuestionItemType(BaseType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            this.readOnly = false;
            ElementName = "Question";
            ElementPrefix = "Q";
            SetNames(elementName, elementPrefix);
            //this.Conv
        }

        #region IChildItemsParent
        IChildItemsParent<QuestionItemType> ci { get => (IChildItemsParent<QuestionItemType>)this; }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ChildItemsType ChildItemsNode
        {
            get { return this.Item1; }
            set { this.Item1 = value; }
        }
        #endregion

    }

    public partial class QuestionItemBaseType : IQuestionBase
    {
        protected QuestionItemBaseType() { }
        public QuestionItemBaseType(BaseType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            this._readOnly = false;
        }

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ListFieldType ListField_Item
        {
            get
            {
                if (Item.GetType() == typeof(ListFieldType))
                    return (ListFieldType)this.Item;
                else return null;
            }
            set { this.Item = value; }
        }


        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ResponseFieldType ResponseField_Item
        {
            get
            {
                if (Item.GetType() == typeof(ResponseFieldType))
                    return (ResponseFieldType)this.Item;
                else return null;
            }
            set { this.Item = value; }
        }
   }
    #endregion

    #region QAS ListItems and Lookups


    public partial class ListType : IQuestionList
    {
        protected ListType() { }
        public ListType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "lst";
            SetNames(elementName, elementPrefix);
        }

        /// <summary>
        /// Replaces Items; ListItem or DisplayedItem
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public List<DisplayedType> QuestionListMembers
        {
            get { return this.Items; }
            set { this.Items = value; }
        }
    }

    public partial class ListFieldType : IListField
    {
        protected ListFieldType() { }
        public ListFieldType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._colTextDelimiter = "|";
            this._numCols = ((byte)(1));
            this._storedCol = ((byte)(1));
            this._minSelections = ((ushort)(1));
            this._maxSelections = ((ushort)(1));
            this._ordered = true;


            ElementPrefix = "lf";
            SetNames(elementName, elementPrefix);
        }

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ListType List
        {
            get
            {
                if (Item.GetType() == typeof(ListType))
                    return (ListType)this.Item;
                else return null;
            }
            set { this.Item = value; }
        }
        /// <summary>
        /// Replaces Item
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public LookupEndPointType LookupEndpoint
        {
            get
            {
                if (Item.GetType() == typeof(LookupEndPointType))
                    return (LookupEndPointType)this.Item;
                else return null;
            }
            set { this.Item = value; }
        }

    }

    public partial class ListItemType : IChildItemsParent<ListItemType> //, IListItem //, IQuestionListMember
    {
        protected ListItemType() { }
        public ListItemType(ListType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            ElementPrefix = "LI";
            SetNames(elementName, elementPrefix);
        }

        #region IChildItemsParent

        /// <summary>
        /// The ChildItems node replaces "Item" (MainNodesType), and may contain:
        ///"ButtonAction", typeof(ButtonItemType),
        ///"DisplayedItem", typeof(DisplayedType),
        ///"InjectForm", typeof(InjectFormType),
        ///"Question", typeof(QuestionItemType),
        ///"Section", typeof(SectionItemType),
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ChildItemsType ChildItemsNode
        {
            get { return this.Item; }
            set { this.Item = value; }
        }

        #endregion


    }

    public partial class ListItemBaseType
    {
        protected ListItemBaseType() { }
        public ListItemBaseType(ListType parentNode, string id = "") : base(parentNode, id)
        {
            this._selected = false;
            this._selectionDisablesChildren = false;
            this._selectionDeselectsSiblings = false;
            this._omitWhenSelected = false;
            this._repeat = "0";
        }


    }

    public partial class LookupEndPointType  //TODO: fix base class in Schema update
    {
        protected LookupEndPointType() { }
        public LookupEndPointType(ListFieldType parentNode, string elementName = "", string elementPrefix = "") : base()
        {
            this._includesHeaderRow = false;
            ElementPrefix = "LEP";
            SetNames(elementName, elementPrefix);
        }
    }

    #endregion

    #region Responses

    public partial class ListItemResponseFieldType
    {
        protected ListItemResponseFieldType() { }
        public ListItemResponseFieldType(ListItemBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._responseRequired = false;
            //if (fillData) AddFillDataTypesDE(parentNode);

            ElementPrefix = "lirf";
            SetNames(elementName, elementPrefix); //this was already called by the superType ResponseField.
        }
    }

    public partial class ResponseFieldType
    {
        protected ResponseFieldType() { }
        public ResponseFieldType(IdentifiedExtensionType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "rf";
            SetNames(elementName, elementPrefix);
            this.Item = null;
        }
    }

    public partial class UnitsType
    {
        protected UnitsType() { }
        public UnitsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            _unitSystem = "UCUM";
            ElementPrefix = "un";
            SetNames(elementName, elementPrefix);
        }
    }

    #endregion

    #endregion


    #endregion

    #region Base Types
    public partial class BaseType : IBaseType //TODO: need to explicitly implement INavigate interface
    {

        #region  Local Members

        ///// <summary>
        ///// sdcTreeBuilder is an object created and held by the top level FormDesign node, 
        ///// but referenced throughout the FormDesign object tree through the BaseType class
        ///// </summary>
        //protected ITreeBuilder sdcTreeBuilder; //TODO: convert to static field

        object propertyName;
        int elementIndex;
        int elementOrder;
        private string _elementName = "";
        private string _elementPrefix = "";
        private SdcTopNodeTypesEnum sdcTopType; //Enum that stores the type of the top level node in the node tree


        /// <summary>
        /// Static counter that resets with each new instance of an IdentifiedExtensionType (IET).
        /// Maintains the sequence of all elements nested under an IET-derived element.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        private static int IETresetCounter { get; set; }
        /// <summary>
        /// Field to hold the ordinal position of an object (XML element) under an IdentifiedExtensionType (IET)-derived object.
        /// This number is used for creating the name attribute suffix.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        private decimal SubIETcounter { get; set; }
        private BaseType _ParentNode;
        private RetrieveFormPackageType _PackageNode;
        private static ITopNode topNodeTemp;

        private static ITopNode TopNodeTemp
        {
            get { return topNodeTemp; }
            set
            {
                if (topNodeTemp is null)
                { topNodeTemp = value; }
                else { throw new Exception("TopNode has already been assigned.  A call to ResetSdcImport() is required before this object can be set for importing a new SDC template;"); }
            }
        }

        #region Base Types
        //internal void RegisterParent<T>(T inParentNode) where T : BaseType
        //{
        //    _ParentNode = inParentNode;
        //    try
        //    {
        //        if (inParentNode != null)
        //        {   //Register parent node
        //            TopNode.ParentNodes.Add(ObjectGUID, inParentNode);  

        //            List<BaseType> kids;
        //            TopNode.ChildNodes.TryGetValue(inParentNode.ObjectGUID, out kids);
        //            if (kids is null)
        //            {
        //                kids = new List<BaseType>();
        //                TopNode.ChildNodes.Add(inParentNode.ObjectGUID, kids);
        //            }
        //            kids.Add(this); 

        //            //inParentNode.IsLeafNode = false; //the parent node has a child node, so it can't be a leaf node
        //        }
        //    }
        //    catch (Exception ex)
        //    { Debug.WriteLine(ex.Message + "/n  ObjectID:" + this.ObjectID.ToString()); }
        //}

        //internal void UnRegisterParent()
        //{
        //    var par = this.ParentNode;
        //    try
        //    {
        //        bool success = false;
        //        if (par != null)
        //        {   if (TopNode.ParentNodes.ContainsKey(this.ObjectGUID))
        //                success = TopNode.ParentNodes.Remove(this.ObjectGUID);
        //           // if (!success) throw new Exception($"Could not remove object from ParentNodes dictionary: name: {this.name ?? "(none)"} , ObjectID: {this.ObjectID}");

        //            if (TopNode.ChildNodes.ContainsKey(par.ObjectGUID))
        //                success = TopNode.ChildNodes[par.ObjectGUID].Remove(this); //Returns a List<BaseType> and removes "item" from that list
        //            //if (!success) throw new Exception($"Could not remove object from ChildNodes dictionary: name: {this.name ?? "(none)"}, ObjectID: {this.ObjectID}");

        //            //if (TopNode.ChildNodes.ContainsKey(this.ObjectGUID))
        //            //    success = TopNode.ChildNodes[this.ObjectGUID].Remove(this);
        //            //if (!success) throw new Exception($"Could not remove object from ChildNodes dictionary: name: {this.name ?? "(none)"}, ObjectID: {this.ObjectID}");

        //            //if(TopNode.ChildNodes[par.ObjectGUID] is null || par.TopNode.ChildNodes[par.ObjectGUID].Count() == 0)
        //            //par.IsLeafNode = true; //the parent node has no child nodes, so it is a leaf node
        //        }
        //    }
        //    catch (Exception ex)
        //    { Debug.WriteLine(ex.Message + "/n  ObjectID:" + this.ObjectID.ToString()); }

        //    this.ParentNode = null;
        //    
        //} //!not tested
        //internal void UnRegisterThis()
        //{
        //    bool success = TopNode.Nodes.Remove(this.ObjectGUID);
        //    if (!success) throw new Exception($"Could not remove object from Nodes dictionary: name: {this.name ?? "(none)"}, ObjectID: {this.ObjectID}");
        //    UnRegisterParent();
        //} //!not tested

        #endregion








        #endregion


        #region Public Members (IBaseType)

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ITopNode TopNode { get; private set; }
        /// <summary>
        ///  Hierarchical level using nested dot notation
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public string DotLevel
        {
            get
            {
                //Walk up parent node tree and place each parent in a stack.
                //pop each node off the stack and determine its position (seq) in its parent object
                BaseType par = ParentNode;
                var s = new Stack<BaseType>();
                s.Push(this);
                while (par != null)
                {
                    s.Push(par);
                    par = par.ParentNode;
                }
                int level = 0;
                var sb = new StringBuilder("0");
                int seq;
                s.Pop();  //pop off the top node, which has no parent.
                while (s.Count > 0)
                {
                    var n = s.Pop();

                    if (n.TopNode.ChildNodes.TryGetValue(n.ParentNode.ObjectGUID, out List<BaseType> lst))
                    { seq = lst.IndexOf(n) + 1; }
                    else { seq = 0; }
                    sb.Append('.').Append(seq); ;
                    level++;
                }
                return sb.ToString();
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public bool AutoNameFlag { get; set; } = false;

        private bool cycleGuarded = false;
        /// <summary>
        /// The root text ("shortName") used to construct the name property.  The code may add a prefix and/or suffix to BaseName
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public string BaseName { get; set; } = "";

        /// <summary>
        /// The name of XML element that is output from this class instance.
        /// Some SDC types are used in conjunction with multiple element names.  
        /// The auto-generated classes do not provide a way to determine the element name form the class instance.
        /// However, it is possible to achieve this effect by reflection of 
        /// attributes at the time of creating each node, and also after hydrating the SDC object tree from XML.
        /// ElementName will be most useful for auto-generating @name attributes for some elements.
        /// In many cases, ElementName will be assigned through class constructors, but it can also be assigned 
        /// through this property after the object is instantiated
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public string ElementName //TODO: remove all references trying to set ElementName, and then remove the set{} option
        {
            get
            {
                return _elementName;
            }
            set  //TODO: remove this setter from IBaseType interface and/or make it protected internal, or just internal.
            {
                _elementName = value;
            }
        }


        /// <summary>
        /// NEW
        /// For the SDC property's XML element, returns the Order value from the property's XMLElementAttribute
        /// Assigned by reflection at the time of object creation.
        /// TODO: Add to IBaseType
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int ElementOrder
        {
            get => elementOrder;
            set
            {
                if (elementOrder == value)
                    return;
                elementOrder = value;
            }
        }

        /// <summary>
        /// NEW
        /// For the SDC property's XML element, if the property is found inside a List object.
        /// Return -1 if this object is not found inside a List object.
        /// TODO: Add to IBaseType
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int ElementIndex
        {
            get
            {
                var par = this.ParentNode;
                if (par is null) return -1;
                TopNode.ChildNodes.TryGetValue(par.ObjectGUID, out List<BaseType> kids);
                if (kids is null || kids.Count == 0) return -1;

                return kids.IndexOf(this);
            }

        }

        /// <summary>
        /// The prefix used 
        /// in the @name attribute that is output from this class instance
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public string ElementPrefix
        {
            get
            { //assign default prefix from the ElementName
                if (_elementPrefix.IsEmpty())
                {
                    _elementPrefix = _elementName;
                    if (_elementName.IsEmpty()) return "";
                    //make sure first letter is lower case for non-IET types:
                    if (!(this.GetType().IsSubclassOf(typeof(IdentifiedExtensionType)))) _elementPrefix = _elementPrefix.Substring(0, 1).ToLower() + _elementPrefix.Substring(1);
                }
                return _elementPrefix;
            }
            set { _elementPrefix = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int ObjectID { get; private set; }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Guid ObjectGUID { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public ItemTypeEnum NodeType { get; private set; }
        //[System.Xml.Serialization.XmlIgnore]
        //[JsonIgnore]
        //public Boolean IsLeafNode { get=> !this.HasChildren();  } //TODO: can use INavigate reflection methods for this, since it changes during tree editing

        /// <summary>
        /// Returns the ID of the parent object (representing the parent XML element)
        /// This is the ObjectID, which is a sequentially assigned integer value.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public int ParentID
        {
            get
            {
                if (ParentNode != null)
                { return ParentNode.ObjectID; }
                else return -1;
            }
        }

        /// <summary>
        /// Retrieve the BaseType object that is the immediate parent of the current object in the object tree
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public BaseType ParentNode
        {
            get
            {
                //return _ParentNode;  //this works for objects that were created with the parentNode constructor
                
                TopNode.ParentNodes.TryGetValue(this.ObjectGUID, out BaseType outParentNode);
                return outParentNode;

            }
            internal set
            {
                _ParentNode = value;
            }
        }
        /// <summary>
        /// Retrieve the BaseType object that is the SDC Package containing the current object in the object tree
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public RetrieveFormPackageType PackageNode
        {
            get => _PackageNode;  //this works for objects that were created with the parentNode constructor
            internal set =>_PackageNode = value; 
        }

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public IdentifiedExtensionType ParentIETypeNode
        {
            get
            {
                BaseType outParentNode;
                outParentNode = this;
                do
                {
                    TopNodeTemp.ParentNodes.TryGetValue(outParentNode.ObjectGUID, out outParentNode);

                    if (outParentNode != null &&
                        outParentNode.GetType().IsSubclassOf(typeof(IdentifiedExtensionType)))
                        return (IdentifiedExtensionType)outParentNode;
                    outParentNode = outParentNode?.ParentNode;
                } while (outParentNode != null);
                return null;
            }
        }
        /// <summary>
        /// Returns the ID property of the closest ancestor of type IdentifiedExtensionType.  
        /// For eCC, this is the Parent node's ID, which is derived from  the parent node's CTI_Ckey, a.k.a. ParentItemCkey.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public string ParentIETypeID
        { get => ParentIETypeNode?.ID; }

        public void SetNames(string elementName = "", string elementPrefix = "", string baseName = "")
        {
            if (TopNodeTemp.GlobalAutoNameFlag || AutoNameFlag)
            {
                if (elementName.Length > 0)
                    ElementName = elementName;
                else if (ElementName.Length == 0) ElementName = GetType().ToString().Replace("Type", "").Replace("type", ""); //assign default ElementName from the type.

                if (elementPrefix.Length > 0)
                    ElementPrefix = elementPrefix;

                if (baseName.Length > 0)
                    BaseName = baseName;
                //else if (ElementPrefix.Length == 0) ElementPrefix = ElementName;

                Debug.WriteLine($"Type: {this.GetType()} ElementName: {ElementName} Prefix:{ElementPrefix} name: {name}");
            }
        }
        /// <summary>
        /// Resets TopNode and IETresetCounter.  This allows the creation of a new SDC tree for unit testing
        /// </summary>
        public static void ResetSdcImport() //This really should be only on the TopNode object (e.g., FormDesign)
        {
            topNodeTemp = null;
            IETresetCounter = 0;
        }

        #endregion

        #region ChangeTracking
        //Properties to mark changed nodes for serialization to database etc.
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Boolean Added { get; private set; }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Boolean Changed { get; private set; }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Boolean Deleted { get; private set; }
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public DateTime UpdatedDateTime { get; private set; }
        #endregion

        protected BaseType()
        {   //Parent Nodes cannot be assigned through this constructor.  
            //After the object tree is created, Parent Nodes can be assigned by using InitNodes<T>
            ObjectGUID = Guid.NewGuid();
            InitBaseType();   
        }

        protected BaseType(BaseType parentNode) //: this()
        {
            ObjectGUID = Guid.NewGuid();
            InitBaseType();
            this.RegisterParent(parentNode);
            
        }

        #region     Init Methods
        private void InitBaseType()
        {
            orderSpecified = true;
            typeSpecified = true;
            styleClassSpecified = true;
            nameSpecified = true;
            order = 0;

            //IsLeafNode = true;

            if (this.GetType().IsSubclassOf(typeof(IdentifiedExtensionType))) IETresetCounter = 0;
            else IETresetCounter++;
            SubIETcounter = IETresetCounter;

            if (TopNodeTemp is null && this is ITopNode)
            {
                TopNodeTemp = (ITopNode)this;
                sdcTopType = this.GetType().Name.ToEnum<SdcTopNodeTypesEnum>();
                //if (sdcTreeBuilder == null) sdcTreeBuilder = new SDCTreeBuilder();  //we create SDCTreeBuilder only in the top node
            }
            else if (TopNodeTemp != null)
            {
                //We can check to see if a nested ITopNode type (e.g., another FormDesignType) has been created at this point.
                //It's not clear that we need to handle this any differently
                //sdcTreeBuilder = ((BaseType)TopNodeTemp).sdcTreeBuilder;
            }
            else throw new InvalidOperationException("TopNodeTemp was null and the Top Node did not implement ITopNode.");
            TopNode = TopNodeTemp;
            ObjectID = TopNode.MaxObjectID++;
            TopNode.Nodes.Add(ObjectGUID, this); //Register This Node
            //TopObject.Nodes.TryGetValue(ObjectID - 1, out BaseType prevNode);
            //if (prevNode != null) TopObject.PreviousNodes.Add(ObjectID, prevNode);

            order = ObjectID;
            //_elementName = SdcUtil.GetPropertyInfo(this).XmlElementName;

            //Debug.WriteLine($"The node with ObjectID: {this.ObjectID} has entered the BaseType ctor. Item type is {this.GetType()}.  "
            //    + $"The parent ObjectID is {this.ParentObjID.ToString()}");
        }

        //!+TODO: InitParentNodesFromXml should be moved out of BaseType, probably into ITopNNode or ISdcUtil
        private static T InitParentNodesFromXml<T>(string sdcXml, T obj) where T : ITopNode 
        {
            //read as XMLDocument to walk tree
            var x = new System.Xml.XmlDocument();
            x.LoadXml(sdcXml);
            XmlNodeList xmlNodeList = x.SelectNodes("//*");

            var dX_obj = new Dictionary<int, Guid>(); //the index is iXmlNode, value is FD ObjectGUID
            int iXmlNode = 0;
            XmlNode xmlNode;

            foreach (BaseType bt in obj.Nodes.Values)
            {   //As we interate through the nodes, we will need code to skip over any non-element node, 
                //and still stay in sync with FD (using iFD). For now, we assume that every nodeList node is an element.
                //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?view=netframework-4.8
                //https://docs.microsoft.com/en-us/dotnet/standard/data/xml/types-of-xml-nodes
                xmlNode = xmlNodeList[iXmlNode];
                while (xmlNode.NodeType.ToString() != "Element")
                {
                    iXmlNode++;
                    xmlNode = xmlNodeList[iXmlNode];
                }
                //Create a new attribute node to hold the node's index in xmlNodeList
                XmlAttribute a = x.CreateAttribute("index");
                a.Value = iXmlNode.ToString();
                var e = (XmlElement)xmlNode;
                e.SetAttributeNode(a);

                //Set the correct Element Name, in case we have errors in the SDC object tree logic
                bt.ElementName = e.LocalName;

                //Create  dictionary to track the matched indexes of the XML and FD node collections
                dX_obj[iXmlNode] = bt.ObjectGUID;
                //Debug.Print("iXmlNode: " + iXmlNode + ", ObjectID: " + bt.ObjectID);

                //Search for parents:
                int parIndexXml = -1;
                Guid parObjectGUID = default;
                bool parExists = false;
                BaseType btPar;
                XmlNode parNode;
                btPar = null;

                parNode = xmlNode.ParentNode;
                parExists = int.TryParse(parNode?.Attributes?.GetNamedItem("index")?.Value, out parIndexXml);//The index of the parent XML node
                if (parExists)
                {
                    parExists = dX_obj.TryGetValue(parIndexXml, out parObjectGUID);// find the matching parent SDC node Object ID
                    if (parExists) { parExists = obj.Nodes.TryGetValue(parObjectGUID, out btPar); } //Find the parent node in FD
                    if (parExists)
                    {
                        //bt.IsLeafNode = true;
                        bt.RegisterParent(btPar);
                        //Debug.WriteLine($"The node with ObjectID: {bt.ObjectID} is leaving InitializeNodesFromSdcXml. Item type is {bt.GetType().Name}.  " +
                        //            $"Parent ObjectID is {bt?.ParentID}, ParentIETypeID: {bt?.ParentIETypeID}, ParentType: {btPar.GetType().Name}");
                    }
                    else { throw new KeyNotFoundException("No parent object was returned from the SDC tree"); }
                }
                else
                {
                    //bt.IsLeafNode = false;
                    //Debug.WriteLine($"The node with ObjectID: {bt.ObjectID} is leaving InitializeNodesFromSdcXml. Item type is {bt.GetType()}.  " +
                    //                $", No Parent object exists");
                }

                iXmlNode++;
            }
            return obj;

        }
        #endregion


        //TODO: why are these internal static methods in BaseType?  Should they be in SdcUtil or another helper class?
        //Answer: Because they operate on the SDC Type itself, not on an object instance.  
        //If they are in the BaseType class, they don't need to be copied into all the ITopNode classes.
        #region Serialization

        //!+XML
        protected static ITopNode GetSdcObjectFromXmlPath<T>(string path) where T : ITopNode
        {
            string sdcXml = System.IO.File.ReadAllText(path);  // System.Text.Encoding.UTF8);
            return GetSdcObjectFromXml<T>(sdcXml);
        }
        protected static T GetSdcObjectFromXml<T>(string sdcXml) where T : ITopNode
        {
            T obj = SdcSerializer<T>.Deserialize(sdcXml);
            //return InitParentNodesFromXml<T>(sdcXml, obj);
            SdcUtil.ReflectNodeDictionariesOrdered(obj);
            return obj;
        }
        //!+JSON
        protected static ITopNode GetSdcObjectFromJsonPath<T>(string path) where T : ITopNode
        {
            string sdcJson = System.IO.File.ReadAllText(path);
            return GetSdcObjectFromJson<T>(sdcJson);
        }
        protected static ITopNode GetSdcObjectFromJson<T>(string sdcJson) where T : ITopNode
        {
            T obj = SdcSerializerJson<T>.DeserializeJson<T>(sdcJson);
            //return InitParentNodesFromXml<T>(sdcXml, obj);
            SdcUtil.ReflectNodeDictionariesOrdered(obj);
            return obj;
        }
        //!+MsgPack
        protected static ITopNode GetSdcObjectFromMsgPackPath<T>(string path) where T : ITopNode
        {
            byte[] sdcMsgPack = System.IO.File.ReadAllBytes(path);
            return GetSdcObjectFromMsgPack<T>(sdcMsgPack);
        }
        protected static ITopNode GetSdcObjectFromMsgPack<T>(byte[] sdcMsgPack) where T : ITopNode
        {
            T obj = SdcSerializerMsgPack<T>.DeserializeMsgPack(sdcMsgPack);
            //return InitParentNodesFromXml<T>(sdcXml, obj);
            SdcUtil.ReflectNodeDictionariesOrdered(obj);
            return obj;
        }


        #endregion  
        ~BaseType() //destructor
        {}
    }

    public partial class ExtensionBaseType : IExtensionBase
    {
        protected ExtensionBaseType() { }
        public ExtensionBaseType(BaseType parentNode) : base(parentNode)
        {  }

    }

    #region IExtensionBaseTypeMember
    public partial class ExtensionType : IExtensionBaseTypeMember
    {
        private IExtensionBaseTypeMember Iebtm { get => (IExtensionBaseTypeMember)this; }
        protected ExtensionType() { }
        public ExtensionType(BaseType parentNode) : base(parentNode) { }
        #region IExtensionBaseTypeMember
        //public bool Remove() => Iebtm.Remove();
        //public bool Move(ExtensionBaseType ebtTarget, int newListIndex = -1) => Iebtm.MoveI(this, ebtTarget, newListIndex);
        #endregion

    }
    public partial class PropertyType : IExtensionBaseTypeMember, IHtmlHelpers
    {
        private IExtensionBaseTypeMember Iebtm { get => (IExtensionBaseTypeMember)this; }
        protected PropertyType() { }
        public PropertyType(ExtensionBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "Property";
            ElementPrefix = "p";
            SetNames(elementName, elementPrefix);
        }

        public HTML_Stype AddHTML()
        {
            this.TypedValue = new DataTypes_SType(this);
            var rtf = new RichTextType(TypedValue);
            var h = (this as IHtmlHelpers).AddHTML(rtf);
            return h;
        }

        #region IExtensionBaseTypeMember
        //public bool Remove() => Iebtm.Remove();
        //public bool Move(ExtensionBaseType ebtTarget, int newListIndex = -1) => Iebtm.MoveI(this, ebtTarget, newListIndex);
        #endregion

    }
    public partial class CommentType : IExtensionBaseTypeMember
    {
        private IExtensionBaseTypeMember Iebtm { get => (IExtensionBaseTypeMember)this; }
        protected CommentType() { }
        public CommentType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "cmt";
            SetNames(elementName, elementPrefix);
        }

        #region IExtensionBaseTypeMember
        //public bool Remove() => Iebtm.Remove();
        //public bool Move(ExtensionBaseType ebtTarget, int newListIndex = -1) => Iebtm.MoveI(this, ebtTarget, newListIndex);
        #endregion

    }
    #endregion

    public partial class IdentifiedExtensionType: IIdentifiedExtensionType
    {
        protected IdentifiedExtensionType() { }
        protected IdentifiedExtensionType(BaseType parentNode, string id = "") : base(parentNode)
        {
            if (!string.IsNullOrWhiteSpace(id))
                this.ID = id;
            else if (this.ObjectGUID != null)
                this.ID = this.ObjectGUID.ToString();
        }

       
    }

    public partial class RepeatingType //this is an SDC abstract class
    {
        protected RepeatingType() { }
        protected RepeatingType(BaseType parentNode, string id = "") : base(parentNode, id)
        {
            this._minCard = ((ushort)(1));
            this._maxCard = ((ushort)(1));
            this._repeat = "0";
        }

    }

    public partial class ChildItemsType
    {
        protected ChildItemsType() { }
        public ChildItemsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "ch";
            SetNames(elementName, elementPrefix);
        }

        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public List<IdentifiedExtensionType> ChildItemsList
        {
            get { return this.Items; }
            set { this.Items = value; }
        }

        bool Remove(int NodeIndex)
        {
            var node = ChildItemsList[NodeIndex];
            if (node != null) return node.Remove();
            return false;

        }
    }

    #endregion

    #region DisplayedType and Members

    public partial class DisplayedType : IChildItemsMember<DisplayedType> //, IQuestionListMember
    {
        protected DisplayedType() { }
        public DisplayedType(BaseType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            this._enabled = true;
            this._visible = true;
            this._mustImplement = true;
            this._showInReport = DisplayedTypeShowInReport.True;
            ElementName = "DisplayedItem";
            ElementPrefix = "DI";
            SetNames(elementName, elementPrefix);
        }

        #region IDisplayedType
        //IDisplayedType idt { get => this as IDisplayedType; }
        //public LinkType AddLink(int insertPosition = -1)
        //{ return idt.AddLinkI(insertPosition); }
        //public BlobType AddBlob(int insertPosition = -1)
        //{ return idt.AddBlobI(insertPosition); }
        //public ContactType AddContact(int insertPosition = -1)
        //{ return idt.AddContactI(insertPosition); }
        //public CodingType AddCodedValue(int insertPosition = -1)
        //{ return idt.AddCodedValueI(insertPosition); }
        #endregion

        #region DisplayedType Events
        //public OnEventType AddOnEvent()
        //{ return idt.AddOnEventI(); }
        //public EventType AddOnEnter()
        //{ return idt.AddOnEnterI(); }
        //public EventType AddOnExit()
        //{ return idt.AddOnExitI(); }
        //public PredGuardType AddActivateIf()
        //{ return idt.AddActivateIfI(); }
        //public PredGuardType AddDeActivateIf()
        //{ return idt.AddDeActivateIfI(); }
        //public bool MoveEvent(EventType ev, List<EventType> targetList = null, int index = -1)
        //{ return idt.MoveEventI(ev, targetList, index); }
        #endregion

        //#region IChildItemMember
        //public bool Remove() => sdcTreeBuilder.Remove(this);
        //public bool Move<T>(T target, int newListIndex) where T : DisplayedType, IChildItemsParent
        //    => sdcTreeBuilder.MoveAsChild(this, target, newListIndex);
        //#endregion

        #region IQuestionListMember
        //IQuestionListMember qlm { get => this as IQuestionListMember; }
        ////Explicit implementaion prevents this interface from being inherited directly by subclasses.
        //public bool Remove(bool removeDecendants) => qlm.RemoveI(removeDecendants);
        //public bool IsMoveAllowedToList(QuestionItemType target, out string error)=> qlm.IsMoveAllowedToListI(target, out error);
        //public bool MoveToList(ListType targetList, out string errList, int newListIndex = -1)
        //    => qlm.MoveToListI(targetList, out errList, newListIndex);
        //public bool MoveToList(ListItemType dropTarget, out string errList, int newListIndex = -1) //not part of interface
        //    => qlm.MoveToListI(dropTarget.ParentNode as ListType, out errList, newListIndex);
        //public bool MoveInList(out string errList, int newListIndex = -1)
        //{
        //    if (!(this.ParentNode is ListType))
        //    {
        //        errList = "The parent node must be List.  It cannot be ChildItems";
        //        return false;
        //    }

        //    return qlm.MoveInListI(out errList, newListIndex);
        //}
        //public bool MoveInList(DisplayedType source, DisplayedType target, bool moveAbove)
        //{
        //    if (!(this.ParentNode is ListType))
        //    {
        //        var err = "The parent node must be List.  It cannot be ChildItems";
        //        return false;
        //    }

        //    return qlm.MoveInListI(source, target, moveAbove);
        //}
        //public bool MoveInList(DisplayedType source, QuestionItemType target, bool moveAbove)
        //{
        //    if (!(this.ParentNode is ListType))
        //    {
        //        var err = "The parent node must be List.  It cannot be ChildItems";
        //        return false;
        //    }

        //    return qlm.MoveInListI(source, target, moveAbove);
        //}
        //public ListItemType ConvertToLI(bool testOnly) => qlm.ConvertToLI_I(testOnly);
        //public DisplayedType ConvertToDI(bool testOnly) => qlm.ConvertToDI_I(testOnly);
        //public ListItemType ConvertToLIR(bool testOnly) => qlm.ConvertToLIR_I(testOnly);
        #endregion

    }

    #region DisplayedType Members

    public partial class BlobType : IDisplayedTypeMember
    {
        protected BlobType() { }
        public BlobType(DisplayedType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "Blob";
            ElementPrefix = "blob";
            SetNames(elementName, elementPrefix);
        }
    }
    public partial class LinkType : IDisplayedTypeMember
    {
        protected LinkType() { }
        public LinkType(DisplayedType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "Link";
            ElementPrefix = "link";
            SetNames(elementName, elementPrefix);
        }
    }

    #region Coding
    public partial class CodingType : IDisplayedTypeMember
    {
        protected CodingType() { }
        public CodingType(ExtensionBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "CodedValed";
            ElementPrefix = "cval";
            SetNames(elementName, elementPrefix);

        }
    }

    public partial class CodeMatchType
    {
        protected CodeMatchType() { }
        public CodeMatchType(CodingType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._codeMatchEnum = CodeMatchTypeCodeMatchEnum.ExactCodeMatch;
            ElementName = "CodeMatch";
            ElementPrefix = "cmat";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected CodeMatchType() { }
    }

    public partial class CodeSystemType
    {
        protected CodeSystemType() { }
        public CodeSystemType(ExtensionBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "CodeSystem";
            ElementPrefix = "csys";
            SetNames(elementName, elementPrefix);
        }
    }

    #endregion

    #endregion


    #endregion

    #region DataTypes
    public partial class DataTypes_DEType
    {
        protected DataTypes_DEType() { }
        public DataTypes_DEType(ResponseFieldType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "Response"; //response element
            ElementPrefix = "rsp";  //response element            
            SetNames(elementName, elementPrefix);
            //if (fillData) sdcTreeBuilder.AddFillDataTypesDE(parentNode);
        }

        /// <summary>
        /// any *_DEType data type
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public BaseType DataTypeDE_Item
        {
            get { return this.Item; }
            set { this.Item = value; }
        }
    }

    public partial class anyType_DEtype
    {
        protected anyType_DEtype() { }
        public anyType_DEtype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "any";
            SetNames(elementName, elementPrefix);
        }
    }


    public partial class DataTypes_SType
    {
        protected DataTypes_SType() { }
        public DataTypes_SType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "DataTypes";
            SetNames(elementName, elementPrefix);
        }

        /// <summary>
        /// any *_SType data type
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public BaseType DataTypeS_Item
        {
            get { return this.Item; }
            set { this.Item = value; }
        }
    }

    public partial class anyURI_DEtype
    {
        protected anyURI_DEtype() { }
        public anyURI_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "uri";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class anyURI_Stype
    {
        protected anyURI_Stype() { }
        public anyURI_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "uri";
            SetNames(elementName, elementPrefix);

        }
    }

    public partial class base64Binary_DEtype
    {
        protected base64Binary_DEtype() { }
        public base64Binary_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "b64";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class base64Binary_Stype
    {
        protected base64Binary_Stype() { }
        public base64Binary_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "b64";
            SetNames(elementName, elementPrefix);

        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "string")] //changed to string
        public string valBase64 { get; set; }
    }

    public partial class boolean_DEtype
    {
        protected boolean_DEtype() { }
        public boolean_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "bool";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class boolean_Stype
    {
        protected boolean_Stype() { }
        public boolean_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "bool";
            SetNames(elementName, elementPrefix);

        }
    }

    public partial class byte_DEtype
    {
        protected byte_DEtype() { }
        public byte_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "byte";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class byte_Stype
    {
        protected byte_Stype() { }
        public byte_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "byte";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class date_DEtype
    {
        protected date_DEtype() { }
        public date_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "date";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class date_Stype
    {
        protected date_Stype() { }
        public date_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "date";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class dateTime_DEtype
    {
        protected dateTime_DEtype() { }
        public dateTime_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "dt";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class dateTime_Stype
    {
        protected dateTime_Stype() { }
        public dateTime_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dt";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class dateTimeStamp_DEtype
    {
        protected dateTimeStamp_DEtype() { }
        public dateTimeStamp_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "dts";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class dateTimeStamp_Stype
    {
        protected dateTimeStamp_Stype() { }
        public dateTimeStamp_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "dts";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class dayTimeDuration_DEtype
    {
        protected dayTimeDuration_DEtype() { }
        public dayTimeDuration_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "dtdur";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class dayTimeDuration_Stype
    {
        protected dayTimeDuration_Stype() { }
        public dayTimeDuration_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dtdur";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class decimal_DEtype
    {
        protected decimal_DEtype() { }
        public decimal_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "dec";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class decimal_Stype
    {
        protected decimal_Stype() { }
        public decimal_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dec";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class double_DEtype
    {
        protected double_DEtype() { }
        public double_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "dbl";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class double_Stype
    {
        protected double_Stype() { }
        public double_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dbl";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class duration_DEtype
    {
        protected duration_DEtype() { }
        public duration_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "dur";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class duration_Stype
    {
        protected duration_Stype() { }
        public duration_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class float_DEtype
    {
        protected float_DEtype() { }
        public float_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "flt";
            //SetNames(elementName, elementPrefix);

        }
    }

    public partial class float_Stype
    {
        protected float_Stype() { }
        public float_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "flt";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class gDay_DEtype
    {
        protected gDay_DEtype() { }
        public gDay_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "day";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class gDay_Stype
    {
        protected gDay_Stype() { }
        public gDay_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "day";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class gMonth_DEtype
    {
        protected gMonth_DEtype() { }
        public gMonth_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "mon";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class gMonth_Stype
    {
        protected gMonth_Stype() { }
        public gMonth_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "mon";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class gMonthDay_DEtype
    {
        protected gMonthDay_DEtype() { }
        public gMonthDay_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "mday";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class gMonthDay_Stype
    {
        protected gMonthDay_Stype() { }
        public gMonthDay_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "mday";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class gYear_DEtype
    {
        protected gYear_DEtype() { }
        public gYear_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "y";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class gYear_Stype
    {
        protected gYear_Stype() { }
        public gYear_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "y";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class gYearMonth_DEtype
    {
        protected gYearMonth_DEtype() { }
        public gYearMonth_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "ym";
            //SetNames(elementName, elementPrefix);
        }
    }
    public partial class gYearMonth_Stype
    {
        protected gYearMonth_Stype() { }
        public gYearMonth_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ym";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class hexBinary_DEtype
    {
        protected hexBinary_DEtype() { }
        public hexBinary_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "hexb";
            //SetNames(elementName, elementPrefix);
        }

    }

    public partial class hexBinary_Stype
    {
        string _hexBinaryStringVal;

        protected hexBinary_Stype() { }
        public hexBinary_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "hexb";
            SetNames(elementName, elementPrefix);
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "string")] //changed to string
        public string valHex
        {
            get { return _hexBinaryStringVal; }
            set { _hexBinaryStringVal = value; }
        }
    }

    public partial class HTML_DEtype
    {
        protected HTML_DEtype()
        { this.Any = new List<System.Xml.XmlElement>(); }
        public HTML_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "html";
            //SetNames(elementName, elementPrefix);
            //this.Any = new List<System.Xml.XmlElement>();
        }
    }

    public partial class HTML_Stype
    {
        protected HTML_Stype()
        { this.Any = new List<System.Xml.XmlElement>(); }
        public HTML_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "html";
            SetNames(elementName, elementPrefix);
            //this.Any = new List<System.Xml.XmlElement>();
        }

    }


    public partial class int_DEtype
    {
        protected int_DEtype() { }
        public int_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "int";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class int_Stype
    {
        protected int_Stype() { }
        public int_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "int";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected int_Stype() { }
    }

    public partial class integer_DEtype
    {
        protected integer_DEtype() { }
        public integer_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;            //ElementPrefix = "intr";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class integer_Stype
    {
        protected integer_Stype() { }
        public integer_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "intr";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected integer_Stype() { }


        /// <summary>
        /// Added to support proper decimal set/get; 
        /// Decimal is the best data type to match W3C integer.
        /// This will not work with XML serializer, bc decimal types always serialize trailing zeros,
        /// and trailing zeros are not allowed with integer types
        /// TODO: Need to truncate (or possibly round) any digits after the decimal point in the setter/getter
        /// For positive/negative etc integers, need to check the sign and throw error if incorrect.
        /// May want to throw errors for for min/max allowed values also - not sure about this)
        /// May need to import System.Numerics.dll to use BigInteger
        /// </summary>
        /// 
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public virtual decimal? valDec
        //rlm 2/11/18 probably don't want to use this
        {
            get
            {
                if (val != null && val.Length > 0)
                { return Convert.ToDecimal(this.val); }
                return null;
            }
            set
            {
                if (value != null)
                { this.val = Math.Truncate(value.Value).ToString(); }
                else this.val = null;

            }
        }

    }

    public partial class long_DEtype
    {
        protected long_DEtype() { }
        public long_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "lng";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class long_Stype
    {
        protected long_Stype() { }
        public long_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "lng";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class negativeInteger_DEtype
    {
        protected negativeInteger_DEtype() { }
        public negativeInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "nint";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class negativeInteger_Stype
    {
        protected negativeInteger_Stype() { }
        public negativeInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "nint";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class nonNegativeInteger_DEtype
    {
        protected nonNegativeInteger_DEtype() { }
        public nonNegativeInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "nnint";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class nonNegativeInteger_Stype
    {
        protected nonNegativeInteger_Stype() { }
        public nonNegativeInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "nnint";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class nonPositiveInteger_DEtype
    {
        protected nonPositiveInteger_DEtype() { }
        public nonPositiveInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "npint";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class nonPositiveInteger_Stype
    {
        protected nonPositiveInteger_Stype() { }
        public nonPositiveInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "npint";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class positiveInteger_DEtype
    {
        protected positiveInteger_DEtype() { }
        public positiveInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "pint";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class positiveInteger_Stype
    {
        protected positiveInteger_Stype() { }
        public positiveInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "pint";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class short_DEtype
    {
        protected short_DEtype() { }
        public short_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "sh";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class short_Stype
    {
        protected short_Stype() { }
        public short_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "sh";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class string_DEtype
    {
        protected string_DEtype() { }
        public string_DEtype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "str";
            //SetNames(elementName, elementPrefix);
        } //{if (elementName.Length > 0) ElementName = elementName; }
    }

    public partial class string_Stype
    {
        protected string_Stype() { }
        public string_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "str";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class time_DEtype
    {
        protected time_DEtype() { }
        public time_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "tim";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class time_Stype
    {
        protected time_Stype() { }
        public time_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "tim";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected time_Stype() { }
    }

    public partial class unsignedByte_DEtype
    {
        protected unsignedByte_DEtype() { }
        public unsignedByte_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "ubyte";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class unsignedByte_Stype
    {
        protected unsignedByte_Stype() { }
        public unsignedByte_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ubyte";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected unsignedByte_Stype() { }
    }

    public partial class unsignedInt_DEtype
    {
        protected unsignedInt_DEtype() { }
        public unsignedInt_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "unint";
            //SetNames(elementName, elementPrefix);
        }

    }

    public partial class unsignedInt_Stype
    {
        protected unsignedInt_Stype() { }
        public unsignedInt_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "uint";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected unsignedInt_Stype() { }
    }

    public partial class unsignedLong_DEtype
    {
        protected unsignedLong_DEtype() { }
        public unsignedLong_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "ulng";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class unsignedLong_Stype
    {
        protected unsignedLong_Stype() { }
        public unsignedLong_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {

            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ulng";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected unsignedLong_Stype() { }
    }

    public partial class unsignedShort_DEtype
    {
        protected unsignedShort_DEtype() { }
        public unsignedShort_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;

            //ElementPrefix = "ush";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class unsignedShort_Stype
    {
        protected unsignedShort_Stype() { }
        public unsignedShort_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {

            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ush";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected unsignedShort_Stype() { }
    }

    public partial class XML_DEtype
    {
        protected XML_DEtype() { }//this.Any = new List<XmlElement>(); }
        public XML_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            //ElementPrefix = "xml";
            //SetNames(elementName, elementPrefix);
            //this.Any = new List<XmlElement>();
        }
    }

    public partial class XML_Stype
    {
        protected XML_Stype()
        { this.Any = new List<XmlElement>(); }
        public XML_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {

            ElementPrefix = "xml";
            SetNames(elementName, elementPrefix);
            this.Any = new List<XmlElement>();
        }
    }

    public partial class yearMonthDuration_DEtype
    {
        protected yearMonthDuration_DEtype() { }
        public yearMonthDuration_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
            //ElementPrefix = "ymd";
            //SetNames(elementName, elementPrefix);
        }
    }

    public partial class yearMonthDuration_Stype
    {
        protected yearMonthDuration_Stype() { }
        public yearMonthDuration_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ymd";
            SetNames(elementName, elementPrefix);
        }
        //!+Replaced in original class: protected yearMonthDuration_Stype() { }
    }
    #endregion

    #region Rules

    public partial class ItemNameType
    {
        protected ItemNameType() { }
        public ItemNameType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "itnm";
            SetNames(elementName, elementPrefix);
        }
    }
    public partial class ItemNameAttributeType
    {
        protected ItemNameAttributeType() { }

        public ItemNameAttributeType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._attributeName = "val";
        }
    }
    public partial class NameType
    {
        protected NameType() { }
        public NameType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "nm";
            SetNames(elementName, elementPrefix);
        }
    }
    public partial class TargetItemIDType
    {
        protected TargetItemIDType() { }
        public TargetItemIDType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "tiid";
            SetNames(elementName, elementPrefix);
        }
    }
    public partial class TargetItemNameType
    {
        protected TargetItemNameType() { }
        public TargetItemNameType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "tinm";
            SetNames(elementName, elementPrefix);
        }
    }
    public partial class TargetItemXPathType
    {
        protected TargetItemXPathType() { }
        public TargetItemXPathType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "tixp";
            SetNames(elementName, elementPrefix);
        } //{if (elementName.Length > 0) ElementName = elementName; }
    }
    public partial class ListItemParameterType
    {
        protected ListItemParameterType() { }
        public ListItemParameterType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {

            this._dataType = "string";
            this._listItemAttribute = "associatedValue";
        }

    }
    public partial class ParameterItemType
    {
        protected ParameterItemType() { }
        public ParameterItemType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._dataType = "string";
            this._sourceItemAttribute = "val";
        }

    }
    public partial class PredAlternativesType
    {
        public PredAlternativesType() { }
        public PredAlternativesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
            this._minAnswered = 1;
            this._maxAnswered = 0;
        }
    }
    public partial class PredEvalAttribValuesType
    {
        protected PredEvalAttribValuesType() { }
        public PredEvalAttribValuesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
            this._boolOp = PredEvalAttribValuesTypeBoolOp.AND;
        }
    }
    public partial class PredGuardTypeSelectionSets
    {
        protected PredGuardTypeSelectionSets() { }
        public PredGuardTypeSelectionSets(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
        }
    }
    public partial class PredSingleSelectionSetsType
    {
        protected PredSingleSelectionSetsType() { }
        public PredSingleSelectionSetsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._maxSelections = ((short)(1));
        }
    }
    public partial class RuleAutoActivateType
    {
        protected RuleAutoActivateType() { }
        public RuleAutoActivateType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._onlyIf = false;
            this._setVisibility = toggleType.@true;
            this._setEnabled = toggleType.@true;
            this._setExpanded = toggleType.@true;
            this._x_removeResponsesWhenDeactivated = false;
        }
    }
    public partial class RuleAutoSelectType
    {
        protected RuleAutoSelectType() { }
        public RuleAutoSelectType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._onlyIf = false;
        }
    }
    public partial class RuleListItemMatchTargetsType
    {
        protected RuleListItemMatchTargetsType() { }
        public RuleListItemMatchTargetsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._attributeToMatch = RuleListItemMatchTargetsTypeAttributeToMatch.associatedValue;
        }
    }
    public partial class SelectionSetsActionType
    {
        protected SelectionSetsActionType() { }
        public SelectionSetsActionType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
        }
    }
    public partial class ValidationTypeSelectionSets
    {
        protected ValidationTypeSelectionSets() { }
        public ValidationTypeSelectionSets(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
        }
    }
    public partial class ValidationTypeSelectionTest
    {
        protected ValidationTypeSelectionTest() { }
        public ValidationTypeSelectionTest(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
        }
    }
    public partial class PredSelectionTestType
    {
        protected PredSelectionTestType() { }
        public PredSelectionTestType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        { }
    }
    public partial class CallFuncType
    {
        protected CallFuncType() { }
        public CallFuncType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._dataType = "string";
        }

    }
    partial class CallFuncBaseType
    {
        protected CallFuncBaseType() { }
        public CallFuncBaseType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._returnList = false;
            this._listDelimiter = "|";
            this._allowNull = true;
        }
    }
    partial class CallFuncBoolType
    {
        protected CallFuncBoolType() { }
        public CallFuncBoolType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
        }
    }


    #endregion
    #region PredActions
    //AttributeEval -       AttributeEvalActionType (actions)
    //ScriptBoolFunc -      ScriptBoolFuncActionType (actions)
    //CallBoolFunction -    CallFuncBoolActionType (actions)
    //MultiSelections -     MultiSelectionsActionType
    //SelectionSets -       SelectionSetsActionType (rule)
    //SelectionTest -       SelectionTestActionType
    //Group -               PredActionType (events)
    //SelectMatchingListItems - RuleSelectMatchingListItemsType (actions)
    public partial class MultiSelectionsActionType
    {
        protected MultiSelectionsActionType()
        { }
        public MultiSelectionsActionType(BaseType parentNode) : base(parentNode)
        { }
    }
    public partial class SelectionTestActionType
    {
        protected SelectionTestActionType()
        { }
        public SelectionTestActionType(BaseType parentNode) : base(parentNode)
        { }
    }

    public partial class PredMultiSelectionSetBoolType
    {
        protected PredMultiSelectionSetBoolType() { }
        public PredMultiSelectionSetBoolType(BaseType parentNode) : base(parentNode)
        { }
    }


    #endregion
    #region  Actions

    public partial class ActionsType : IActions
    {
        protected ActionsType() {}
        public ActionsType(ExtensionBaseType parentNode) : base(parentNode) { ElementName = "Actions"; ElementPrefix = "act"; }

      
    }
    public partial class ActActionType
    {
        protected ActActionType() { }
        public ActActionType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "Action"; }
        [XmlIgnore]
        public List<ExtensionBaseType> ActAction_Items
        {
            get { return Items; }
            set
            {
                if (Items == value)
                    return;
                Items = value;
                OnPropertyChanged(nameof(ActAction_Items), this);
            }
        }

    }
    public partial class RuleSelectMatchingListItemsType
    {
        protected RuleSelectMatchingListItemsType() { }
        public RuleSelectMatchingListItemsType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "SelectMatchingListItems"; }
    }
    public partial class ActAddCodeType
    {
        protected ActAddCodeType() { }
        public ActAddCodeType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "AddCode"; }

    }
    public partial class ActInjectType : InjectFormType
    {
        protected ActInjectType() { }
        public ActInjectType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "Inject"; }

    }
    public partial class ActSaveResponsesType
    {
        protected ActSaveResponsesType() { }
        public ActSaveResponsesType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "Save"; }
    }
    public partial class ActSendReportType
    {
        protected ActSendReportType() { }
        public ActSendReportType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "SendReport"; }

        internal List<ExtensionBaseType> Email_Phone_WebSvc_List
        {
            get { return this.Items; }
            set { this.Items = value; }
        }
    }
    public partial class ActSendMessageType
    {
        protected ActSendMessageType() { }
        public ActSendMessageType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "SendMessage"; } //"SendMessage111" in Schema

        /// <summary>
        /// List&lt;BaseType> accepts: EmailAddressType, PhoneNumberType, WebServiceType
        /// </summary>
        internal List<ExtensionBaseType> Email_Phone_WebSvc_List
        {
            get { return this.Items; }
            set { this.Items = value; }
        }
    }
    public partial class ActSetAttributeType
    {
        protected ActSetAttributeType() { }
        public ActSetAttributeType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "SetAttributeValue"; }
    }
    public partial class ActSetAttrValueScriptType
    {
        protected ActSetAttrValueScriptType() { }
        public ActSetAttrValueScriptType(ActionsType parentNode) : base(parentNode) 
        { ElementName = "SetAttributeValueScript"; }
    }
    public partial class ActSetBoolAttributeValueCodeType
    {
        protected ActSetBoolAttributeValueCodeType()
        {
            ElementName = "SetBoolAttributeValueCode";
            this._attributeName = "val";
        }
        public ActSetBoolAttributeValueCodeType(ActionsType parentNode) : base(parentNode)
        {
            ElementName = "SetBoolAttributeValueCode";
            this._attributeName = "val";
        }
    }
    public partial class ScriptCodeBoolType
    {
        protected ScriptCodeBoolType()
        {
            ElementName = "";
            this._not = false;
        }
        public ScriptCodeBoolType(ActionsType parentNode) : base(parentNode)
        {
            ElementName = "";
            this._not = false;
        }
    }
    public partial class ActShowFormType
    {
        protected ActShowFormType() { }
        public ActShowFormType(ActionsType parentNode) : base(parentNode) { ElementName = "ShowForm"; }
    }
    public partial class ActShowMessageType
    {
        protected ActShowMessageType() { }
        public ActShowMessageType(ActionsType parentNode) : base(parentNode) { ElementName = "ShowMessage"; }
    }
    public partial class ActShowReportType
    {
        protected ActShowReportType() { }
        public ActShowReportType(ActionsType parentNode) : base(parentNode) { ElementName = "ShowReport"; }
    }
    public partial class ActPreviewReportType
    {
        protected ActPreviewReportType() { }
        public ActPreviewReportType(ActionsType parentNode) : base(parentNode) { ElementName = "PreviewReport"; }
    }
    public partial class ActValidateFormType
    {
        protected ActValidateFormType() { }
        public ActValidateFormType(ActionsType parentNode) : base(parentNode)
        {
            ElementName = "ValidateForm";
            this._validateDataTypes = false;
            this._validateRules = false;
            this._validateCompleteness = false;
        }

        public ActValidateFormType Fill_ActValidateFormType()
        { return null; }
    }

    public partial class ScriptBoolFuncActionType
    {
        protected ScriptBoolFuncActionType() { }
        public ScriptBoolFuncActionType(ActionsType parentNode) : base(parentNode) { ElementName = "ScriptBoolFunc"; }
    }

    public partial class ScriptCodeAnyType
    {
        protected ScriptCodeAnyType()
        {
            this._dataType = "string";
        }
        public ScriptCodeAnyType(ActionsType parentNode) : base(parentNode)
        {
            ElementName = "RunCode";
            this._dataType = "string";
        }
    }
    public partial class ScriptCodeBaseType
    {
        protected ScriptCodeBaseType()
        {
            ElementName = "";
            this._returnList = false;
            this._listDelimiter = "|";
            this._allowNull = true;
        }

        public ScriptCodeBaseType(ActionsType parentNode) : base(parentNode)
        {
            ElementName = "";
            this._returnList = false;
            this._listDelimiter = "|";
            this._allowNull = true;
        }
    }
    public partial class CallFuncActionType
    {
        protected CallFuncActionType() { }
        public CallFuncActionType(ActionsType parentNode) : base(parentNode) { ElementName = "CallFunction"; }
    }


    public partial class CallFuncBoolActionType
    {
        protected CallFuncBoolActionType() { }
        public CallFuncBoolActionType(ActionsType parentNode) : base(parentNode) { ElementName = "CallBoolFunction"; }
    }










    #endregion
    #region Events
    public partial class OnEventType : IDisplayedTypeMember
    {
        protected OnEventType() { }
        public OnEventType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "onev";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class RulesType
    {
        protected RulesType() { }
        public RulesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "rul";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class EventType : IDisplayedTypeMember
    {
        protected EventType() { }
        public EventType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "evnt";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class PredGuardType : IDisplayedTypeMember
    {

        protected PredGuardType() { }
        public PredGuardType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
            this._boolOp = PredEvalAttribValuesTypeBoolOp.AND;
        }
    }




    public partial class PredActionType
    {
        protected PredActionType() { }
        public PredActionType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._not = false;
            this._boolOp = PredEvalAttribValuesTypeBoolOp.AND;

            ElementPrefix = "cga";
            SetNames(elementName, elementPrefix);
        }

    }

    public partial class FuncBoolBaseType
    {
        protected FuncBoolBaseType() { }
        public FuncBoolBaseType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this._allowNull = true;
            ElementPrefix = "fbb";
            SetNames(elementName, elementPrefix);
        }

    }


    #endregion

    #region Contacts

    public partial class ContactType : IDisplayedTypeMember, IAddPerson, IAddOrganization
    {
        protected ContactType() { }
        public ContactType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "cntct";
            SetNames(elementName, elementPrefix);
        }

        //public PersonType AddPerson()
        //{
        //    return (this as IAddPerson).AddPersonI(this);
        //}
        //public OrganizationType AddOganization()
        //{
        //    return (this as IAddOrganization).AddOrganizationI(this);
        //}

    }

    public partial class OrganizationType
    {
        protected OrganizationType() { }
        public OrganizationType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "org";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class PersonType
    {
        protected PersonType() { }
        public PersonType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "pers";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class AddressType
    {
        protected AddressType() { }
        public AddressType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "adrs";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class AreaCodeType
    {
        protected AreaCodeType() { }
        public AreaCodeType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "arcd";
            SetNames(elementName, elementPrefix);
        }
    }
    #endregion

    #region Resources
    public partial class RichTextType : IHtmlHelpers
    {
        protected RichTextType() { }
        public RichTextType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "rtt";
            SetNames(elementName, elementPrefix);
        }

        IHtmlHelpers ihh { get => this; }
        public HTML_Stype AddHTML()
        {
            var h = ihh.AddHTML(this);
            return h;
        }
    }


    #endregion

    #region Classes that need ctor parameters

    #region RequestForm (Package)
    public partial class ComplianceRuleType
    {
        protected ComplianceRuleType() { }
        public ComplianceRuleType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "cr";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class SubmissionRuleType
    {
        protected SubmissionRuleType() { }
        public SubmissionRuleType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "sr";
            SetNames(elementName, elementPrefix);
        }
    }


    public partial class HashType
    {
        protected HashType() { }
        public HashType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "hsh";
            SetNames(elementName, elementPrefix);
        }
    }


    public partial class IdentifierType
    {
        protected IdentifierType() { }
        public IdentifierType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "id";
            SetNames(elementName, elementPrefix);

        }
    }

    public partial class LanguageCodeISO6393_Type
    {
        protected LanguageCodeISO6393_Type() { }
        public LanguageCodeISO6393_Type(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "lc";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class LanguageType
    {
        protected LanguageType() { }
        public LanguageType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "lng";
            SetNames(elementName, elementPrefix);

        }
    }

    public partial class ProvenanceType
    {
        protected ProvenanceType() { }
        public ProvenanceType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "prv";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class ReplacedIDsType
    {
        protected ReplacedIDsType() { }
        public ReplacedIDsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "rid";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class VersionType
    {
        protected VersionType() { }
        public VersionType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "ver";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class VersionTypeChanges
    {
        protected VersionTypeChanges() { }
        public VersionTypeChanges(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "vch";
            SetNames(elementName, elementPrefix);
        }
    }


    #endregion

    #region Contacts classes

    public partial class ContactsType
    {
        protected ContactsType() { }
        public ContactsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            this.ElementPrefix = "ctc";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class CountryCodeType
    {
        protected CountryCodeType() { }
        public CountryCodeType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "ctc";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class DestinationType
    {
        protected DestinationType() { }
        public DestinationType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "dst";
            SetNames(elementName, elementPrefix);

        }
    }


    public partial class PhoneNumberType
    {
        protected PhoneNumberType() { }
        public PhoneNumberType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "phn";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class PhoneType
    {
        protected PhoneType() { }
        public PhoneType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementName = "PhoneType";
            ElementPrefix = "pht";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class JobType
    {
        protected JobType() { }
        public JobType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "job";
            SetNames(elementName, elementPrefix);
        }
    }
    #endregion

    #region  Email
    public partial class EmailAddressType
    {
        protected EmailAddressType() { }
        public EmailAddressType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "emd";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class EmailType
    {
        protected EmailType() { }
        public EmailType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "em";
            SetNames(elementName, elementPrefix);
            //this.Usage = new string_Stype();
            //this.EmailClass = new string_Stype();
            //this.EmailAddress = new EmailAddressType();
        }
    }

    #endregion

    #region Files


    public partial class ApprovalType
    {
        protected ApprovalType() { }
        public ApprovalType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "app";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class AssociatedFilesType
    {
        protected AssociatedFilesType() { }
        public AssociatedFilesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "asf";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class AcceptabilityType
    {
        protected AcceptabilityType() { }
        public AcceptabilityType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "acc";
            SetNames(elementName, elementPrefix);
        }
    }


    public partial class FileDatesType
    {
        protected FileDatesType() { }
        public FileDatesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "fld";
            SetNames(elementName, elementPrefix);
        }
    }
    public partial class FileHashType
    {
        protected FileHashType() { }
        public FileHashType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "flh";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class FileType
    {
        protected FileType() { }
        public FileType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "fil";
            SetNames(elementName, elementPrefix);
        }
    }

    public partial class FileUsageType
    {
        protected FileUsageType() { }
        public FileUsageType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            ElementPrefix = "flu";
            SetNames(elementName, elementPrefix);
        }
    }

    #endregion

    #endregion

    #region Registry Summary Types

    #endregion


}

