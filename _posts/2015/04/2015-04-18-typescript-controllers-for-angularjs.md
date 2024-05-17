---
layout: default
title: "TypeScript controller for AngularJS"
bodyclass: blog-article
---

AngularJS 1.3 doesn't have native support for TypeScript like [AngularJS 2.0 will have](http://blogs.msdn.com/b/typescript/archive/2015/03/05/angular-2-0-built-on-typescript.aspx).
Here's a decent way to write them for 1.3.

<!-- more -->

We want to use the full power of TypeScript for classes, properties and methods. Here's a basic controller and directive.

<script src="https://gist.github.com/cdroulers/3d9606869f10803757dc.js"></script>

<script src="https://gist.github.com/cdroulers/813fcaf03df177db5420.js"></script>

As we can see, the controller is a normal TypeScript class. It also `extends` another class that has base functionality (yay code re-use!). We can
declare normal TypeScript properties which can be used in the view as if they were normal fields.

Notice the `public static $inject` member of the class. This is for AngularJS' injector service!

At the end of the file, we register the controller and directive. The catch is here! Adding `controllerAs` and `bindToController` does most of the magic.

These options tell AngularJS to add a `ctrl` member to the `$scope` which is the instance of the controller it will create. `bindToController` then tells
AngularJS to bind the scope variable (the attributes on our directive) to the controller instead of on `$scope`.

This means that the `SomeModel` member of the controller will be filled by whatever is passed into the `model` attribute on the directive declaration.

In the view, we have to prefix everything with `ctrl`, but that's a tiny price to pay for easier, cleaner controllers. No more huge functions declaring more
functions and so on. This also gives better refactoring power and type safety since it's a class with clear members and functions.