using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
//using SDC.Schema;

namespace SDC.Schema.Tests
{
    [TestClass]
    public class MiscTests
    {
        FormDesignType fd;

        public FormDesignType FD
        {
            get => fd;
            set => fd = value;
        }

        public MiscTests()
        {
            BaseType.ResetSdcImport();
            string path = Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
            fd = FormDesignType.DeserializeFromXmlPath(path);
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
        public void Test1()
        {

        }

    }
}