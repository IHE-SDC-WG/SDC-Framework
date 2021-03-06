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
/// This type is used to act upon the value of common item properties. If
/// an "act" property (a property with the "act" prefix) has no value assigned, it is
/// ignored. If it has a value, then that property on the target item(s) assume(s) that
/// stated value when an attached Boolean condition evaluates to true. The attached
/// condition may be an "If" statement or any expression that evaluates to a Boolean
/// value.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ActSetAttrValueScriptType : ScriptCodeAnyType
{
    
    #region Private fields
    private ItemNameAttributeType _target;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute("Target", Order=0)]
    public virtual ItemNameAttributeType Target
    {
        get
        {
            return this._target;
        }
        set
        {
            if ((this._target == value))
            {
                return;
            }
            if (((this._target == null) 
                        || (_target.Equals(value) != true)))
            {
                this._target = value;
                this.OnPropertyChanged("Target", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether Target should be serialized
    /// </summary>
    public virtual bool ShouldSerializeTarget()
    {
        return (_target != null);
    }
}
}
#pragma warning restore
