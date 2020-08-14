using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using SDC.Schema;

namespace SDC_Tests
{
    [TestClass]
    public class NavigationTests
    {
        FormDesignType fd;
        private TestContext testContextInstance;

        private FormDesignType FD
        {
            get => fd;
            set => fd = value;
        }

        public NavigationTests()
        {
            BaseType.ResetSdcImport();
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
        public void MoveNext()
        {
            Stopwatch.StartNew();
            var a = (float)Stopwatch.GetTimestamp();

            int i = 0;
            BaseType n = FD;
            string content;
            while (n != null)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);
                n = SdcUtil.NextElement(n);
                i++;
            }
            Debug.Print(
                ((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency)
                .ToString());
        }
        [TestMethod]
        public void MoveNextSlow()
        {
            Stopwatch.StartNew();
            var a = (float)Stopwatch.GetTimestamp();

            int i = 0;
            BaseType n = FD;
            string content;
            while (n != null)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);

                n = SdcUtil.NextElement2(n);
                i++;
            }
            Debug.Print(
                ((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency)
                .ToString());
        }
        [TestMethod]
        public void MovePrevSlow()
        {
            BaseType n = SdcUtil.GetLastDescendant(FD);
            int i = FD.Nodes.Count() - 1;
            string content;

            Stopwatch.StartNew();
            var a = (float)Stopwatch.GetTimestamp();

            while (n != null)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);

