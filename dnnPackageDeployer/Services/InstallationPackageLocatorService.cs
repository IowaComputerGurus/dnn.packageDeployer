using IowaComputerGurus.Utilities.DnnPackageDeployer.Providers;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IowaComputerGurus.Utilities.DnnPackageDeployer.Services
{
    public interface IInstallationPackageLocatorService
    {
        List<FileInfo> FindLatestVersionPackages(string directoryPath);
    }
    public class InstallationPackageLocatorService : IInstallationPackageLocatorService
    {
        private const string InstallPackageSearchFormat = "*_install.zip";
        private ILocalFileService _localFileService;
        private IConfigurationProvider _configurationProvider;
        private ILog _logger;

        public InstallationPackageLocatorService(ILocalFileService localFileService, IConfigurationProvider configurationProvider, ILog logger)
        {
            _localFileService = localFileService;
            _configurationProvider = configurationProvider;
            _logger = logger;
        }


        public List<FileInfo> FindLatestVersionPackages(string directoryPath)
        {
            //Get all package directories
            var toSearch = _localFileService.FindDirectories(directoryPath, _configurationProvider.PackageDirectoryName);
            _logger.Debug($"Searching {toSearch.Count} directories for install packages");

            var fileList = new List<FileInfo>();
            foreach(var dir in toSearch)
            {
                //Find install packages
                _logger.Debug($"Searching {dir.FullName} for install pacakges");                
                var possibleFiles = _localFileService.FindFiles(dir.FullName, InstallPackageSearchFormat);
                _logger.Debug($"Found {possibleFiles.Count} possible items");

                //if none skip
                if (possibleFiles.Count == 0)
                    continue;

                //If only 1, go for it
                if (possibleFiles.Count == 1)
                {
                    fileList.Add(possibleFiles[0]);
                    continue;
                }

                //Otherwise get the most recent
                var mostRecent = fileList.OrderByDescending(f => f.LastWriteTimeUtc).First();
                fileList.Add(mostRecent);
            }
            return fileList;
        }
    }
}
