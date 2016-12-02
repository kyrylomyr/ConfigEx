using System;
using System.Collections.Generic;

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
            var text = string.Join(Environment.NewLine, GetSettingsList());
            Console.WriteLine(text);
        }

        public List<string> GetSettingsList()
        {
            return new List<string>
                   {
                       $"{nameof(_config.StringSetting)}: {_config.StringSetting}",
                       $"{nameof(_config.IntSetting)}: {_config.IntSetting}",
                       $"{nameof(_config.CustomDateSetting)}: {_config.CustomDateSetting}",
                       $"{nameof(_config.OverriddenSetting)}: {_config.OverriddenSetting}"
                   };
        }
    }
}