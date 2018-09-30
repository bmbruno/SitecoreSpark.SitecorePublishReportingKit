# README #

## About

Sitecore Publish Reporting Kit (SPRK) is a tool for logging all publishing activity in Sitecore at the item level.

* Current version: 1.5.0
* About & Download: [SPRK (www.brandonbruno.com)](https://www.brandonbruno.com/sections/development/sprk.html)

## Features

* Provides detailed log files for publishing operations in Sitecore.
* Provides detailed information about each item published, including:
  * Publish mode (incremental / full)
  * Username
  * Item ID
  * Publish result (success / fail / etc.)
  * Source database
  * Target database
  * Date/time of publish
* Configuration of log location and filename.
* Reporting interface built into the Sitecore Client backend.
* View, sort, filter, and export publish logs to CSV format.
* Publish Queue Viewer: detailed report of items in the incremental publish queue.

## Requirements

* .NET 4.5.2 or greater
* Sitecore 8.2 or greater (tested on 8.2.0, 8.2.4, 8.2.6, 9.0.0, 9.0.1)

## Getting Started

#### 1. Installation ####

SPRK is installed via a Sitecore package zip file. Install the package "Sitecore Publish Reporting Kit 1.1.0.zip" in the instance of Sitecore that handles content publishing. This is most likely your content management instance.

#### 2. Configuration ####

After installing the package, open the following configuration file and be sure it is configured for your environment:

```
\App_Config\Include\SitecoreSpark\SPRK\SitecoreSpark.SPRK.Settings.config
```

The following settings are available:

* **`SitecoreSpark.SPRK.Enabled`** - enables / disables the SPRK publish logging altogether
* **`SitecoreSpark.SPRK.LogFolder`** - path where publish logs should be written; by default, this will be Sitecore's default location for logs - "$(dataFolder)/logs/"
* **`SitecoreSpark.SPRK.LogPrefix`** - filename prefix for log files; all log files are appended with a dateformat (yyyyMMdd)

The default values provided should work for most Sitecore environments.

#### 3. Verify the Installation ####

To verify that the module is working properly, ensure that you see the expected log file appear once a publish is complete. If using default settings, you should see "SPRK.log.YYYYMMDD.txt" ('YYYYMMDD' matching the date) in your Sitecore log folder.

You can also verify that the "SPRK Publish Logs" reporting tools are loading from the Sitecore Launchpad.

## Upgrading

If you're upgrading from a previous version, the Sitecore package will prompt you for overwrite/skip actions. Most files are safe to overwrite, but if you have modified any of the following files from their defaults, you will want to merge them manually:

```
\App_Config\Include\SitecoreSpark\SPRK\SitecoreSpark.SPRK.Settings.config
```

## Troubleshooting 

If you don't see a log file after publishing, open the ShowConfig.aspx utility and verify that the following changes were made to `<sitecore>` configuration:

Pipelines processors added under the `<publish>` node:

```
SitecoreSpark.SPRK.Publishing.Pipelines.Publish.PublishLoggerStartProcessor
SitecoreSpark.SPRK.Publishing.Pipelines.Publish.PublishLoggerEndProcessor
```

Pipeline processors added under the `<publishItem>` node:

```
SitecoreSpark.SPRK.Publishing.Pipelines.PublishItem.PublishLoggerProcessor
```

Configurator added under the `<services>` node:

```
SitecoreSpark.SPRK.SparkConfigurator
```

## Other Notes

* SPRK does NOT log items published through the [Sitecore Publishing Service](https://dev.sitecore.net/Downloads/Sitecore_Publishing_Service.aspx).

* Any possible errors and exceptions generated by SPRK will be logged in the standard Sitecore diagnostics log.

* The "SPRK Publish Logs" icon appears on the Sitecore Launchpad. By default, all common Sitecore roles can access these reports. If you need to make changes to this icon (security, location, etc.), it is located in the `Core` database:

  ```
  core://sitecore/client/Applications/Launchpad/PageSettings/Buttons/Tools/SPRK Publish Logs
  ```

## Dependency Injection

SPRK uses Sitecore 8.2's built-in Microsoft dependecy injection library. If you are using a third-party DI library, you will have to register a few classes based on the following registrations:

```
serviceCollection.AddTransient<ReportController>();
serviceCollection.AddSingleton<ISparkLogger, SparkLogger>();
serviceCollection.AddScoped<ILogManager<LogItem>, LogManager>();
```
  
## Upcoming Features
These features are planned for upcoming releases, but this list is subject to change at any time.


* **Log customization**. Choose what fields appear in your publish log via Sitecore configuration.
* **Automated reporting**. Hook into the publish logging process and fire off reports (email, etc.) when certain conditions are met.
* **NuGet Package**. Make SPRK deployable from your solution.
* **More Support**. Backport to Sitecore 8.0, 8.1.

## Contact the Author

For questions / comments / issues, contact me:
* Twitter: [@BrandonMBruno](https://www.twitter.com/BrandonMBruno) or [@SitecoreSpark](https://www.twitter.com/SitecoreSpark)
* Email: bmbruno [at] gmail [dot] com
 
## License

MIT License. See accompanying "License.txt" file.