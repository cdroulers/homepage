---
layout: default
title: "Sharing TypeScript code between projects"
bodyclass: blog-article
---

In this current project, we are building a distributed system that communicates through REST APIs. We are planning on having multiple web interfaces and yesterday, we started the second
one.

<!-- more -->

The web projects use [AngularJS](https://angularjs.org/) for the framework and [TypeScript](http://www.typescriptlang.org/) as the language. Our first problem was that both Web UIs needed to access the same REST api (our shared data such as provinces, genders and taxes). At first, we simply copied the TypeScript modules, but that's obviously a no-go.

TypeScript doesn't compile to a DLL or anything that is easily shared, especially considering that we have multiple toolkits requiring the output such as bundle, 
[Chutzpah, ReSharper and Jasmine]({% post_url 2015/03/2015-03-07-system-web-optimization-bundle-config-listing %}).

The solution is came up with so far is to have a shared regular C# project. We also use this project to share a few utility classes to generate HTML in our `.cshtml` view files.

Any and all TypeScript definition files (`.d.ts` files) should be in this project. Ideally all under the same folder (`~/scripts/typings`) so we can reference them dynamically.

In this shared project, create your TypeScript files the same way you usually would. Then configure this project to output only one file and to generate a declaration file :

![Project properties](/assets/images/typescript-build-output.jpg)

The result of this is a shared.js file that contains all your code from the shared project and a shared.d.ts file that contains definitions you can refer from your other projects.

In each project you want to use the shared code from, [add a link](https://support.microsoft.com/kb/306234) to the two generated files (shared.d.ts and shared.js). Make sure that they aren't part 
of the shared project (do not include them).

![The linked files](/assets/images/typescript-links.jpg)

Now for the fun part. In each project you want to use the shared stuff in, make sure there are no typings directly in it and add the following directly in the `.csproj` file.

{% highlight xml %}
<TypeScriptCompile Include="..\SharedProject\scripts\typings\**\*.d.ts">
   <Link>scripts\typings\%(RecursiveDir)%(FileName)</Link>
</TypeScriptCompile>
{% endhighlight %}

This will automatically link all the typings from the shared project into the current project. The TypeScript compiler did not like having to read the same definition file multiple times.

![The linked definition files](/assets/images/typescript-links-typing.jpg)

And there you go! Since you have links to definition files for external libraries and to your shared.d.ts, everything necessary should be available in the web projects!

It's also necessary to add a post-build event to the projects that copies the `shared.js` to wherever you can include it in your HTML files in the end, otherwise, the actual application
will not work because of missing references!