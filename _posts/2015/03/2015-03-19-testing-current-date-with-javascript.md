---
layout: default
title: "Testing the current date with JavaScript"
bodyclass: blog-article
---

When you have time-sensitive operations that require using the current date, it's always a mess to test.

<!-- more -->

With JavaScript, the easiest way to get the current date is simply `new Date()` or `new Date(Date.now())`. But if we start using that everywhere, when
we get to a point where we want to test operations that lookup the current date, we become a bit stuck. Since our test data would be static, but not 
the current date.

Therefore, in our application, we simply created a global method called `getCurrentDate` which, by default, returns `new Date()`.

In our JavaScript tests, we can override the method in the `beforeEach` setup of the test.

{% highlight javascript %}
beforeEach(() => {
    window.getCurrentDate = () => {
        return new Date("2015-03-19");
    };
});
{% endhighlight %}

With that, our current date is always what we want in tests. We've also used it in our Selenium tests to ensure our UI thinks it's the date we want.

{% highlight csharp %}
{% raw %}
((IJavaScriptExecutor)SeleniumDriver).ExecuteScript(
    string.Format(@"app.GetCurrentDate = function() {{
        return new Date(new Date('{0}').setMinutes(new Date().getTimezoneOffset() + 60));
    }}", p0));
{% endraw %}
{% endhighlight %}

It requires making changes to an existing app if you are already using `new Date()` everywhere. It's a small cost to pay for testability!