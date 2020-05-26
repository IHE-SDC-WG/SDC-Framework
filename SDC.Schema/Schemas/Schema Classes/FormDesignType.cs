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
/// Start here. This is the top level of the SDCFormDesign object model.
/// It represents the definition for the information content of a single data-entry
/// form.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ihe:qrph:sdc:2016")]
[System.Xml.Serialization.XmlRootAttribute("FormDesign", Namespace="urn:ihe:qrph:sdc:2016", IsNullable=false)]
public partial class FormDesignType : IdentifiedExtensionType
{
    
    #region Private fields
    private bool _shouldSerializecompletionStatus;
    
    private bool _shouldSerializeapprovalStatus;
    
    private bool _shouldSerializenewData;
    
    private bool _shouldSerializechangedData;
    
    private bool _shouldSerializeinstanceVersionPrev;
    
    private bool _shouldSerializeinstanceVersion;
    
    private EventType _beforeLoadForm;
    
    private EventType _beforeLoadData;
    
    private EventType _beforeShowForm;
    
    private EventType _beforeDataSubmit;
    
    private EventType _beforeCloseForm;
    
    private List<OnEventType> _onEvent;
    
    private SectionItemType _header;
    
    private SectionItemType _body;
    
    private SectionItemType _footer;
    
    private RulesType _rules;
    
    private string _lineage;
    
    private string _version;
    
    private string _versionPrev;
    
    private string _fullURI;
    
    private string _filename;
    
    private string _formTitle;
    
    private string _basedOnURI;
    
    private string _instanceID;
    
    private System.DateTime _instanceVersion;
    
    private string _instanceVersionURI;
    
    private System.DateTime _instanceVersionPrev;
    
    private FormDesignTypeApprovalStatus _approvalStatus;
    
    private FormDesignTypeCompletionStatus _completionStatus;
    
    private bool _changedData;
    
    private bool _newData;
    #endregion
    
    /// <summary>
    /// NEW: This event is fired before the page is loaded
    /// into memory, and before stored form data is loaded. It may be used,
    /// e.g., for authentication, to retrieve/prepare stored data, and/or to
    /// control form rendering according to user
    /// preferences.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public virtual EventType BeforeLoadForm
    {
        get
        {
            return this._beforeLoadForm;
        }
        set
        {
            if ((this._beforeLoadForm == value))
            {
                return;
            }
            if (((this._beforeLoadForm == null) 
                        || (_beforeLoadForm.Equals(value) != true)))
            {
                this._beforeLoadForm = value;
                this.OnPropertyChanged("BeforeLoadForm", value);
            }
        }
    }
    
    /// <summary>
    /// NEW: This event is fired after the page is loaded into
    /// memory, before stored form data is loaded, and before the form is
    /// visible. For example, It may be used to determine the data to be
    /// loaded and to perform the data loading.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public virtual EventType BeforeLoadData
    {
        get
        {
            return this._beforeLoadData;
        }
        set
        {
            if ((this._beforeLoadData == value))
            {
                return;
            }
            if (((this._beforeLoadData == null) 
                        || (_beforeLoadData.Equals(value) != true)))
            {
                this._beforeLoadData = value;
                this.OnPropertyChanged("BeforeLoadData", value);
            }
        }
    }
    
