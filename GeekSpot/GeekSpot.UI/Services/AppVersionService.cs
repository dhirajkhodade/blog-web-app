using GeekSpot.Domain.Interfaces;
using System.Reflection;

namespace GeekSpot.UI.Services
{
    public class AppVersionService : IAppVersionService
    {
        public string Version => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
