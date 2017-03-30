using IowaComputerGurus.Utilities.DnnPackageDeployer.Providers;
using log4net;

namespace IowaComputerGurus.Utilities.DnnPackageDeployer.Services
{

    public class LocalFileSystemDeployService : IDnnPackageDeployService
    {
        private ILocalFileService _localFileService;
        private IConfigurationProvider _configurationProvider;
        private IInstallationPackageLocatorService _installationPackageLocatorService;
        private ILog _logger;

        public LocalFileSystemDeployService(ILocalFileService localFileService, IConfigurationProvider configurationProvider, IInstallationPackageLocatorService installationPackageLocatorService, ILog logger)
        {
            _localFileService = localFileService;
            _configurationProvider = configurationProvider;
            _installationPackageLocatorService = installationPackageLocatorService;
            _logger = logger;
        }

        public void DeployPackages()
        {
            //Validate configuration
            if (!_localFileService.DirectoryExists(_configurationProvider.RootSearchPath))
            {
                _logger.Error($"Desired search path {_configurationProvider.RootSearchPath} not found");
                return;
            }
            if (!_localFileService.DirectoryExists(_configurationProvider.DeployToPath))
            {
                _logger.Error($"Desired search path {_configurationProvider.DeployToPath} not found");
                return;
            }

            //Remove older files
            _logger.Debug("Removing install files from deploy path");
            _localFileService.DeleteFiles(_configurationProvider.DeployToPath, "*_install.zip");

            //Get the new files
            var toDeploy = _installationPackageLocatorService.FindLatestVersionPackages(_configurationProvider.RootSearchPath);
            _logger.Debug($"{toDeploy.Count} files found to deploy");

            //Copy files
            _localFileService.CopyFiles(toDeploy, _configurationProvider.DeployToPath);
            _logger.Debug("Finished deploy process");
        }
    }
}
