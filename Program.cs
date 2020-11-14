using System;
using System.Collections.Generic;
using System.Configuration;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;

using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleinumCSharp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string remoteHub = ConfigurationManager.AppSettings["SeleniumRemoteHub"];
            IWebDriver driver = new RemoteWebDriver(
                new Uri(remoteHub),
                new ChromeOptions()
            );

            UsingDriverManager(DriverOption.Firefox);
            UsingTheRemoteWebDriver();
            CrossBrowserTesting(driver);

            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadKey();
        }

        enum DriverOption
        {
            Chrome,
            Edge,
            Firefox,
            IE,
            Opera
        }

        /// <summary>
        /// Using the Web Driver Manager
        /// </summary>
        private static void UsingDriverManager(DriverOption driverOption)
        {
            IWebDriver webDriver = null;
            IDriverConfig driverConfig = null;

            switch (driverOption)
            {
                case DriverOption.Chrome:
                    driverConfig = new ChromeConfig();
                    webDriver = new ChromeDriver();
                    break;
                case DriverOption.Edge:
                    driverConfig = new EdgeConfig();
                    webDriver = new EdgeDriver();
                    break;
                case DriverOption.Firefox:
                    driverConfig = new FirefoxConfig();
                    webDriver = new FirefoxDriver("./");
                    break;
                case DriverOption.IE:
                    driverConfig = new InternetExplorerConfig();
                    webDriver = new InternetExplorerDriver();
                    break;
                case DriverOption.Opera:
                    driverConfig = new OperaConfig();
                    webDriver = new OperaDriver();
                    break;
            }

            new DriverManager().SetUpDriver(driverConfig);
            webDriver.Navigate().GoToUrl("https://www.google.com");
            System.Console.WriteLine($"Title : {webDriver.Title}");
            webDriver.Quit();
        }

        /// <summary>
        /// Reused code from 
        /// https://github.com/crossbrowsertesting/selenium-nunit/blob/master/BasicTest.cs
        /// </summary>
        /// <param name="driver"></param>
        private static void CrossBrowserTesting(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://crossbrowsertesting.github.io/todo-app.html");
            // Check the title
            driver.FindElement(By.Name("todo-4")).Click();
            driver.FindElement(By.Name("todo-5")).Click();

            // If both clicks worked, then the following List should have length 2
            IList<IWebElement> elems = driver.FindElements(By.ClassName("done-true"));
            // so we'll assert that this is correct.
            System.Console.WriteLine(elems.Count);

            driver.FindElement(By.Id("todotext")).SendKeys("run your first selenium test");
            driver.FindElement(By.Id("addbutton")).Click();

            // lets also assert that the new todo we added is in the list
            string spanText = driver.FindElement(By.XPath("/html/body/div/div/div/ul/li[6]/span")).Text;
            System.Console.WriteLine($"Run your first selenium test {spanText}");
            driver.FindElement(By.LinkText("archive")).Click();

            elems = driver.FindElements(By.ClassName("done-false"));
            System.Console.WriteLine(elems.Count);
        }

        private static void UsingTheRemoteWebDriver()
        {
            var capability = new ChromeOptions();
            string remoteHub = ConfigurationManager.AppSettings["SeleniumRemoteHub"];
           
            IWebDriver driver = new RemoteWebDriver(
              new Uri(remoteHub), capability
            );

            driver.Navigate().GoToUrl("https://www.google.com");
            System.Console.WriteLine(driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("C#");
            query.Submit();

            System.Console.WriteLine(driver.Title);
            driver.Quit();
        }
    }
}
