using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using global::SDC;
using System.Collections.Generic;
using System.Data;
using SDC.Schema;
using System.Linq;
using System.Security.Claims;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
//using SDC.Schema;

namespace SDC_Tests
{
    [TestClass]
    public class SDC_XML
    {
        public SDC_XML()
        {
        }

        [TestMethod]
        public void DeserializeDemogFormDesignFromPath()
        {
            BaseType.ClearTopNode();
            //string path = @".\Test files\Demog CCO Lung Surgery.xml";

            string path = Path.Combine(".", "Test files", "Demog CCO Lung Surgery.xml");
            //if (!File.Exists(path)) path = @"/Test files/Demog CCO Lung Surgery.xml";
            //string sdcFile = File.ReadAllText(path, System.Text.Encoding.UTF8);
            DemogFormDesignType FD = DemogFormDesignType.DeserializeFromXmlPath(path);
            var myXML = FD.GetXml(); 
            Debug.Print(myXML);
            //Debug.Print(FD.GetJson());
            var doc = new XmlDocument();
            doc.LoadXml(myXML);
            var json = JsonConvert.SerializeXmlNode(doc);
            Debug.Print(json);
            doc = JsonConvert.DeserializeXmlNode(json);
            Debug.Print(doc.OuterXml);

        }
        [TestMethod]
        public void DeserializePkgFromPath()
        {
            BaseType.ClearTopNode();
            //string path = @".\Test files\..Sample SDCPackage.xml";
            string path = Path.Combine(".", "Test files", "..Sample SDCPackage.xml");
            //string sdcFile = File.ReadAllText(path, System.Text.Encoding.UTF8);
            var Pkg = RetrieveFormPackageType.DeserializeFromXmlPath(path);
            FormDesignType FD = (FormDesignType)Pkg.Nodes.Values.Where(n => n.GetType() == typeof(FormDesignType)).FirstOrDefault();


            var Q = (QuestionItemType)Pkg.Nodes.Values.Where(
                t => t.GetType() == typeof(QuestionItemType)).Where(
                q => ((QuestionItemType)q).ID == "37387.100004300").FirstOrDefault();
            var DI = Q.AddChildDisplayedItem("DDDDD");//should add to end of the <List>
            DI.name = "my added DI";

            DisplayedType DI1 = (DisplayedType)Pkg.Nodes.Values.Where(n => n.name == "my added DI").First();
            DisplayedType DI2 = (DisplayedType)Q.ChildItemsNode.Items[0];
            QuestionItemType Q1 = (QuestionItemType)DI2.ParentNode.ParentNode;
            string diName = Q.Item1.Items[0].name;
            string diName2 = Q.ChildItemsNode.ChildItemsList[0].ID;
            int i = Q.ChildItemsNode.ChildItemsList.Count();
            bool b1 = Q.ChildItemsNode.ShouldSerializeItems();

            var myXML = Pkg.GetXml();


            Debug.Print(myXML);

        }
        [TestMethod]
        public void DeserializeDEFromPath()
        {
            BaseType.ClearTopNode();
            //string path = @".\Test files\DE sample.xml";
            string path = Path.Combine(".", "Test files", "DE sample.xml");
            //string sdcFile = File.ReadAllText(path, System.Text.Encoding.UTF8);
            DataElementType DE = DataElementType.DeserializeFromXmlPath(path);
            var myXML = DE.GetXml();
            Debug.Print(myXML);
            Debug.Print(DE.GetJson());


        }
        public void DeserializeDEFromXml()
        {
            BaseType.ClearTopNode();
            //string path = @".\Test files\DE sample.xml";
            string path = Path.Combine(".", "Test files", "DE sample.xml");
            string sdcFile = File.ReadAllText(path, System.Text.Encoding.UTF8);
            DataElementType DE = DataElementType.DeserializeFromXml(sdcFile);
            var myXML = DE.GetXml();
            Debug.Print(myXML);

        }

