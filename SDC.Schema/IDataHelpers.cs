using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Sources;
using System.Xml;
using Newtonsoft.Json;

namespace SDC.Schema
{
    public interface IDataHelpers
    {
        #region Data Helpers
        static DataTypes_DEType AddDataTypesDE(
          ResponseFieldType rfParent,
          ItemChoiceType dataTypeEnum = ItemChoiceType.@string,
          dtQuantEnum quantifierEnum = dtQuantEnum.EQ,
          object value = null)
        {
            rfParent.Response = new DataTypes_DEType(rfParent);

            switch (dataTypeEnum)
            {
                case ItemChoiceType.HTML:
                    {
                        var dt = new HTML_DEtype(rfParent.Response);
                        dt.Any = value as List<XmlElement> ?? new List<XmlElement>();
                        dt.AnyAttr = new List<XmlAttribute>();
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.XML: //TODO: Need to be able to add custom attributes to first wrapper element - see anyType; in fact, do we even need XML as a separate type?
                    {
                        var dt = new XML_DEtype(rfParent.Response);
                        dt.Any = new List<XmlElement>();
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.anyType:
                    {
                        var dt = new anyType_DEtype(rfParent.Response);
                        dt.Any = new List<XmlElement>();
                        dt.AnyAttr = new List<XmlAttribute>();
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.anyURI:
                    {
                        var dt = new anyURI_DEtype(rfParent.Response);
                        dt.val = (string)value;
                    }
                    break;
                case ItemChoiceType.base64Binary:
                    {
                        var dt = new base64Binary_DEtype(rfParent.Response);
                        dt.val = (byte[])value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.boolean:
                    {
                        var dt = new boolean_DEtype(rfParent.Response);
                        dt.val = (bool)value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@byte:
                    {
                        var dt = new byte_DEtype(rfParent.Response);
                        dt.val = (sbyte)value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.date:
                    {
                        var dt = new date_DEtype(rfParent.Response);
                        dt.val = (DateTime)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@dateTime: //TODO: added the "@" symbol to dateTime and dateTimeStamp here, in AddFillDataTypesDE, and in the 2 ItemChoiceType files.  Also fixed bug in the DateTypes_DEtype with the wrong ItemTypeNames from xsd2code - on dateTime and dateTimeStamp.
                    {
                        var dt = new dateTime_DEtype(rfParent.Response);
                        if (value != null)  //TODO: value testing may be needed for the other dateTime and duration types in this method, and also in AddFillDataTypesDE
                        {
                            var test = value as DateTime?;
                            if (test != null)
                                dt.val = (DateTime)test;
                            else
                                try
                                {
                                    var sTest = DateTime.Parse(value.ToString());
                                    dt.val = sTest;
                                }
                                catch (Exception) //ex)
                                { }
                        }
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@dateTimeStamp:
                    {
                        var dt = new dateTimeStamp_DEtype(rfParent.Response);
                        dt.val = (DateTime)value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@decimal:
                    {
                        var dt = new decimal_DEtype(rfParent.Response);
                        dt.val = (decimal)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@double:
                    {
                        var dt = new double_DEtype(rfParent.Response);
                        dt.val = (double)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.duration:
                    {
                        var dt = new duration_DEtype(rfParent.Response);
                        dt.val = (string)value;   //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                    }
                    break;
                case ItemChoiceType.@float:
                    {
                        var dt = new float_DEtype(rfParent.Response);
                        dt.val = (float)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.gDay:
                    {
                        var dt = new gDay_DEtype(rfParent.Response);
                        dt.val = (string)value; ;  //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.gMonth:
                    {
                        var dt = new gMonth_DEtype(rfParent.Response);
                        dt.val = (string)value; ;  //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.gMonthDay:
                    {
                        var dt = new gMonthDay_DEtype(rfParent.Response);
                        dt.val = (string)value; ;  //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.gYear:
                    {
                        var dt = new gYear_DEtype(rfParent.Response);
                        dt.val = (string)value; ;  //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.gYearMonth:
                    {
                        var dt = new gYearMonth_DEtype(rfParent.Response);
                        dt.val = (string)value; //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.hexBinary:
                    {
                        var dt = new hexBinary_DEtype(rfParent.Response);
                        dt.val = (byte[])value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@int:
                    {
                        var dt = new int_DEtype(rfParent.Response);
                        dt.val = (int)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.integer:
                    {
                        var dt = new integer_DEtype(rfParent.Response);
                        dt.val = (string)value; //(string)value; C# string data type in xsdCode++ - uses string because there is no integer (truncated decimal) format in .NET
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@long:
                    {
                        var dt = new long_DEtype(rfParent.Response);
                        dt.val = (long)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.negativeInteger:
                    {
                        var dt = new negativeInteger_DEtype(rfParent.Response);
                        dt.val = (string)value;  //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.nonNegativeInteger:
                    {
                        var dt = new nonNegativeInteger_DEtype(rfParent.Response);
                        dt.val = (string)value;  //TODO:  bug in xsdCode++ - wrong data type?
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.nonPositiveInteger:
                    {
                        var dt = new nonPositiveInteger_DEtype(rfParent.Response);
                        dt.val = (string)value; //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.positiveInteger:
                    {
                        var dt = new positiveInteger_DEtype(rfParent.Response);
                        dt.val = (string)value;//TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@short:
                    {
                        var dt = new short_DEtype(rfParent.Response);
                        dt.val = (short)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.@string:
                    {
                        var dt = new @string_DEtype(rfParent.Response);
                        dt.val = (string)value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.time:
                    {
                        var dt = new time_DEtype(rfParent.Response);
                        dt.val = (DateTime)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.unsignedByte:
                    {
                        var dt = new unsignedByte_DEtype(rfParent.Response);
                        dt.val = (byte)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.unsignedInt:
                    {
                        var dt = new unsignedInt_DEtype(rfParent.Response);
                        dt.val = (uint)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.unsignedLong:
                    {
                        var dt = new unsignedLong_DEtype(rfParent.Response);
                        dt.val = (ulong)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.unsignedShort:
                    {
                        var dt = new unsignedShort_DEtype(rfParent.Response);
                        dt.val = (ushort)value;
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                case ItemChoiceType.yearMonthDuration:
                    {
                        var dt = new yearMonthDuration_DEtype(rfParent.Response);
                        dt.val = (string)value; ;  //TODO: C# string data type in xsdCode++
                        dt.quantEnum = quantifierEnum;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
                default:
                    {
                        var dt = new @string_DEtype(rfParent.Response);
                        dt.val = (string)value;
                        rfParent.Response.DataTypeDE_Item = dt;
                    }
                    break;
            }

            rfParent.Response.ItemElementName = (ItemChoiceType2)dataTypeEnum;
            return rfParent.Response;

        }

        DataTypes_SType AddDataTypesS();
        DataTypesDateTime_DEType DataTypesDateTimeDE();
        DataTypesDateTime_SType DataTypesDateTimeS();
        DataTypesNumeric_DEType AddDataTypesNumericDE();
        DataTypesNumeric_SType AddDataTypesNumericS();



        dtQuantEnum AssignQuantifier(string quantifier)
        {
            var dtQE = new dtQuantEnum();

            switch (quantifier)
            {
                case "EQ":
                case "=":
                case"==":
                    dtQE = dtQuantEnum.EQ;
                    break;
                case "GT":
                case ">":
                    dtQE = dtQuantEnum.GT;
                    break;
                case "GTE":
                case ">=":
                    dtQE = dtQuantEnum.GTE;
                    break;
                case "LT":
                case "<":
                    dtQE = dtQuantEnum.LT;
                    break;
                case "LTE":
                case "<=":
                    dtQE = dtQuantEnum.LTE;
                    break;
                case "APPROX":
                case "~":
                case "@":
                    dtQE = dtQuantEnum.APPROX;
                    break;
                case "":
                    dtQE = dtQuantEnum.EQ;
                    break;
                case null:
                    dtQE = dtQuantEnum.EQ;
                    break;
                default:
                    dtQE = dtQuantEnum.EQ;
                    break;
            }
            return dtQE;
        }


        static DataTypes_DEType AddHTML_DE(ResponseFieldType rfParent, List<XmlElement> valEl = null, List<XmlAttribute> valAtt = null)
        {
            rfParent.Response = new DataTypes_DEType(rfParent);

            var dt = new HTML_DEtype(rfParent.Response);
            dt.Any = valEl?? new List<XmlElement>();
            dt.AnyAttr = valAtt?? new List<XmlAttribute>();
            rfParent.Response.DataTypeDE_Item = dt;

            rfParent.Response.ItemElementName = ItemChoiceType2.HTML;
            return rfParent.Response;
        }
        static DataTypes_DEType AddXML_DE(ResponseFieldType rfParent, List<XmlElement> valEl = null)
        {
            rfParent.Response = new DataTypes_DEType(rfParent);

            var dt = new XML_DEtype(rfParent.Response);
            dt.Any = valEl ?? new List<XmlElement>();
            rfParent.Response.DataTypeDE_Item = dt;

            rfParent.Response.ItemElementName = ItemChoiceType2.XML;
            return rfParent.Response;
        }
        static DataTypes_DEType AddAny_DE(ResponseFieldType rfParent, List<XmlElement> valEl = null, List<XmlAttribute> valAtt = null, string nameSpace = null, string schema = null)
        {
            rfParent.Response = new DataTypes_DEType(rfParent);

            var dt = new anyType_DEtype(rfParent.Response);
            dt.@namespace = nameSpace ?? null;
            dt.schema = schema ?? null;
            dt.Any = valEl ?? new List<XmlElement>();
            dt.AnyAttr = valAtt ?? new List<XmlAttribute>();
            rfParent.Response.DataTypeDE_Item = dt;

            rfParent.Response.ItemElementName = ItemChoiceType2.HTML;
            return rfParent.Response;
        }
        static DataTypes_DEType AddBase64_DE(ResponseFieldType rfParent, byte[] value = null, string mediaType = null)
        {
            rfParent.Response = new DataTypes_DEType(rfParent);

            var dt = new base64Binary_DEtype(rfParent.Response);
            dt.val = value;
            dt.mediaType = mediaType;
            rfParent.Response.DataTypeDE_Item = dt;

            rfParent.Response.ItemElementName = ItemChoiceType2.base64Binary;
            return rfParent.Response;
        }


        #endregion

    }






    public interface IHtmlHelpers
    {
        HTML_Stype AddHTML(RichTextType rt)
        {
            HTML_Stype html = null;

            html = new HTML_Stype(rt);
            rt.RichText = html;
            html.Any = new List<XmlElement>();

            return html;

            //TODO: Check XHTML builder here:
            //https://gist.github.com/rarous/3150395,
            //http://www.authorcode.com/code-snippet-converting-xmlelement-to-xelement-and-xelement-to-xmlelement-in-vb-net/
            //https://msdn.microsoft.com/en-us/library/system.xml.linq.loadoptions%28v=vs.110%29.aspx

        }

    }


}
