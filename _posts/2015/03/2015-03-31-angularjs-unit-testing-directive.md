---
layout: default
title: "AngularJS Unit Testing a Directive"
bodyclass: blog-article
---

Writing an AngularJS directive is all fun and games until you start fixing bugs. Then regression appear and you look like an idiot.

<!-- more -->

So I wrote my [`numeric` directive]({% post_url 2015/03/2015-03-24-angular-parsers-and-formatters %}) recently and a few features were added and bugs reported.
I added them and fixed them and obviously, since I had no tests, regressions appeared. So I set out to test my directive!

Here are the basics.

{% highlight javascript %}
describe("shared.ui.directives.numeric",() => {
    var element: ng.IAugmentedJQuery,
        scope: tests.shared.ui.directives.INumericTestScope,
        timeout: ng.ITimeoutService;

    // Load required modules
    beforeEach(module("shared"));
    beforeEach(module("shared.ui"));

    beforeEach(inject(($rootScope: ng.IRootScopeService, 
        $compile: ng.ICompileService, 
        $timeout: ng.ITimeoutService) => {
            timeout = $timeout;
            scope = $rootScope.$new();

            // Generate the HTML that we would typically use for this directive
            var html = "<input type=\"text\" ng-model=\"Price\" " + 
                "numeric=\"{ filter: 'currency', decimals: 2 }\" " + 
                "min=\"MinValue\" max=\"MaxValue\">";
            // Set the scope values to sane defaults for testing.
            scope.Price = 100;
            scope.MinValue = 0;
            scope.MaxValue = 250;
            // This basically creates the DOM element from the string
            // And runs through the full Angular $digest scope.
            element = $compile(html)(scope);
            // append the element to the body during the test to see behaviour
            // while debugging
            element.appendTo(document.body);
            // This is called multiple times as it forces Angular
            // To re-evaluate its cycle
            scope.$digest();
    }));

    afterEach(() => {
        // Remove from body after test, obviously.
        element.remove();
    });
    
    it("should mark as invalid for min and valid when min changes",() => {
        scope.MinValue = 10;
        scope.$digest();

        element
            // Enters the value in the input field
            .val("0")
            // Forces Angular to realize the value has changed
            .trigger("input")
            // Force the blur event, which does the formatting.
            // Not necessary for this specific test, but I added it as an example
            .blur();
        var hasInvalidValidation = element.hasClass("ng-invalid-min");
        expect(hasInvalidValidation).toBe(true);

        scope.MinValue = 0;
        scope.$digest();
        // Since the numeric directive uses $timeout while $watching min and max
        // We have to flush it synchronously for it to execute. ngMock
        // adds the flush method.
        timeout.flush();
        hasInvalidValidation = element.hasClass("ng-invalid-min");
        expect(hasInvalidValidation).toBe(false);
    });
});
{% endhighlight %}

Carefully read the comments in the above code for specific tips and tricks on how to test it.

I now have a basic set up to test this directive, I can easily TDD the hell out of it for new features or bug reports!