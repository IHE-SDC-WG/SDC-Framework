using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using SDC.Schema;

namespace SDC_Tests
{
    [TestClass]
    public class MoveTests
    {
        private TestContext testContextInstance;

        FormDesignType fd;

        public FormDesignType FD
        {
            get => fd;
            set => fd = value;
        }

        public MoveTests()
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
        public void MoveListItemInList()
        {
            var li = FD.Nodes.Where(n => 
                n.Value is ListItemType liTest &&
                liTest.ID == "38493.100004300").FirstOrDefault().Value
                as ListItemType;
            Assert.IsTrue(li is ListItemType);
            var list = (ListType)li.ParentNode;


            li.Move(list, out List<string> errList, 6);
            Assert.IsTrue(errList is null || errList.Count() == 0);
            Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == 6);            

            li.MoveinList(out errList, 5);
            Assert.IsTrue(errList is null || errList.Count() == 0);
            Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == 5);




        }
        [TestMethod]
        public void MoveListItemToOtherList()
        {
            var li = FD.Nodes.Where(n =>
                n.Value is ListItemType liTest &&
                liTest.ID == "38493.100004300").FirstOrDefault().Value
                as ListItemType;
            Assert.IsTrue(li is ListItemType);
            var list = (ListType)li.ParentNode;

            var list2 = FD.Nodes.Where(n =>
                n.Value is ListType liTest &&
                liTest.name == "lst_58267_3").FirstOrDefault().Value
                as ListType;
            Assert.IsTrue(list2 is ListType);

            //Move to different List (list2)
            li.MoveToList(list2, out List<string> errList, 2);
            Assert.IsTrue(errList is null || errList.Count() == 0);
            Assert.IsTrue(SdcUtil.GetPropertyInfo(li).ItemIndex == 2);
            Assert.AreEqual(list2, li.ParentNode);
        }
        [TestMethod]
        public void MoveListDInList()
        {

        }

        [TestMethod]
        public void MoveListDItoOtherList()
        {

        }
        [TestMethod]
        public void MoveListDIQuestionChild()
        {

        }
        [TestMethod]
        public void MoveQuestionInChildItems()
        {

        }
        [TestMethod]
        public void MoveQuestionToNewChildItems()
        {

        }
        public void MoveSectionToNewChildItems()
        {

        }
    }
}