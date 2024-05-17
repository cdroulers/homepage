---
layout: default
title: "AngularJS HTTP Interceptor to set headers"
bodyclass: blog-article
---

Our new system is built in a distributed way. We have a bunch of services that have a clear responsibility and they are accessible over REST. Here's how we handle a few 
headers for calling them in AngularJS.

<!-- more -->

The simplest use-case for this is internationalisation. Our app will need to be available in both French and English. Therefore, we are using HTTP's `Accept-Language` header.

The REST API checks for this header and returns any localisable data in that language as well as error messages or any other resource that can be translated.

To do this globally in our AngularJS application, we used an [HTTP interceptor](https://docs.angularjs.org/api/ng/service/$http#interceptors). Here's the gist :

{% highlight javascript %}
angular
    .module("shared", [])
    .factory("httpRequestInterceptor",() => {
        return {
            request: (config) => {
                // The language can be configured any other way.
                // We'll eventually use $("html").attr("lang").
                config.headers["Accept-Language"] = "fr-CA"; 
                return config;
            }
        };
    })
    .config(["$httpProvider", ($httpProvider: ng.IHttpProvider) => {
        $httpProvider.interceptors.push("httpRequestInterceptor");
    }])
{% endhighlight %}

This is also used by our authentication library to set the `Authentication` header for obvious purposes.

This means that any request made in our app via the `$http` provider will have those headers.