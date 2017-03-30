using IowaComputerGurus.Utilities.DnnPackageDeployer.Providers;
using IowaComputerGurus.Utilities.DnnPackageDeployer.Services;
using log4net;
using log4net.Config;
using Ninject;
using Ninject.Activation;

namespace IowaComputerGurus.Utilities.DnnPackageDeployer
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            //Setup log4net
            XmlConfigurator.Configure();

            //Setup Ninject
            SetupNinJect();

            //Get the job and run it
            var job = _kernel.Get<IDnnPackageDeployService>();
            job.DeployPackages();
        }

        private static void SetupNinJect()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ILocalFileService>().To<LocalFileService>();
            _kernel.Bind<IConfigurationProvider>().To<ConfigurationProvider>();
            _kernel.Bind<IDnnPackageDeployService>().To<LocalFileSystemDeployService>();
            _kernel.Bind<IInstallationPackageLocatorService>().To<InstallationPackageLocatorService>();
            _kernel.Bind<ILog>().ToProvider<LogProvider>();
        }

        internal class LogProvider : Provider<ILog>
        {
            protected override ILog CreateInstance(IContext context)
            {
                var serviceName = context.Request.ParentRequest.Service;
                return LogManager.GetLogger(serviceName);
            }
        }
    }
}