    /// <summary>
    /// NEW: This event is fired after the page is loaded is
    /// memory, after the data is loaded into the form, but before the form
    /// is displayed. It may be used to perform form activities that are
    /// controlled by the loaded data.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public virtual EventType BeforeShowForm
    {
        get
        {
            return this._beforeShowForm;
        }
        set
        {
            if ((this._beforeShowForm == value))
            {
                return;
            }
            if (((this._beforeShowForm == null) 
                        || (_beforeShowForm.Equals(value) != true)))
            {
                this._beforeShowForm = value;
                this.OnPropertyChanged("BeforeShowForm", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    public virtual EventType BeforeDataSubmit
    {
        get
        {
            return this._beforeDataSubmit;
        }
        set
        {
            if ((this._beforeDataSubmit == value))
            {
                return;
            }
            if (((this._beforeDataSubmit == null) 
                        || (_beforeDataSubmit.Equals(value) != true)))
            {
                this._beforeDataSubmit = value;
                this.OnPropertyChanged("BeforeDataSubmit", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=4)]
    public virtual EventType BeforeCloseForm
    {
        get
        {
            return this._beforeCloseForm;
        }
        set
        {
            if ((this._beforeCloseForm == value))
            {
                return;
            }
            if (((this._beforeCloseForm == null) 
                        || (_beforeCloseForm.Equals(value) != true)))
            {
                this._beforeCloseForm = value;
                this.OnPropertyChanged("BeforeCloseForm", value);
            }
        }
    }
    
    /// <summary>
    /// Generic event handler - eventName must be
    /// specified.
    /// </summary>
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
    /// Optional Section that stays at the top of a
    /// form.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=6)]
    public virtual SectionItemType Header
    {
        get
        {
            return this._header;
        }
        set
        {
            if ((this._header == value))
            {
                return;
            }
            if (((this._header == null) 
                        || (_header.Equals(value) != true)))
            {
                this._header = value;
                this.OnPropertyChanged("Header", value);
            }
        }
    }
    
    /// <summary>
    /// Main Section of form
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=7)]
    public virtual SectionItemType Body
    {
        get
        {
            return this._body;
        }
        set
        {
            if ((this._body == value))
            {
                return;
            }
            if (((this._body == null) 
                        || (_body.Equals(value) != true)))
            {
                this._body = value;
                this.OnPropertyChanged("Body", value);
            }
        }
    }
    
    /// <summary>
    /// Optional Section that stays at the bottom of a
    /// form.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(Order=8)]
    public virtual SectionItemType Footer
    {
        get
        {
            return this._footer;
        }
        set
        {
            if ((this._footer == value))
            {
                return;
            }
            if (((this._footer == null) 
                        || (_footer.Equals(value) != true)))
            {
                this._footer = value;
                this.OnPropertyChanged("Footer", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=9)]
    public virtual RulesType Rules
    {
        get
        {
            return this._rules;
        }
        set
        {
            if ((this._rules == value))
            {
                return;
            }
            if (((this._rules == null) 
                        || (_rules.Equals(value) != true)))
            {
                this._rules = value;
                this.OnPropertyChanged("Rules", value);
            }
        }
    }
    
    /// <summary>
    /// A string identifier that is used to group multiple
    /// versions of a single form. The lineage is constant for all versions of a
    /// single kind of form. When appended to baseURI, it can be used to
    /// retrieve all versions of one particular form. Example:
    /// @lineage="Lung.Bmk.227"
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string lineage
    {
        get
        {
            return this._lineage;
        }
        set
        {
            if ((this._lineage == value))
            {
                return;
            }
            if (((this._lineage == null) 
                        || (_lineage.Equals(value) != true)))
            {
                this._lineage = value;
                this.OnPropertyChanged("lineage", value);
            }
        }
    }
    
    /// <summary>
    /// @version contains the version text for the current form.
    /// It is designed to be used in conjunction with @baseURI and @lineage.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string version
    {
        get
        {
            return this._version;
        }
        set
        {
            if ((this._version == value))
            {
                return;
            }
            if (((this._version == null) 
                        || (_version.Equals(value) != true)))
            {
                this._version = value;
                this.OnPropertyChanged("version", value);
            }
        }
    }
    
    /// <summary>
    /// @versionPrev identifies the immediate previous version of
    /// the current FDF. The format is the same as version. The primary role of
    /// this optional attribute is to allow automated comparisons between a
    /// current FDF and the immediate previous FDF version. This is often
    /// helpful when deciding whether to adopt a newer version of an FDF.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string versionPrev
    {
        get
        {
            return this._versionPrev;
        }
        set
        {
            if ((this._versionPrev == value))
            {
                return;
            }
            if (((this._versionPrev == null) 
                        || (_versionPrev.Equals(value) != true)))
            {
                this._versionPrev = value;
                this.OnPropertyChanged("versionPrev", value);
            }
        }
    }
    
    /// <summary>
    /// The full URI that uniquely identifies the current form. It
    /// is created by concatenating @baseURI + lineage + version. Each of the
    /// components is separated by a single forward slash.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
    public virtual string fullURI
    {
        get
        {
            return this._fullURI;
        }
        set
        {
            if ((this._fullURI == value))
            {
                return;
            }
            if (((this._fullURI == null) 
                        || (_fullURI.Equals(value) != true)))
            {
                this._fullURI = value;
                this.OnPropertyChanged("fullURI", value);
            }
        }
    }
    
    /// <summary>
    /// @filename is the filename of the FDF when is saved to a
    /// file storage device (e.g., a disk or USB drive). The filename appears
    /// inside the FDF XML to help ensure the identity of the FDF content in
    /// case the saved filename (on a disk drive, etc.) has been changed for any
    /// reason.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string filename
    {
        get
        {
            return this._filename;
        }
        set
        {
            if ((this._filename == value))
            {
                return;
            }
            if (((this._filename == null) 
                        || (_filename.Equals(value) != true)))
            {
                this._filename = value;
                this.OnPropertyChanged("filename", value);
            }
        }
    }
    
    /// <summary>
    /// @formTitle is a human readable title for display when
    /// choosing forms. Added 4/27/16
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string formTitle
    {
        get
        {
            return this._formTitle;
        }
        set
        {
            if ((this._formTitle == value))
            {
                return;
            }
            if (((this._formTitle == null) 
                        || (_formTitle.Equals(value) != true)))
            {
                this._formTitle = value;
                this.OnPropertyChanged("formTitle", value);
            }
        }
    }
    
    /// <summary>
    /// @basedOnURI is a URI that identifies the SDC form that
    /// that the current FDF is based upon. In most cases, this should be a
    /// standard SDC form that is modified and/or extended by the current FDF.
    /// It’s best to avoid using prefixes like "http://" or "https://" because
    /// these can occasionally cause XML validation errors when used in a
    /// URI-typed field. The URI format should be the same format used in
    /// fullURI, which is patterned after the SDC web service API.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
    public virtual string basedOnURI
    {
        get
        {
            return this._basedOnURI;
        }
        set
        {
            if ((this._basedOnURI == value))
            {
                return;
            }
            if (((this._basedOnURI == null) 
                        || (_basedOnURI.Equals(value) != true)))
            {
                this._basedOnURI = value;
                this.OnPropertyChanged("basedOnURI", value);
            }
        }
    }
    
    /// <summary>
    /// @instanceID is unique string (e.g., a GUID) used to
    /// identify a unique instance of a form, such as a form used during a
    /// single patient encounter. The @instanceID is used to track saved form
    /// responses across time and across multiple episodes of editing by
    /// end-users. This string does not change for each edit session of a form
    /// or package instance. The @instanceID is required in an FDF-R; It is not
    /// allowed in an FDF.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual string instanceID
    {
        get
        {
            return this._instanceID;
        }
        set
        {
            if ((this._instanceID == value))
            {
                return;
            }
            if (((this._instanceID == null) 
                        || (_instanceID.Equals(value) != true)))
            {
                this._instanceID = value;
                this.OnPropertyChanged("instanceID", value);
            }
        }
    }
    
    /// <summary>
    /// @instanceVersion Timestamp is used to identify a unique instance of a form.
    /// Used for tracking form responses across time and across multiple
    /// episodes of editing by end-users. This field must change for each edit
    /// session of a form instance. The instanceVersion is required in an FDF-R;
    /// It is not allowed in an FDF.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual System.DateTime instanceVersion
    {
        get
        {
            return this._instanceVersion;
        }
        set
        {
            if ((_instanceVersion.Equals(value) != true))
            {
                this._instanceVersion = value;
                this.OnPropertyChanged("instanceVersion", value);
            }
            _shouldSerializeinstanceVersion = true;
        }
    }
    
    /// <summary>
    /// NEW: Globally-unique URI used to identify a unique
    /// instance of a form's saved responses. It is used for tracking form
    /// responses across time and across multiple episodes of editing by
    /// end-users. The instanceVersionURI must change for each edit/save session
    /// of a form instance (defined by instanceVersion). The instanceVersionURI
    /// should be formatted similarly to the fullURI but must include values for
    /// instanceID and instanceVersion. The instanceVersion value is the release
    /// date/time for the new version, in W3C datetime format. An example
    /// instanceVersionURI is:
    /// instanceVersionURI="_baseURI=cap.org&_lineage=Lung.Bmk.227&_version=1.001.011.RC1
    /// &_instanceID=Abc1dee2fg987&_instanceVersion=2019-07-16T19:20:30+01:00&_docType=sdcFDFR
    /// " It is possible to create a shorter URI without the _baseURI, _lineage
    /// and _version parameters, as long as the URI is able to globally and
    /// uniquely identify and retrieve the instance and version of the FDF-R
    /// that was transmitted:
    /// instanceVersionURI="_instanceID=Abc1dee2fg987&_instanceVersion=2019-07-16T19:20:30+01:00&_docType=sdcFDFR"
    /// Note that the FR webservice endpoint URI is not provided in the
    /// instanceVersionURI. The FR endpoint and its security settings may be
    /// found in the SDC Package that contains the FDF-R, at
    /// SDCPackage/SubmissionRule. An FR may also be provided in a custom FDF
    /// Property if desired. The docType for instanceVersionURI is sdcFDFR. The
    /// specific order of components shown in the URI examples is not required,
    /// but the component order shown above is suggested for consistency and
    /// readability. The instanceVersionURI is not required, and is not allowed
    /// in an FDF.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
    public virtual string instanceVersionURI
    {
        get
        {
            return this._instanceVersionURI;
        }
        set
        {
            if ((this._instanceVersionURI == value))
            {
                return;
            }
            if (((this._instanceVersionURI == null) 
                        || (_instanceVersionURI.Equals(value) != true)))
            {
                this._instanceVersionURI = value;
                this.OnPropertyChanged("instanceVersionURI", value);
            }
        }
    }
    
    /// <summary>
    /// NEW: Unique dateTime used to identify the immediate
    /// previous instance of an form instance. Used for tracking form responses
    /// across time and across multiple episodes of editing by end-users. This
    /// field must change for each edit session of a form instance.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual System.DateTime instanceVersionPrev
    {
        get
        {
            return this._instanceVersionPrev;
        }
        set
        {
            if ((_instanceVersionPrev.Equals(value) != true))
            {
                this._instanceVersionPrev = value;
                this.OnPropertyChanged("instanceVersionPrev", value);
            }
            _shouldSerializeinstanceVersionPrev = true;
        }
    }
    
    /// <summary>
    /// Describes report fitness for clinical or other action
    /// inProcess: currently being edited, users should not rely on results
    /// preliminary: report is awaiting final review and approval approved:
    /// report is fit for clinical or other action; often synonymous with final
    /// cancelled: report/procedure has been aborted before issued retracted:
    /// report has been deemed unfit for clinical or other action
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual FormDesignTypeApprovalStatus approvalStatus
    {
        get
        {
            return this._approvalStatus;
        }
        set
        {
            if ((_approvalStatus.Equals(value) != true))
            {
                this._approvalStatus = value;
                this.OnPropertyChanged("approvalStatus", value);
            }
            _shouldSerializeapprovalStatus = true;
        }
    }
    
    /// <summary>
    /// The extent to which a report contains all of the requested
    /// information pending: no information is yet available incomplete: some
    /// requested information is not yet available complete: all information is
    /// available in the requested report
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual FormDesignTypeCompletionStatus completionStatus
    {
        get
        {
            return this._completionStatus;
        }
        set
        {
            if ((_completionStatus.Equals(value) != true))
            {
                this._completionStatus = value;
                this.OnPropertyChanged("completionStatus", value);
            }
            _shouldSerializecompletionStatus = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual bool changedData
    {
        get
        {
            return this._changedData;
        }
        set
        {
            if ((_changedData.Equals(value) != true))
            {
                this._changedData = value;
                this.OnPropertyChanged("changedData", value);
            }
            _shouldSerializechangedData = true;
        }
    }
    
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public virtual bool newData
    {
        get
        {
            return this._newData;
        }
        set
        {
            if ((_newData.Equals(value) != true))
            {
                this._newData = value;
                this.OnPropertyChanged("newData", value);
            }
            _shouldSerializenewData = true;
        }
    }
    
    /// <summary>
    /// Test whether OnEvent should be serialized
    /// </summary>
    public virtual bool ShouldSerializeOnEvent()
    {
        return OnEvent != null && OnEvent.Count > 0;
    }
    
    /// <summary>
    /// Test whether instanceVersion should be serialized
    /// </summary>
    public virtual bool ShouldSerializeinstanceVersion()
    {
        if (_shouldSerializeinstanceVersion)
        {
            return true;
        }
        return (_instanceVersion != default(System.DateTime));
    }
    
    /// <summary>
    /// Test whether instanceVersionPrev should be serialized
    /// </summary>
    public virtual bool ShouldSerializeinstanceVersionPrev()
    {
        if (_shouldSerializeinstanceVersionPrev)
        {
            return true;
        }
        return (_instanceVersionPrev != default(System.DateTime));
    }
    
    /// <summary>
    /// Test whether changedData should be serialized
    /// </summary>
    public virtual bool ShouldSerializechangedData()
    {
        if (_shouldSerializechangedData)
        {
            return true;
        }
        return (_changedData != default(bool));
    }
    
    /// <summary>
    /// Test whether newData should be serialized
    /// </summary>
    public virtual bool ShouldSerializenewData()
    {
        if (_shouldSerializenewData)
        {
            return true;
        }
        return (_newData != default(bool));
    }
    
    /// <summary>
    /// Test whether approvalStatus should be serialized
    /// </summary>
    public virtual bool ShouldSerializeapprovalStatus()
    {
        if (_shouldSerializeapprovalStatus)
        {
            return true;
        }
        return (_approvalStatus != default(FormDesignTypeApprovalStatus));
    }
    
    /// <summary>
    /// Test whether completionStatus should be serialized
    /// </summary>
    public virtual bool ShouldSerializecompletionStatus()
    {
        if (_shouldSerializecompletionStatus)
        {
            return true;
        }
        return (_completionStatus != default(FormDesignTypeCompletionStatus));
    }
    
    /// <summary>
    /// Test whether BeforeLoadForm should be serialized
    /// </summary>
    public virtual bool ShouldSerializeBeforeLoadForm()
    {
        return (_beforeLoadForm != null);
    }
    
    /// <summary>
    /// Test whether BeforeLoadData should be serialized
    /// </summary>
    public virtual bool ShouldSerializeBeforeLoadData()
    {
        return (_beforeLoadData != null);
    }
    
    /// <summary>
    /// Test whether BeforeShowForm should be serialized
    /// </summary>
    public virtual bool ShouldSerializeBeforeShowForm()
    {
        return (_beforeShowForm != null);
    }
    
    /// <summary>
    /// Test whether BeforeDataSubmit should be serialized
    /// </summary>
    public virtual bool ShouldSerializeBeforeDataSubmit()
    {
        return (_beforeDataSubmit != null);
    }
    
    /// <summary>
    /// Test whether BeforeCloseForm should be serialized
    /// </summary>
    public virtual bool ShouldSerializeBeforeCloseForm()
    {
        return (_beforeCloseForm != null);
    }
    
    /// <summary>
    /// Test whether Header should be serialized
    /// </summary>
    public virtual bool ShouldSerializeHeader()
    {
        return (_header != null);
    }
    
    /// <summary>
    /// Test whether Body should be serialized
    /// </summary>
    public virtual bool ShouldSerializeBody()
    {
        return (_body != null);
    }
    
    /// <summary>
    /// Test whether Footer should be serialized
    /// </summary>
    public virtual bool ShouldSerializeFooter()
    {
        return (_footer != null);
    }
    
    /// <summary>
    /// Test whether Rules should be serialized
    /// </summary>
    public virtual bool ShouldSerializeRules()
    {
        return (_rules != null);
    }
    
    /// <summary>
    /// Test whether lineage should be serialized
    /// </summary>
    public virtual bool ShouldSerializelineage()
    {
        return !string.IsNullOrEmpty(lineage);
    }
    
    /// <summary>
    /// Test whether version should be serialized
    /// </summary>
    public virtual bool ShouldSerializeversion()
    {
        return !string.IsNullOrEmpty(version);
    }
    
    /// <summary>
    /// Test whether versionPrev should be serialized
    /// </summary>
    public virtual bool ShouldSerializeversionPrev()
    {
        return !string.IsNullOrEmpty(versionPrev);
    }
    
    /// <summary>
    /// Test whether fullURI should be serialized
    /// </summary>
    public virtual bool ShouldSerializefullURI()
    {
        return !string.IsNullOrEmpty(fullURI);
    }
    
    /// <summary>
    /// Test whether filename should be serialized
    /// </summary>
    public virtual bool ShouldSerializefilename()
    {
        return !string.IsNullOrEmpty(filename);
    }
    
    /// <summary>
    /// Test whether formTitle should be serialized
    /// </summary>
    public virtual bool ShouldSerializeformTitle()
    {
        return !string.IsNullOrEmpty(formTitle);
    }
    
    /// <summary>
    /// Test whether basedOnURI should be serialized
    /// </summary>
    public virtual bool ShouldSerializebasedOnURI()
    {
        return !string.IsNullOrEmpty(basedOnURI);
    }
    
    /// <summary>
    /// Test whether instanceID should be serialized
    /// </summary>
    public virtual bool ShouldSerializeinstanceID()
    {
        return !string.IsNullOrEmpty(instanceID);
    }
    
    /// <summary>
    /// Test whether instanceVersionURI should be serialized
    /// </summary>
    public virtual bool ShouldSerializeinstanceVersionURI()
    {
        return !string.IsNullOrEmpty(instanceVersionURI);
    }
}
}
#pragma warning restore
