---
layout: default
title: "Testing AngularJS controllers"
bodyclass: blog-article
---

So I just blogged about [writing controllers in TypeScript]({% post_url 2015/04/2015-04-18-typescript-controllers-for-angularjs %}). Here's how to write tests
for them now.

<!-- more -->

We recently refactored **all** our controllers to become classes and also (quite importantly) not use `$scope.$watch` anymore. We went this way after experiencing
problems with `$watch` itself and testability. [This blog article](http://www.benlesh.com/2013/10/title.html) outlines good reasons not to do it.

Our general guideline is to now use JavaScript properties when we want to have a value changing affect another in the same model or a child model. We also use
events when crossing model or controller borders.

With that in mind, anything that can be made to happen in a model is a straight forward test. Then, most things that can happen in a controller become easy to
test as well! There should be a very limited set of actions that a controller can do. It can `$emit` or `$broadcast` events, it can call AJAX (via a service
ideally) and it can respond to events.

Here's a simple sample test class for a controller.

{% highlight javascript %}
describe("app.controllers.YourController",() => {
    var scope: ng.IScope;
    var controller: app.controllers.YourController;
    var serviceMock: app.services.IService;
    var rootScope: ng.IRootScopeService;
    var q: ng.IQService;

    beforeEach(module("app"));

    beforeEach(() => {
        inject((
                $rootScope: ng.IRootScopeService, 
                $q: ng.IQService, 
                $injector: ng.IInjector) => {
            scope = <any>$rootScope.$new();
            rootScope = $rootScope;
            q = $q;

            serviceMock = jasmine.createSpyObj("app.services.Service", ["GetAll"]);
            (<jasmine.Spy>serviceMock.GetAll).and.returnValue(["thing"]);

            controller = new app.controllers.YourController(
                scope,
                serviceMock,
                $injector.get("otherDependency"));
        });
    });

    it("should load all things",() => {
        expect(controller.Things).not.toBeNull();
        expect(controller.Things.length).toBe(0);
        rootScope.$digest();
        expect(controller.Things.length).toBe(1);
    });

    describe("MyFunction",() => {
        it("should return true by default",() => {
            expect(controller.MyFunction()).toBe(true);
        });

        it("should return false when other thing is changed is false",() => {
            controller.OtherThing = false;
            expect(controller.MyFunction()).toBe(false);
        });
    });
});
{% endhighlight %}

This is a very cut down example, but it demonstrates a few things. First, mocking a service is quite easy with Jasmine. Then, using `$injector` to get
other dependencies you don't need for testing if you're too lazy to mock them (I know I was!).

Looking back, passing null into dependencies that aren't used in the tests would be much easier... I'll see about that later!