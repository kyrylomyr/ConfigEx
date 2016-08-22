using FakeItEasy;
using MyApp.RefLib;
using NUnit.Framework;

namespace MyApp.Tests
{
    [TestFixture]
    public class SettingsPrinterTests
    {
        [Test]
        public void SamplePrintTest()
        {
            var fakeConfig = A.Fake<RefLibConfig>();
            A.CallTo(() => fakeConfig.StringSetting).Returns("Faked string setting value");

            var printer = new SettingsPrinter(fakeConfig);
            printer.Print();
        }
    }
}
