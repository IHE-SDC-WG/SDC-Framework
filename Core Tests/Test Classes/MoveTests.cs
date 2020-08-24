using SDC.Schema.Tests;
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

//using SDC.Schema;

namespace SDC.Schema.Tests
{
    [TestClass]
    public class MoveTests
    {
        private TestContext testContextInstance;

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
        public void MoveListItemInList()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                //FD.TopNode.ReorderNodes();
                var li = Setup.FD.Nodes.Where(n => 
                    n.Value is ListItemType liTest &&
                    liTest.ID == "38493.100004300").FirstOrDefault().Value
                    as ListItemType;
                Assert.IsTrue(li is ListItemType);

                List<BaseType> lst1;
                List<BaseType> lst2;
                List<BaseType> lst3;

                lst1 = SdcUtil.ReflectChildList(Setup.FD.GetListItemByID("51689.100004300"));
                lst2 = SdcUtil.ReflectChildList(Setup.FD.GetListItemByID("38493.100004300"));
                lst3 = SdcUtil.ReflectChildList(Setup.FD.GetItemByName("lst_44135_3"));

                lst3 = SdcUtil.ReflectSubtree(Setup.FD.GetSectionByID("43969.100004300"));
                //foreach (var n in lst3) Debug.Print(n.name);
                var tc = new TreeComparer();
                lst3.Sort(tc);
                foreach (var n in lst3) Debug.Print(n.name + ": " + n.ElementName + ", " + n.ObjectID);

                var lst4 = Setup.FD.Nodes.Values.ToList();
                var res = lst4[0].GetType().GetProperties()
                    .Where(p => p.GetCustomAttributes<XmlElementAttribute>().Count() > 0 && p.GetValue(lst4[0]) != null)
                    .Select(p => p.GetValue(lst4[0])).ToList();

                var propList = new List<BaseType>();

                while (false)
                {
                    foreach (object o in res)
                    {
                        var bt = o as BaseType;
                        if (bt != null)
                        {
                            Debug.Print(bt.name);
                            propList.Add(bt);
                        }
                        else
                        if (o is IList il) foreach (var n in il.OfType<BaseType>())
                            {
                                Debug.Print(n.name);
                                propList.Add(n);
                            }
                    }
                }

                propList.Sort(new TreeComparer());
                int i = 0;
                foreach (var n in propList) Debug.Print((i++).ToString() + ": " + n.name);

                SdcUtil.ReflectSubtree(lst4[0], true, true);
                foreach (var n in lst4) Debug.Print(n.name + ": " + n.ElementName + ", " + n.ObjectID.ToString() + ", order:" + n.order.ToString() );


                lst4.Sort(tc);
                var list = (ListType)li.ParentNode;
            


                li.Move(list, 6);                
                Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == 6);

                li.Move(list, 99);
                Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == list.Items.Count()-1);

                li.Move(list, 0);
                Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == 0);

                li.Move(list);
                Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == list.Items.Count() - 1);
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");

        }
        [TestMethod]
        public void MoveListItemToOtherList()
        {
            Setup.TimerStart($"==>{Setup.CallerName()} Started");
                var li = Setup.FD.Nodes.Where(n =>
                    n.Value is ListItemType liTest &&
                    liTest.ID == "38493.100004300").FirstOrDefault().Value
                    as ListItemType;
                Assert.IsTrue(li is ListItemType);
                var list = (ListType)li.ParentNode;

                var list2 = Setup.FD.Nodes.Where(n =>
                    n.Value is ListType liTest &&
                    liTest.name == "lst_58267_3").FirstOrDefault().Value
                    as ListType;
                Assert.IsTrue(list2 is ListType);

                //Move to different List (list2)
                li.Move(list2, 2);
                Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == 2);
                Assert.AreEqual(list2, li.ParentNode);
            Setup.TimerPrintSeconds("  seconds: ", $"\r\n<=={Setup.CallerName()} Complete");
        }
        [TestMethod]
        public void MoveListDInList()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }

        [TestMethod]
        public void MoveListDItoOtherList()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void MoveListDIQuestionChild()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void MoveQuestionInChildItems()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void MoveQuestionToNewChildItems()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        public void MoveSectionToNewChildItems()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
    }
}