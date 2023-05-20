using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestFixture]
public class AmazonTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    [TearDown]
    public void Teardown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void CompleteAmazonTasks()
    {
        driver.Navigate().GoToUrl("https://www.amazon.com");

        // Check if the homepage is loaded
        Assert.That(driver.Title, Is.EqualTo("Amazon.com"));

        // Search for "laptop"
        IWebElement searchBox = driver.FindElement(By.Id("twotabsearchtextbox"));
        searchBox.SendKeys("laptop");
        searchBox.SendKeys(Keys.Enter);

        // Add the non-discounted products in stock on the first page to the cart
        var productElements = driver.FindElements(By.XPath("//span[@class='a-price-whole' and not(ancestor::span[@class='a-text-price a-color-price'])]" +
            "/ancestor::div[@data-index]//a[@role='link']"));

        foreach (var productElement in productElements)
        {
            productElement.Click();
            var addToCartButton = driver.FindElement(By.Id("add-to-cart-button"));
            addToCartButton.Click();
            driver.Navigate().Back();
        }

        // Go to cart and check if the products are correct
        driver.FindElement(By.Id("nav-cart")).Click();
        var cartItems = driver.FindElements(By.XPath("//div[@data-item-count]"));

        // Assert that the cart contains the expected number of items
        Assert.That(cartItems.Count, Is.EqualTo(productElements.Count));
    }
}