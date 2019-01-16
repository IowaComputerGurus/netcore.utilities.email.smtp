# NetCore.Utilities.Email ![](https://img.shields.io/github/license/iowacomputergurus/netcore.utilities.email.svg)
A utility to assist in creating Excel spreadsheets in .NET Core and ASP.NET Core applications using the EPPlus.Core library.  This utility allows you to export .NET Object to Excel by simply adding metadata information regarding the desired column formats, title etc.  Allowing quick & consistent excel exports.

## Build Status
Master Branch ![Master Status](https://iowacomputergurus.visualstudio.com/ICG%20Open%20Source/_apis/build/status/NetCore%20Utilities%20Email?branchName=master)
Develop Branch ![Develop Status](https://iowacomputergurus.visualstudio.com/ICG%20Open%20Source/_apis/build/status/NetCore%20Utilities%20Email?branchName=develop)


## NuGet Package Information
ICG.NetCore.Utilities.Email ![](https://img.shields.io/nuget/v/icg.netcore.utilities.email.svg) ![](https://img.shields.io/nuget/dt/icg.netcore.utilities.email.svg)

## Dependencies
This project depends on the EPPlus.Core NuGet package.  No changes are made to the EPPlus.Core package, and its usage is goverened by its own license agreement.

## Usage

## Installation
Standard installation via HuGet Package Manager
```
Install-Package ICG.NetCore.Utilities.Email
```

## Setup
To setup the needed dependency injection items for this library, add the following line in your DI setup.
```
services.UseIcgNetCoreUtilitiesEmail();
```

## Usage

We are continuing to document, the ISmtpService, is the primary usage point for sending emails.  XML documention is available for all classes.

## Related Projects

ICG has a number of other related projects as well

* [AspNetCore.Utilities](https://www.github.com/iowacomputergurus/aspnetcore.utilities)
* [NetCore.Utilities.Spreadsheet](https://www.github.com/iowacomputergurus/netcore.utilities.spreadsheet)
* [NetCore.Utilities.UnitTesting](https://www.github.com/iowacomputergurus/netcore.utilities.unittesting)