                n = SdcUtil.PrevElement(n);
                i--;
            }
            Debug.Print(
                ((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency)
                .ToString());
        }

        [TestMethod]
        public void MovePrevFast()
        {
            Stopwatch.StartNew();
            var a = (float)Stopwatch.GetTimestamp();
            var total = a;
            BaseType[] sortedNodes = new BaseType[FD.Nodes.Count()];
            int i = 0;
            BaseType n = FD;
            string content;
            n = FD;
            while (n != null)
            {
                sortedNodes[i] = n;
                n = SdcUtil.NextElement(n);
                i++;
            }

            Debug.Print("Seconds to Create Node Array: " + ((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency).ToString());
            Debug.Print("Seconds per Node: " + (((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency) / FD.Nodes.Count()).ToString());
            a = (float)Stopwatch.GetTimestamp();

            for (i = FD.Nodes.Count() - 1; i >= 0; i--)
            {
                n = sortedNodes[i];
                if (n is DisplayedType)
                    content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType)
                    content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);
            }

            Debug.Print("Output Time" + ((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency).ToString());
            Debug.Print("Seconds per Node" + (((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency) / FD.Nodes.Count()).ToString());
            Debug.Print("Total Time: " + ((Stopwatch.GetTimestamp() - total) / Stopwatch.Frequency).ToString());
        }
        [TestMethod]
        public void IsList()
        {

        }

        [TestMethod]
        public void GetNamedItem()
        {

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


            Debug.Print(SdcUtil.GetPropertyInfo(qList[1]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(sList[1]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(aList[1]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(cList[1]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(pList[1]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(qList[10]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(sList[10]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(aList[10]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(cList[10]).ToString());
            Debug.Print(SdcUtil.GetPropertyInfo(pList[10]).ToString());


        }
        [TestMethod]
        public void GetPropertyInfoMetadata_Complete()
        {
            Stopwatch.StartNew();
            var a = Stopwatch.GetTimestamp();

            foreach (var n in FD.Nodes)
            {
                SdcUtil.GetPropertyInfo(n.Value);
                //Debug.Print(ISdcUtil.GetPropertyInfo(n.Value).ToString());
            }
            Debug.Print(((((float)Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency) / FD.Nodes.Count()).ToString());
        }
        [TestMethod]
        public void TreeComparer()
        {
            BaseType.ResetSdcImport();
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
            Debug.Print(tc.Compare(n[0], n[3]).ToString());
            Debug.Print(tc.Compare(n[0], n[8]).ToString());
            Debug.Print(tc.Compare(n[1], n[8]).ToString());
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
            Debug.Print(tc.Compare(n[2], n[1]).ToString());
            Debug.Print(tc.Compare(n[4], n[0]).ToString());
            Debug.Print(tc.Compare(n[6], n[4]).ToString());
            Debug.Print(tc.Compare(n[20], n[2]).ToString());
            Debug.Print(tc.Compare(n[40], n[0]).ToString());
            Debug.Print(tc.Compare(n[60], n[0]).ToString());
            Debug.Print(tc.Compare(n[100], n[0]).ToString());
            Debug.Print("\r\n");

            try { Debug.Print(tc.Compare(n[100], adr[0]).ToString()); } catch { Debug.Print("error caught"); }
            try { Debug.Print(tc.Compare(adr[0], n[100]).ToString()); } catch { Debug.Print("error caught"); }
            try { Debug.Print(tc.Compare(n[10], adr[12]).ToString()); } catch { Debug.Print("error caught"); }
            try { Debug.Print(tc.Compare(adr[100], adr[100]).ToString()); } catch { Debug.Print("error caught"); }




            Debug.Print(((float)(Stopwatch.GetTimestamp() - a) / ((float)Stopwatch.Frequency)).ToString());


            //Seconds per comparison: @ 0.0006 sec/comparison
            a = Stopwatch.GetTimestamp();
            for (int i = 0; i < 100; i++)
            {
                tc.Compare(n[299], n[301]);
                tc.Compare(n[2101], n[120]);
            }
            Debug.Print(((float)(Stopwatch.GetTimestamp() - a) / ((float)Stopwatch.Frequency) / 200).ToString());

        }
        [TestMethod]
        public void ParentNodesFromXml()
        {
            BaseType.ResetSdcImport();
            string path = Path.Combine(".", "Test files", "Adrenal.Bx.Res.129_3.004.001.REL_sdcFDF_test.xml");
            var FDbad = FormDesignType.DeserializeFromXmlPath(path); //used to compare nodes in another tree
            var adr = FDbad.Nodes.Values.ToArray<BaseType>();

            foreach (var n in adr)
            {
                Debug.Print(n.name + ", par: " + n.ParentNode?.name);
            }

        }

        [TestMethod]
        public void GetEventParent()
        {

        }

        [TestMethod]
        public void IsItemChangeAllowed()
        {
        }
        [TestMethod]
        public void ReflectPropertyInfoList()
        {
            var lst = SdcUtil.ReflectPropertyInfoList(FD);
            foreach (var n in lst)
                Debug.Print($"{n.XmlOrder}:\t Name: {n.PropertyInfo.Name}, \t Type: {n.PropertyInfo.PropertyType.Name}");

        }
        [TestMethod]
        public void ReflectChildList()
        {
            var lst = SdcUtil.ReflectChildList(FD);
            foreach (var n in lst)
                Debug.Print($"{n.order}: \t Name: {n.name}");

        }
        [TestMethod]
        public void ReflectSubtree()
        {
            var lst = FD.TopNode.GetItemByName("S_57219")
                .GetSubtree();
            foreach (var n in lst)
                Debug.Print($"{n.order}: \t Name: {n.name}");
        }
        [TestMethod]
        public void ReflectAttributesAll()
        {
            var lst = FD.TopNode.GetItemByName("S_57219")
                .GetXmlAttributesFilled();
            foreach (var n in lst) Debug.Print($"{n.Name}");
        }
        [TestMethod]
        public void ReflectAttributesFilled()
        {
            var lst = FD.TopNode.GetItemByName("S_57219")
                .GetXmlAttributesFilled();

            foreach (var n in lst) Debug.Print($"{n.Name}");
        }
        [TestMethod]
        public void Misc()
        {
            SectionItemType S;
            //need AddActionNode
            //Prevent adding nodes without  going through dictionaries
            //Ensure that all add/remove functions use dictionaries   
            //
            //S.AddOnExit().Actions.AddActSendReport();



        }


    }
}