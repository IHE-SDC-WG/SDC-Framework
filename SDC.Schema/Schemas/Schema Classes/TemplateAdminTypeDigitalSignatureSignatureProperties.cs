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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:ihe:qrph:sdc:2016")]
public partial class TemplateAdminTypeDigitalSignatureSignatureProperties : ExtensionBaseType
{
    
    #region Private fields
    private ContactType _signer;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesSignerPublicKeyCertificate _signerPublicKeyCertificate;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesSignatureReason _signatureReason;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesSignatureDateTimeStamp _signatureDateTimeStamp;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesX_CertificateAuthority _x_CertificateAuthority;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesX_SignerPublicKey _x_SignerPublicKey;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesX_SignatureAlgorithm _x_SignatureAlgorithm;
    
    private TemplateAdminTypeDigitalSignatureSignaturePropertiesImage _image;
    #endregion
    
    [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    public virtual ContactType Signer
    {
        get
        {
            return this._signer;
        }
        set
        {
            if ((this._signer == value))
            {
                return;
            }
            if (((this._signer == null) 
                        || (_signer.Equals(value) != true)))
            {
                this._signer = value;
                this.OnPropertyChanged("Signer", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesSignerPublicKeyCertificate SignerPublicKeyCertificate
    {
        get
        {
            return this._signerPublicKeyCertificate;
        }
        set
        {
            if ((this._signerPublicKeyCertificate == value))
            {
                return;
            }
            if (((this._signerPublicKeyCertificate == null) 
                        || (_signerPublicKeyCertificate.Equals(value) != true)))
            {
                this._signerPublicKeyCertificate = value;
                this.OnPropertyChanged("SignerPublicKeyCertificate", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesSignatureReason SignatureReason
    {
        get
        {
            return this._signatureReason;
        }
        set
        {
            if ((this._signatureReason == value))
            {
                return;
            }
            if (((this._signatureReason == null) 
                        || (_signatureReason.Equals(value) != true)))
            {
                this._signatureReason = value;
                this.OnPropertyChanged("SignatureReason", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesSignatureDateTimeStamp SignatureDateTimeStamp
    {
        get
        {
            return this._signatureDateTimeStamp;
        }
        set
        {
            if ((this._signatureDateTimeStamp == value))
            {
                return;
            }
            if (((this._signatureDateTimeStamp == null) 
                        || (_signatureDateTimeStamp.Equals(value) != true)))
            {
                this._signatureDateTimeStamp = value;
                this.OnPropertyChanged("SignatureDateTimeStamp", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=4)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesX_CertificateAuthority X_CertificateAuthority
    {
        get
        {
            return this._x_CertificateAuthority;
        }
        set
        {
            if ((this._x_CertificateAuthority == value))
            {
                return;
            }
            if (((this._x_CertificateAuthority == null) 
                        || (_x_CertificateAuthority.Equals(value) != true)))
            {
                this._x_CertificateAuthority = value;
                this.OnPropertyChanged("X_CertificateAuthority", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=5)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesX_SignerPublicKey X_SignerPublicKey
    {
        get
        {
            return this._x_SignerPublicKey;
        }
        set
        {
            if ((this._x_SignerPublicKey == value))
            {
                return;
            }
            if (((this._x_SignerPublicKey == null) 
                        || (_x_SignerPublicKey.Equals(value) != true)))
            {
                this._x_SignerPublicKey = value;
                this.OnPropertyChanged("X_SignerPublicKey", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=6)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesX_SignatureAlgorithm X_SignatureAlgorithm
    {
        get
        {
            return this._x_SignatureAlgorithm;
        }
        set
        {
            if ((this._x_SignatureAlgorithm == value))
            {
                return;
            }
            if (((this._x_SignatureAlgorithm == null) 
                        || (_x_SignatureAlgorithm.Equals(value) != true)))
            {
                this._x_SignatureAlgorithm = value;
                this.OnPropertyChanged("X_SignatureAlgorithm", value);
            }
        }
    }
    
    [System.Xml.Serialization.XmlElementAttribute(Order=7)]
    public virtual TemplateAdminTypeDigitalSignatureSignaturePropertiesImage Image
    {
        get
        {
            return this._image;
        }
        set
        {
            if ((this._image == value))
            {
                return;
            }
            if (((this._image == null) 
                        || (_image.Equals(value) != true)))
            {
                this._image = value;
                this.OnPropertyChanged("Image", value);
            }
        }
    }
    
    /// <summary>
    /// Test whether Signer should be serialized
    /// </summary>
    public virtual bool ShouldSerializeSigner()
    {
        return (_signer != null);
    }
    
    /// <summary>
    /// Test whether SignerPublicKeyCertificate should be serialized
    /// </summary>
    public virtual bool ShouldSerializeSignerPublicKeyCertificate()
    {
        return (_signerPublicKeyCertificate != null);
    }
    
    /// <summary>
    /// Test whether SignatureReason should be serialized
    /// </summary>
    public virtual bool ShouldSerializeSignatureReason()
    {
        return (_signatureReason != null);
    }
    
    /// <summary>
    /// Test whether SignatureDateTimeStamp should be serialized
    /// </summary>
    public virtual bool ShouldSerializeSignatureDateTimeStamp()
    {
        return (_signatureDateTimeStamp != null);
    }
    
    /// <summary>
    /// Test whether X_CertificateAuthority should be serialized
    /// </summary>
    public virtual bool ShouldSerializeX_CertificateAuthority()
    {
        return (_x_CertificateAuthority != null);
    }
    
    /// <summary>
    /// Test whether X_SignerPublicKey should be serialized
    /// </summary>
    public virtual bool ShouldSerializeX_SignerPublicKey()
    {
        return (_x_SignerPublicKey != null);
    }
    
    /// <summary>
    /// Test whether X_SignatureAlgorithm should be serialized
    /// </summary>
    public virtual bool ShouldSerializeX_SignatureAlgorithm()
    {
        return (_x_SignatureAlgorithm != null);
    }
    
    /// <summary>
    /// Test whether Image should be serialized
    /// </summary>
    public virtual bool ShouldSerializeImage()
    {
        return (_image != null);
    }
}
}
#pragma warning restore
