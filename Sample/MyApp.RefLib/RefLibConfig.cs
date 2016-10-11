using System;
using ConfigEx;

namespace MyApp.RefLib
{
    public class RefLibConfig : ConfigBase
    {
        public RefLibConfig() : base(new CustomConverter())
        {
        }

        public virtual string StringSetting => Get<string>();

        public virtual int IntSetting => Get<int>();

        public virtual DateTime CustomDateSetting => Get<DateTime>();

        public virtual string OverriddenSetting => GetOverridden<string>();
    }
}
