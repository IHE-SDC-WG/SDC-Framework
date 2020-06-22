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
/// This type is a template for a data entry field that accepts entries
/// (responses or answers) of any data type, including text, numbers, dates, and Base 64
/// - encoded blobs (images, sounds, video, other binary formats, etc.). This type
/// cannot contain ListItems, but it may be a child of a Question or a
/// ListItem.
/// </summary>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ListItemResponseFieldType))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ResponseFieldType : ExtensionBaseType
{
    
    #region Private fields
    private DataTypes_DEType _response;
    
    private ExtensionBaseType _item;
    
    private RichTextType _textAfterResponse;
    
    private UnitsType _responseUnits;
    
    private List<EventType> _afterChange;
    
    private List<OnEventType> _onEvent;
    #endregion
    
    /// <summary>
    /// W3C Schema data type details
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("Response", Order=0)]
    public virtual DataTypes_DEType Response
    {
        get
        {
            return this._response;
        }
        set
        {
            if ((this._response == value))
            {
                return;
            }
            if (((this._response == null) 
                        || (_response.Equals(value) != true)))
            {
                this._response = value;
                this.OnPropertyChanged("Response", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute("CallSetValue", typeof(CallFuncActionType), Order=1)]
    [System.Xml.Serialization.XmlElementAttribute("SetValue", typeof(ScriptCodeAnyType), Order=1)]
    public virtual ExtensionBaseType Item
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
    /// TextAfterResponse is fixed text that appears after (to
    /// the right of) the user's response on the data entry form. This may
    /// be text for units such as "mm", "cm", etc.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("TextAfterResponse", Order=2)]
    public virtual RichTextType TextAfterResponse
    {
        get
        {
            return this._textAfterResponse;
        }
        set
        {
            if ((this._textAfterResponse == value))
            {
                return;
            }
            if (((this._textAfterResponse == null) 
                        || (_textAfterResponse.Equals(value) != true)))
            {
                this._textAfterResponse = value;
                this.OnPropertyChanged("TextAfterResponse", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute("ResponseUnits", Order=3)]
    public virtual UnitsType ResponseUnits
    {
        get
        {
            return this._responseUnits;
        }
        set
        {
            if ((this._responseUnits == value))
            {
                return;
            }
            if (((this._responseUnits == null) 
                        || (_responseUnits.Equals(value) != true)))
            {
                this._responseUnits = value;
                this.OnPropertyChanged("ResponseUnits", value);
            }
        }
    }
    
    /// <summary>
    /// Event that occurs after the Response value is changed,
    /// usually fired after a user leaves the Response
    /// field.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("AfterChange", Order=4)]
    public virtual List<EventType> AfterChange
    {
        get
        {
            return this._afterChange;
        }
        set
        {
            if ((this._afterChange == value))
            {
                return;
            }
            if (((this._afterChange == null) 
                        || (_afterChange.Equals(value) != true)))
            {
                this._afterChange = value;
                this.OnPropertyChanged("AfterChange", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute("OnEvent", Order=5)]
    public virtual List<OnEventType> OnEvent
    {
        get
        {
            return this._onEvent;
        }
        set
        {
            if ((this._onEvent == value))
            {
                return;
            }
            if (((this._onEvent == null) 
                        || (_onEvent.Equals(value) != true)))
            {
                this._onEvent = value;
                this.OnPropertyChanged("OnEvent", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether AfterChange should be serialized
    /// </summary>
    public virtual bool ShouldSerializeAfterChange()
    {
        return AfterChange != null && AfterChange.Count > 0;
    }
    
    /// <summary>
    /// Test whether OnEvent should be serialized
    /// </summary>
    public virtual bool ShouldSerializeOnEvent()
    {
        return OnEvent != null && OnEvent.Count > 0;
    }
    
    /// <summary>
    /// Test whether Response should be serialized
    /// </summary>
    public virtual bool ShouldSerializeResponse()
    {
        return (_response != null);
    }
    
    /// <summary>
    /// Test whether Item should be serialized
    /// </summary>
    public virtual bool ShouldSerializeItem()
    {
        return (_item != null);
    }
    
    /// <summary>
    /// Test whether TextAfterResponse should be serialized
    /// </summary>
    public virtual bool ShouldSerializeTextAfterResponse()
    {
        return (_textAfterResponse != null);
    }
    
    /// <summary>
    /// Test whether ResponseUnits should be serialized
    /// </summary>
    public virtual bool ShouldSerializeResponseUnits()
    {
        return (_responseUnits != null);
    }
}
}
#pragma warning restore
