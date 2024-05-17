---
layout: default
title: "Web API 2 output caching"
bodyclass: blog-article
---

When building REST APIs, a time comes when you want to cache data. You get more users or your app gets slower because of data accumulation.

<!-- more -->

In ASP.NET MVC, there's a practical attribute called `OutputCacheAttribute` which will send caching headers back to the client AND can also cache the results of queries
on the server for even more caching.

There's no such attribute in Web API. So on my search to find a solution, I found [StrathWeb's Output Cache](https://github.com/filipw/AspNetWebApi-OutputCache). It's simple,
effective and quite extendable.

It can be added directly on a method as such:

{% highlight csharp %}
[OutputCache(ClientTimeSpan = 24 * 60 * 60, ServerTimeSpan = 24 * 60 * 60)]
public IEnumerable<MyDTO> GetAll()
{
    return null;
}
{% endhighlight %}

or even globally with a filter:

{% highlight csharp %}
public class FilterConfig
{
    public static void RegisterGlobalFilters(HttpFilterCollection filters)
    {
        // Cache all requests for a few hours by default.
        filters.Add(new CacheOutputAttribute()
        {
            ClientTimeSpan = 24 * 60 * 60, 
            ServerTimeSpan = 24 * 60 * 60
        });
    }
}
{% endhighlight %}

When adding a `ServerTimeSpan`, an ETag is generated and it's even easier to return 304 Not Modified later on.

It's also extendable in many ways. For now, I'm only using the cache key generation via `ICacheKeyGenerator`, which let's me decide how to cache items.

In our APIs, we will generally cache results by the `Accept-Language` header since the results vary. For other APIs, we plan on caching by user, so we will
create the key using the `Authorization` Header.

So far, it doesn't output the `Expires` header, but it doesn't seem necessary for the moment. I'll have to read more about HTTP caching to make sure I understand
and then contribute to the project!