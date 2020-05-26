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
/// A structure to describe a single change in an XML document
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ChangeType : ExtensionBaseType
{
    
    #region Private fields
    private TargetItemIDType _targetItemID;
    
    private TargetItemNameType _targetItemName;
    
    private TargetItemXPathType _targetItemXPath;
    
    private DataTypes_SType _newValue;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public virtual TargetItemIDType TargetItemID
    {
        get
        {
            return this._targetItemID;
        }
        set
        {
            if ((this._targetItemID == value))
            {
                return;
            }
            if (((this._targetItemID == null) 
                        || (_targetItemID.Equals(value) != true)))
            {
                this._targetItemID = value;
                this.OnPropertyChanged("TargetItemID", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public virtual TargetItemNameType TargetItemName
    {
        get
        {
            return this._targetItemName;
        }
        set
        {
            if ((this._targetItemName == value))
            {
                return;
            }
            if (((this._targetItemName == null) 
                        || (_targetItemName.Equals(value) != true)))
            {
                this._targetItemName = value;
                this.OnPropertyChanged("TargetItemName", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public virtual TargetItemXPathType TargetItemXPath
    {
        get
        {
            return this._targetItemXPath;
        }
        set
        {
            if ((this._targetItemXPath == value))
            {
                return;
            }
            if (((this._targetItemXPath == null) 
                        || (_targetItemXPath.Equals(value) != true)))
            {
                this._targetItemXPath = value;
                this.OnPropertyChanged("TargetItemXPath", value);
            }
        }
    }
    
    /// <summary>
    /// The new value that the targeted item's content is set to.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    public virtual DataTypes_SType NewValue
    {
        get
        {
            return this._newValue;
        }
        set
        {
            if ((this._newValue == value))
            {
                return;
            }
            if (((this._newValue == null) 
                        || (_newValue.Equals(value) != true)))
            {
                this._newValue = value;
                this.OnPropertyChanged("NewValue", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether TargetItemID should be serialized
    /// </summary>
    public virtual bool ShouldSerializeTargetItemID()
    {
        return (_targetItemID != null);
    }
    
    /// <summary>
    /// Test whether TargetItemName should be serialized
    /// </summary>
    public virtual bool ShouldSerializeTargetItemName()
    {
        return (_targetItemName != null);
    }
    
    /// <summary>
    /// Test whether TargetItemXPath should be serialized
    /// </summary>
    public virtual bool ShouldSerializeTargetItemXPath()
    {
        return (_targetItemXPath != null);
    }
    
    /// <summary>
    /// Test whether NewValue should be serialized
    /// </summary>
    public virtual bool ShouldSerializeNewValue()
    {
        return (_newValue != null);
    }
}
}
#pragma warning restore
