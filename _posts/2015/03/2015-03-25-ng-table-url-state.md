---
layout: default
title: "ngTable URL state saving"
bodyclass: blog-article
---

A popular and useful AngularJS plugin, [ngTable](http://ngmodules.org/modules/ng-table), doesn't save its state anywhere by default. Here's how to fix it.

<!-- more -->

So you build a neat table with lots of data and paging and sorting and grouping and whatnot. But then, after playing with it and leaving the page, you hit the back button
and you've lost your place! You're back on the first page with default sorting and grouping.

So you want to put it in the URL, obviously. So when you change the state, the URL changes and the back button works (and bookmarks as well!).

Quick and easy solution :

{% highlight javascript %}
var firstLoad = true;
$scope.DealerListParams = new ngTableParams(
    angular.extend({ page: 1, count: 25, sorting: { Id: "asc" } }, $location.search()),
    {
        total: 0,
        counts: [5, 10, 25, 50],
        getData: ($defer: ng.IDeferred, params: ngTable.INgTableParams) => {
            if (!firstLoad) {
                $location.search(params.url());
            }
            firstLoad = false;

            // Regular loading code here
        }
    });
{% endhighlight %}

We use `angular.extend` to pass defaults OR load data from the URL with `$location.search()`. In the `getData` function, we make sure we don't change the URL on the first page
load. But if we change the sorting or grouping or page or page count, the page's URL will change to something like

    ?page=1&count=25&sorting%5BName%5D=asc

Now if you click another link and leave the page, when you hit back, the URL will contain this information and the `angular.extend` will take these values and pass them on to `getData`.

Unfortunately, this triggers a full routing reload. Which means the entire controller is called. It's probably not the end of the world, but it's kind of annoying since it flashes content
and could call more AJAX than needed.

So we extend the `$location` object with a new method:

{% highlight javascript %}
app.run([
    "$location", 
    "$route", 
    "$rootScope", 
    ($location: ng.ILocationService, 
     $route: ng.route.IRouteService, 
     $rootScope: ng.IRootScopeService) => {
        (<any>$location).searchReplace = () => {
            var lastRoute = $route.current;
            var un = $rootScope.$on("$locationChangeSuccess", () => {
                $route.current = lastRoute;
                un();
            });
            return $location.search.apply($location, arguments);
        };
    }]);
{% endhighlight %}

Now, we can simply call `$location.searchReplace(params.url())` in our `getData` function and the controller will not be reloaded. Back and forward will still reload the full page, but I
haven't gotten around to optimising that yet.

**UPDATE: As requested by a commenter (Bob Clingan), here is the code in standard JavaScript syntax.**

{% highlight javascript %}
var firstLoad = true;
$scope.DealerListParams = new ngTableParams(
    angular.extend({ page: 1, count: 25, sorting: { Id: "asc" } }, $location.search()),
    {
        total: 0,
        counts: [5, 10, 25, 50],
        getData: function ($defer, params) {
            if (!firstLoad) {
                $location.search(params.url());
            }
            firstLoad = false;

            // Regular loading code here
        }
    });
{% endhighlight %}

Extension code:

{% highlight javascript %}
app.run([
    "$location", 
    "$route", 
    "$rootScope", 
    function ($location, 
     $route, 
     $rootScope) {
        $location.searchReplace = function () {
            var lastRoute = $route.current;
            var un = $rootScope.$on("$locationChangeSuccess", function () {
                $route.current = lastRoute;
                un();
            });
            return $location.search.apply($location, arguments);
        };
    }]);
{% endhighlight %}