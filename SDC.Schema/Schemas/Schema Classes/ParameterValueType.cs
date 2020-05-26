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
/// This type describes strongly-typed parameters used in functions and web services.  Values are hard-coded as constants in the XML instance document.  They are not user-entered values.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ParameterValueType : DataTypes_SType
{
    
    #region Private fields
    private string _paramName;
    #endregion
    
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="NCName")]
    public virtual string paramName
    {
        get
        {
            return this._paramName;
        }
        set
        {
            if ((this._paramName == value))
            {
                return;
            }
            if (((this._paramName == null) 
                        || (_paramName.Equals(value) != true)))
            {
                this._paramName = value;
                this.OnPropertyChanged("paramName", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether paramName should be serialized
    /// </summary>
    public virtual bool ShouldSerializeparamName()
    {
        return !string.IsNullOrEmpty(paramName);
    }
}
}
#pragma warning restore
