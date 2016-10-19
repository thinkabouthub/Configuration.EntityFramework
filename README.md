# <img src="https://github.com/thinkabouthub/Configuration.EntityFramework/blob/master/configuration.entityframework_icon.png" width="48">onfiguration.EntityFramework 
## For .NET Framework & .NET Core

.NET Core supports a variety of different configuration options. Application configuration data can come from files using built-in support for JSON, XML, and INI formats, as well as from environment variables, command line arguments or even an in-memory collection. So why is there no support for EntityFramework? Well now there is!

Configuration.EntityFramework is a custom configuration provider for the [.NET Core Configuration system](https://docs.asp.net/en/latest/fundamentals/configuration.html). It's built on [EntityFrameworkCore](https://docs.efproject.net/en/latest/) allowing configuration settings to be stored in a wide variety of [repositories](https://docs.efproject.net/en/latest/providers/), including;

* Microsoft SQL Server,
* SQLite,
* Npgsql (PostgreSQL),
* MySQL (Official),
* Pomelo (MySQL),
* Microsoft SQL Server Compact Edition,
* IBM Data Servers,
* InMemory (for Testing),
* Devart (MySQL, Oracle, PostgreSQL, SQLite, DB2, SQL Server, and more),
* Oracle (Coming Soon).

## Build Status
<a href="https://www.myget.org/"><img src="https://www.myget.org/BuildSource/Badge/configuration-entityframework?identifier=915f4809-89a9-4512-8f0f-044d3ed0b017" alt="configuration-entityframework MyGet Build Status" /></a>

# So why use Configuration.EntityFramework?
Some settings, such as a connection string or those required during the initialisation of an application may be better located in a local file rather than a repository. However, in many cases a Configuration.EntityFramework can present some distinct advantages, including;  

1. Makes use of the .NET Core configuration provider model,
2. Support for complex types,
3. Access settings in a strongly typed fashion using the [Options pattern] (https://docs.asp.net/en/latest/fundamentals/configuration.html#options-config-objects),
4. Enhanced Configuration Management and Change Control. This is particularly relevant to a distributed environment,
5. Transactional update of settings for whole of environment,
6. Common settings can be shared among many applications. Support for single point of change,
7. In a complex system with many related applications or services it's not uncommon to have many configuration files. By persisting settings to a database, the dependency on these configuration files can be reduced,
8. All settings for a select criteria, such as environment, application or section can be retrieved with a single query,
9. Allow end users to update settings via the EntityFramework Context,

# Compatibility

Configuration.EntityFramework is **compatible** with **.NET Core** and **.NET 4.6.2 or greater**.

# How do I get started?
Our [Sample Project](https://github.com/thinkabouthub/Configuration.EntityFramework/tree/master/sample) demonstrates how to use Configuration.EntityFramework and gives you some starting points for learning more. Additionally, the [Getting Started](https://github.com/thinkabouthub/Configuration.EntityFramework/wiki/getting-started/) tutorial walks you through using Configuration.EntityFramework in a simple ASP.NET Core app.

## Get Packages
You can get Configuration.EntityFramework by [grabbing the latest NuGet packages](https://www.nuget.org/packages?q=configuration.entityframework). If you're feeling adventurous, [continuous integration builds are on MyGet](https://www.myget.org/feed/configuration-entityframework/package/nuget/Configuration.EntityFramework).

[Release notes](https://github.com/thinkabouthub/Configuration.EntityFramework/wiki/release-notes) are available on the wiki.
## Get Help
**Need help with Configuration.EntityFramework?** We're ready to answer your questions on [Stack Overflow](http://stackoverflow.com/questions/tagged/configuration.entityframework). Our [blog page](https://thinkabout.ghost.io/) is also a useful source of information and updates.

## Project

- [Configuration.EntityFramework](https://www.nuget.org/packages?q=configuration.entityframework) - [this repo](https://github.com/thinkabouthub/Configuration.EntityFramework).
