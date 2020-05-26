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
/// ListField is a grouper for list-like answer choices, which may be
/// derived from either a set of answer choice lists (a List composed of ListItems) or
/// or a list obtained from a LookupEndpoint URI.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
public partial class ListFieldType : ExtensionBaseType
{
    
    #region Private fields
    private bool _shouldSerializeordered;
    
    private bool _shouldSerializemaxSelections;
    
    private bool _shouldSerializeminSelections;
    
    private RichTextType _listHeaderText;
    
    private CodeSystemType _defaultCodeSystem;
    
    private ExtensionBaseType _item;
    
    private List<PredSelectionTestType> _illegalListItemPairings;
    
    private List<PredSingleSelectionSetsType> _illegalCoSelectedListItems;
    
    private List<EventType> _afterChange;
    
    private List<OnEventType> _onEvent;
    
    private string _colTextDelimiter;
    
    private byte _numCols;
    
    private byte _storedCol;
    
    private ushort _minSelections;
    
    private ushort _maxSelections;
    
    private bool _ordered;
    
    private string _defaultListItemDataType;
    #endregion
    //
    ///// <summary>
    ///// ListFieldType class constructor
    ///// </summary>
    //public ListFieldType()
    //{
    //    this._colTextDelimiter = "|";
    //    this._numCols = ((byte)(1));
    //    this._storedCol = ((byte)(1));
    //    this._minSelections = ((ushort)(1));
    //    this._maxSelections = ((ushort)(1));
    //    this._ordered = true;
    //}
    
    /// <summary>
    /// The header row for a set of list items. If the list
    /// has more than one column, the column text is separated by the
    /// colTextDelimiter.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public virtual RichTextType ListHeaderText
    {
        get
        {
            return this._listHeaderText;
        }
        set
        {
            if ((this._listHeaderText == value))
            {
                return;
            }
            if (((this._listHeaderText == null) 
                        || (_listHeaderText.Equals(value) != true)))
            {
                this._listHeaderText = value;
                this.OnPropertyChanged("ListHeaderText", value);
            }
        }
    }
    
