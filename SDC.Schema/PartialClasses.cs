
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
        { init(); }
        public FormDesignType(BaseType parentNode = null, string id = "")
        : base(parentNode, id)
        //TODO: add ID, lineage, baseURI, version, etc to this constructor? (only ID is required)
        { init(); }
        private void init()
        {

        }

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
        { init(); }
        public DataElementType(string id = "") : base(null)
        {
            init();
            //TODO:Add dictionaries for nodes etc
            //TODO:Make sure BaseType constructor functions work
        }
        private void init()
        {

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
        { init(); }
        public RetrieveFormPackageType(string id = "") //: base(null, false)
        {
            init();//TODO:Make sure BaseType constructor functions work
        }
        private void init()
        {

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
        { init(); }
        public PackageListType(string id = "") //: base( null, false)
        {
            init();//TODO:Make sure BaseType constructor functions work
        }
        private void init()
        {

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
        protected ButtonItemType() { init(); }
        public ButtonItemType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode)
        {
            init(); 
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementName = "ButtonAction";
            ElementPrefix = "B";
        }

    }

    public partial class InjectFormType : IChildItemsMember<InjectFormType>
    {
        protected InjectFormType() { init(); }
        public InjectFormType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode, id)
        {
            init();
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._repeat = "0";
            ElementName = "InjectForm";
            ElementPrefix = "Inj";
        }
    }

    public partial class SectionBaseType
    {
        public SectionBaseType() { init(); }
        internal SectionBaseType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode, id)
        {
            init();
            
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._ordered = true;
            ElementName = "Section";
            ElementPrefix = "S";
        }

        //public void FillSectionBaseType()
        //{ sdcTreeBuilder.FillSectionBase(this); }
    }

    public partial class SectionItemType : IChildItemsParent<SectionItemType>, IChildItemsMember<SectionItemType>
    {
        protected SectionItemType() { init(); } //change back to protected
        public SectionItemType(BaseType parentNode, string id = "", string elementPrefix = "") : base(parentNode, id)
        { init(); }
        private void init()
        {

        }

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
        protected QuestionItemType() { init(); }  //need public parameterless constructor to support generics
        public QuestionItemType(BaseType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            init();
            this.readOnly = false;
            ElementName = "Question";
            ElementPrefix = "Q";
            SetNames(elementName, elementPrefix);
            //this.Conv
        }
        private void init()
        {

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
        protected QuestionItemBaseType() { init(); }
        public QuestionItemBaseType(BaseType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            init();
            this._readOnly = false;
        }
        private void init()
        {

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
        protected ListType() { init(); }
        public ListType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "lst";
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

    {// #NeedsTest
        protected ListFieldType() { init(); }
        public ListFieldType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();            
            SetNames(elementName, elementPrefix);
        }

        private void init()
        {
            ElementPrefix = "lf";
            this._colTextDelimiter = "|";
            this._numCols = ((byte)(1));
            this._storedCol = ((byte)(1));
            this._minSelections = ((ushort)(1));
            this._maxSelections = ((ushort)(1));
            this._ordered = true;
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
        protected ListItemType() { init(); }
        public ListItemType(ListType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "LI";
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
        protected ListItemBaseType() { init(); }
        public ListItemBaseType(ListType parentNode, string id = "") : base(parentNode, id)
        {
            init();
        }
        private void init()
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
        protected LookupEndPointType() { init(); }
        public LookupEndPointType(ListFieldType parentNode, string elementName = "", string elementPrefix = "") : base()
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._includesHeaderRow = false;
            ElementPrefix = "LEP";
        }
    }

    #endregion

    #region Responses

    public partial class ListItemResponseFieldType
    {
        protected ListItemResponseFieldType() { init(); }
        public ListItemResponseFieldType(ListItemBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //if (fillData) AddFillDataTypesDE(parentNode);
            SetNames(elementName, elementPrefix); //this was already called by the superType ResponseField.
        }
        private void init()
        {
            this._responseRequired = false;
            ElementPrefix = "lirf";
        }
    }

    public partial class ResponseFieldType
    {
        protected ResponseFieldType() { init(); }
        public ResponseFieldType(IdentifiedExtensionType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "rf";            
            this.Item = null; // #NeedsTest
        }
    }

    public partial class UnitsType
    {
        protected UnitsType() { init(); }
        public UnitsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();

            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            _unitSystem = "UCUM";
            ElementPrefix = "un";
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
        /// //TODO: this will be a problem when moving nodes in the tree, since the counter will be incorrect; 
        /// this will need to be calculated by walking up the parent tree to the closest IET ancestor.  
        /// It should not have a setter
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
                else if (ElementName.IsEmpty()) ElementName = GetType().ToString().Replace("Type", "").Replace("type", ""); //assign default ElementName from the type.

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
            IETresetCounter = 0; //TODO: this will be a problem when moving nodes in the tree, since the counter will be incorrect
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
        {
            init();
            //Parent Nodes cannot be assigned through this constructor.  
            //After the object tree is created, Parent Nodes can be assigned by using InitNodes<T>
        }

        protected BaseType(BaseType parentNode) //: this()
        {
            init();
            this.RegisterParent(parentNode);            
        }
        private void init()
        {
            ObjectGUID = Guid.NewGuid();
            InitBaseType();
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
        protected ExtensionBaseType() { init();  }
        public ExtensionBaseType(BaseType parentNode) : base(parentNode)
        { init();  }
        private void init()
        {

        }
    }

    #region IExtensionBaseTypeMember
    public partial class ExtensionType : IExtensionBaseTypeMember
    {
        private IExtensionBaseTypeMember Iebtm { get => (IExtensionBaseTypeMember)this; }
        protected ExtensionType() { init(); }
        public ExtensionType(BaseType parentNode) : base(parentNode) { init(); }
        private void init()
        {

        }

        #region IExtensionBaseTypeMember
        //public bool Remove() => Iebtm.Remove();
        //public bool Move(ExtensionBaseType ebtTarget, int newListIndex = -1) => Iebtm.MoveI(this, ebtTarget, newListIndex);
        #endregion

    }
    public partial class PropertyType : IExtensionBaseTypeMember, IHtmlHelpers
    {
        private IExtensionBaseTypeMember Iebtm { get => (IExtensionBaseTypeMember)this; }
        protected PropertyType() { init(); }
        public PropertyType(ExtensionBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementName = "Property";
            ElementPrefix = "p";
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
        protected CommentType() { init(); }
        public CommentType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "cmt";
        }

        #region IExtensionBaseTypeMember
        //public bool Remove() => Iebtm.Remove();
        //public bool Move(ExtensionBaseType ebtTarget, int newListIndex = -1) => Iebtm.MoveI(this, ebtTarget, newListIndex);
        #endregion

    }
    #endregion

    public partial class IdentifiedExtensionType: IIdentifiedExtensionType
    {
        protected IdentifiedExtensionType() { init(); }
        protected IdentifiedExtensionType(BaseType parentNode, string id = "") : base(parentNode)
        {
            this.ID = id;
            init();
            //if (!string.IsNullOrWhiteSpace(id)) // #IsThisCorrect
            //    this.ID = id;
            //else if (this.ObjectGUID != null)
            //    this.ID = this.ObjectGUID.ToString();
        }
        private void init()
        {   //The ID may be assigned later by a deserializer (using the protected consructor) after this runs, but that should be OK
            if (string.IsNullOrWhiteSpace(ID))
                this.ID = this.ObjectGUID.ToString();// #IsThisCorrect 
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
        protected ChildItemsType() { init(); }
        public ChildItemsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "ch";
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
        protected DisplayedType() { init(); }
        public DisplayedType(BaseType parentNode, string id = "", string elementName = "", string elementPrefix = "") : base(parentNode, id)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._enabled = true;
            this._visible = true;
            this._mustImplement = true;
            this._showInReport = DisplayedTypeShowInReport.True;
            ElementName = "DisplayedItem";
            ElementPrefix = "DI";
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
        protected BlobType() { init(); }
        public BlobType(DisplayedType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementName = "Blob";
            ElementPrefix = "blob";
        }
    }

    public partial class LinkType : IDisplayedTypeMember
    {
        protected LinkType() { init(); }
        public LinkType(DisplayedType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementName = "Link";
            ElementPrefix = "link";
        }
    }

    #region Coding
    public partial class CodingType : IDisplayedTypeMember
    {
        protected CodingType() { init(); }
        public CodingType(ExtensionBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);

        }
        private void init()
        {
            ElementName = "Coding";
            ElementPrefix = "cod";
        }
    }

    public partial class CodeMatchType
    {
        protected CodeMatchType() { init(); }
        public CodeMatchType(CodingType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._codeMatchEnum = CodeMatchTypeCodeMatchEnum.ExactCodeMatch;
            ElementName = "CodeMatch";
            ElementPrefix = "cmat";
        }
    }

    public partial class CodeSystemType
    {
        protected CodeSystemType() { init(); }
        public CodeSystemType(ExtensionBaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementName = "CodeSystem";
            ElementPrefix = "csys";
        }
    }

    #endregion

    #endregion


    #endregion

    #region DataTypes
    public partial class DataTypes_DEType
    {
        protected DataTypes_DEType() { init(); }
        public DataTypes_DEType(ResponseFieldType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
            //if (fillData) sdcTreeBuilder.AddFillDataTypesDE(parentNode);
        }
        private void init()
        {
            ElementName = "Response"; //response element
            ElementPrefix = "rsp";  //response element            
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
        protected anyType_DEtype() { init(); }
        public anyType_DEtype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "any";
        }
    }


    public partial class DataTypes_SType
    {
        protected DataTypes_SType() { init(); }
        public DataTypes_SType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            ElementPrefix = "DataTypes";
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {

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
        protected anyURI_DEtype() { init(); }
        public anyURI_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "uri";
        }
    }

    public partial class anyURI_Stype
    {
        protected anyURI_Stype() { init(); }
        public anyURI_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);

        }
        private void init()
        {
            ElementPrefix = "uri";
        }
    }

    public partial class base64Binary_DEtype
    {
        protected base64Binary_DEtype() { init(); }
        public base64Binary_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "b64";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {

        }
    }

    public partial class base64Binary_Stype
    {
        protected base64Binary_Stype() { init(); }
        public base64Binary_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "b64";
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "string")] //changed to string
        public string valBase64 { get; set; }
    }

    public partial class boolean_DEtype
    {
        protected boolean_DEtype() { init(); }
        public boolean_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "bool";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {

        }
    }

    public partial class boolean_Stype
    {
        protected boolean_Stype() { init(); }
        public boolean_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "bool";
        }
    }

    public partial class byte_DEtype
    {
        protected byte_DEtype() { init(); }
        public byte_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "byte";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class byte_Stype
    {
        protected byte_Stype() { init(); }
        public byte_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "byte";
        }
    }

    public partial class date_DEtype
    {
        protected date_DEtype() { init(); }
        public date_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "date";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class date_Stype
    {
        protected date_Stype() { init(); }
        public date_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "date";
        }
    }

    public partial class dateTime_DEtype
    {
        protected dateTime_DEtype() { init(); }
        public dateTime_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "dt";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class dateTime_Stype
    {
        protected dateTime_Stype() { init(); }
        public dateTime_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dt";
        }
    }

    public partial class dateTimeStamp_DEtype
    {
        protected dateTimeStamp_DEtype() { init(); }
        public dateTimeStamp_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "dts";
            //SetNames(elementName, elementPrefix);
        }        
        private void init()
        {

        }
    }

    public partial class dateTimeStamp_Stype
    {
        protected dateTimeStamp_Stype() { init(); }
        public dateTimeStamp_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "dts";
        }
    }

    public partial class dayTimeDuration_DEtype
    {
        protected dayTimeDuration_DEtype() { init(); }
        public dayTimeDuration_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "dtdur";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class dayTimeDuration_Stype
    {
        protected dayTimeDuration_Stype() { init(); }
        public dayTimeDuration_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dtdur";
        }
    }

    public partial class decimal_DEtype
    {
        protected decimal_DEtype() { init(); }
        public decimal_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "dec";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class decimal_Stype
    {
        protected decimal_Stype() { init(); }
        public decimal_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dec";
        }
    }

    public partial class double_DEtype
    {
        protected double_DEtype() { init(); }
        public double_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "dbl";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class double_Stype
    {
        protected double_Stype() { init(); }
        public double_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "dbl";
        }
    }

    public partial class duration_DEtype
    {
        protected duration_DEtype() { init(); }
        public duration_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "dur";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class duration_Stype
    {
        protected duration_Stype() { init(); }
        public duration_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "";
        }
    }

    public partial class float_DEtype
    {
        protected float_DEtype() { init(); }
        public float_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "flt";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class float_Stype
    {
        protected float_Stype() { init(); }
        public float_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "flt";
        }
    }

    public partial class gDay_DEtype
    {
        protected gDay_DEtype() { init(); }
        public gDay_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
        init();
            //ElementPrefix = "day";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class gDay_Stype
    {
        protected gDay_Stype() { init(); }
        public gDay_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "day";
        }
    }

    public partial class gMonth_DEtype
    {
        protected gMonth_DEtype() { init(); }
        public gMonth_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "mon";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class gMonth_Stype
    {
        protected gMonth_Stype() { init(); }
        public gMonth_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "mon";
        }
    }

    public partial class gMonthDay_DEtype
    {
        protected gMonthDay_DEtype() { init(); }
        public gMonthDay_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "mday";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class gMonthDay_Stype
    {
        protected gMonthDay_Stype() { init(); }
        public gMonthDay_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "mday";
        }
    }

    public partial class gYear_DEtype
    {
        protected gYear_DEtype() { init(); }
        public gYear_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "y";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class gYear_Stype
    {
        protected gYear_Stype() { init(); }
        public gYear_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "y";
        }
    }

    public partial class gYearMonth_DEtype
    {
        protected gYearMonth_DEtype() { init(); }
        public gYearMonth_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "ym";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }
    public partial class gYearMonth_Stype
    {
        protected gYearMonth_Stype() { init(); }
        public gYearMonth_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ym";
        }
    }

    public partial class hexBinary_DEtype
    {
        protected hexBinary_DEtype() { init(); }
        public hexBinary_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "hexb";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {

        }
    }

    public partial class hexBinary_Stype
    {
        string _hexBinaryStringVal;

        protected hexBinary_Stype() { init(); }
        public hexBinary_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "hexb";
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
        protected HTML_DEtype() { init(); } 
        public HTML_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "html";
            //SetNames(elementName, elementPrefix);
            //this.Any = new List<System.Xml.XmlElement>();
        }
        private void init()
        {
        }
    }

    public partial class HTML_Stype
    {
        protected HTML_Stype()
        {
            init();
        }
        public HTML_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();            
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.Any = new List<System.Xml.XmlElement>();  // #NeedsTest
            this.AnyAttr = new List<System.Xml.XmlAttribute>();
            ElementPrefix = "html";
        }
    }


    public partial class int_DEtype
    {
        protected int_DEtype() { init(); }
        public int_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "int";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class int_Stype
    {
        protected int_Stype() { init(); }
        public int_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "int";
        }
    }

    public partial class integer_DEtype
    {
        protected integer_DEtype() { init(); }
        public integer_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;            //ElementPrefix = "intr";
        }
    }

    public partial class integer_Stype
    {
        protected integer_Stype() { init(); }
        public integer_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "intr";
        }


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
        protected long_DEtype() { init(); }
        public long_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "lng";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class long_Stype
    {
        protected long_Stype() { init(); }
        public long_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "lng";
        }
    }

    public partial class negativeInteger_DEtype
    {
        protected negativeInteger_DEtype() { init(); }
        public negativeInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "nint";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class negativeInteger_Stype
    {
        protected negativeInteger_Stype() { init(); }
        public negativeInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "nint";
        }
    }

    public partial class nonNegativeInteger_DEtype
    {
        protected nonNegativeInteger_DEtype() { init(); }
        public nonNegativeInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "nnint";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class nonNegativeInteger_Stype
    {
        protected nonNegativeInteger_Stype() { init(); }
        public nonNegativeInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "nnint";
        }
    }

    public partial class nonPositiveInteger_DEtype
    {
        protected nonPositiveInteger_DEtype() { init(); }
        public nonPositiveInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "npint";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class nonPositiveInteger_Stype
    {
        protected nonPositiveInteger_Stype() { init(); }
        public nonPositiveInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "npint";
        }
    }

    public partial class positiveInteger_DEtype
    {
        protected positiveInteger_DEtype() { init(); }
        public positiveInteger_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "pint";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class positiveInteger_Stype
    {
        protected positiveInteger_Stype() { init(); }
        public positiveInteger_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "pint";
        }
    }

    public partial class short_DEtype
    {
        protected short_DEtype() { init(); }
        public short_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "sh";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class short_Stype
    {
        protected short_Stype() { init(); }
        public short_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "sh";
        }
    }

    public partial class string_DEtype
    {
        protected string_DEtype() { init(); }
        public string_DEtype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "str";
            //SetNames(elementName, elementPrefix);
        } //{if (elementName.Length > 0) ElementName = elementName; }
        private void init()
        {

        }
    }

    public partial class string_Stype
    {
        protected string_Stype() { init(); }
        public string_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "str";
        }
    }

    public partial class time_DEtype
    {
        protected time_DEtype() { init(); }
        public time_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "tim";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class time_Stype
    {
        protected time_Stype() { init(); }
        public time_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "tim";
        }
    }

    public partial class unsignedByte_DEtype
    {
        protected unsignedByte_DEtype() { init(); }
        public unsignedByte_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "ubyte";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class unsignedByte_Stype
    {
        protected unsignedByte_Stype() { init(); }
        public unsignedByte_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ubyte";
        }
    }

    public partial class unsignedInt_DEtype
    {
        protected unsignedInt_DEtype() { init(); }
        public unsignedInt_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "unint";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }

    }

    public partial class unsignedInt_Stype
    {
        protected unsignedInt_Stype() { init(); }
        public unsignedInt_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "uint";
        }
    }

    public partial class unsignedLong_DEtype
    {
        protected unsignedLong_DEtype() { init(); }
        public unsignedLong_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "ulng";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class unsignedLong_Stype
    {
        protected unsignedLong_Stype() { init(); }
        public unsignedLong_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ulng";
        }
    }

    public partial class unsignedShort_DEtype
    {
        protected unsignedShort_DEtype() { init(); }
        public unsignedShort_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "ush";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class unsignedShort_Stype
    {
        protected unsignedShort_Stype() { init(); }
        public unsignedShort_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ush";
        }
    }

    public partial class XML_DEtype
    {
        protected XML_DEtype() { init(); }//this.Any = new List<XmlElement>(); }
        public XML_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "xml";
            //SetNames(elementName, elementPrefix);
            //this.Any = new List<XmlElement>();
        }
        private void init()
        {

        }
    }

    public partial class XML_Stype
    {
        protected XML_Stype()
        {
            init();
        }
        public XML_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.Any = new List<System.Xml.XmlElement>();  // #NeedsTest
            //this.AnyAttr = new List<System.Xml.XmlAttribute>(); // Add AnyAttr to Schema? #NeedsFix ?
            ElementPrefix = "xml";
        }
    }

    public partial class yearMonthDuration_DEtype
    {
        protected yearMonthDuration_DEtype() { init(); }
        public yearMonthDuration_DEtype(DataTypes_DEType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            //ElementPrefix = "ymd";
            //SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowGT = false;
            this._allowGTE = false;
            this._allowLT = false;
            this._allowLTE = false;
            this._allowAPPROX = false;
        }
    }

    public partial class yearMonthDuration_Stype
    {
        protected yearMonthDuration_Stype() { init(); }
        public yearMonthDuration_Stype(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._quantEnum = dtQuantEnum.EQ;
            ElementPrefix = "ymd";
        }
    }
    #endregion

    #region Rules

    public partial class ItemNameType
    {
        protected ItemNameType() { init(); }
        public ItemNameType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "itnm";
        }
    }
    public partial class ItemNameAttributeType
    {
        protected ItemNameAttributeType() { init(); }

        public ItemNameAttributeType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._attributeName = "val";
        }
    }
    public partial class NameType
    {
        protected NameType() { init(); }
        public NameType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "nm";
        }
    }
    public partial class TargetItemIDType
    {
        protected TargetItemIDType() { init(); }
        public TargetItemIDType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "tiid";
        }
    }
    public partial class TargetItemNameType
    {
        protected TargetItemNameType() { init(); }
        public TargetItemNameType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "tinm";
        }
    }
    public partial class TargetItemXPathType
    {
        protected TargetItemXPathType() { init(); }
        public TargetItemXPathType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "tixp";
        }
        //{if (elementName.Length > 0) ElementName = elementName; }
    }
    public partial class ListItemParameterType
    {
        protected ListItemParameterType() { init(); }
        public ListItemParameterType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            this._listItemAttribute = "associatedValue";
        }
        private void init()
        {
            this._dataType = "string";
        }
    }
    public partial class ParameterItemType
    {
        protected ParameterItemType() { init(); }
        public ParameterItemType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._dataType = "string";
            this._sourceItemAttribute = "val";
        }
    }
    public partial class PredAlternativesType
    {
        public PredAlternativesType() { init(); }
        public PredAlternativesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._not = false;
            this._minAnswered = 1;
            this._maxAnswered = 0;
        }
    }
    public partial class PredEvalAttribValuesType
    {
        protected PredEvalAttribValuesType() { init(); }
        public PredEvalAttribValuesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }       
        private void init()
        {
            this._not = false;
            this._boolOp = PredEvalAttribValuesTypeBoolOp.AND;
        }
    }
    public partial class PredGuardTypeSelectionSets
    {
        protected PredGuardTypeSelectionSets() { init(); }
        public PredGuardTypeSelectionSets(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._not = false;
        }
    }
    public partial class PredSingleSelectionSetsType
    {
        protected PredSingleSelectionSetsType() { init(); }
        public PredSingleSelectionSetsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._maxSelections = ((short)(1));
        }
    }
    public partial class RuleAutoActivateType
    {
        protected RuleAutoActivateType() { init(); }
        public RuleAutoActivateType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
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
        protected RuleAutoSelectType() { init(); }
        public RuleAutoSelectType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._onlyIf = false;
        }
    }
    public partial class RuleListItemMatchTargetsType
    {
        protected RuleListItemMatchTargetsType() { init(); }
        public RuleListItemMatchTargetsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._attributeToMatch = RuleListItemMatchTargetsTypeAttributeToMatch.associatedValue;
        }
    }
    public partial class SelectionSetsActionType
    {
        protected SelectionSetsActionType() { init(); }
        public SelectionSetsActionType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._not = false;
        }
    }
    public partial class ValidationTypeSelectionSets
    {
        protected ValidationTypeSelectionSets() { init(); }
        public ValidationTypeSelectionSets(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._not = false;
        }
    }
    public partial class ValidationTypeSelectionTest
    {
        protected ValidationTypeSelectionTest() { init(); }
        public ValidationTypeSelectionTest(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._not = false;
        }
    }
    public partial class PredSelectionTestType
    {
        protected PredSelectionTestType() { init(); }
        public PredSelectionTestType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        { init(); }
        private void init()
        {

        }
    }
    public partial class CallFuncType
    {
        protected CallFuncType() { init(); }
        public CallFuncType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._dataType = "string";
        }
    }
    partial class CallFuncBaseType
    {
        protected CallFuncBaseType() { init(); }
        public CallFuncBaseType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._returnList = false;
            this._listDelimiter = "|";
            this._allowNull = true;
        }
    }
    partial class CallFuncBoolType
    {
        protected CallFuncBoolType() { init(); }
        public CallFuncBoolType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
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
        { init(); }
        public MultiSelectionsActionType(BaseType parentNode) : base(parentNode)
        { init(); }
        private void init()
        {

        }
    }
    public partial class SelectionTestActionType
    {
        protected SelectionTestActionType()
        { init(); }
        public SelectionTestActionType(BaseType parentNode) : base(parentNode)
        { init(); }
        private void init()
        {

        }
    }

    public partial class PredMultiSelectionSetBoolType
    {
        protected PredMultiSelectionSetBoolType() { init(); }
        public PredMultiSelectionSetBoolType(BaseType parentNode) : base(parentNode)
        { init(); }
        private void init()
        {

        }
    }


    #endregion
    #region  Actions

    public partial class ActionsType : IActions
    {
        protected ActionsType() { init(); }
        public ActionsType(ExtensionBaseType parentNode) : base(parentNode) {
            init(); 
        }
        private void init()
        {
            ElementName = "Actions"; 
            ElementPrefix = "act";
        }
    }
    public partial class ActActionType
    {
        protected ActActionType() { init(); }
        public ActActionType(ActionsType parentNode) : base(parentNode) 
        {
            init(); 
        }
        private void init()
        {
            ElementName = "Action";
        }
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
        protected RuleSelectMatchingListItemsType() { init(); }
        public RuleSelectMatchingListItemsType(ActionsType parentNode) : base(parentNode) 
        { 
            init(); 
        }
        private void init()
        {
            ElementName = "SelectMatchingListItems";
        }
    }
    public partial class ActAddCodeType
    {
        protected ActAddCodeType() { init(); }
        public ActAddCodeType(ActionsType parentNode) : base(parentNode) 
        { 
            init(); 
        }
        private void init()
        {
            ElementName = "AddCode";
        }
    }
    public partial class ActInjectType : InjectFormType
    {
        protected ActInjectType() { init(); }
        public ActInjectType(ActionsType parentNode) : base(parentNode) 
        {
            init();
        }
        private void init()
        {
            ElementName = "Inject";
        }
    }
    public partial class ActSaveResponsesType
    {
        protected ActSaveResponsesType() { init(); }
        public ActSaveResponsesType(ActionsType parentNode) : base(parentNode) 
        {
            init();
        }
        private void init()
        {
            ElementName = "Save";
        }
    }
    public partial class ActSendReportType
    {
        protected ActSendReportType() { init(); }
        public ActSendReportType(ActionsType parentNode) : base(parentNode) 
        {
            init();
        }
        private void init()
        {
            ElementName = "SendReport";
        }

        internal List<ExtensionBaseType> Email_Phone_WebSvc_List
        {
            get { return this.Items; }
            set { this.Items = value; }
        }
    }
    public partial class ActSendMessageType
    {
        protected ActSendMessageType() { init(); }
        public ActSendMessageType(ActionsType parentNode) : base(parentNode)
        {
            init();
        } //"SendMessage111" in Schema
        private void init()
        {
            ElementName = "SendMessage";
        }

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
        protected ActSetAttributeType() { init(); }
        public ActSetAttributeType(ActionsType parentNode) : base(parentNode) 
        {
            init();
        }
        private void init()
        {
            ElementName = "SetAttributeValue";
        }
    }
    public partial class ActSetAttrValueScriptType
    {
        protected ActSetAttrValueScriptType() { init(); }
        public ActSetAttrValueScriptType(ActionsType parentNode) : base(parentNode) 
        {
            init(); 
        }
        private void init()
        {
            ElementName = "SetAttributeValueScript";
        }
    }
    public partial class ActSetBoolAttributeValueCodeType
    {
        protected ActSetBoolAttributeValueCodeType() { init(); }
        public ActSetBoolAttributeValueCodeType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "SetBoolAttributeValueCode";
            this._attributeName = "val";
        }
    }
    public partial class ScriptCodeBoolType
    {
        protected ScriptCodeBoolType() { init(); }
        public ScriptCodeBoolType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "";
            this._not = false;
        }
    }
    public partial class ActShowFormType
    {
        protected ActShowFormType() { init(); }
        public ActShowFormType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "ShowForm";
        }
    }
    public partial class ActShowMessageType
    {
        protected ActShowMessageType() { init(); }
        public ActShowMessageType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "ShowMessage";
        }
    }
    public partial class ActShowReportType
    {
        protected ActShowReportType() { init(); }

        public ActShowReportType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "ShowReport";
        }
    }
    public partial class ActPreviewReportType
    {
        protected ActPreviewReportType() { init(); }
        public ActPreviewReportType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "PreviewReport";
        }
    }
    public partial class ActValidateFormType
    {
        protected ActValidateFormType() { init(); }
        public ActValidateFormType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
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
        protected ScriptBoolFuncActionType() { init(); }
        public ScriptBoolFuncActionType(ActionsType parentNode) : base(parentNode)
        {
            init();
        }
        private void init()
        {
            ElementName = "ScriptBoolFunc";
        }
    }

    public partial class ScriptCodeAnyType
    {
        protected ScriptCodeAnyType()
        { init(); }
        public ScriptCodeAnyType(ActionsType parentNode) : base(parentNode)
        { init(); }
        private void init()
        {
            ElementName = "RunCode";
            this._dataType = "string";
        }
    }
    public partial class ScriptCodeBaseType
    {
        protected ScriptCodeBaseType() { init(); }
        public ScriptCodeBaseType(ActionsType parentNode) : base(parentNode)
        { init(); }
        private void init()
        {
            ElementName = "";
            this._returnList = false;
            this._listDelimiter = "|";
            this._allowNull = true;
        }
    }
    public partial class CallFuncActionType
    {
        protected CallFuncActionType() { init(); }
        public CallFuncActionType(ActionsType parentNode) : base(parentNode) 
        { init(); }
        private void init()
        {
            ElementName = "CallFunction";
        }
    }

    public partial class CallFuncBoolActionType
    {
        protected CallFuncBoolActionType() { init(); }
        public CallFuncBoolActionType(ActionsType parentNode) : base(parentNode)
        { init(); }
        private void init()
        {
            ElementName = "CallBoolFunction";
        }
    }

    #endregion
    #region Events
    public partial class OnEventType : IDisplayedTypeMember
    {
        protected OnEventType() { init(); }
        public OnEventType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "onev";
        }
    }

    public partial class RulesType
    {
        protected RulesType() { init(); }
        public RulesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "rul";
        }
    }

    public partial class EventType : IDisplayedTypeMember
    {
        protected EventType() { init(); }
        public EventType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "evnt";
        }
    }

    public partial class PredGuardType : IDisplayedTypeMember
    {

        protected PredGuardType() { init(); }
        public PredGuardType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
        }
        private void init()
        {
            this._not = false;
            this._boolOp = PredEvalAttribValuesTypeBoolOp.AND;
        }
    }

    public partial class PredActionType
    {
        protected PredActionType() { init(); }
        public PredActionType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._not = false;
            this._boolOp = PredEvalAttribValuesTypeBoolOp.AND;
            ElementPrefix = "cga";
        }
    }

    public partial class FuncBoolBaseType
    {
        protected FuncBoolBaseType() { init(); }
        public FuncBoolBaseType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this._allowNull = true;
            ElementPrefix = "fbb";
        }
    }


    #endregion

    #region Contacts

    public partial class ContactType : IDisplayedTypeMember, IAddPerson, IAddOrganization
    {
        protected ContactType() { init(); }
        public ContactType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "cntct";
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
        protected OrganizationType() { init(); }
        public OrganizationType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "org";
        }
    }

    public partial class PersonType
    {
        protected PersonType() { init(); }
        public PersonType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "pers";
        }
    }

    public partial class AddressType
    {
        protected AddressType() { init(); }
        public AddressType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "adrs";
        }
    }

    public partial class AreaCodeType
    {
        protected AreaCodeType() { init(); }
        public AreaCodeType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "arcd";
        }
    }
    #endregion

    #region Resources
    public partial class RichTextType : IHtmlHelpers
    {
        protected RichTextType() { init(); }
        public RichTextType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "rtt";
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
        protected ComplianceRuleType() { init(); }
        public ComplianceRuleType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "cr";
        }
    }

    public partial class SubmissionRuleType
    {
        protected SubmissionRuleType() { init(); }
        public SubmissionRuleType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "sr";
        }

    }


    public partial class HashType
    {
        protected HashType() { init(); }
        public HashType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "hsh";
        }
    }


    public partial class IdentifierType
    {
        protected IdentifierType() { init(); }
        public IdentifierType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);

        }
        private void init()
        {
            this.ElementPrefix = "id";
        }
    }

    public partial class LanguageCodeISO6393_Type
    {
        protected LanguageCodeISO6393_Type() { init(); }
        public LanguageCodeISO6393_Type(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "lc";
        }
    }

    public partial class LanguageType
    {
        protected LanguageType() { init(); }
        public LanguageType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);

        }
        private void init()
        {
            this.ElementPrefix = "lng";
        }
    }

    public partial class ProvenanceType
    {
        protected ProvenanceType() { init(); }
        public ProvenanceType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "prv";
        }
    }

    public partial class ReplacedIDsType
    {
        protected ReplacedIDsType() { init(); }
        public ReplacedIDsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "rid";
        }
    }

    public partial class VersionType
    {
        protected VersionType() { init(); }
        public VersionType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "ver";
        }
    }

    public partial class VersionTypeChanges
    {
        protected VersionTypeChanges() { init(); }
        public VersionTypeChanges(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            this.ElementPrefix = "vch";
        }
    }


    #endregion

    #region Contacts classes

    public partial class ContactsType
    {
        protected ContactsType() { init(); }
        public ContactsType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            this.ElementPrefix = "ctc";
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {

        }
    }

    public partial class CountryCodeType
    {
        protected CountryCodeType() { init(); }
        public CountryCodeType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "ctc";
        }
    }

    public partial class DestinationType
    {
        protected DestinationType() { init(); }
        public DestinationType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);

        }
        private void init()
        {
            ElementPrefix = "dst";
        }
    }


    public partial class PhoneNumberType
    {
        protected PhoneNumberType() { init(); }
        public PhoneNumberType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "phn";
        }
    }

    public partial class PhoneType
    {
        protected PhoneType() { init(); }
        public PhoneType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementName = "PhoneType";
            ElementPrefix = "pht";
        }
    }

    public partial class JobType
    {
        protected JobType() { init(); }
        public JobType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "job";
        }
    }
    #endregion

    #region  Email
    public partial class EmailAddressType
    {
        protected EmailAddressType() { init(); }
        public EmailAddressType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "emd";
        }
    }

    public partial class EmailType
    {
        protected EmailType() { init(); }
        public EmailType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
            //this.Usage = new string_Stype();
            //this.EmailClass = new string_Stype();
            //this.EmailAddress = new EmailAddressType();
        }
        private void init()
        {
            ElementPrefix = "em";
        }
    }

    #endregion

    #region Files


    public partial class ApprovalType
    {
        protected ApprovalType() { init(); }
        public ApprovalType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "app";
        }
    }

    public partial class AssociatedFilesType
    {
        protected AssociatedFilesType() { init(); }
        public AssociatedFilesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "asf";
        }
    }

    public partial class AcceptabilityType
    {
        protected AcceptabilityType() { init(); }
        public AcceptabilityType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();            
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "acc";
        }
    }


    public partial class FileDatesType
    {
        protected FileDatesType() { init(); }
        public FileDatesType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();            
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "fld";
        }
    }
    public partial class FileHashType
    {
        protected FileHashType() { init(); }
        public FileHashType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "flh";
        }
    }

    public partial class FileType
    {
        protected FileType() { init(); }
        public FileType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "fil";
        }
    }

    public partial class FileUsageType
    {
        protected FileUsageType() { init(); }
        public FileUsageType(BaseType parentNode, string elementName = "", string elementPrefix = "") : base(parentNode)
        {
            init();            
            SetNames(elementName, elementPrefix);
        }
        private void init()
        {
            ElementPrefix = "flu";
        }
    }

    #endregion

    #endregion

    #region Registry Summary Types

    #endregion

}

