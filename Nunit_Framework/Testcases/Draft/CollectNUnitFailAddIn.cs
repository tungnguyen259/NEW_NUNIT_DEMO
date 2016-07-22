//using System;
//using NUnit.Core;
//using NUnit.Core.Extensibility;
//using System.Diagnostics;

//namespace Nunit_Framework.Testcases.Draft
//{
//    [NUnitAddin(Name = "CollectNUnitFailAddIn", Description = "do something", Type = ExtensionType.Core)]
//    public class CollectNUnitFailAddIn : IAddin, EventListener
//    {
//        public bool Install(IExtensionHost host)
//        {
//            //IExtensionPoint suiteBuilders = host.GetExtensionPoint("SuiteBuilders");
//            //IExtensionPoint testBuilders = host.GetExtensionPoint("TestCaseBuilders");
//            IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");
//            if (listeners == null)
//                return false;

//            listeners.Install(this);
//            return true;
//        }
//        public void RunFinished(Exception exception)
//        {
//            //do something here.
//        }

//        public void RunFinished(TestResult result)
//        {
//            //do something here.
//        }

//        public void RunStarted(string name, int testCount)
//        {
//            Debug.Write("Test Started " + name + " " + testCount);
//            //do something here.
//        }

//        public void SuiteFinished(TestResult result)
//        {
//            //do something here.
//        }

//        public void SuiteStarted(TestName testName)
//        {
//            //do something here.
//        }
//        public void TestStarted(TestName testName)
//        {
//            //do something here.
//        }
//        public void TestFinished(TestResult result)
//        {
//            //do something here.
//        }

//        public void TestOutput(TestOutput testOutput)
//        {
//            //do something here.
//        }

//        public void UnhandledException(Exception exception)
//        {
//            //do something here.
//        }
//    }
//}
