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
public partial class CodeMatchType : BaseType
{
    
    #region Private fields
    private bool _shouldSerializecodeMatchEnum;
    
    private string_Stype _codingMatchComment;
    
    private CodeMatchTypeCodeMatchEnum _codeMatchEnum;
    #endregion
    
    ///// <summary>
    ///// CodeMatchType class constructor
    ///// </summary>
    //public CodeMatchType()
    //{
    //    this._codeMatchEnum = CodeMatchTypeCodeMatchEnum.ExactCodeMatch;
    //}
    
    /// <summary>
    /// Comment about the degree of match between the mapped item and the assigned
    /// code.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public virtual string_Stype CodingMatchComment
    {
        get
        {
            return this._codingMatchComment;
        }
        set
        {
            if ((this._codingMatchComment == value))
            {
                return;
            }
            if (((this._codingMatchComment == null) 
                        || (_codingMatchComment.Equals(value) != true)))
            {
                this._codingMatchComment = value;
                this.OnPropertyChanged("CodingMatchComment", value);
            }
        }
    }
    
    /// <summary>
    /// The degree of match between the coded item and the assigned code.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(CodeMatchTypeCodeMatchEnum.ExactCodeMatch)]
    public virtual CodeMatchTypeCodeMatchEnum codeMatchEnum
    {
        get
        {
            return this._codeMatchEnum;
        }
        set
        {
            if ((_codeMatchEnum.Equals(value) != true))
            {
                this._codeMatchEnum = value;
                this.OnPropertyChanged("codeMatchEnum", value);
            }
            _shouldSerializecodeMatchEnum = true;
        }
    }
    
    /// <summary>
    /// Test whether codeMatchEnum should be serialized
    /// </summary>
    public virtual bool ShouldSerializecodeMatchEnum()
    {
        if (_shouldSerializecodeMatchEnum)
        {
            return true;
        }
        return (_codeMatchEnum != default(CodeMatchTypeCodeMatchEnum));
    }
    
    /// <summary>
    /// Test whether CodingMatchComment should be serialized
    /// </summary>
    public virtual bool ShouldSerializeCodingMatchComment()
    {
        return (_codingMatchComment != null);
    }
}
}
#pragma warning restore
