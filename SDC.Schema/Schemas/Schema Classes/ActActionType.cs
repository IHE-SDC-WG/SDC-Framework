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
/// Add a custom action, not defined in the basic SDC
/// Schemas.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ActActionType : ExtensionBaseType
{
    
    #region Private fields
    private List<ExtensionBaseType> _items;
    
    private string _action;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute("ListItemParameterRef", typeof(ListItemParameterType), Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("ParameterRef", typeof(ParameterItemType), Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("ParameterValue", typeof(ParameterValueType), Order=0)]
    public virtual List<ExtensionBaseType> Items
    {
        get
        {
            return this._items;
        }
        set
        {
            if ((this._items == value))
            {
                return;
            }
            if (((this._items == null) 
                        || (_items.Equals(value) != true)))
            {
                this._items = value;
                this.OnPropertyChanged("Items", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string action
    {
        get
        {
            return this._action;
        }
        set
        {
            if ((this._action == value))
            {
                return;
            }
            if (((this._action == null) 
                        || (_action.Equals(value) != true)))
            {
                this._action = value;
                this.OnPropertyChanged("action", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether Items should be serialized
    /// </summary>
    public virtual bool ShouldSerializeItems()
    {
        return Items != null && Items.Count > 0;
    }
    
    /// <summary>
    /// Test whether action should be serialized
    /// </summary>
    public virtual bool ShouldSerializeaction()
    {
        return !string.IsNullOrEmpty(action);
    }
}
}
#pragma warning restore
