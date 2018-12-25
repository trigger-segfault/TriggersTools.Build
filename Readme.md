# TriggersTools.Build ![AppIcon](https://i.imgur.com/OEEDtge.png)

A collection of .NET libraries for automated assignment and modification of assembly info, either through MSBuild property or assembly info file. Both are designed to begin working immediately after being installed as a NuGet package. Although `CopyrightYear` requires that the current year in the copyright information be replaced with `{YEAR}`.

[![Discord](https://img.shields.io/discord/436949335947870238.svg?style=flat&logo=discord&label=chat&colorB=7389DC&link=https://discord.gg/vB7jUbY)](https://discord.gg/vB7jUbY)

***

# BuildTime ![AppIcon](https://i.imgur.com/2hKtTYg.png)

[![NuGet Version](https://img.shields.io/nuget/v/TriggersTools.Build.BuildTime.svg?style=flat)](https://www.nuget.org/packages/TriggersTools.Build.BuildTime/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/TriggersTools.Build.BuildTime.svg?style=flat)](https://www.nuget.org/packages/TriggersTools.Build.BuildTime/)
[![Creation Date](https://img.shields.io/badge/created-july%202018-A642FF.svg?style=flat)](https://github.com/trigger-death/TriggersTools.Build/commit/1815ace69913ee52b43418dea600d84721d111d8)

Automatically assigns an `AssemblyBuildTimeAttribute` to the assembly during the beginning of the build. Build time can be aquired through extension methods such as `Assembly.GetBuildTime()` with `AssemblyBuildTimeExtensions` in the namespace `TriggersTools.Build`. Unlike relying on the [linker time](https://stackoverflow.com/a/1600990/7517185), (which already no longer works in .NET Core 1.1 and later), this method guarantees that the build time will be present as long as it was compiled with MSBuild.

You can check if an assembly has an `AssemblyBuildTimeAttribute` by calling the extension method `Assembly.HasBuildTime()`.

# CopyrightYear ![AppIcon](https://i.imgur.com/YxSBdo7.png)

[![NuGet Version](https://img.shields.io/nuget/v/TriggersTools.Build.CopyrightYear.svg?style=flat)](https://www.nuget.org/packages/TriggersTools.Build.CopyrightYear/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/TriggersTools.Build.CopyrightYear.svg?style=flat)](https://www.nuget.org/packages/TriggersTools.Build.CopyrightYear/)
[![Creation Date](https://img.shields.io/badge/created-july%202018-A642FF.svg?style=flat)](https://github.com/trigger-death/TriggersTools.Build/commit/1815ace69913ee52b43418dea600d84721d111d8)

Replaces all instances of `{YEAR}` in copyrights with the current year. Works with the MSBuild `$(Copyright)` property and the `AssemblyCopyrightAttribute`.

Assign the `$(CopyrightYearAssemblyInfo)` property in your project file as your input assembly info file if you're using one different from `Properties\AssemblyInfo.cs`. Assembly files are local to `$(ProjectDir)` unless rooted.

See [`Framework.Test.csproj`](https://github.com/trigger-death/TriggersTools.Build/blob/master/samples/Framework.Test/Framework.Test.csproj#L17) for an example of assigning the `$(CopyrightYearAssemblyInfo)` property.
