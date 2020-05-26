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
public partial class int_DEtype : int_Stype
{
    
    #region Private fields
    private bool _shouldSerializeallowAPPROX;
    
    private bool _shouldSerializeallowLTE;
    
    private bool _shouldSerializeallowLT;
    
    private bool _shouldSerializeallowGTE;
    
    private bool _shouldSerializeallowGT;
    
    private bool _shouldSerializemaxExclusive;
    
    private bool _shouldSerializeminExclusive;
    
    private bool _shouldSerializemaxInclusive;
    
    private bool _shouldSerializeminInclusive;
    
    private int _minInclusive;
    
    private int _maxInclusive;
    
    private int _minExclusive;
    
    private int _maxExclusive;
    
    private byte _totalDigits;
    
    private string _mask;
    
    private bool _allowGT;
    
    private bool _allowGTE;
    
    private bool _allowLT;
    
    private bool _allowLTE;
    
    private bool _allowAPPROX;
    #endregion
    
    ///// <summary>
    ///// int_DEtype class constructor
    ///// </summary>
    //public int_DEtype()
    //{
    //    this._allowGT = false;
    //    this._allowGTE = false;
    //    this._allowLT = false;
    //    this._allowLTE = false;
    //    this._allowAPPROX = false;
    //}
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual int minInclusive
    {
        get
        {
            return this._minInclusive;
        }
        set
        {
            if ((_minInclusive.Equals(value) != true))
            {
                this._minInclusive = value;
                this.OnPropertyChanged("minInclusive", value);
            }
            _shouldSerializeminInclusive = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual int maxInclusive
    {
        get
        {
            return this._maxInclusive;
        }
        set
        {
            if ((_maxInclusive.Equals(value) != true))
            {
                this._maxInclusive = value;
                this.OnPropertyChanged("maxInclusive", value);
            }
            _shouldSerializemaxInclusive = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual int minExclusive
    {
        get
        {
            return this._minExclusive;
        }
        set
        {
            if ((_minExclusive.Equals(value) != true))
            {
                this._minExclusive = value;
                this.OnPropertyChanged("minExclusive", value);
            }
            _shouldSerializeminExclusive = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual int maxExclusive
    {
        get
        {
            return this._maxExclusive;
        }
        set
        {
            if ((_maxExclusive.Equals(value) != true))
            {
                this._maxExclusive = value;
                this.OnPropertyChanged("maxExclusive", value);
            }
            _shouldSerializemaxExclusive = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual byte totalDigits
    {
        get
        {
            return this._totalDigits;
        }
        set
        {
            if ((_totalDigits.Equals(value) != true))
            {
                this._totalDigits = value;
                this.OnPropertyChanged("totalDigits", value);
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
    /// Test whether minInclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializeminInclusive()
    {
        if (_shouldSerializeminInclusive)
        {
            return true;
        }
        return (_minInclusive != default(int));
    }
    
    /// <summary>
    /// Test whether maxInclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializemaxInclusive()
    {
        if (_shouldSerializemaxInclusive)
        {
            return true;
        }
        return (_maxInclusive != default(int));
    }
    
    /// <summary>
    /// Test whether minExclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializeminExclusive()
    {
        if (_shouldSerializeminExclusive)
        {
            return true;
        }
        return (_minExclusive != default(int));
    }
    
    /// <summary>
    /// Test whether maxExclusive should be serialized
    /// </summary>
    public virtual bool ShouldSerializemaxExclusive()
    {
        if (_shouldSerializemaxExclusive)
        {
            return true;
        }
        return (_maxExclusive != default(int));
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
    /// Test whether mask should be serialized
    /// </summary>
    public virtual bool ShouldSerializemask()
    {
        return !string.IsNullOrEmpty(mask);
    }
}
}
#pragma warning restore
