# Bartender

[![Build status](https://ci.appveyor.com/api/projects/status/9sajnjc8wfmnrd6n?svg=true)](https://ci.appveyor.com/project/Vtek/bartender) [![NuGet version](https://badge.fury.io/nu/Bartender.svg)](https://badge.fury.io/nu/Bartender)

Bartender is a CQRS propose without any dependencies and available for all netstandard 1.1 compliant platform.


## Installation

```
PM> Install-Package Bartender
```


## Features

  * Synchronous/Asynchronous message dispatching
  * Suit well with IoC
  * Simple Publish-Subscribe mode
  * Automatic validation


## Getting started

#### Dependencies
First thing you have to do with **Bartender** is to implement the *[IDependencyContainer](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IDependencyContainer.cs)* interface and register it with a IoC container. 

You also need to register your handlers (*[IHandler](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IHandler.cs)*, *[IAsyncHandler](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IAsyncHandler.cs)*, *[ICancellableAsyncHandler](https://github.com/Vtek/Bartender/blob/master/src/Bartender/ICancellableAsyncHandler.cs)*) and bind dispatch interfaces (*[IDispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IDispatcher.cs)*, *[IAsyncDispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IAsyncDispatcher.cs)*, *[ICancellableAsyncDispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/ICancellableAsyncDispatcher.cs)*) to the *[Dispatcher](https://github.com/Vtek/Bartender/blob/master/src/Bartender/Dispatcher.cs)* class.

This step really depend on which IoC container you use in your project. Check out project samples, you can find dependencies registration with StructureMap [here](https://github.com/Vtek/Bartender/blob/master/samples/ConsoleApplication/ConsoleApplication/Registries/InfrastructureRegistry.cs)

#### Write a message and handle it !
A message is just an implementation of the *[IMessage](https://github.com/Vtek/Bartender/blob/master/src/Bartender/IMessage.cs)* interface. You can consider the interface to work as a marker and it never impose to write specific code. When mark as *IMessage* an *Object* can be dispatched with the library. For exemple if you want to retreive informations of a person who's got an identifier, you can define a message like this :

```Csharp
public class GetPersonByIdQuery : IMessage
{
    public int Id { get; }
    
    public GetPersonById(int id)
    {
        Id = id;
    }
}
```

> By convention in **CQRS** when you define a message to read data it have to be suffix with **Query** and by opposition **Command** define a write operation message. The library don't force you to respect this principle but it is a good way to organise your project.


When your message is define, you have to write an handler for it. Handler can be implement with synchronous or ascynchronous execution :

```Csharp
//Synchronous handler
public class GetPersonByIdQueryHandler : IHandler<GetPersonByIdQuery, GetPersonReadModel>
{
    public GetPersonReadModel Handle(GetPersonByIdQuery message)
    {
        //add you logic code here
    }
}

//Asynchronous handler
public class GetPersonByIdQueryHandler : IAsyncHandler<GetPersonByIdQuery, GetPersonReadModel>
{
    public GetPersonReadModel HandleAsync(GetPersonByIdQuery message)
    {
        //add you logic code here
    }
}
```

The nature of queries is to retreive data, but for a command you can define to return an object or not (for fire & forget operation). To deal with this case, IHandler can be implement without return :

```Csharp
public class CreatePersonCommandHandler : IHandler<CreatePersonCommand>
{
    public void Handle(CreatePersonCommand message)
    {
        //add you logic code here
    }
}
```

When handlers are register with IoC and can be finded by your *IDependencyContainer* implementation, the message is dispatchable :

```Csharp
IDispatcher dispatcher = new Dispatcher();
dispatcher.Dispatch<GetPersonByIdQuery, GetPersonReadModel>(new GetPersonByIdQuery(1));
```

If your define an asynchronous handler, dispatch it with *IAsyncDispatcher* : 
```Csharp
IAsyncDispatcher dispatcher = new Dispatcher();
await dispatcher.DispatchAsync<CreatePersonCommand>(new CreatePersonCommand(1, "Name"));
```

> As you certainly notice, Dispatcher explicitly implement dispatching interfaces. This type of implementation force developer to use the instance as interface and suit well with IoC too :)


## Advance features

#### Publication

#### Validation

#### Cancellable dispatching


## Licence

MIT
