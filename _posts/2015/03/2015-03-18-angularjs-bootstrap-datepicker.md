---
layout: default
title: "AngularJS Bootstrap Datepicker"
bodyclass: blog-article
---

The Twitter bootstrap framework is a great tool to quick-start applications and continue building them in the long term. 
They even built an [AngularJS library](https://angular-ui.github.io/bootstrap/) to easily integrate the two.

<!-- more -->

I wanted to integrate the [Datepicker](https://angular-ui.github.io/bootstrap/#/datepicker) since we have a birth date field in our forms. The problem was that
according to the example, we had to declare a boolean `opened`property AND an `open` function on the scope to set `opened` to true. 
(Read the example linked previously to see what I mean)

This is a bit annoying. I really don't want to pollute my controller code with multiple boolean values and functions just to open a datepicker when clicking on the add-on button.

The solution I found while researching is the following:

Modify the global scope to have a globally available function for opening date pickers:

{% highlight javascript linenos %}
angular.module("shared.ui").config(($provide) => {
    $provide.decorator("$rootScope",($delegate) => {
        $delegate.__proto__.OpenDatePicker = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            var fieldName = $($event.target).parents(".input-group:first")
                .find(":input[datepicker-options]").attr("name");
            this["datePickerOpenedFor" + fieldName] = true;
        };
        return $delegate;
    });
})
{% endhighlight %}

And then, when creating the datepicker, this method can be used automatically:

{% highlight html linenos %}
<div class="input-group has-addon">
    <input 
        class="form-control" 
        datepicker-options="{ }" 
        datepicker-popup=""
        id="dateOfBirth"
        is-open="datePickerOpenedFordateOfBirth"
        name="dateOfBirth"
        ng-click="OpenDatePicker($event)"
        ng-model="DateOfBirth">
    <span class="input-group-btn">
        <button type="button" class="btn btn-default" ng-click="OpenDatePicker($event)">
            <i class="glyphicon glyphicon-calendar"></i>
        </button>
    </span>
</div>
{% endhighlight %}

As you can see, the `OpenDatePicker` function is available on every scope. It basically just looks for the input's name to modify the `datePickerOpenedFor{name}` boolean property.

Now I can easily add new Datepickers anywhere I want without worrying about setting it up manually.