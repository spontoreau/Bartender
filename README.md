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
First thing you have to do with Bartender is to implement the *IDependencyContainer* interface and register it into a IoC container.


## Licence

MIT
