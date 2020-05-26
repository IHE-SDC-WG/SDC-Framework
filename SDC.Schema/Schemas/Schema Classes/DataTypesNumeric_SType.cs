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
/// NEW:
/// SDC datatypes in Simple (S) format, based mostly on W3C specifications.  Uses baseAttributes and Extension capability to enhance the list of Data Types.        **CHECK for ERRORS and completeness**
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class DataTypesNumeric_SType : ExtensionBaseType
{
    
    #region Private fields
    private BaseType _item;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute("byte", typeof(byte_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("decimal", typeof(decimal_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("double", typeof(double_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("float", typeof(float_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("int", typeof(int_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("integer", typeof(integer_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("long", typeof(long_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("negativeInteger", typeof(negativeInteger_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("nonNegativeInteger", typeof(nonNegativeInteger_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("nonPositiveInteger", typeof(nonPositiveInteger_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("positiveInteger", typeof(positiveInteger_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("short", typeof(short_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("unsignedByte", typeof(unsignedByte_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("unsignedInt", typeof(unsignedInt_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("unsignedLong", typeof(unsignedLong_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("unsignedShort", typeof(unsignedShort_Stype), IsNullable=true, Order=0)]
    [System.Xml.Serialization.XmlElementAttribute("yearMonthDuration", typeof(yearMonthDuration_Stype), IsNullable=true, Order=0)]
    public virtual BaseType Item
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
