using NUnit.Framework;
using Nunit_Framework.PageActions;
using NUnit.Framework.Interfaces;
using RelevantCodes.ExtentReports;
using System;
using System.IO;
using OpenQA.Selenium;
using System.Threading;
using System.Diagnostics;
using Fenton.Selenium.SuperDriver;
using System.Drawing;
using System.Collections.Generic;
using OpenQA.Selenium.Support.Extensions;
using System.Windows.Forms;

namespace Nunit_Framework.Testcases
{
    public class BaseTestMethods
    {
        protected ExtentReports extentReports;
        protected ExtentReports extentReportSummary;
        protected static ExtentTest test;
        protected static ExtentTest testSummary;
        static string driverDirectory = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName;
        protected string resultDirectory = Path.Combine(driverDirectory, "TestResults");
        protected string reportName, screenshotName, reportSummaryName;
        static string resultSummaryDirectory = Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName, "TestResults", "ReportsSummary", DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"));

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reportSummaryName = "TestSummary_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html";
            if (!Directory.Exists(resultSummaryDirectory))
            {
                Directory.CreateDirectory(resultSummaryDirectory);
            }
            extentReportSummary = new ExtentReports(resultSummaryDirectory + "\\" + reportSummaryName, false);
            testSummary = extentReportSummary.StartTest(TestContext.CurrentContext.Test.Name, "Report summary");

            Pages.LoginPage.LaunchSite();
        }

