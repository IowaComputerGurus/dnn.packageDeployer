# dnn.packageDeployer
A utility application for the deploying of DNN modules, helpful for those with larger DNN solutions and multiple install packages.

## Configuration
The following information is defined in the local configuration file for the executable
  <appSettings>
    <add key="RootSearchPath" value="C:\Test"/>
    <add key="DeployToPath" value="C:\Deploy" />
    <add key="PackageDirectoryName" value="packages"/>
  </appSettings>
Using this you will wan to configure  the utility to run in your environment.  The below table details the configuration options.
Setting | Value
--- | ---
RootSearchPath | This is the folder path, absolute or relative, to the source code installation
DeployToPath | This is the folder path to the location that will be used to store all install packages to prepare for installation
PackageDirectoryName | The name of the folder where your actual installation packages are found.  Older package format was packages.

## Usage

The current version of this utility is geared towards developer support.  Retreiving all of the individual DNN packages from a single directory.  Future versions of this utility will add the ability to push to a remote location, and ideally, eventually trigger an upgrade with status reporting.

To use in your environment download the latest release, extract the files to a location as desired, and then run.

## Logging

Each execution of the application will log to both the console and an execution.log file stored in the same folder as the application.  This can be helpful for reporting.