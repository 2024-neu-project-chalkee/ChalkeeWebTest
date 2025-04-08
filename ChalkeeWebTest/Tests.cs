using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ChalkeeWebTest;

[TestClass]
public sealed class Tests
{
    private WebDriver _webDriver;
    private static string _email;
    private static string _password;
    private const string _sut = "http://localhost:5173/";
    
    [TestInitialize]
    public void InitializeWebDriver()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        _email = configuration["Credentials:Email"]!;
        _password = configuration["Credentials:Password"]!;
        _webDriver = new FirefoxDriver();
    }

    [TestCleanup]
    public void TeardownWebDriver()
    {
        _webDriver.Quit();
    }

    private IWebElement? Navigate(string path) => _webDriver.FindElements(By.CssSelector(path)).First();

    private IWebElement WaitUntilElementIsAvailable(string path)
    {
        WebDriverWait wait = new(_webDriver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(path)));
        return _webDriver.FindElements(By.CssSelector(path)).First();
    }
    
    [TestMethod]
    public void TitleTest()
    {
        _webDriver.Url = _sut;
        Assert.AreEqual("Chalkee", _webDriver.Title);
    }

    [TestMethod]
    public void DashboardTitleTest()
    {
        _webDriver.Url = _sut + "signin";
        Assert.AreEqual("Chalkee - Sign in", _webDriver.Title);
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        WebDriverWait wait = new(_webDriver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.TitleIs("Chalkee - Dashboard"));
        Assert.AreEqual("Chalkee - Dashboard", _webDriver.Title);
    }
    
    [TestMethod]
    public void TimetableTitleTest()
    {
        _webDriver.Url = _sut + "signin";
        Assert.AreEqual("Chalkee - Sign in", _webDriver.Title);
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        WaitUntilElementIsAvailable("header nav:first-of-type button")!.Click();
        Navigate("header nav:last-of-type button:nth-child(3)")!.Click();
        WebDriverWait wait = new(_webDriver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.TitleIs("Chalkee - My timetables"));
        Assert.AreEqual("Chalkee - My timetables", _webDriver.Title);
    }
    
    [TestMethod]
    public void InfoTitleTest()
    {
        _webDriver.Url = _sut + "signin";
        Assert.AreEqual("Chalkee - Sign in", _webDriver.Title);
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        WaitUntilElementIsAvailable("header nav:first-of-type button")!.Click();
        Navigate("header nav:last-of-type button:nth-child(2)")!.Click();
        WebDriverWait wait = new(_webDriver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.TitleIs("Chalkee - My info"));
        Assert.AreEqual("Chalkee - My info", _webDriver.Title);
    }

    [TestMethod]
    public void DashboardTest()
    {
        _webDriver.Url = _sut + "signin";
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        Assert.AreEqual(1, WaitUntilElementIsAvailable("main:has(.island-col)").FindElements(By.CssSelector(".island-col")).Count);
        Assert.AreEqual("Dashboard", Navigate("main:has(.island-col)")!.FindElement(By.CssSelector("h1")).Text);
    }
    
    [TestMethod]
    public void TimetableTest()
    {
        _webDriver.Url = _sut + "signin";
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        WaitUntilElementIsAvailable("header nav:first-of-type button")!.Click();
        Navigate("header nav:last-of-type button:nth-child(3)")!.Click();
        Assert.AreEqual(1, WaitUntilElementIsAvailable("main:has(.island-col > table)").FindElements(By.CssSelector("table")).Count);
    }

    [TestMethod]
    public void InfoTest()
    {
        _webDriver.Url = _sut + "signin";
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        WaitUntilElementIsAvailable("header nav:first-of-type button")!.Click();
        Navigate("header nav:last-of-type button:nth-child(2)")!.Click();
        Assert.AreEqual(2, WaitUntilElementIsAvailable("main:has(.island-col)").FindElements(By.CssSelector(".island-col")).Count);
    }
    
    [TestMethod]
    public void SignOutTitleTest()
    {
        _webDriver.Url = _sut + "signin";
        Assert.AreEqual("Chalkee - Sign in", _webDriver.Title);
        Navigate("form input[type='email']")!.SendKeys(_email);
        Navigate("form input[type='password']")!.SendKeys(_password);
        Navigate("form button")!.Click();
        Assert.AreEqual("Chalkee - Dashboard", _webDriver.Title);
        WaitUntilElementIsAvailable("header nav:first-of-type button")!.Click();
        Navigate("header nav:last-of-type button:last-child")!.Click();
        WebDriverWait wait = new(_webDriver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.TitleIs("Chalkee"));
        Assert.AreEqual("Chalkee", _webDriver.Title);
    }
}