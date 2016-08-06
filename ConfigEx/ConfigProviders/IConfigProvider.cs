using System.Configuration;

namespace ConfigEx.ConfigProviders
{
    public interface IConfigProvider
    {
        Configuration Get();
    }
}