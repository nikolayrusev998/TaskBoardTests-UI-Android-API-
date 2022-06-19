using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TaskBoard.SeleniumUITests
{
    public class SeleniumUITests
    {

        private const string url = "https://taskboard.nikolayrusev1.repl.co/";
       // private const string url = "https://taskboard.nakov.repl.co/";
        private WebDriver driver;
        [SetUp]
        public void OpenBrowser()
        {

            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void Test_ListAllTasks_CheckFirstDoneTask()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);
            var contactsLink = driver.FindElement(By.LinkText("Task Board"));

            //Act
            contactsLink.Click();


            //Assert

            var boardDone = driver.FindElement(By.CssSelector("body > main > div > div:nth-child(3)"));
            var boardDoneTitle = boardDone.FindElement(By.CssSelector("#task1 > tbody > tr.title > td")).Text;

            Assert.That(boardDoneTitle, Is.EqualTo("Project skeleton"));

        }
        [Test]
        public void Test_SearchTasks_FindByKeyword()
        {

            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.CssSelector("a:nth-child(3) > .icon")).Click();
            driver.FindElement(By.Id("keyword")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys("home");
            driver.FindElement(By.Id("search")).Click();

            var title = driver.FindElement(By.CssSelector(".title > td")).Text;

            Assert.That(title, Is.EqualTo("Home page"));


        }
        [Test]
        public void Test_SearchTasks_InvalidData()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.CssSelector("a:nth-child(3) > .icon")).Click();

            driver.FindElement(By.Id("keyword")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys("missing125454555555391238");
            driver.FindElement(By.Id("keyword")).SendKeys(Keys.Enter);

            var searchResult = driver.FindElement(By.Id("searchResult")).Text;




            Assert.That(searchResult, Is.EqualTo("No tasks found."));
        }
        [Test]
        public void Test_CreateContact_InvalidData()
        {
            //Arragne
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.LinkText("Create")).Click();


            driver.FindElement(By.Id("description")).SendKeys("asdahsda");
            driver.FindElement(By.Id("create")).Click();


            var errorMsg = driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(errorMsg, Is.EqualTo("Error: Title cannot be empty!"));

        }


        [Test]
        public void Test_CreateValidDataTas_VerifyIsProperlyAdded()
        {
            driver.Navigate().GoToUrl(url);
           


            var newTaskButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2) > span.icon"));
            newTaskButton.Click();

            var newTaskTitle = driver.FindElement(By.Id("title"));
            newTaskTitle.Click();


            var enterTitle = "title" + DateTime.Now;
            newTaskTitle.SendKeys(enterTitle);


            var newTaskDescription = driver.FindElement(By.Id("description"));
            newTaskDescription.Click();


            var addDescription = "ne i tozi description";
            newTaskDescription.SendKeys(addDescription);
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();
            driver.Navigate().GoToUrl(url + "/boards");
            var allTasks = driver.FindElements(By.CssSelector(".tasks-grid .task:nth-child(1) .task-entry"));
            var lastTask = allTasks.Last();
            var titleLabel = lastTask.FindElement(By.CssSelector("tr.title > td")).Text;
            var descriptionLabel = lastTask.FindElement(By.CssSelector("tr.description > td > div")).Text;

            Assert.That(titleLabel, Is.EqualTo(enterTitle));
            Assert.That(descriptionLabel, Is.EqualTo(addDescription));
        }
    }
}
