using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;
using System.Xml.Schema;

//using SDC;
namespace SDC.Schema
{
    /// <summary>
    /// </summary>
    /// A public/internal interface inherited by all types that sit at the top of the SDC class hierarchy
    /// Used by FormDesignType, DemogFormDesignType, DataElementType, RetrieveFormPackageType, and PackageListType
    /// The interface provides a common way to fill the above object trees using a single set of shared code.
    /// It also provdes a set of consistent, type-specific, public utilities for working with SDC objects
    public interface ITopNode: IBaseType
    {
        /// <summary>
        /// Dictionary.  Given an Node ID (int), returns the node's object reference.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        Dictionary<Guid, BaseType> Nodes { get; }
        /// <summary>
        /// Dictionary.  Given a Node ObjectGUID, return the *parent* node's object reference
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        Dictionary<Guid, BaseType> ParentNodes { get; }

        /// <summary>
        /// Dictionary.  Given a NodeID ObjectGUID, return a list of the child nodes object reference
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        Dictionary<Guid, List<BaseType>> ChildNodes { get; }


        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        int GetMaxObjectID { get; }
        [JsonIgnore]
        internal int MaxObjectID { get; set; }
        /// <summary>
        /// Automatically create and assign element names to all SDC elements
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        bool GlobalAutoNameFlag { get; set; }

        #region Serialization
        /// <summary>
        /// Returns SDC XML from the SDC object tree.  The XML top node is determined by the top-level object tree node:
        /// FormDesignType, DemogFormDesignType, DataElementType, RetrieveFormPackageType, or PackageListType
        /// </summary>
        /// <returns></returns>
        public string GetXml();
        /// <summary>
        /// Not yet supported
        /// </summary>
        /// <returns></returns>
        public string GetJson();
        //{
        //    var doc = new XmlDocument();
        //    doc.LoadXml(GetXml()); ;
        //    return JsonConvert.SerializeXmlNode(doc);
        //}

        /// <summary>
        /// Not yet supported
        /// </summary>
        /// <returns></returns>
        public byte[] GetMsgPack();

        public void SaveXmlToFile(string path, Exception ex = null);
        public void SaveJsonToFile(string path, Exception ex = null);
        public void SaveMsgPackToFile(string path, Exception ex = null);
        #endregion

    }


}
