using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.IO;
using System.Linq;
//using SDC.Schema;

namespace SDC_Tests
{
    [TestClass]
    public class FormDesignBuilder
    {
        public TestContext testContextInstance;

        FormDesignType fd;

        public FormDesignType FD
        {
            get => fd;
            set => fd = value;
        }

        public FormDesignBuilder()
        {
            BaseType.ResetSdcImport();
            string path = Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
            fd = FormDesignType.DeserializeFromXmlPath(path);
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
        public void Misc()
        {

        }
    }
}