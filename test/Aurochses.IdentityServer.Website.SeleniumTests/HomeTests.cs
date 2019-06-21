using Aurochses.Xunit.Selenium;
using OpenQA.Selenium;
using Xunit;

namespace Aurochses.IdentityServer.Website.SeleniumTests
{
    public class HomeTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public HomeTests(SeleniumFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [ClassData(typeof(SeleniumWebDriverDataGenerator))]
        public void Should_find_search_box(SeleniumWebDriverType type)
        {
            // Arrange
            var driver = _fixture.GetWebDriver(type);

            // Act
            driver.Navigate().GoToUrl(_fixture.GetUrl());

            // Assert
            var p = driver.FindElement(By.TagName("p"));

            Assert.Equal("p", p.TagName);
        }
    }
}