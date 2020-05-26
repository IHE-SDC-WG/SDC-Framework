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
/// This Rule component evaluates the @selected status of any set of
/// ListItems at runtime, and returns a true or false value based on the @selected
/// status of each ListItem.
/// </summary>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(MultiSelectionsActionType))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class PredMultiSelectionSetBoolType : FuncBoolBaseType
{
    
    #region Private fields
    private string _selectedItemSet;
    #endregion
    
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKENS")]
    public virtual string selectedItemSet
    {
        get
        {
            return this._selectedItemSet;
        }
        set
        {
            if ((this._selectedItemSet == value))
            {
                return;
            }
            if (((this._selectedItemSet == null) 
                        || (_selectedItemSet.Equals(value) != true)))
            {
                this._selectedItemSet = value;
                this.OnPropertyChanged("selectedItemSet", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether selectedItemSet should be serialized
    /// </summary>
    public virtual bool ShouldSerializeselectedItemSet()
    {
        return !string.IsNullOrEmpty(selectedItemSet);
    }
}
}
#pragma warning restore
