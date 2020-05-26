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
/// Programming code or pseudocode that describes a calculation.  The code returns a value of the data type required by the parent Response field.  To assist with enabling the code in the form, the referenced form items and properties should be referenced by @name under the parameters elemeent.  It is possible to add mulitple calculation expressions to produce equivalent results using different programming languages and URIs.  The @ type attribute may be used to distinguish between them.  An Extension may be used instead of or along with an Expression and Parameters list.  Expressions may populate Responses that are set to @readOnly = "true" to ensure that all responses are calculated and not latered by the user.  Alternatively, the user may change a value created by (or instead of) the Expression.
/// </summary>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ScriptBoolFuncActionType))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ActSetBoolAttributeValueCodeType))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ScriptCodeBoolType : ScriptCodeBaseType
{
    
    #region Private fields
    private bool _shouldSerializenot;
    
    private bool _not;
    
    private string _validationMessage;
    #endregion
    
    ///// <summary>
    ///// ScriptCodeBoolType class constructor
    ///// </summary>
    //public ScriptCodeBoolType()
    //{
    //    this._not = false;
    //}
    
    /// <summary>
    /// If @not="true" then the logical value of the conditions of the parent element is negated; true becomes false and false becomes true.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public virtual bool not
    {
        get
        {
            return this._not;
        }
        set
        {
            if ((_not.Equals(value) != true))
            {
                this._not = value;
                this.OnPropertyChanged("not", value);
            }
            _shouldSerializenot = true;
        }
    }
    
    /// <summary>
    /// Optional message that appears when the rule evaluates to true
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string validationMessage
    {
        get
        {
            return this._validationMessage;
        }
        set
        {
            if ((this._validationMessage == value))
            {
                return;
            }
            if (((this._validationMessage == null) 
                        || (_validationMessage.Equals(value) != true)))
            {
                this._validationMessage = value;
                this.OnPropertyChanged("validationMessage", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether not should be serialized
    /// </summary>
    public virtual bool ShouldSerializenot()
    {
        if (_shouldSerializenot)
        {
            return true;
        }
        return (_not != default(bool));
    }
    
    /// <summary>
    /// Test whether validationMessage should be serialized
    /// </summary>
    public virtual bool ShouldSerializevalidationMessage()
    {
        return !string.IsNullOrEmpty(validationMessage);
    }
}
}
#pragma warning restore
