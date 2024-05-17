---
layout: default
title: "Testing client-side code in Visual Studio"
bodyclass: blog-article
---

I had to write unit tests for AngularJS at work. We use Visual Studio and ReSharper for development and it has good support for Jasmine.

The tests are written in TypeScript as well, since we like the syntax and compile time safety it offers.

<!-- more -->

Here is what it looks like in general.

{% highlight js %}
describe("The Dealer controllers",() => {

    beforeEach(() => {
        // Global set up
    });

    describe("(ListController)",() => {

        it("should call the api service to retrieve all dealers",() => {
            // The code for the test.
        });
    });

    describe("(DetailController)",() => {

        beforeEach(() => {
            // Set up for the following tests only.
        });

        it("should call the api service to retrieve the specified dealer",() => {
            // A test!
        });
    });

    afterEach(() => {
        // Tear down code here.
    });
});
{% endhighlight %}

The harder parts was getting ReSharper to pick up on all the references and include them in the test page it creates for Jasmine. Using the following:

    /// <reference path="../../models/Dealer.ts" />

in each file that referenced others helped build a tree of dependencies and generate the proper page. For Chutzpah, the test runner we use on our build
server, it was a bit easier since we can simply enumerate the files one after the other.

The last step here is getting something a bit simpler and automatic for new code. Right now, if we add new files, Resharper might not pick them up if
we don't add a &lt;reference&gt; and Chutzpah.json needs to be updated manually.