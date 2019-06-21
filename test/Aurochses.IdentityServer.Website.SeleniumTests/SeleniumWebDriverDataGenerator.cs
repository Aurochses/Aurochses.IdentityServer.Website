using System.Collections;
using System.Collections.Generic;
using Aurochses.Xunit.Selenium;

namespace Aurochses.IdentityServer.Website.SeleniumTests
{
    public class SeleniumWebDriverDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            // todo: there is no Edge on Windows Server 2012 r2
            //new object[] { SeleniumWebDriverType.Edge },
            // todo: solve Firefox driver don't see running
            //new object[] { SeleniumWebDriverType.Firefox },
            new object[] { SeleniumWebDriverType.GoogleChrome }
            // todo: IE issue on server must be solved
            //new object[] { SeleniumWebDriverType.InternetExplorer }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
