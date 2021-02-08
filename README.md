# NetCore.Utilities.Email.Smtp ![](https://img.shields.io/github/license/iowacomputergurus/netcore.utilities.email.smtp.svg)
This library provides an easy to use implmentation of SMTP based email delivery using the MailKit library internally.  This abstraction with proper interfaces allows email implementation inside of your project with little effort and easy to manage integration.

This package depends on the ICG.NetCore.Utilities.Email project for template implementation 

## Build Status

| Branch | Status |
| --- | --- |
] Master | ![Master Status](https://iowacomputergurus.visualstudio.com/ICG%20Open%20Source/_apis/build/status/NetCore%20Utilities%20Email%20Smtp?branchName=master) |
| Develop | ![Develop Status](https://iowacomputergurus.visualstudio.com/ICG%20Open%20Source/_apis/build/status/NetCore%20Utilities%20Email%20Smtp?branchName=develop)


## SonarCloud Analysis

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=alert_status)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=coverage)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=security_rating)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=sqale_index)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)


## NuGet Package Information
ICG.NetCore.Utilities.Email.Smtp ![](https://img.shields.io/nuget/v/icg.netcore.utilities.email.smtp.svg) ![](https://img.shields.io/nuget/dt/icg.netcore.utilities.email.smtp.svg)

## Dependencies
This project depends on the (MailKit)[https://github.com/jstedfast/MailKit] NuGet package. 

## Usage

## Installation
Standard installation via HuGet Package Manager
```
Install-Package ICG.NetCore.Utilities.Email.Snto
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