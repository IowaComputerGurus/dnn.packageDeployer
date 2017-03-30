using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IowaComputerGurus.Utilities.DnnPackageDeployer.Services
{
    public interface ILocalFileService
    {
        bool DirectoryExists(string directoryPath);
        List<DirectoryInfo> FindDirectories(string directoryPath, string searchPattern);
        List<FileInfo> FindFiles(string directoryPath, string searchPattern);
        void CopyFiles(List<FileInfo> toCopy, string destinationDirectoryPath);
        void DeleteFiles(string directoryPath, string searchPattern);
        void DeleteFile(string filePath);
    }

    public class LocalFileService : ILocalFileService
    {
        private readonly ILog _logger;

        public LocalFileService(ILog logger)
        {
            _logger = logger;
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public List<DirectoryInfo> FindDirectories(string direstoryPath, string searchPattern)
        {
            if (!DirectoryExists(direstoryPath))
                throw new ArgumentException("Provided directory was not found", nameof(direstoryPath));

            var parentDirectoryInfo = new DirectoryInfo(direstoryPath);
            return parentDirectoryInfo.GetDirectories(searchPattern, SearchOption.AllDirectories).ToList();
        }

        public List<FileInfo> FindFiles(string directoryPath, string searchPattern)
        {
            var directory = new DirectoryInfo(directoryPath);
            return directory.GetFiles(searchPattern).ToList();
        }

        public void CopyFiles(List<FileInfo> toCopy, string destinationDirectoryPath)
        {
            foreach(var file in toCopy)
            {
                _logger.Debug($"Copying {file.Name} to deploy directory");
                File.Copy(file.FullName, Path.Combine(destinationDirectoryPath, file.Name));
            }
        }

        public void DeleteFiles(string directoryPath, string searchPattern)
        {
            //Validate directory
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException("Provided directory was not found", nameof(directoryPath));

            //Get the files
            _logger.Debug($"Removing files matching '{searchPattern}' in directory {directoryPath}");
            var toDelete = FindFiles(directoryPath, searchPattern);

            //Delete the files
            _logger.Debug($"Found {toDelete.Count} files to delete");
            foreach(var file in toDelete)
            {
                DeleteFile(file.FullName);
            }
        }


        public void DeleteFile(string filePath)
        {
            try
            {
                _logger.Debug($"Deleting file {filePath}");
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to delete file", ex);
            }
        }
    }
}
