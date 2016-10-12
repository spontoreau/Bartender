# Bartender

[![Build status](https://ci.appveyor.com/api/projects/status/9sajnjc8wfmnrd6n?svg=true)](https://ci.appveyor.com/project/Vtek/bartender) [![NuGet version](https://badge.fury.io/nu/Bartender.svg)](https://badge.fury.io/nu/Bartender)

Bartender is a CQRS propose without any dependencies and avaible for all netstandard 1.1 compliant platform.


## Installation

```
PM> Install-Package Bartender
```


## Features

  * Synchronous/Asynchronous message dispatching
  * Suit well with IoC
  * Simple Publish-subscribe mode
  * Automatic validation


## Getting started

#### Dependencies
First thing you have to do with **Bartender** is to implement the *[IDependencyContainer](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IDependencyContainer.cs)* interface and register it into a IoC container. 

You also need to register your handlers which implements handle behavior (*[IHandler](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IHandler.cs)*, *[IAsyncHandler](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IAsyncHandler.cs)*, *[ICancellableAsyncHandler](https://github.com/Vtek/Bartender/blob/master/src/Bartender/ICancellableAsyncHandler.cs)*) and bind dispatch interfaces (*[IDispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IDispatcher.cs)*, *[IAsyncDispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IAsyncDispatcher.cs)*, *[ICancellableAsyncDispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/ICancellableAsyncDispatcher.cs)*) to the *[Dispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/Dispatcher.cs)* class.

The complexity of this step really depend on which IoC container you use in your project. Check out project samples, you can find dependencies registration with StructureMap [here](https://github.com/Vtek/Bartender/blob/master/samples/ConsoleApplication/ConsoleApplication/Registries/InfrastructureRegistry.cs)

#### Write a message and handle it !


## Licence

MIT