    /// <summary>
    /// If coded values are used for items in a List
    /// (including ListItem and LookupEndPoint lists), then the default
    /// coding system should be specified here. For ListItem nodes, any
    /// exceptions to the coding system may be specified on the individual
    /// ListItem nodes. For LookupEndPoints, the endpoint data can
    /// optionally specify any exceptions in a dedicated CodeSystem column
    /// in the returned list data.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public virtual CodeSystemType DefaultCodeSystem
    {
        get
        {
            return this._defaultCodeSystem;
        }
        set
        {
            if ((this._defaultCodeSystem == value))
            {
                return;
            }
            if (((this._defaultCodeSystem == null) 
                        || (_defaultCodeSystem.Equals(value) != true)))
            {
                this._defaultCodeSystem = value;
                this.OnPropertyChanged("DefaultCodeSystem", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute("List", typeof(ListType), Order=2)]
    [System.Xml.Serialization.XmlElementAttribute("LookupEndPoint", typeof(LookupEndPointType), Order=2)]
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
    
    [System.Xml.Serialization.XmlElementAttribute("IllegalListItemPairings", Order=3)]
    public virtual List<PredSelectionTestType> IllegalListItemPairings
    {
        get
        {
            return this._illegalListItemPairings;
        }
        set
        {
            if ((this._illegalListItemPairings == value))
            {
                return;
            }
            if (((this._illegalListItemPairings == null) 
                        || (_illegalListItemPairings.Equals(value) != true)))
            {
                this._illegalListItemPairings = value;
                this.OnPropertyChanged("IllegalListItemPairings", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute("IllegalCoSelectedListItems", Order=4)]
    public virtual List<PredSingleSelectionSetsType> IllegalCoSelectedListItems
    {
        get
        {
            return this._illegalCoSelectedListItems;
        }
        set
        {
            if ((this._illegalCoSelectedListItems == value))
            {
                return;
            }
            if (((this._illegalCoSelectedListItems == null) 
                        || (_illegalCoSelectedListItems.Equals(value) != true)))
            {
                this._illegalCoSelectedListItems = value;
                this.OnPropertyChanged("IllegalCoSelectedListItems", value);
            }
        }
    }
    
    /// <summary>
    /// Event that occurs after List Field selections are
    /// changed.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute("AfterChange", Order=5)]
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
    
    [System.Xml.Serialization.XmlElementAttribute("OnEvent", Order=6)]
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
    /// Character in the DisplayText that separates the columns
    /// and rows in a single or multi-column list.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute("|")]
    public virtual string colTextDelimiter
    {
        get
        {
            return this._colTextDelimiter;
        }
        set
        {
            if ((this._colTextDelimiter == value))
            {
                return;
            }
            if (((this._colTextDelimiter == null) 
                        || (_colTextDelimiter.Equals(value) != true)))
            {
                this._colTextDelimiter = value;
                this.OnPropertyChanged("colTextDelimiter", value);
            }
        }
    }
    
    /// <summary>
    /// Number of columns in the list
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(typeof(byte), "1")]
    public virtual byte numCols
    {
        get
        {
            return this._numCols;
        }
        set
        {
            if ((_numCols.Equals(value) != true))
            {
                this._numCols = value;
                this.OnPropertyChanged("numCols", value);
            }
        }
    }
    
    /// <summary>
    /// Determines which column of the list is stored in a
    /// database. This list is one-based.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(typeof(byte), "1")]
    public virtual byte storedCol
    {
        get
        {
            return this._storedCol;
        }
        set
        {
            if ((_storedCol.Equals(value) != true))
            {
                this._storedCol = value;
                this.OnPropertyChanged("storedCol", value);
            }
        }
    }
    
    /// <summary>
    /// Minimum number of answer choices (list items) that must be
    /// selected by the user. Default value is 1. NEW: changed minimum value to
    /// 1. Removed: If set to 0, then this question need not be answered by the
    /// user.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(typeof(ushort), "1")]
    public virtual ushort minSelections
    {
        get
        {
            return this._minSelections;
        }
        set
        {
            if ((_minSelections.Equals(value) != true))
            {
                this._minSelections = value;
                this.OnPropertyChanged("minSelections", value);
            }
            _shouldSerializeminSelections = true;
        }
    }
    
    /// <summary>
    /// Maximum number of answer choices (list items) that can be
    /// selected by the user. Must be greater than or equal to minSelections,
    /// and no larger than the total number of list items. A value of 0
    /// indicates no limit to the number of selected list items (answers). This
    /// effectively means that the question is multi-select. (Abbreviated as QM)
    /// A value of 1 (the default) indicates that the question list is
    /// single-select. (Abbreviated as QS)
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(typeof(ushort), "1")]
    public virtual ushort maxSelections
    {
        get
        {
            return this._maxSelections;
        }
        set
        {
            if ((_maxSelections.Equals(value) != true))
            {
                this._maxSelections = value;
                this.OnPropertyChanged("maxSelections", value);
            }
            _shouldSerializemaxSelections = true;
        }
    }
    
    /// <summary>
    /// If false, then the form implementation may change the
    /// order of items in the list.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(true)]
    public virtual bool ordered
    {
        get
        {
            return this._ordered;
        }
        set
        {
            if ((_ordered.Equals(value) != true))
            {
                this._ordered = value;
                this.OnPropertyChanged("ordered", value);
            }
            _shouldSerializeordered = true;
        }
    }
    
    /// <summary>
    /// This attribute contains an SDC datatype enumeration. The
    /// selected value is the datatype of the content for all
    /// ListItem/@associatedValue content in the current List. It is used
    /// instead of associatedValueType. This element is used only if the
    /// ListItems are all associated with coded values from a single coding
    /// system. If associatedValueType on a ListItem has a datatype assigned,
    /// then the latter datatype overrides the content in
    /// defaultListItemDataType.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string defaultListItemDataType
    {
        get
        {
            return this._defaultListItemDataType;
        }
        set
        {
            if ((this._defaultListItemDataType == value))
            {
                return;
            }
            if (((this._defaultListItemDataType == null) 
                        || (_defaultListItemDataType.Equals(value) != true)))
            {
                this._defaultListItemDataType = value;
                this.OnPropertyChanged("defaultListItemDataType", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether IllegalListItemPairings should be serialized
    /// </summary>
    public virtual bool ShouldSerializeIllegalListItemPairings()
    {
        return IllegalListItemPairings != null && IllegalListItemPairings.Count > 0;
    }
    
    /// <summary>
    /// Test whether IllegalCoSelectedListItems should be serialized
    /// </summary>
    public virtual bool ShouldSerializeIllegalCoSelectedListItems()
    {
        return IllegalCoSelectedListItems != null && IllegalCoSelectedListItems.Count > 0;
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
    /// Test whether minSelections should be serialized
    /// </summary>
    public virtual bool ShouldSerializeminSelections()
    {
        if (_shouldSerializeminSelections)
        {
            return true;
        }
        return (_minSelections != default(ushort));
    }
    
    /// <summary>
    /// Test whether maxSelections should be serialized
    /// </summary>
    public virtual bool ShouldSerializemaxSelections()
    {
        if (_shouldSerializemaxSelections)
        {
            return true;
        }
        return (_maxSelections != default(ushort));
    }
    
    /// <summary>
    /// Test whether ordered should be serialized
    /// </summary>
    public virtual bool ShouldSerializeordered()
    {
        if (_shouldSerializeordered)
        {
            return true;
        }
        return (_ordered != default(bool));
    }
    
    /// <summary>
    /// Test whether ListHeaderText should be serialized
    /// </summary>
    public virtual bool ShouldSerializeListHeaderText()
    {
        return (_listHeaderText != null);
    }
    
    /// <summary>
    /// Test whether DefaultCodeSystem should be serialized
    /// </summary>
    public virtual bool ShouldSerializeDefaultCodeSystem()
    {
        return (_defaultCodeSystem != null);
    }
    
    /// <summary>
    /// Test whether Item should be serialized
    /// </summary>
    public virtual bool ShouldSerializeItem()
    {
        return (_item != null);
    }
    
    /// <summary>
    /// Test whether colTextDelimiter should be serialized
    /// </summary>
    public virtual bool ShouldSerializecolTextDelimiter()
    {
        return !string.IsNullOrEmpty(colTextDelimiter);
    }
    
    /// <summary>
    /// Test whether defaultListItemDataType should be serialized
    /// </summary>
    public virtual bool ShouldSerializedefaultListItemDataType()
    {
        return !string.IsNullOrEmpty(defaultListItemDataType);
    }
}
}
#pragma warning restore
