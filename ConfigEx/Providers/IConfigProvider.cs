using System.Configuration;

namespace ConfigEx.Providers
{
    public interface IConfigProvider
    {
        Configuration Get();
    }
}