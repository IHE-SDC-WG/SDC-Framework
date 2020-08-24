using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using SDC.Schema;

namespace SDC.Schema.Tests
{
    [TestClass]
    public class NavigationTests
    {
        private TestContext testContextInstance;

        public NavigationTests()
        {            
            //previous test runs change locations of some SDC nodes 
            //This can cause some Assert methods, whch depend on the order of ObjectIDs, to fail.
            //So we Reset the source SDC xml before starting this test suite
            Setup.Reset();  
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
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            int i = 0;
            BaseType n = Setup.FD;
            string content;
            while (n != null)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);
                n = SdcUtil.ReflectNextElement(n);
                i++;
            }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");

        }
        [TestMethod]
        public void MoveNextReflect()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
            int i = 0;
            BaseType n = Setup.FD;
            string content;
            while (n != null)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);
                n = SdcUtil.ReflectNextElement2(n);
                i++;
            }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void MoveNextNodes()
        {          
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
            int i = 0;
            BaseType n = Setup.FD;
            string content;
            var cn = n.TopNode.ChildNodes;

            //loop through ChildNodes
            BaseType firstChild;
            BaseType nextSib;

            MoveNext(n);
            List<BaseType> childList;
            List<BaseType> sibList;
            void MoveNext(BaseType n)
            {
                firstChild = null;
                nextSib = null;
                btPrint(n);
                n.order = i;  //almost instananeous
                Assert.IsTrue(n.ObjectID == i);//very fast
                i++;
                

                if (cn.TryGetValue(n.ObjectGUID, out childList))
                {
                    firstChild = childList[0];
                    if (firstChild != null)
                        MoveNext(firstChild);
                }


                var par = n.ParentNode;
                if (par != null)
                {
                    if (cn.TryGetValue(par.ObjectGUID, out sibList))
                    {
                        var index = sibList.IndexOf(n);
                        if (index < sibList.Count - 1)
                        {
                            nextSib = sibList[index + 1];
                            if (nextSib != null)
                                MoveNext(nextSib);
                        }
                    }
                }
            }



            void btPrint(BaseType n)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);
            }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void NodesToSortedList()
        {


            Setup.TimerStart($"==>{Setup.CallerName()} Started");
            int i = 0;
            BaseType n = Setup.FD;
            string content;
            var cn = n.TopNode.ChildNodes;

            //loop through ChildNodes
            BaseType firstChild;
            BaseType nextSib;

            List<BaseType> childList;
            List<BaseType> sibList;
            var sortedList = new List<BaseType>();
            BaseType[] sortedArray = new BaseType[Setup.FD.Nodes.Count] ;

            MoveNext(n);

            void MoveNext(BaseType n)
            {
                firstChild = null;
                nextSib = null;
                //btPrint(n);
                n.order = i;  //almost instananeous
                sortedList.Add(n);
                sortedArray[i] = n;
                Assert.IsTrue(n.ObjectID == i);//very fast
                i++;


                if (cn.TryGetValue(n.ObjectGUID, out childList))
                {
                    firstChild = childList[0];
                    if (firstChild != null)
                        MoveNext(firstChild);
                }


                var par = n.ParentNode;
                if (par != null)
                {
                    if (cn.TryGetValue(par.ObjectGUID, out sibList))
                    {
                        var index = sibList.IndexOf(n);
                        if (index < sibList.Count - 1)
                        {
                            nextSib = sibList[index + 1];
                            if (nextSib != null)
                                MoveNext(nextSib);
                        }
                    }
                }
            }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void GetLastDescendantNode()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            var n = SdcUtil.GetLastDescendant(Setup.FD.Body);
            Assert.IsTrue(n.ElementName =="LocalFunctionName" && n.type=="submit");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }


        [TestMethod]
        public void NodesToSortedListbyTreeComparer()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            var sa = Setup.FD.Nodes.Values.ToList();
            sa.Sort(new TreeComparer());

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void MoveNextSlow()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                int i = 0;
                BaseType n = Setup.FD;
                string content;
                while (n != null)
                {
                    if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                    else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                    else content = "";

                    Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);

                    n = SdcUtil.GetNextElement(n);
                    i++;
                }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void MovePrevSlow()
        {

            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            BaseType n = SdcUtil.GetLastDescendant(Setup.FD);
            int i = Setup.FD.Nodes.Count() - 1;
            string content;

            while (n != null)
            {
                if (n is DisplayedType) content = ": title: " + (n as DisplayedType).title;
                else if (n is PropertyType) content = ": " + (n as PropertyType).propName + ": " + (n as PropertyType).val;
                else content = "";

                Debug.Print(n.ObjectID.ToString().PadLeft(4) + ": " + i.ToString().PadLeft(4) + ": " + (n.name ?? "").PadRight(20) + ": " + (n.ElementName ?? "").PadRight(25) + content);

                n = SdcUtil.GetPrevElement(n);
                i--;
            }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void MovePrevFast()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
            
            Stopwatch.StartNew();
            
            var a = (float)Stopwatch.GetTimestamp();


            var total = a;
            BaseType[] sortedNodes = new BaseType[Setup.FD.Nodes.Count()];
            int i = 0;
            BaseType n = Setup.FD;
            string content;
            n = Setup.FD;
            while (n != null)
            {
                sortedNodes[i] = n;
                n = SdcUtil.ReflectNextElement(n);
                i++;
            }

            Debug.Print("Seconds to Create Node Array: " + ((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency).ToString());
            Debug.Print("Seconds per Node: " + (((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency) / Setup.FD.Nodes.Count()).ToString());
            a = (float)Stopwatch.GetTimestamp();

            for (i = Setup.FD.Nodes.Count() - 1; i >= 0; i--)
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
            Debug.Print("Seconds per Node" + (((Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency) / Setup.FD.Nodes.Count()).ToString());
            Debug.Print("Total Time: " + ((Stopwatch.GetTimestamp() - total) / Stopwatch.Frequency).ToString());
            

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void IsList()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void GetNamedItem()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void GetPropertyInfoMetadata()
        { //given an SDC item, find the property that references it in the item.ParentNode class


            Setup.TimerStart($"==>{Setup.CallerName()} Started");
            var t = (ITopNode)Setup.FD;

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
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void GetPropertyInfoMetadata_Complete()
        {

            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                Stopwatch.StartNew();
                var a = Stopwatch.GetTimestamp();

                foreach (var n in Setup.FD.Nodes)
                {
                    SdcUtil.GetPropertyInfo(n.Value);
                    //Debug.Print(ISdcUtil.GetPropertyInfo(n.Value).ToString());
                }
                Debug.Print(((((float)Stopwatch.GetTimestamp() - a) / Stopwatch.Frequency) / Setup.FD.Nodes.Count()).ToString());
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void TreeComparer()
        {

            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                BaseType.ResetSdcImport();
                string path = Path.Combine(".", "Test files", "Adrenal.Bx.Res.129_3.004.001.REL_sdcFDF_test.xml");
                var FDbad = FormDesignType.DeserializeFromXmlPath(path); //used to compare nodes in another tree
                var adr = FDbad.Nodes.Values.ToArray(); //this creates shallow copies with do not retain ParentNode refs, etc.


                var tc = new TreeComparer();
                var n = Setup.FD.Nodes.Values.ToArray();//this creates shallow copies with do not retain ParentNode refs, etc.

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
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void ParentNodesFromXml()
        {

            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                BaseType.ResetSdcImport();
                string path = Path.Combine(".", "Test files", "Adrenal.Bx.Res.129_3.004.001.REL_sdcFDF_test.xml");
                var FDbad = FormDesignType.DeserializeFromXmlPath(path); //used to compare nodes in another tree
                var adr = FDbad.Nodes.Values.ToArray<BaseType>();

                foreach (var n in adr)
                {
                    Debug.Print(n.name + ", par: " + n.ParentNode?.name);
                }
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void GetEventParent()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }

        [TestMethod]
        public void IsItemChangeAllowed()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void ReflectPropertyInfoList()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                var lst = SdcUtil.ReflectPropertyInfoList(Setup.FD);
                foreach (var n in lst)
                    Debug.Print($"{n.XmlOrder}:\t Name: {n.PropertyInfo.Name}, \t Type: {n.PropertyInfo.PropertyType.Name}");
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void ReflectChildList()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                var lst = SdcUtil.ReflectChildList(Setup.FD);
                foreach (var n in lst)
                    Debug.Print($"{n.order}: \t Name: {n.name}");
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void ReflectSubtree()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                var lst = Setup.FD.TopNode.GetItemByName("S_57219")
                    .GetSubtreeList();
                foreach (var n in lst)
                    Debug.Print($"{n.order}: \t Name: {n.name}");
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void GetXmlAttributeAll()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                var lst = Setup.FD.TopNode.GetItemByName("S_57219")
                    .GetXmlAttributesAll();
                foreach (var n in lst) Debug.Print($"{n.Name}");
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void GetXmlAttributesFilled()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                var lst = Setup.FD.TopNode.GetItemByName("S_57219")
                    .GetXmlAttributesFilled();
                foreach (var n in lst) Debug.Print($"{n.Name}");
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");

        }
        [TestMethod]
        public void Misc()
        {
            //SectionItemType S;
            //need AddActionNode
            //Prevent adding nodes without  going through dictionaries
            //Ensure that all add/remove functions use dictionaries   
            //
            //S.AddOnExit().Actions.AddActSendReport();
            Setup.TimerStart($"==>{Setup.CallerName()} Started");

            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }


    }
}