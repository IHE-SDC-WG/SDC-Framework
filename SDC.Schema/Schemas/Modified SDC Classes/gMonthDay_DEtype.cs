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
public partial class gMonthDay_DEtype : gMonthDay_Stype
{
    
    #region Private fields
    private bool _shouldSerializeallowAPPROX;
    
    private bool _shouldSerializeallowLTE;
    
    private bool _shouldSerializeallowLT;
    
    private bool _shouldSerializeallowGTE;
    
    private bool _shouldSerializeallowGT;
    
    private string _minInclusive;
    
    private string _maxInclusive;
    
    private string _minExclusive;
    
    private string _maxExclusive;
    
    private string _mask;
    
    private bool _allowGT;
    
    private bool _allowGTE;
    
    private bool _allowLT;
    
    private bool _allowLTE;
    
    private bool _allowAPPROX;
    #endregion
    
    ///// <summary>
    ///// gMonthDay_DEtype class constructor
    ///// </summary>
    //public gMonthDay_DEtype()
    //{
    //    this._allowGT = false;
    //    this._allowGTE = false;
    //    this._allowLT = false;
    //    this._allowLTE = false;
    //    this._allowAPPROX = false;
    //}
    
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="gMonthDay")]
    public virtual string minInclusive
    {
        get
        {
            return this._minInclusive;
        }
        set
        {
            if ((this._minInclusive == value))
            {
                return;
            }
            if (((this._minInclusive == null) 
                        || (_minInclusive.Equals(value) != true)))
            {
                this._minInclusive = value;
                this.OnPropertyChanged("minInclusive", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="gMonthDay")]
    public virtual string maxInclusive
    {
        get
        {
            return this._maxInclusive;
        }
        set
        {
            if ((this._maxInclusive == value))
            {
                return;
            }
            if (((this._maxInclusive == null) 
                        || (_maxInclusive.Equals(value) != true)))
            {
                this._maxInclusive = value;
                this.OnPropertyChanged("maxInclusive", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="gMonthDay")]
    public virtual string minExclusive
    {
        get
        {
            return this._minExclusive;
        }
        set
        {
            if ((this._minExclusive == value))
            {
                return;
            }
            if (((this._minExclusive == null) 
                        || (_minExclusive.Equals(value) != true)))
            {
                this._minExclusive = value;
                this.OnPropertyChanged("minExclusive", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="gMonthDay")]
    public virtual string maxExclusive
    {
        get
        {
            return this._maxExclusive;
        }
        set
        {
            if ((this._maxExclusive == value))
            {
                return;
            }
            if (((this._maxExclusive == null) 
                        || (_maxExclusive.Equals(value) != true)))
            {
                this._maxExclusive = value;
                this.OnPropertyChanged("maxExclusive", value);
            }
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
    [System.ComponentModel.DefaultValueAttribute(false)]
    public virtual bool allowGT
    {
        get
        {
            return this._allowGT;
        }
        set
        {
            if ((_allowGT.Equals(value) != true))
            {
                this._allowGT = value;
                this.OnPropertyChanged("allowGT", value);
            }
            _shouldSerializeallowGT = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public virtual bool allowGTE
    {
        get
        {
            return this._allowGTE;
        }
        set
        {
            if ((_allowGTE.Equals(value) != true))
            {
                this._allowGTE = value;
                this.OnPropertyChanged("allowGTE", value);
            }
            _shouldSerializeallowGTE = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public virtual bool allowLT
    {
        get
        {
            return this._allowLT;
        }
        set
        {
            if ((_allowLT.Equals(value) != true))
            {
                this._allowLT = value;
                this.OnPropertyChanged("allowLT", value);
            }
            _shouldSerializeallowLT = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public virtual bool allowLTE
    {
        get
        {
            return this._allowLTE;
        }
        set
        {
            if ((_allowLTE.Equals(value) != true))
            {
                this._allowLTE = value;
                this.OnPropertyChanged("allowLTE", value);
            }
            _shouldSerializeallowLTE = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public virtual bool allowAPPROX
    {
        get
        {
            return this._allowAPPROX;
        }
        set
        {
            if ((_allowAPPROX.Equals(value) != true))
            {
                this._allowAPPROX = value;
                this.OnPropertyChanged("allowAPPROX", value);
            }
            _shouldSerializeallowAPPROX = true;
        }
    }
    
    /// <summary>
    /// Test whether allowGT should be serialized
    /// </summary>
    public virtual bool ShouldSerializeallowGT()
    {
        if (_shouldSerializeallowGT)
        {
            return true;
        }
        return (_allowGT != default(bool));
    }
    
    /// <summary>
    /// Test whether allowGTE should be serialized
    /// </summary>
    public virtual bool ShouldSerializeallowGTE()
    {
        if (_shouldSerializeallowGTE)
        {
            return true;
        }
        return (_allowGTE != default(bool));
    }
    
    /// <summary>
    /// Test whether allowLT should be serialized
    /// </summary>
    public virtual bool ShouldSerializeallowLT()
    {
        if (_shouldSerializeallowLT)
        {
            return true;
        }
        return (_allowLT != default(bool));
    }
    
    /// <summary>
    /// Test whether allowLTE should be serialized
    /// </summary>
    public virtual bool ShouldSerializeallowLTE()
    {
        if (_shouldSerializeallowLTE)
        {
            return true;
        }
        return (_allowLTE != default(bool));
    }
    
    /// <summary>
    /// Test whether allowAPPROX should be serialized
    /// </summary>
    public virtual bool ShouldSerializeallowAPPROX()
    {
        if (_shouldSerializeallowAPPROX)
        {
            return true;
        }
        return (_allowAPPROX != default(bool));
    }
    
    /// <summary>
    /// Test whether minInclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializeminInclusive()
    {
        return !string.IsNullOrEmpty(minInclusive);
    }
    
    /// <summary>
    /// Test whether maxInclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializemaxInclusive()
    {
        return !string.IsNullOrEmpty(maxInclusive);
    }
    
    /// <summary>
    /// Test whether minExclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializeminExclusive()
    {
        return !string.IsNullOrEmpty(minExclusive);
    }
    
    /// <summary>
    /// Test whether maxExclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializemaxExclusive()
    {
        return !string.IsNullOrEmpty(maxExclusive);
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
