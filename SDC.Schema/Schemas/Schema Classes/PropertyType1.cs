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

/// <summary>
/// MOVED: From SDCFormDesign
/// CHANGED:
/// Property is any type of data that is associated with any form item (e.g., section, question, list item) in any context.  The Property context is specified by the @type attribute.  Examples of @type values may include:
/// -alternate language representations, e.g., 'spanish'
/// -'instruction'
/// -'tooltip'
/// -'help' for a pop-up help box
/// -'outlining' (such as question numbering)
/// 
/// The @type enumerations must be defined and documented for the particular use case, and are currently out of scope in this schema definition.  Multiple type tokens for a single element are supported.
/// 
/// RENAMED:  'OtherText' to 'Property' 9/14/2016
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class PropertyType1 : DataTypes_SType
{
    
    #region Private fields
    private string _propName;
    
    private string _propClass;
    
    private string _val;
    #endregion
    
    /// <summary>
    /// NEW
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKEN")]
    public virtual string propName
    {
        get
        {
            return this._propName;
        }
        set
        {
            if ((this._propName == value))
            {
                return;
            }
            if (((this._propName == null) 
                        || (_propName.Equals(value) != true)))
            {
                this._propName = value;
                this.OnPropertyChanged("propName", value);
            }
        }
    }
    
    /// <summary>
    /// NEW
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKENS")]
    public virtual string propClass
    {
        get
        {
            return this._propClass;
        }
        set
        {
            if ((this._propClass == value))
            {
                return;
            }
            if (((this._propClass == null) 
                        || (_propClass.Equals(value) != true)))
            {
                this._propClass = value;
                this.OnPropertyChanged("propClass", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string val
    {
        get
        {
            return this._val;
        }
        set
        {
            if ((this._val == value))
            {
                return;
            }
            if (((this._val == null) 
                        || (_val.Equals(value) != true)))
            {
                this._val = value;
                this.OnPropertyChanged("val", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether propName should be serialized
    /// </summary>
    public virtual bool ShouldSerializepropName()
    {
        return !string.IsNullOrEmpty(propName);
    }
    
    /// <summary>
    /// Test whether propClass should be serialized
    /// </summary>
    public virtual bool ShouldSerializepropClass()
    {
        return !string.IsNullOrEmpty(propClass);
    }
    
    /// <summary>
    /// Test whether val should be serialized
    /// </summary>
    public virtual bool ShouldSerializeval()
    {
        return !string.IsNullOrEmpty(val);
    }
}
}
#pragma warning restore
