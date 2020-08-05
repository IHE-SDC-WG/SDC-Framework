using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.IO;
using System.Linq;
//using SDC.Schema;

namespace SDC_Tests
{
    [TestClass]
    public class ChangeTypeTests
    {
        FormDesignType fd;
        private TestContext testContextInstance;

        public FormDesignType FD
        {
            get => fd;
            set => fd = value;
        }

        public ChangeTypeTests()
        {
            BaseType.ResetSdcImport();
            string path = Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
            fd = FormDesignType.DeserializeFromXmlPath(path);
        }
        [TestMethod]
        public void LItoDI()
        {

        }
        [TestMethod]
        public void DItoLI()
        {

        }
        [TestMethod]
        public void DItoQ()
        {

        }
        [TestMethod]
        public void DItoS()
        {

        }
    }
}