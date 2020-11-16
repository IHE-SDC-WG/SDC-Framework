using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using SDC.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace SDC.Schema.Tests
{
    public static class Setup
    {
        public static FormDesignType FD;
        private static float TimerStartTime;
        private static string _XmlPath =>
            Path.Combine(".", "Test files", "Breast.Invasive.Staging.359_.CTP9_sdcFDF.xml");
        private static string _Xml;

        public static string DataElementXml { get; set; }
        public static string DemogFormDesignXml { get; set; }
        public static string RetrieveFormXml { get; set; }
        public static string MessageXml { get; set; }
        public static string RetrieveFormComplexXml { get; set; }
        public static string IdrXml { get; set; }
        public static string FormDesignWithHtmlXml { get; set; }

        static Setup()
        {
            Reset();
            //Reset();
        }
        public static string FormDesignXml { get; set; }

        public static void TimerStart(string message = "")
        {
            Stopwatch.StartNew();
            TimerStartTime  = (float)Stopwatch.GetTimestamp();
            if (!message.IsEmpty()) Debug.Print(message);
        }
        public static string TimerGetSeconds()
        {
            return(
                ((Stopwatch.GetTimestamp() - TimerStartTime) / Stopwatch.Frequency)
                .ToString());
        }
        public static void TimerPrintSeconds(string messageBefore = "", string messageAfter = "")
        {
            Debug.Print (messageBefore +
                ((Stopwatch.GetTimestamp() - TimerStartTime) / Stopwatch.Frequency)
                .ToString()
                + messageAfter);
        }


        [TestMethod]
        public static string GetXmlPath()
        {
            return _XmlPath;
        }
        public static string GetXml()
        {
            return System.IO.File.ReadAllText(_XmlPath);
        }
        public static void TraceMessage(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("message: " + message);
            Trace.WriteLine("member name: " + memberName);
            Trace.WriteLine("source file path: " + sourceFilePath);
            Trace.WriteLine("source line number: " + sourceLineNumber);

        }
        public static string CallerName([CallerMemberName] string memberName = "")
        => memberName;  

        public static void Reset()
        {
            Setup.TimerStart("==>Setup starting----------");
            BaseType.ResetSdcImport();
            _Xml = System.IO.File.ReadAllText(_XmlPath);
            FD = FormDesignType.DeserializeFromXml(_Xml);
            Setup.TimerPrintSeconds("  seconds: ", "\r\n<==Setup finished----------\r\n");
        }



    }
}
