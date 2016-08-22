using ConfigEx;

namespace MyApp
{
    public class MainConfig : ConfigBase
    {
        public virtual string AppNameSetting => Get<string>();

        public virtual string OverriddenSetting => Get<string>();
    }
}
