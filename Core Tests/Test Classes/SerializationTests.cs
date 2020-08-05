using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
//using SDC.Schema;

namespace SDC_Tests
{
    [TestClass]
    public class SerializationTests
    {

        string formDesignWithHtmlXml;
        string idrXml;
        string retrieveFormComplexXml;
        string messageXml;
        string retrieveFormXml;
        string demogFormDesignXml;
        string dataElementXml;
        string formDesignXml;

        public SerializationTests()
        {
        }

        public string FormDesignXml
        {
            get => formDesignXml;
            set => formDesignXml = value;
        }

        public string DataElementXml
        {
            get => dataElementXml;
            set => dataElementXml = value;
        }

        public string DemogFormDesignXml
        {
            get => demogFormDesignXml;
            set => demogFormDesignXml = value;
        }


        public string RetrieveFormXml
        {
            get => retrieveFormXml;
            set => retrieveFormXml = value;
        }

        public string MessageXml
        {
            get => messageXml;
            set => messageXml = value;
        }

        public string RetrieveFormComplexXml
        {
            get => retrieveFormComplexXml;
            set => retrieveFormComplexXml = value;
        }

        public string IdrXml
        {
            get => idrXml;
            set => idrXml = value;
        }

        public string FormDesignWithHtmlXml
        {
            get => formDesignWithHtmlXml;
            set => formDesignWithHtmlXml = value;
        }


        [TestMethod]
        public void RoundtripFormDesign()
        {

        }
        [TestMethod]
        public void RoundtripDemogFormDesign()
        {

        }
        [TestMethod]
        public void RoundtripDataElement()
        {

        }
        [TestMethod]
        public void RoundtripPackage()
        {

        }
        [TestMethod]
        public void RoundtripComplexPackage()
        {

        }
        [TestMethod]
        public void RoundtripIdr()
        {

        }
        [TestMethod]
        public void RoundtripMap()
        {

        }


    }
}