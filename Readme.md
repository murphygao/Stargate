# Stargate

[![Build Status](https://travis-ci.org/AiursoftWeb/Stargate.svg?branch=master)](https://travis-ci.org/AiursoftWeb/Stargate)

The core message queue for aiursoft web apps.

## How to run

**We strongly recommend running this app on Windows 10 or Windows Server 2016**

### Dependencies

* SQL Server LocalDb
* .NET Core 2.0 SDK

### Run with command

Please excuse the following commands in the project folder:

    set ASPNETCORE_ENVIRONMENT Development
    dotnet restore
    dotnet ef database update
    dotnet run

### Run in Visual Studio

Please install Visual Studio 2017 with .NET Core development kit.

1. Double click the `.sln` file.
2. Strike `F5`.

## How to publish

Please excuse the following commands in the project folder:

    set ASPNETCORE_ENVIRONMENT Production
    dotnet restore
    dotnet ef database update
    dotnet publish

If you have IIS installed already, just config the web path to:

    .\bin\Debug\netcoreapp2.0\publish

## What is the relationship with other Aiursoft apps

It acts as a broadcast platform itself, allowing developers to create several new channels for their specific applications. Cannot access each other's channel between different applications. Each channel will have a connection key.

It itself acts as a push tool, and the client can use the WebSocket protocol to connect to a specific channel. If after the client connects to the channel, the developer pushes any information to the channel, all clients of the channel will receive the information.

The life of the channel itself is 24 hours. The life of the credential itself is 20 minutes. Messages missed by the client are not responsible for retransmission. Once a client connects to a channel, no new push messages are lost after the connection unless the network is interrupted.

Its application information, credential information, and channel information all exist in its own database. All of its messages exist in its own memory. Once received by all clients, each message is cleared immediately. Therefore it is not responsible for storing any messages.

It is not responsible for checking that the client actually read the message, nor is it responsible for accepting any message from the client. It just faithfully pushed every message to every client.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you have push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your personal workflow cruft out of sight.

We're also interested in your feedback for the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.