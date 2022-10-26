namespace BLKAutoFramework.Utility
{
    public interface IFeaturesConfiguration
    {
        string ApplicationURL { get; }
        string BrowserType { get; }
        string JIRAUserName { get; }
        string JIRAUserAPIKey { get; }
        string JIRAProjectKey { get; }
    }
}
