---
layout: default
title: "AngularJS and TypeScript directives"
bodyclass: blog-article
---

TypeScript has not taken much ground in the AngularJS community yet. Here's a small tutorial on how to write directives in TypeScript.

<!-- more -->

We're currently building a UI that has multiple independent sections. For each of them, we will write a directive that handles everything on its own.

With that in mind, here's a simple TypeScript directive for AngularJS.

{% highlight javascript %}
module app.controllers {

    export interface ISomeScope {
        Customer: models.Customer;
        Provinces: shared.models.Province[];
    }
    
    export function consumer($scope: ISomeScope,
        $routeParams: IIdNumberRouteParams,
        provincesService: shared.services.IProvincesApiService) {

        $scope.Provinces = [];

        // Here, $scope.Customer should be equal to what is passed in as a parameter.
        // See the next snippet for a sample.
        
        var provincesPromise = provincesService.GetAll().then(data => {
            $scope.Provinces = data;
            return data;
        });
    }
}

angular
    .module("app")
    .controller(
        "app.controllers.customer", 
        ["$scope", "$routeParams", "shared.services.IProvincesApiService"])
    .directive(
        "customerEditor", 
        () => {
            return {
                scope: {
                    Customer: "=customer"
                },
                templateUrl: "app/customers/customer-editor.html",
                replace: true,
                controller: "app.controllers.customer"
            };
        });
{% endhighlight %}

Then, it can be used in a view as such:

{% highlight html %}
    <customer-editor customer="CurrentScope.CurrentCustomer" />
{% endhighlight %}

Those a little less familiar with [AngularJS directives](https://docs.angularjs.org/guide/directive) will need to understand that when declaring it, the scope value declares which
attributes can be passed to that directive.

Declaring the Customer in the `ISomeScope` interface is very practical since it's now explicit what this controller needs. It also makes for easier testing.