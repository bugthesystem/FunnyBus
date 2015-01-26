FunnyBus
========

Simple event aggregator/messenger for .net applications

[![Build status](https://ci.appveyor.com/api/projects/status/dif9b6d50xrw54yo?svg=true)](https://ci.appveyor.com/project/ziyasal/funnybus)

* [Nuget Package](https://www.nuget.org/packages/FunnyBus/)

```chasrp
Install-Package FunnyBus
```

**Sample**

* Initialize bus
```csharp 
var builder = new ContainerBuilder();
builder.RegisterAssemblyTypes(typeof(ProductHandler).Assembly)
       .AsImplementedInterfaces().SingleInstance(); 
builder.Register(context => Bus.Instance).As<IBus>();

IContainer container = builder.Build();
Bus.Configure(context => context.SetResolver(new AutofacFunnyDependencyResolver(container)));
```
* Message handler definition
```csharp
 public class ProductHandler : 
    IHandle<CreateProductMessage>, 
    IHandle<DeleteProductMessage,DeleteProductResult>
 {
     public void Handle(CreateProductMessage message)
     {
         Console.WriteLine(message.Name);
     }
     public DeleteProductResult Handle(DeleteProductMessage message)
     {
         Console.WriteLine(message.Message);
         return new DeleteProductResult { Success=true };
     }
 }
```
* Message definition
```csharp
public class DeleteProductMessage
{
   public int Id {get; set;}
}
```
* Publish Message
```csharp
var bus = container.Resolve<IBus>();
bus.Publish(new CreateProductMessage{ Name = "Funny Product" });

//If there is a return type
DeleteProductResult result=bus.Publish<DeleteProductResult>(new DeleteProductMessage{ Id = 10 });
```
* Register handler

Message handlers are automatically scanned and registered.
To disable this behavior set **AutoScanHandlers** option **false**;
```csharp
var bus = container.Resolve<IBus>();
 _bus.Subscribe<ProductHandler>();
```
* Remove handler
```csharp
var bus = container.Resolve<IBus>();
_bus.UnSubscribe<ProductHandler>();
```