        [TestMethod]
        public void DeserializeFormDesignFromPath()
        {
            BaseType.ClearTopNode();
            //string path = @".\Test files\CCO Lung Surgery.xml";
            //string path = @".\Test files\Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml";
            string path = Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
            //string path = @".\Test files\Adrenal.Bx.Res.129_3.004.001.REL_sdcFDF_test.xml";
            string sdcFile = File.ReadAllText(path, System.Text.Encoding.UTF8);
            
            var FD = FormDesignType.DeserializeFromXmlPath(path);
            //SDC.Schema.FormDesignType FD = SDC.Schema.FormDesignType.DeserializeSdcFromFile(sdcFile);
            string myXML;
            //myXML =  SdcSerializer<FormDesignType>.Serialize(FD);

            //Test adding and reading FD object model
                var Q = (QuestionItemType)FD.Nodes.Values.Where(
                    t => t.GetType() == typeof(QuestionItemType)).Where(
                    q => ((QuestionItemType)q).ID == "58218.100004300").FirstOrDefault();

                var DI = Q.AddChildDisplayedItem("DDDDD");//should add to end of the <List>
                DI.name = DI.ID; 
                DI.title = DI.ID;

                var P = Q.AddProperty(); P.name = "PPPPP"; P.propName = "PPPPP";
                var S = Q.AddChildSection("SSSSS", 0); S.name = "SSSSS";
            //Q.Move(new SectionItemType(), -1); Q.AddComment(); Q.Remove();
            //var li = new ListItemType(Q.ListField_Item.List,"abc" ); var b = li.SelectIf.returnVal; var rv = li.OnSelect[0].returnVal;
            
                DisplayedType DI1 = (DisplayedType)FD.Nodes.Values.Where(n => n.name == DI.ID)?.First();
                DisplayedType DI2 = (DisplayedType)Q.ChildItemsNode.Items[0];
                QuestionItemType Q1 = (QuestionItemType)DI2.ParentNode.ParentNode;
            myXML = SDCHelpers.XmlReorder(FD.GetXml());
            myXML = SDCHelpers.XmlFormat(myXML);
            var S1 = Q.AddOnEnter().Actions.AddActInject().Item = new SectionItemType(   //Need to add AddActionsNode to numerous classes via IHasActionsNode
                parentNode: Q, 
                id:"myid", 
                elementName:"", 
                elementPrefix:"s");

            Debug.Print(myXML);
            FD.Clear();
            //var myMP = FD.GetMsgPack();
            //FD.SaveMsgPackToFile("C:\\MPfile");  //also support REST transactions, like sending packages to SDC endpoints; consider FHIR support
            var myJson = FD.GetJson();
            Debug.Print(myJson);
        }

        [TestMethod]
        public void SdcToJson()
        {
            
        }
        [TestMethod]
        public void JsonToXML()
        {
            
        }


    }
    [TestClass]
    public class FormDesignBuilder
    {
        
        public FormDesignBuilder()
        {

        }

        [TestMethod]
        public void AddRemoveHeader()
        {
            
        }
        [TestMethod]
        public void AddRemoveFooter()
        {
            
        }
        [TestMethod]
        public void AddQuestions()
        {
            
        }
        [TestMethod]
        public void AddListItemToQuestionList()
        {
            
        }
        [TestMethod]
        public void AddListItemOnListItem()
        {
            
        }
        [TestMethod]
        public void AdListItemOnDisplayedItem()
        {
            
        }
        [TestMethod]
        public void AddDisplayedItemToQuestionList()
        {

        }
        [TestMethod]
        public void AddDisplayedItemOnListItem()
        {

        }
        [TestMethod]
        public void AdDisplayedItemOnDisplayedItem()
        {

        }
        [TestMethod]
        public void AddDisplayedItemAsChild()
        {
            
        }
        [TestMethod]
        public void AddQuestionAsChild()
        { //to LI, DI in List, DI, S, Q

        }
        [TestMethod]
        public void AddSectionAsChild()
        {

        }
        
        [TestMethod]
        public void AddProperties()
        {
            
        }
        [TestMethod]
        public void AddComments()
        {
            
        }
    }

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
    [TestClass]
    public class ValidationTests
    {
        private TestContext testContextInstance;
        public ValidationTests()
        {
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        [TestMethod]
        public void ValidateJsonFormDesign()
        {
            
        }
        [TestMethod]
        public void ValidateXmlFormDesign()
        {
            
        }

        [TestMethod]
        public void ValidateXmlDemogFormDesign()
        {

        }

        public void ValidateXmlPackage()
        {

        }
        public void ValidateJsonPackage()
        {

        }

        [TestMethod]
        public void ValidateXmlDataElement()
        {

        }

        [TestMethod]
        public void ValidateXmlMap()
        {

        }

    }
    [TestClass]
    public class NavigationTests
    {
        FormDesignType fd;
        private TestContext testContextInstance;
        
