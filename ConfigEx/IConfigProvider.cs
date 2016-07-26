using System.Configuration;

namespace ConfigEx
{
    public interface IConfigProvider
    {
        Configuration Get();
    }
}