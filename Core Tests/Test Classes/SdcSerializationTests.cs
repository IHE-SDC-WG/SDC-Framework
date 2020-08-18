using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SDC.Schema;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

//using SDC.Schema;

namespace SDC.Schema.Tests
{
    [TestClass]
    public class SdcSerializationTests
    {
        [TestMethod]
        public void DeserializeDemogFormDesignFromPath()
        {
            Setup.TimerStart("==>[] Started");

            
            BaseType.ResetSdcImport();
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
            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void DeserializePkgFromPath()
        {
            BaseType.ResetSdcImport();
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
            BaseType.ResetSdcImport();
            //string path = @".\Test files\DE sample.xml";
            string path = Path.Combine(".", "Test files", "DE sample.xml");
            //string sdcFile = File.ReadAllText(path, System.Text.Encoding.UTF8);
            DataElementType DE = DataElementType.DeserializeFromXmlPath(path);
            var myXML = DE.GetXml();
            Debug.Print(myXML);
            Debug.Print(DE.GetJson());


        }
        [TestMethod]
        public void DeserializeDEFromXml()
        {
            BaseType.ResetSdcImport();
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
            BaseType.ResetSdcImport();
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
            myXML = SdcUtil.XmlReorder(FD.GetXml());
            myXML = SdcUtil.XmlFormat(myXML);



            ListItemType li;


            //var S1 = Q.AddOnEnter().Actions.AddActInject().Item = new SectionItemType(   //Need to add AddActionsNode to numerous classes via IHasActionsNode
            //    parentNode: Q,
            //    id: "myid",
            //    elementName: "",
            //    elementPrefix: "s");

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
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void JsonToXML()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }


    }
}