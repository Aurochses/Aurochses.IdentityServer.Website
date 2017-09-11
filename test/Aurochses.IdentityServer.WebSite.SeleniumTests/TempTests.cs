using Aurochses.IdentityServer.WebSite.SeleniumTests.Fakes;
using Aurochses.Testing.Selenium;
using OpenQA.Selenium;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.SeleniumTests
{
    public class TempTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public TempTests(SeleniumFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [ClassData(typeof(SeleniumWebDriverDataGenerator))]
        // todo: solve this test
        public void Should_find_search_box(SeleniumWebDriverType type)
        {
            // Arrange
            var driver = _fixture.GetWebDriver(type);

            // Act
            driver.Navigate().GoToUrl(_fixture.GetUrl());

            // Assert
            var img = driver.FindElement(By.ClassName("icon-banner"));

            Assert.Equal("img", img.TagName);
        }
    }
}