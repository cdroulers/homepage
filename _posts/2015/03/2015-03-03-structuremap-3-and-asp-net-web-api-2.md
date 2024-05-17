---
layout: default
title: "StructureMap 3 and ASP.NET Web Api 2"
bodyclass: blog-article
---

The easiest way to integrate StructureMap 3 and ASP.NET Web Api 2 is quite simple. StructureMap has a NuGet package that does it, but it creates 5 new files in a project
and doesn't solve the problem well.

<!-- more -->

[This article](http://blog.ploeh.dk/2012/09/28/DependencyInjectionandLifetimeManagementwithASP.NETWebAPI/) gives an overview of why using the 'IDependencyScope' is not
the best solution.

The final solution looks like this

{% highlight csharp %}
public class StructureMapWebApiControllerActivator : IHttpControllerActivator
{
    private readonly IContainer _container;

    public StructureMapWebApiControllerActivator(IContainer container)
    {
        _container = container;
    }

    public IHttpController Create(
        HttpRequestMessage request,
        HttpControllerDescriptor controllerDescriptor,
        Type controllerType)
    {
        var nested = _container.GetNestedContainer();
        var instance = nested.GetInstance(controllerType) as IHttpController;
        request.RegisterForDispose(nested);
        return instance;
    }
}
{% endhighlight %}

You then register it in the GlobalConfiguration, usually in the App_Start/WebApiConfig.cs file

{% highlight csharp %}
    IContainer container = new Container(c => c.AddRegistry<ProvincesRegistry>());
    config.Services.Replace(
        typeof(IHttpControllerActivator), 
        new StructureMapWebApiControllerActivator(container));
{% endhighlight %}

And bam! Integration. The RegisterForDispose call also ensures that any IDisposable entities in your StructureMap configuration are properly disposed after a request.