        [TearDown]
        public void TearDown()
        {
            CreateReportResult();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //Pos-condition
            BrowserManager.DeleteAllCookies();
            BrowserManager.CloseBrowser();

            extentReportSummary.EndTest(testSummary);
            extentReportSummary.Flush();
            //extentReportSummary.Close();

            // For Jenkins report
            var source = Path.Combine(resultSummaryDirectory, reportSummaryName);
            var targetPath = Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName, "TestResults", "JenkinsReport");
            var destination = Path.Combine(targetPath, "TestSummaryJenkins.html");

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            File.Copy(source, destination, true);
        }

        protected void StartTest()
        {
            resultDirectory = Path.Combine(resultDirectory, DateTime.Now.ToString("yyyy-MM-dd"), TestContext.CurrentContext.Test.MethodName);
            if (!Directory.Exists(resultDirectory))
            {
                Directory.CreateDirectory(resultDirectory);
            }
            resultDirectory += "\\";
            reportName = TestContext.CurrentContext.Test.MethodName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html";
            screenshotName = TestContext.CurrentContext.Test.MethodName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

            extentReports = new ExtentReports(resultDirectory + reportName, true);
            test = extentReports.StartTest(TestContext.CurrentContext.Test.MethodName, "");
            
            //change report config
            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            extentReports.LoadConfig(path + @"\TestData\extent-config.xml");
            extentReportSummary.LoadConfig(path + @"\TestData\extent-config.xml");
        }

        protected void CreateReportResult()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
            LogStatus logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = LogStatus.Fail;
                    break;
                case TestStatus.Skipped:
                    logstatus = LogStatus.Skip;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = LogStatus.Unknown;
                    break;
                default:
                    logstatus = LogStatus.Pass;
                    break;
            }

            test.Log(logstatus, "Test ended, result is: " + logstatus + stacktrace);
            if (logstatus == LogStatus.Fail)
            {
                TakeScreenshot(resultDirectory, screenshotName);
                test.Log(LogStatus.Info, "Snapshot below: " + test.AddScreenCapture(resultDirectory + "\\" + screenshotName));
            }
            TestContext.Write("Detail Results has been saved in: " + resultDirectory);

            testSummary.AppendChild(test);
            extentReports.Flush();
            extentReports.EndTest(test);

            resultDirectory = Path.Combine(driverDirectory, "TestResults");
        }

        protected void TakeScreenshot(string resultDirectory, string screenshotName)
        {
            try
            {
                if (!Directory.Exists(resultDirectory))
                    Directory.CreateDirectory(resultDirectory);

                //string screenshotFilePath = Path.Combine(resultDirectory, screenshotName);
                //SuperUtility.SaveScreenshot(BrowserManager.Browser, screenshotFilePath, System.Drawing.Imaging.ImageFormat.Png);

                ITakesScreenshot takesScreenshot = BrowserManager.Browser as ITakesScreenshot;

                if (takesScreenshot != null)
                {
                    var screenshot = takesScreenshot.GetScreenshot();
                    string screenshotFilePath = Path.Combine(resultDirectory, screenshotName);
                    screenshot.SaveAsFile(screenshotFilePath, System.Drawing.Imaging.ImageFormat.Png);
                    Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);

            }
        }

        protected void stepLogging(String logMessage)
        {
            test.Log(LogStatus.Info, "Test step: " + logMessage);
        }


        /// <summary>
        /// Get entire page
        /// </summary>
        /// <returns></returns>
        public Image GetEntireScreenshot()
        {
            // Get the total size of the page
            var totalWidth = (int)(long)((IJavaScriptExecutor)BrowserManager.Browser).ExecuteScript("return document.body.offsetWidth"); //documentElement.scrollWidth");
            var totalHeight = (int)(long)((IJavaScriptExecutor)BrowserManager.Browser).ExecuteScript("return  document.body.parentNode.scrollHeight");
            // Get the size of the viewport
            var viewportWidth = (int)(long)((IJavaScriptExecutor)BrowserManager.Browser).ExecuteScript("return document.body.clientWidth"); //documentElement.scrollWidth");
            var viewportHeight = (int)(long)((IJavaScriptExecutor)BrowserManager.Browser).ExecuteScript("return window.innerHeight"); //documentElement.scrollWidth");

            // We only care about taking multiple images together if it doesn't already fit
            if (totalWidth <= viewportWidth && totalHeight <= viewportHeight)
            {
                var screenshot = BrowserManager.Browser.TakeScreenshot();
                return ScreenshotToImage(screenshot);
            }
            // Split the screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the totalHeight is reached
            for (var y = 0; y < totalHeight; y += viewportHeight)
            {
                var newHeight = viewportHeight;
                // Fix if the height of the element is too big
                if (y + viewportHeight > totalHeight)
                {
                    newHeight = totalHeight - y;
                }
                // Loop until the totalWidth is reached
                for (var x = 0; x < totalWidth; x += viewportWidth)
                {
                    var newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (x + viewportWidth > totalWidth)
                    {
                        newWidth = totalWidth - x;
                    }
                    // Create and add the Rectangle
                    var currRect = new Rectangle(x, y, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }
            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            var previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    var xDiff = rectangle.Right - previous.Right;
                    var yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    ((IJavaScriptExecutor)BrowserManager.Browser).ExecuteScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                }
                // Take Screenshot
                var screenshot = BrowserManager.Browser.TakeScreenshot();
                // Build an Image out of the Screenshot
                var screenshotImage = ScreenshotToImage(screenshot);
                // Calculate the source Rectangle
                var sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);
                // Copy the Image
                using (var graphics = Graphics.FromImage(stitchedImage))
                {
                    graphics.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }
                // Set the Previous Rectangle
                previous = rectangle;
            }
            return stitchedImage;
        }
        private static Image ScreenshotToImage(Screenshot screenshot)
        {
            Image screenshotImage;
            using (var memStream = new MemoryStream(screenshot.AsByteArray))
            {
                screenshotImage = Image.FromStream(memStream);
            }
            return screenshotImage;
        }

        /// <summary>
        /// Capture full screen
        /// </summary>
        /// <param name="resultDirectory"></param>
        /// <param name="screenshotName"></param>
        private void CaptureFullScreen(string resultDirectory, string screenshotName)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                bitmap.Save(resultDirectory + "\\" + screenshotName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
