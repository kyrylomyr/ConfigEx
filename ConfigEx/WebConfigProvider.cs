using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;

namespace ConfigEx
{
    public sealed class WebConfigProvider : IConfigProvider
    {
        public Configuration Get()
        {
            try
            {
                return WebConfigurationManager.OpenWebConfiguration("~/");
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Failed to open Web.config file", ex);
            }
        }
    }
}
