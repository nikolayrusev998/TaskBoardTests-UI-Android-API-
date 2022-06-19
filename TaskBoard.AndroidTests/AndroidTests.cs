using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace TaskBoard.AndroidTests
{
    public class AndroidTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactsBookUrl = "https://taskboard.nikolayrusev1.repl.co/api";
        //  private const string ContactsBookUrl = "https://taskboard.nakov.repl.co/api";
        private const string appLocation = @"C:\taskboard-androidclient.apk";

        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {

            options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }
        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchTask_CreateTask_VerifyResults()
        {
            // Arrange
            var urlField = driver.FindElement(By.Id("taskboard.androidclient:id/editTextApiUrl"));
            urlField.Clear();
            urlField.SendKeys(ContactsBookUrl);

            var title = "title" + DateTime.Now;

            var buttonConnect = driver.FindElement(By.Id("taskboard.androidclient:id/buttonConnect"));
            buttonConnect.Click();

            var firstListedTask = driver.FindElement(By.XPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout[2]/android.view.ViewGroup/android.widget.LinearLayout/androidx.recyclerview.widget.RecyclerView/android.widget.TableLayout[1]/android.widget.TableRow[3]/android.widget.TextView[2]")).Text;
            Assert.That(firstListedTask, Is.EqualTo("Project skeleton"));


            var buttonAdd = driver.FindElement(By.Id("taskboard.androidclient:id/buttonAdd"));
            buttonAdd.Click();


            var titleField = driver.FindElement(By.Id("taskboard.androidclient:id/editTextTitle"));
            titleField.SendKeys(title);

            var buttonCreate = driver.FindElement(By.Id("taskboard.androidclient:id/buttonCreate"));
            buttonCreate.Click();



            var searchField = driver.FindElement(By.Id("taskboard.androidclient:id/editTextKeyword"));
            searchField.Clear();
            searchField.SendKeys(title);

            var buttonSearch = driver.FindElement(By.Id("taskboard.androidclient:id/buttonSearch"));
            buttonSearch.Click();

            var createdTask = driver.FindElement(By.Id("taskboard.androidclient:id/textViewTitle")).Text;
            Assert.That(createdTask, Is.EqualTo(title));



        }

    }
}