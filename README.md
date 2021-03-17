# NetCore.Utilities.Email.Smtp ![](https://img.shields.io/github/license/iowacomputergurus/netcore.utilities.email.smtp.svg)
This library provides an easy to use implmentation of SMTP based email delivery using the MailKit library internally.  This abstraction with proper interfaces allows email implementation inside of your project with little effort and easy to manage integration.

This package depends on the ICG.NetCore.Utilities.Email project for template implementation 

## Build Status

| Branch | Status |
| --- | --- |
| Master | ![Master Status](https://iowacomputergurus.visualstudio.com/ICG%20Open%20Source/_apis/build/status/NetCore%20Utilities%20Email%20Smtp?branchName=master) |
| Develop | ![Develop Status](https://iowacomputergurus.visualstudio.com/ICG%20Open%20Source/_apis/build/status/NetCore%20Utilities%20Email%20Smtp?branchName=develop)


## SonarCloud Analysis

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=alert_status)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=coverage)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=security_rating)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=IowaComputerGurus_netcore.utilities.email.smtp&metric=sqale_index)](https://sonarcloud.io/dashboard?id=IowaComputerGurus_netcore.utilities.email.smtp)


## NuGet (ICG.NetCore.Utilities.Email.Smtp)

![](https://img.shields.io/nuget/v/icg.netcore.utilities.email.smtp.svg) ![](https://img.shields.io/nuget/dt/icg.netcore.utilities.email.smtp.svg)

## Dependencies
The following additional NuGet packages are installed with this extension.

* [MailKit](https://github.com/jstedfast/MailKit) - For email delivery
* [ICG NET Core Utilieis Email](https://github.com/IowaComputerGurus/netcore.utilities.email) - For Email Template Configuration

## Usage

### Installation
Standard installation via HuGet Package Manager
```
Install-Package ICG.NetCore.Utilities.Email.Smtp
```

### Setup & Configuration Options
To setup the needed dependency injection items for this library, add the following line in your DI setup.
```
services.UseIcgNetCoreUtilitiesEmailSmtp();
```

Additionally you must specify the needed configuration elements within your AppSettings.json file

```
  "SmtpServiceOptions": {
    "AdminEmail": "test@test.com",
    "Server": "test.smtp.com",
    "Port": "527",
    "UseSsl": true,
    "SenderUsername": "MySender",
    "SenderPassword": "Password",
    "AlwaysTemplateEmails": true,
    "AddEnvironmentSuffix": true
  },
  "EmailTemplateSettings": {
    "DefaultTemplatePath": "Template.html",
    "AdditionalTemplates": { "SpecialTemplate": "File.html" }
  }
```

| Setting | Description
| AdminEmail | This is the email address used as the "from" address and also for any usage of the "SendToAdministrator" option
| Server | The SMTP Server address to use
| Port | The Port to use for outbound emails
| UseSsl | Should SSL be used for emails
| SenderUsername | The username that should be used to connect to SMTP
| SenderPassword | The password that should be used to connect to SMTP
| AlwaysTemplateEmails | If selected ALL emails sent will be templated, by default using the "DefaultTemplate" as configured
| AddEnvironmentSuffix | If selected, all outbound emails sent from non-production addresses will have the environment name added to the end of the subject
| DefaultTemplatePath | The path, relative to the application root, where the default HTML template can be found for emails
| AdditionalTemplates | These are name/value pairs of additional templates and totally optional

### Usage

Usage is primarly completed by injecting the ISmtpService interface to your respective project, one injected emails can be sent with a single line of code. 

```
_smtpService.SendEmail("recipient@me.com", "My Subject", "<p>Hello!</p>");
```
Inline documentation exists for all API methods. We will continue to add more to this documentation in the future (PR's Welcome)

## Related Projects

ICG has a number of other related projects as well

* [AspNetCore.Utilities](https://www.github.com/iowacomputergurus/aspnetcore.utilities)
* [NetCore.Utilities.Spreadsheet](https://www.github.com/iowacomputergurus/netcore.utilities.spreadsheet)
* [NetCore.Utilities.UnitTesting](https://www.github.com/iowacomputergurus/netcore.utilities.unittesting)