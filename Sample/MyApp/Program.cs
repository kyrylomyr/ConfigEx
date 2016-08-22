using System;
using MyApp.RefLib;

namespace MyApp
{
    internal class Program
    {
        private static void Main()
        {
            var config = new MainConfig();

            Console.WriteLine("*** Main Config settings ***");
            Console.WriteLine($"{nameof(config.AppNameSetting)}: {config.AppNameSetting}");
            Console.WriteLine($"{nameof(config.OverriddenSetting)}: {config.OverriddenSetting}");

            Console.WriteLine();

            var refLibConfig = new RefLibConfig();
            var printer = new SettingsPrinter(refLibConfig);
            printer.Print();

            Console.Read();
        }
    }
}
