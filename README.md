# Nugety for .NET Framework

Nugety for Microsoft .NET provides support for the **Modular Composition** of both Web and Native applications. It employs a Provider Model for the discovery of modules which are then loaded into the [Assembly Load Context](https://github.com/thinkabouthub/NugetyCore/wiki/Assembly-Load-Context). Nugety is best suited to ASP.NET Core or any application which employs an IoC container such as [Autofac](https://autofac.org), [Castle](http://www.castleproject.org/container/index.html), [Spring.Net](http://www.springframework.net/) or [Unity](http://unity.codeplex.com/). An IoC container will allow the modules to interact while still maintaining the all important **Seperation of Concerns**.
 
## Build Status
<a href="https://www.myget.org/"><img src="https://www.myget.org/BuildSource/Badge/nugety?identifier=751a41fc-bba0-4db0-951c-2633cb3ae9c0" alt="nugety MyGet Build Status" /></a>

# So why use Nugety?

This question is best answered by the likes of **Martin Fowler** in his blog post [Sacrificial Architecture](http://martinfowler.com/bliki/SacrificialArchitecture.html). The most effective way of adhering to the principal of Sacrificial Architecture is through Modular Architecture. Modular architecture encourages the logical and physical Separation of Concerns allowing functionality to be easily upgraded or replaced.

In comparison to monolithic composition of components and assemblies which is arguably an anti-pattern, Modular design is now actively encouraged by Microsoft and is a fundamental principal of ASP.NET Core.

Nugety allows all related aspects of a feature, including Services, Components and Static Files to be registered with the host application. Nugety allows all related aspects of a feature, including Services, Components and Static Files to be registered with the host application. [NugetyCore](https://github.com/thinkabouthub/NugetyCore) will also load each [Module Initializer](https://github.com/thinkabouthub/NugetyCore/wiki/Module-Initializer) assembly and their dependencies via a module specific [Assembly Load Context](https://github.com/thinkabouthub/NugetyCore/wiki/Assembly-Load-Context). This will help in the Separation of Concerns and in minimising any leakage between modules.

[More details](https://github.com/thinkabouthub/NugetyCore/wiki/Use-Cases) are available on the wiki.

# How do I get started?
Our [Getting Started](https://github.com/thinkabouthub/NugetyCore/wiki/getting-started/) tutorial walks you through integrating Nugety with a simple ASP.NET Core app and gives you some starting points for learning more.

## Get Packages

You can get Nugety by [grabbing the latest NuGet packages](https://www.myget.org/feed/nugety/package/nuget/Nugety). If you're feeling adventurous, [continuous integration builds are on MyGet](https://www.myget.org/gallery/nugety).

[Release notes](https://github.com/thinkabouthub/nugety/release-notes) are available on the wiki.

## Get Help

**Need help with Nugety?** We're ready to answer your questions on [Stack Overflow](http://stackoverflow.com/questions/tagged/nugety). Our [blog page](https://thinkabout.ghost.io/) is also a useful source of information and updates.

##Super-duper quick start

[Create a Module assembly](https://github.com/thinkabouthub/NugetyCore/wiki/create-module/), [get Module](https://github.com/thinkabouthub/NugetyCore/wiki/get-module/) using `NugetyCatalog` and then [load Module Initialiser](https://github.com/thinkabouthub/NugetyCore/wiki/load-module/).

```C#
var modules = new NugetyCatalog()
	.FromDirectory()
	.GetModules<IModuleInitializer>().Load();
```

[Set File Name Filter Pattern for Module Initialiser Assembly](https://github.com/thinkabouthub/NugetyCore/wiki/SetFileNameFilterPattern/):

```C#
var modules = new NugetyCatalog()
	.Options.SetFileNameFilterPattern("*module*.dll")
	.FromDirectory()
	.GetModules<IModuleInitializer>().Load();
```

[Set Module Name Filter Pattern for Modules to load](https://github.com/thinkabouthub/NugetyCore/wiki/SetModuleNameFilterPattern/):

```C#
var modules = new NugetyCatalog()
	.Options.SetModuleNameFilterPattern("*Development*")
	.FromDirectory()
	.GetModules<IModuleInitializer>().Load();
```

[Set root Modules Directory](https://github.com/thinkabouthub/NugetyCore/wiki/Load-From-Directory/). The default root directory is "Nugety".

```C#
var modules = new NugetyCatalog()
	.FromDirectory("DevelopmentModules")
	.GetModules<IModuleInitializer>().Load();
```

[Set Modules to be resolved](https://github.com/thinkabouthub/NugetyCore/wiki/get-module/). The default behaviour is for all modules found in the root Nugety Modules Directory to resolve.

```C#
var modules = new NugetyCatalog()
	.GetModules<IModuleInitializer>("swagger", "autho").Load();
```

**[Intrigued? Check out our Getting Started walkthrough!](https://github.com/thinkabouthub/NugetyCore/wiki/getting-started/)**

## Project

**There is a growing number of [application integration libraries] that make using Nugety with your application a snap.**

- [Nugety](https://www.myget.org/feed/nugety/package/nuget/Nugety) - Core Nugety API [this repo](https://github.com/thinkabouthub/Nugety).
- [Nugety.AspNetCore](https://www.myget.org/feed/nugety/package/nuget/Nugety.AspNetCore) - ASP.NET integration for Nugety [this repo](https://github.com/thinkabouthub/Nugety).
- [Nugety.Autofac](https://github.com/thinkabouthub/nugety) - Support for Nugety as Autofac modules [this repo](https://github.com/thinkabouthub/Nugety).
- [Nugety.Nuget](https://www.myget.org/feed/nugety/package/nuget/Nugety.Nuget) - Support for Nuget as Nugety source [this repo](https://github.com/thinkabouthub/Nugety).
