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

[System.Xml.Serialization.XmlIncludeAttribute(typeof(double_DEtype))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class double_Stype : BaseType
{
    
    #region Private fields
    private bool _shouldSerializequantEnum;
    
    private bool _shouldSerializeval;
    
    private double _val;
    
    private string _mask;
    
    private dtQuantEnum _quantEnum;
    #endregion
    
    ///// <summary>
    ///// double_Stype class constructor
    ///// </summary>
    //public double_Stype()
    //{
    //    this._quantEnum = dtQuantEnum.EQ;
    //}
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual double val
    {
        get
        {
            return this._val;
        }
        set
        {
            if ((_val.Equals(value) != true))
            {
                this._val = value;
                this.OnPropertyChanged("val", value);
            }
            _shouldSerializeval = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string mask
    {
        get
        {
            return this._mask;
        }
        set
        {
            if ((this._mask == value))
            {
                return;
            }
            if (((this._mask == null) 
                        || (_mask.Equals(value) != true)))
            {
                this._mask = value;
                this.OnPropertyChanged("mask", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(dtQuantEnum.EQ)]
    public virtual dtQuantEnum quantEnum
    {
        get
        {
            return this._quantEnum;
        }
        set
        {
            if ((_quantEnum.Equals(value) != true))
            {
                this._quantEnum = value;
                this.OnPropertyChanged("quantEnum", value);
            }
            _shouldSerializequantEnum = true;
        }
    }
    
    /// <summary>
    /// Test whether val should be serialized
    /// </summary>
    public virtual bool ShouldSerializeval()
    {
        if (_shouldSerializeval)
        {
            return true;
        }
        return (_val != default(double));
    }
    
    /// <summary>
    /// Test whether quantEnum should be serialized
    /// </summary>
    public virtual bool ShouldSerializequantEnum()
    {
        if (_shouldSerializequantEnum)
        {
            return true;
        }
        return (_quantEnum != default(dtQuantEnum));
    }
    
    /// <summary>
    /// Test whether mask should be serialized
    /// </summary>
    public virtual bool ShouldSerializemask()
    {
        return !string.IsNullOrEmpty(mask);
    }
}
}
#pragma warning restore
