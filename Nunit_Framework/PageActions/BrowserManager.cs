using Fenton.Selenium.SuperDriver;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Nunit_Framework.PageActions
{
    public class BrowserManager
    {
        private static string FirefoxVersion = ConfigurationManager.AppSettings["FirefoxVersion"];
        private static string firefoxPlatform = ConfigurationManager.AppSettings["FirefoxPlatform"];
        private static string ChromeVersion = ConfigurationManager.AppSettings["ChromeVersion"];
        private static string chromePlatform = ConfigurationManager.AppSettings["ChromePlatform"];
        private static string IEVersion = ConfigurationManager.AppSettings["IEVersion"];
        private static string iePlatform = ConfigurationManager.AppSettings["IEPlatform"];
        private static string EdgeVersion = ConfigurationManager.AppSettings["EdgeVersion"];
        private static string edgePlatform = ConfigurationManager.AppSettings["EdgePlatform"];
        private static string appbrowser = ConfigurationManager.AppSettings["AppBrowser"];
        private static string grid = ConfigurationManager.AppSettings["grid"];
        private static string sauceLabs = ConfigurationManager.AppSettings["sauceLabs"];
        private static readonly Dictionary<String, IWebDriver> context = new Dictionary<String, IWebDriver>();
        public static IWebDriver Browser
        {
            get
            {
                if (context.Count == 0)
                {
                    var uri = new Uri(ConfigurationManager.AppSettings["SeleniumHubUrl"]);

                    if (grid.Equals("yes"))
                    {
                        switch (appbrowser)
                        {
                            case "Parallel":
                                {
                                    context.Add("Driver", new SuperWebDriver(GetDriverSuiteGrid()));
                                    break;
                                }
                            case "ie":
                                {
                                    DesiredCapabilities capability = DesiredCapabilities.InternetExplorer();
                                    capability.SetCapability(CapabilityType.Version, IEVersion);
                                    var IEoptions = new InternetExplorerOptions();
                                    IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                                    context.Add("Driver", new RemoteWebDriver(uri, capability));
                                    break;
                                }
                            case "Chrome":
                                {
                                    DesiredCapabilities capability = DesiredCapabilities.Chrome();
                                    capability.SetCapability(CapabilityType.Version, ChromeVersion);
                                    context.Add("Driver", new RemoteWebDriver(uri, capability));
                                    break;
                                }
                            case "Edge":
                                {
                                    EdgeOptions options = new EdgeOptions();
                                    options.PageLoadStrategy = EdgePageLoadStrategy.Eager;
                                    DesiredCapabilities capability = DesiredCapabilities.Edge();
                                    capability.SetCapability(CapabilityType.Version, EdgeVersion);
                                    context.Add("Driver", new RemoteWebDriver(uri, capability));
                                    break;
                                }
                            default:
                                {
                                    DesiredCapabilities capability = DesiredCapabilities.Firefox();
                                    capability.SetCapability(CapabilityType.Version, FirefoxVersion);
                                    context.Add("Driver", new RemoteWebDriver(uri, capability));
                                    break;
                                }
                        }
                        return context["Driver"];
                    }

                    if (sauceLabs.Equals("yes"))
                    {
                        switch (appbrowser)
                        {
                            case "Parallel":
                                {
                                   context.Add("Driver", new SuperWebDriver(GetDriverSuiteRunOnSauceLabs()));
                                    break;
                                }
                            case "ie":
                                {
                                    context.Add("Driver", driverRunOnSauceLabs("Internet Explorer", IEVersion, iePlatform));
                                    break;
                                }
                            case "Chrome":
                                {
                                    context.Add("Driver", driverRunOnSauceLabs("Chrome", ChromeVersion, chromePlatform));
                                    break;
                                }
                            case "Edge":
                                {
                                    context.Add("Driver", driverRunOnSauceLabs("MicrosoftEdge", EdgeVersion, edgePlatform));
                                    break;
                                }
                            default:
                                {
                                    context.Add("Driver", driverRunOnSauceLabs("firefox", FirefoxVersion, firefoxPlatform));
                                    break;
                                }
                        }
                        return context["Driver"];
                    }

                    switch (appbrowser)
                    {
                        case "Parallel":
                            {
                                context.Add("Driver", new SuperWebDriver(GetDriverSuiteNonGrid()));
                                break;
                            }
                        case "ie":
                            {
                                var IEoptions = new InternetExplorerOptions();
                                IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                                context.Add("Driver", (IWebDriver)new InternetExplorerDriver(IEoptions));
                                break;
                            }
                        case "Chrome":
                            {

                                context.Add("Driver", (IWebDriver)new ChromeDriver());
                                break;
                            }
                        case "Edge":
                            {
                                EdgeOptions options = new EdgeOptions();
                                options.PageLoadStrategy = EdgePageLoadStrategy.Eager;
                                context.Add("Driver", (IWebDriver)new EdgeDriver());
                                break;
                            }
                        default:
                            {

                                context.Add("Driver", (IWebDriver)new FirefoxDriver());
                                break;
                            }
                    }
                }
                return context["Driver"];
            }
        }


        private static IWebDriver driverRunOnSauceLabs(string browser, string version, string platform)
        {
            var uri = new Uri("http://ondemand.saucelabs.com:80/wd/hub");
            DesiredCapabilities caps = new DesiredCapabilities();
            caps.SetCapability(CapabilityType.BrowserName, browser);
            caps.SetCapability(CapabilityType.Version, version);
            caps.SetCapability(CapabilityType.Platform, platform);
            caps.SetCapability("username", ConfigurationManager.AppSettings["usernameSauceLabs"]);
            caps.SetCapability("accessKey", ConfigurationManager.AppSettings["accessKeySauceLabs"]);
            caps.SetCapability("name", TestContext.CurrentContext.Test.Name);
            return new RemoteWebDriver(uri, caps, TimeSpan.FromSeconds(600));
        }

        private static IList<IWebDriver> GetDriverSuiteRunOnSauceLabs()
        {
            IList<IWebDriver> drivers = new List<Func<IWebDriver>>
            {
               () =>  { return (IWebDriver) driverRunOnSauceLabs("Internet Explorer", IEVersion, iePlatform) ; },
               () =>  { return (IWebDriver) driverRunOnSauceLabs("Chrome", ChromeVersion, chromePlatform); },
               () =>  { return (IWebDriver) driverRunOnSauceLabs("MicrosoftEdge", EdgeVersion, edgePlatform); },
               () =>  { return (IWebDriver) driverRunOnSauceLabs("firefox", FirefoxVersion, firefoxPlatform); },
            }.AsParallel().Select(d => d()).ToList();
            return drivers;
        }

        private static IList<IWebDriver> GetDriverSuiteGrid()
        {
            var IEoptions = new InternetExplorerOptions();
            IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            EdgeOptions options = new EdgeOptions();
            options.PageLoadStrategy = EdgePageLoadStrategy.Eager;
            var uri = new Uri(ConfigurationManager.AppSettings["SeleniumHubUrl"]);

            //Check node version in Parallel Grid mode
            string FirefoxVersion = ConfigurationManager.AppSettings["FirefoxVersion"];
            string ChromeVersion = ConfigurationManager.AppSettings["ChromeVersion"];
            string IEVersion = ConfigurationManager.AppSettings["IEVersion"];
            DesiredCapabilities capabilityChrome = DesiredCapabilities.Chrome();
            capabilityChrome.SetCapability(CapabilityType.Version, ChromeVersion);
            DesiredCapabilities capabilityIE = DesiredCapabilities.InternetExplorer();
            capabilityIE.SetCapability(CapabilityType.Version, IEVersion);
            DesiredCapabilities capabilityFF = DesiredCapabilities.Firefox();
            capabilityFF.SetCapability(CapabilityType.Version, FirefoxVersion);

            //Demo Multi version


            // Allow some degree of parallelism when creating drivers, which can be slow
            IList<IWebDriver> drivers = new List<Func<IWebDriver>>
            {
                () =>  { return new RemoteWebDriver(uri, capabilityChrome); },
                () =>  { return new RemoteWebDriver(uri, capabilityIE); },
                () =>  { return new RemoteWebDriver(uri, capabilityFF); },
            }.AsParallel().Select(d => d()).ToList();
            return drivers;
        }
        private static IList<IWebDriver> GetDriverSuiteNonGrid()
        {
            var IEoptions = new InternetExplorerOptions();
            IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            IList<IWebDriver> drivers = new List<Func<IWebDriver>>
            {
                () =>  { return (IWebDriver)new ChromeDriver(); },
                () =>  { return (IWebDriver)new FirefoxDriver(); },
                () =>  { return (IWebDriver)new InternetExplorerDriver(IEoptions); },
            }.AsParallel().Select(d => d()).ToList();
            return drivers;
        }


        public static void GoToUrl(string url)
        {
            Browser.Navigate().GoToUrl(url);
            Browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            var IEoptions = new InternetExplorerOptions();
            IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
        }

        public static void MaximizeWindow()
        {
            Browser.Manage().Window.Maximize();
        }

        public static void DeleteAllCookies()
        {
            Browser.Manage().Cookies.DeleteAllCookies();
        }

        public static void CloseBrowser()
        {
            if (Browser.GetType() == typeof(InternetExplorerDriver))
            {
                //Delete cookies
                ProcessStartInfo psInfo = new ProcessStartInfo();
                psInfo.FileName = Path.Combine(Environment.SystemDirectory, "RunDll32.exe");
                psInfo.Arguments = "InetCpl.cpl,ClearMyTracksByProcess 2";
                psInfo.CreateNoWindow = true;
                psInfo.UseShellExecute = false;
                psInfo.RedirectStandardError = true;
                psInfo.RedirectStandardOutput = true;
                Process p = new Process { StartInfo = psInfo };
                p.Start();
                p.WaitForExit(10000);

                //quit driver
                Browser.Quit();
                context.Clear();
            }
            else
            {
                DeleteAllCookies();
                Browser.Close();
                context.Clear();
            }
        }
    }
}
