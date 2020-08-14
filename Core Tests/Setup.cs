using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Core_Tests
{
    public static class Setup
    {
        static FormDesignType FD;
        static string formDesignWithHtmlXml;
        static string idrXml;
        static string retrieveFormComplexXml;
        static string messageXml;
        static string retrieveFormXml;
        static string demogFormDesignXml;
        static string dataElementXml;
        static string formDesignXml;
        static Setup()
        {
            BaseType.ResetSdcImport();
            string path = Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
            FD = FormDesignType.DeserializeFromXmlPath(path);
        }
        public static string FormDesignXml
        {
            get => formDesignXml;
            set => formDesignXml = value;
        }

        public static string DataElementXml
        {
            get => dataElementXml;
            set => dataElementXml = value;
        }

        public static string DemogFormDesignXml
        {
            get => demogFormDesignXml;
            set => demogFormDesignXml = value;
        }


        public static string RetrieveFormXml
        {
            get => retrieveFormXml;
            set => retrieveFormXml = value;
        }

        public static string MessageXml
        {
            get => messageXml;
            set => messageXml = value;
        }

        public static string RetrieveFormComplexXml
        {
            get => retrieveFormComplexXml;
            set => retrieveFormComplexXml = value;
        }

        public static string IdrXml
        {
            get => idrXml;
            set => idrXml = value;
        }

        public static string FormDesignWithHtmlXml
        {
            get => formDesignWithHtmlXml;
            set => formDesignWithHtmlXml = value;
        }
    }
}
