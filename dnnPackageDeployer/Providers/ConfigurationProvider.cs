using System.Configuration;

namespace IowaComputerGurus.Utilities.DnnPackageDeployer.Providers
{
    public interface IConfigurationProvider
    {
        string RootSearchPath { get; }
        string DeployToPath { get; }
        string PackageDirectoryName { get; }
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public string RootSearchPath => ConfigurationManager.AppSettings["RootSearchPath"];
        public string DeployToPath => ConfigurationManager.AppSettings["DeployToPath"];
        public string PackageDirectoryName => ConfigurationManager.AppSettings["PackageDirectoryName"];
    }
}
