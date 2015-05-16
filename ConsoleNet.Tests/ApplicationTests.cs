using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ConsoleNet;
using System.Threading.Tasks;

namespace ConsoleNet.Tests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public async Task RunApplicationAndMakeSureTheMethodRunsWithTheGivenArgs()
        {
            const string paramName = "param1";
            const string paramValue = "param1Value";

            Application application = new Application("Test Application", new FrameworkCommand[]
                {
                    new FrameworkCommand("run", (app) =>
                    {
                        string value;
                        if (app.TryGetParam(paramName, out value))
                        {
                            if (value != paramValue)
                            {
                                Assert.Fail("The param was not the correct value.");
                            }
                        }
                        else Assert.Fail("The param was not able to be gotten.");
                    })
                });

            await application.RunAsync(new string[] { "run ", "--" + paramName + " " + paramValue });
        }
    }
}
