using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDC.Schema;
using System;
using System.IO;
using System.Linq;
//using SDC.Schema;

namespace SDC.Schema.Tests
{
    [TestClass]
    public class ChangeTypeTests
    {

        [TestMethod]
        public void LItoDI()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void DItoLI()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void DItoQ()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
        [TestMethod]
        public void DItoS()
        {
            Setup.TimerStart("==>[] Started");

            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==[] Complete");
        }
    }
}