---
layout: default
title: "Listing bundle files from System.Web.Optimization"
bodyclass: blog-article
---

[System.Web.Optimization](https://aspnetoptimization.codeplex.com/) is a framework for ASP.NET Web Pages allowing automatic optimisation of static files such
as JavaScript and CSS. It can bundle and minify files and it can also use a CDN when available.

<!-- more -->

Bundles are a no-brainer in an ASP.NET web app nowadays. They can automatically use minified files or minify them and even fall back to a CDN in production.

In our current project, we've been using [AngularJS](https://angularjs.org/) and [Jasmine](http://jasmine.github.io/) for testing it. To run our tests, we're now up to 
three runners:

#### ReSharper 9+

For integration with all our other tests (C# tests written in NUnit). It means we can right-click our solution in Visual Studio and Run All Tests.

#### SpecRunner.html

SpecRunner.html with [Jasmine boot](http://jasmine.github.io/2.2/custom_boot.html) allows starting the tests in a browser and debugging easily with the availabe tools.

#### Chutzpath

[Chutzpah](https://github.com/mmanela/chutzpah) is a JavaScript test runner that we use in our automated builds on Atlassian's [Bamboo](https://www.atlassian.com/software/bamboo).

### Referencing files

Each of these solutions needs to reference all available JavaScript files. The Web app uses the bundling system directly, since that's exactly what it's for.

SpecRunner.html uses it as well, since it's part of the Web project and we can simply open it in our favourite browser and debug away.

Chutzpah and ReSharper are a bit tricker.

ReSharper builds a dependency tree of files according to <code>///&lt;reference path="" /&gt;</code> references. This means that anytime we add a JavaScript or TypeScript file, 
we have to make sure that it is referenced properly within the tree. This can get messy.

Chutzpah uses a [JSON file](https://github.com/mmanela/chutzpah/wiki/Chutzpah.json-Settings-File) that lists all required dependencies.

### Keeping the files up-to-date

From the start, we had trouble keeping these three different runners synchronised. Someone would add a file to a bundle, add a test and run it with SpecRunner.html, then push to the server.
A few minutes later, the build was broken because Chutzpah.json wasn't updated. Or someone would get the latest changes and run via ReSharper and the tests would all fail.

### The solution

The bundle configuration can be queried in code, so I wrote a quick and hacky solution that involves building the bundle and asking it for all the files it found.

{% highlight csharp %}
public static class BundleHelper
{
    /// <summary>
    /// Gets the files from a list of bundle names.
    /// </summary>
    /// <param name="root">The root folder of your web application.</param>
    /// <param name="bundleNames">The bundle names you want to get files from.</param>
    public static List<string> GetFiles(string root, params string[] bundleNames)
    {
        var coll = new BundleCollection();
        // BundleConfig should be your web application's BundleConfig.cs file.
        BundleConfig.RegisterBundles(coll);
        BundleTable.VirtualPathProvider = new FileVirtualPathProvider(root);
        var resolver = new BundleResolver(coll, new SuperFakeHttpContext(string.Empty));

        var results = new List<string>();
        foreach (var bundle in bundleNames)
        {
            results.AddRange(resolver.GetBundleContents(bundle));
        }

        return results;
    }
}
{% endhighlight %}

It requires a reference to <del>[MvcContrib.TestHelper](https://www.nuget.org/packages/MvcContrib.TestHelper) and</del> the following two classes:

* [SuperFakeHttpContext](https://gist.github.com/cdroulers/276d3772d43154141884)
* [FileVirtualPathProvider](https://gist.github.com/cdroulers/b9020717cd3fbd894747)

The code is currently not flexible at all, but it's a good start. The plan is to get this to execute post-build of the Web Project, which means anytime someone adds new files, we're
guaranteed the references will be up-to-date and all the tests will pass.

It's a decent amount of set up but it will end up saving headaches for everyone!

**(EDIT)**: The reference to MvcContrib.TestHelper is not required after all. I also make a tiny .exe that can call a static method. The build of the web application now calls the executable
with a static method that builds the configuration files properly. Automation!