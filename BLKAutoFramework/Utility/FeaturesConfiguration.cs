using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace BLKAutoFramework.Utility
{
    public class FeaturesConfiguration : IFeaturesConfiguration
    {
        private readonly IConfiguration _configuration;
#pragma warning disable CS8601 // Possible null reference assignment.
        private static readonly string? BasePath = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly()!.Location).LocalPath);
#pragma warning restore CS8601 // Possible null reference assignment.
        private static readonly string? ProjectRootPath = BasePath!.Split(new string[] { "bin" }, StringSplitOptions.None)[0];
        public FeaturesConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(ProjectRootPath ?? ".")
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
        public string ApplicationURL
        {
            get => _configuration["BLKSupportPortal:ApplicationURL"];
        }
        public string BrowserType
        {
            get => _configuration["BLKSupportPortal:BrowserType"];
        }
        public string JIRAUserName
        {
            get => _configuration["BLKSupportPortal:JIRAUserName"];
        }
        public string JIRAUserAPIKey
        {
            get => _configuration["BLKSupportPortal:JIRAUserAPIKey"];
        }
        public string JIRAProjectKey
        {
            get => _configuration["BLKSupportPortal:JIRAProjectKey"];
        }
    }
}