        public FormDesignType FD
        {
            get => fd;
            set => fd = value;
        }
        
        public NavigationTests()
        {
            BaseType.ClearTopNode();
            string path = Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
            fd = FormDesignType.DeserializeFromXmlPath(path);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        [TestMethod]
        public void MoveNext_ListSib()
        {
            
        }

        [TestMethod]
        public void MovePrev_ListSib()
        {

        }
        [TestMethod]
        public void MoveNext_SameClass()
        {

        }
        [TestMethod]
        public void MoveNext_NextClass()
        {

        }
        public void MovePrev_SameClass()
        {

        }
        [TestMethod]
        public void MovePrev_PrevClass()
        {

        }
        [TestMethod]
        public void IsList()
        {
            
        }
        [TestMethod]
        public void ReflectSdcElement()
        {

            var t = (ITopNode)FD;
            var qList = t.Nodes.Where(n => n.Value is QuestionItemType).Select(n=>n.Value).ToList();
            var sList = t.Nodes.Where(n => n.Value is SectionItemType).Select(n => n.Value).ToList();
            var aList = t.Nodes.Where(n => n.Value is ListItemType).Select(n => n.Value).ToList();
            var cList = t.Nodes.Where(n => n.Value is ChildItemsType).Select(n => n.Value).ToList();
            var pList = t.Nodes.Where(n => n.Value is PropertyType).Select(n => n.Value).ToList();

            var tpl = IHelpers.ReflectSdcElement(qList[0]);
            Debug.Print(((QuestionItemType)qList[4]).title, tpl.itemElementName, tpl.itemPropertyOrder);

        }

        [TestMethod]
        public void GetNamedItem()
        {
            
        }

        [TestMethod]
        public void GetParentIEnumerable()
        {
            int index = -1;
            List<BaseType> par;
            var t = (ITopNode)FD;
            var qList = t.Nodes.Where(n => n.Value is QuestionItemType).Select(n => n.Value).ToList();
            var sList = t.Nodes.Where(n => n.Value is SectionItemType).Select(n => n.Value).ToList();
            var aList = t.Nodes.Where(n => n.Value is ListItemType).Select(n => n.Value).ToList();
            var cList = t.Nodes.Where(n => n.Value is ChildItemsType).Select(n => n.Value).ToList();
            var pList = t.Nodes.Where(n => n.Value is PropertyType).Select(n => n.Value).ToList();

            par = IHelpers.GetParentIEnumerable(qList[3], out index)?.ToList();
            Debug.Print(SDCHelpers.NS(qList[3]?.name) + ", Par: " + Interaction.IIf((index > -1), par?[index]?.name??"", "null"));
            Console.WriteLine(SDCHelpers.NS(qList[3]?.name) + ", Par: " + Interaction.IIf((index > -1), par[index]?.name??"", "null"));
            par = IHelpers.GetParentIEnumerable(sList[3], out index)?.ToList();
            Debug.Print(SDCHelpers.NS(sList[3]?.name) + ", Par: " + Interaction.IIf((index > -1), par?[index]?.name??"", "null"));
            par = IHelpers.GetParentIEnumerable(aList[3], out index)?.ToList();
            Debug.Print(SDCHelpers.NS(aList[3]?.name) + ", Par: " + Interaction.IIf((index > -1), par?[index]?.name??"", "null"));
            par = IHelpers.GetParentIEnumerable(cList[3], out index)?.ToList();
            Debug.Print(SDCHelpers.NS(cList[3]?.name) + ", Par: " + Interaction.IIf((index > -1), par?[index]?.name??"", "null"));
            par = IHelpers.GetParentIEnumerable(pList[15], out index)?.ToList();
            Debug.Print(SDCHelpers.NS(pList[15]?.name) + ", Par: " + Interaction.IIf((index > -1), par?[index]?.name ?? "", "null"));




        }
        [TestMethod]
        public void GetItemPropertyName()
        {
            var t = (ITopNode)FD;
            int propertyIndex;
            IEnumerable<BaseType> ieProperty;
            var qList = t.Nodes.Where(n => n.Value is QuestionItemType).Select(n => n.Value).ToList();
            var sList = t.Nodes.Where(n => n.Value is SectionItemType).Select(n => n.Value).ToList();
            var aList = t.Nodes.Where(n => n.Value is ListItemType).Select(n => n.Value).ToList();
            var cList = t.Nodes.Where(n => n.Value is ChildItemsType).Select(n => n.Value).ToList();
            var pList = t.Nodes.Where(n => n.Value is PropertyType).Select(n => n.Value).ToList();

            Debug.Print(IHelpers.GetPropertyName(qList[3], out ieProperty, out propertyIndex));
            Debug.Print(IHelpers.GetPropertyName(sList[3], out ieProperty, out propertyIndex));
            Debug.Print(IHelpers.GetPropertyName(aList[3], out ieProperty, out propertyIndex));
            Debug.Print(IHelpers.GetPropertyName(cList[3], out ieProperty, out propertyIndex));
            Debug.Print(IHelpers.GetPropertyName(pList[15], out ieProperty, out propertyIndex));

        }

        [TestMethod]
        public void GetPropertyInfoMetadata()
        { //given an SDC item, find the property that references it in the item.ParentNode class
            var t = (ITopNode)FD;

            var qList = t.Nodes.Where(n => n.Value is QuestionItemType).Select(n => n.Value).ToList();
            var sList = t.Nodes.Where(n => n.Value is SectionItemType).Select(n => n.Value).ToList();
            var aList = t.Nodes.Where(n => n.Value is ListItemType).Select(n => n.Value).ToList();
            var cList = t.Nodes.Where(n => n.Value is ChildItemsType).Select(n => n.Value).ToList();
            var pList = t.Nodes.Where(n => n.Value is PropertyType).Select(n => n.Value).ToList();


            Debug.Print(IHelpers.GetPropertyInfo(qList[1]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(sList[1]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(aList[1]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(cList[1]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(pList[1]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(qList[10]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(sList[10]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(aList[10]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(cList[10]).ToString());
            Debug.Print(IHelpers.GetPropertyInfo(pList[10]).ToString());


        }
        [TestMethod]
        public void GetPropertyInfoMetadata_Complete()
        {
            Stopwatch.StartNew();
            var a = Stopwatch.GetTimestamp();

            foreach (var n in FD.Nodes)
            {
                IHelpers.GetPropertyInfo(n.Value); 
                //Debug.Print(IHelpers.GetPropertyInfo(n.Value).ToString());
            }
            Debug.Print((a-Stopwatch.GetTimestamp()).ToString());
        }
        [TestMethod]
        public void TreeComparer()
        {
            BaseType.ClearTopNode();
            string path = Path.Combine(".", "Test files", "Adrenal.Bx.Res.129_3.004.001.REL_sdcFDF_test.xml");
            var FDbad = FormDesignType.DeserializeFromXmlPath(path); //used to compare nodes in another tree
            var adr = FDbad.Nodes.Values.ToArray(); //this creates shallow copies with do not retain ParentNode refs, etc.


            var tc = new TreeComparer();
            var n = FD.Nodes.Values.ToArray();//this creates shallow copies with do not retain ParentNode refs, etc.

            Stopwatch.StartNew();
            var a = Stopwatch.GetTimestamp();

            Debug.Print(tc.Compare(n[0], n[1]).ToString());
            Debug.Print(tc.Compare(n[0], n[2]).ToString());
            Debug.Print(tc.Compare(n[0], n[3]).ToString());
            Debug.Print(tc.Compare(n[0], n[10]).ToString());
            Debug.Print(tc.Compare(n[0], n[20]).ToString());
            Debug.Print(tc.Compare(n[0], n[30]).ToString());
            Debug.Print(tc.Compare(n[0], n[50]).ToString());
            Debug.Print("\r\n");
            Debug.Print(tc.Compare(n[1], n[1]).ToString());
            Debug.Print(tc.Compare(n[2], n[2]).ToString());
            Debug.Print(tc.Compare(n[3], n[3]).ToString());
            Debug.Print(tc.Compare(n[10], n[10]).ToString());
            Debug.Print(tc.Compare(n[20], n[20]).ToString());
            Debug.Print(tc.Compare(n[30], n[30]).ToString());
            Debug.Print(tc.Compare(n[50], n[50]).ToString());
            Debug.Print("\r\n expected results: -1, 1, -1, 1, -1, 1, -1 ");
            Debug.Print(tc.Compare(n[1], n[2]).ToString());// -1
            Debug.Print(tc.Compare(n[2], n[1]).ToString());// 1
            Debug.Print(tc.Compare(n[33], n[34]).ToString());// -1
            Debug.Print(tc.Compare(n[20], n[10]).ToString());// 1
            Debug.Print(tc.Compare(n[199], n[201]).ToString());// -1
            Debug.Print(tc.Compare(n[201], n[200]).ToString());// 1
            Debug.Print(tc.Compare(n[29], n[32]).ToString());// -1
            Debug.Print("\r\n expected results: -1, 1, -1, 1, -1, 1, -1 ");
            Debug.Print(tc.Compare(n[299], n[301]).ToString());// -1
            Debug.Print(tc.Compare(n[401], n[300]).ToString());// 1
            Debug.Print(tc.Compare(n[39], n[42]).ToString());// -1
            Debug.Print(tc.Compare(n[21], n[11]).ToString());// 1
            Debug.Print(tc.Compare(n[11], n[12]).ToString());// -1
            Debug.Print(tc.Compare(n[341], n[133]).ToString());// 1
            Debug.Print(tc.Compare(n[101], n[120]).ToString());// -1


            Debug.Print("\r\n");
            Debug.Print(tc.Compare(n[2], n[0]).ToString());
            Debug.Print(tc.Compare(n[4], n[0]).ToString());
            Debug.Print(tc.Compare(n[6], n[0]).ToString());
            Debug.Print(tc.Compare(n[20], n[0]).ToString());
            Debug.Print(tc.Compare(n[40], n[0]).ToString());
            Debug.Print(tc.Compare(n[60], n[0]).ToString());
            Debug.Print(tc.Compare(n[100], n[0]).ToString());
            Debug.Print("\r\n");

            try { Debug.Print(tc.Compare(n[100], adr[0]).ToString()); } catch { Debug.Print("error caught");}
            try{Debug.Print(tc.Compare(adr[0], n[100]).ToString()); } catch { Debug.Print("error caught"); }
            try {Debug.Print(tc.Compare(n[10], adr[12]).ToString()); } catch { Debug.Print("error caught"); }
            try {Debug.Print(tc.Compare(adr[100], adr[100]).ToString()); } catch { Debug.Print("error caught"); }
            
            
            
            
            Debug.Print( ( (float)(Stopwatch.GetTimestamp() - a) / ((float)Stopwatch.Frequency)) .ToString());


        }
        [TestMethod]
        public void ParentNodesFromXml()
        {
            BaseType.ClearTopNode();
            string path = Path.Combine(".", "Test files", "Adrenal.Bx.Res.129_3.004.001.REL_sdcFDF_test.xml");
            var FDbad = FormDesignType.DeserializeFromXmlPath(path); //used to compare nodes in another tree
            var adr = FDbad.Nodes.Values.ToArray<BaseType>();

            foreach (var n in adr)
            {
                Debug.Print(n.name + ", par: " + n.ParentNode?.name);
            }

        }

        [TestMethod]
        public void GetNextInList()
        {
            
        }
        [TestMethod]
        public void GetPreviousInList()
        {
            
        }
        [TestMethod]
        public void GetNextSib()
        {
            
        }
        [TestMethod]
        public void GetPrevSib()
        {
            
        }

        [TestMethod]
        public void GetEventParent()
        {
            
        }

        [TestMethod]
        public void IsItemChangeAllowed()
        {
            
        }


    }

    [TestClass]
    public class MiscTests
    {   
        private TestContext testContextInstance;
        public MiscTests()
        {
        }
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }

        }

        [TestMethod]
        public void Fibonacci()
        {
            var serializer = new XmlSerializer(typeof(BaseType));

            (int curr, int prev) Fib(int i)
            {
                if (i == 0) return (1, 0);
                var (curr, prev) = Fib(i - 1);
                return (curr + prev, curr);
            }

            var a = Fib(9);
            var b = a.ToTuple();
        }

        [TestMethod]
        public void Test()
        {
            
        }

    }


}