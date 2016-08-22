using System;

namespace MyApp.RefLib
{
    public class SettingsPrinter
    {
        private readonly RefLibConfig _config;

        public SettingsPrinter(RefLibConfig config)
        {
            _config = config;
        }

        public void Print()
        {
            Console.WriteLine("*** RefLib Config settings ***");
            Console.WriteLine($"{nameof(_config.StringSetting)}: {_config.StringSetting}");
            Console.WriteLine($"{nameof(_config.IntSetting)}: {_config.IntSetting}");
            Console.WriteLine($"{nameof(_config.CustomDateSetting)}: {_config.CustomDateSetting}");
            Console.WriteLine($"{nameof(_config.OverriddenSetting)}: {_config.OverriddenSetting}");
        }
    }
}
