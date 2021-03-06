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
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class TargetItemIDType : anyURI_Stype
{
    
    #region Private fields
    private string _targetItemText;
    
    private string _targetProperty;
    #endregion
    
    /// <summary>
    /// Displayed text on the targetted item.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string targetItemText
    {
        get
        {
            return this._targetItemText;
        }
        set
        {
            if ((this._targetItemText == value))
            {
                return;
            }
            if (((this._targetItemText == null) 
                        || (_targetItemText.Equals(value) != true)))
            {
                this._targetItemText = value;
                this.OnPropertyChanged("targetItemText", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string targetProperty
    {
        get
        {
            return this._targetProperty;
        }
        set
        {
            if ((this._targetProperty == value))
            {
                return;
            }
            if (((this._targetProperty == null) 
                        || (_targetProperty.Equals(value) != true)))
            {
                this._targetProperty = value;
                this.OnPropertyChanged("targetProperty", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether targetItemText should be serialized
    /// </summary>
    public virtual bool ShouldSerializetargetItemText()
    {
        return !string.IsNullOrEmpty(targetItemText);
    }
    
    /// <summary>
    /// Test whether targetProperty should be serialized
    /// </summary>
    public virtual bool ShouldSerializetargetProperty()
    {
        return !string.IsNullOrEmpty(targetProperty);
    }
}
}
#pragma warning restore
