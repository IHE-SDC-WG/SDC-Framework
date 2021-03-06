// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 5.1.87.0. www.xsd2code.com
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace SDC.Schema
{
using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ihe:qrph:sdc:2016")]
public partial class DataSourceTypeXML : DataStoreType
{
    
    #region Private fields
    private string_Stype _item;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute("ItemID", typeof(string_Stype), Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("SQL", typeof(SQL_Type), Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("XPath", typeof(XPathType), Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("XQuery", typeof(XQueryType), Order=0)]
    public virtual string_Stype Item
    {
        get
        {
            return this._item;
        }
        set
        {
            if ((this._item == value))
            {
                return;
            }
            if (((this._item == null) 
                        || (_item.Equals(value) != true)))
            {
                this._item = value;
                this.OnPropertyChanged("Item", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether Item should be serialized
    /// </summary>
    public virtual bool ShouldSerializeItem()
    {
        return (_item != null);
    }
}
}
#pragma warning restore
