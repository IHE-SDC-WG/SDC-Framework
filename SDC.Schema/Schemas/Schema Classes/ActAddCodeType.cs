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
/// Add all or part of a coding section to the designated target
/// site.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ActAddCodeType : ItemNameType
{
    
    #region Private fields
    private CodingType _code;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute("Code", Order=0)]
    public virtual CodingType Code
    {
        get
        {
            return this._code;
        }
        set
        {
            if ((this._code == value))
            {
                return;
            }
            if (((this._code == null) 
                        || (_code.Equals(value) != true)))
            {
                this._code = value;
                this.OnPropertyChanged("Code", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether Code should be serialized
    /// </summary>
    public virtual bool ShouldSerializeCode()
    {
        return (_code != null);
    }
}
}
#pragma warning restore
