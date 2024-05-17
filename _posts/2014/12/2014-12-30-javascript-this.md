---
layout: default
title: "JavaScript this binding"
bodyclass: blog-article
---

While reading articles this morning, I came across [this one](http://blog.javascripting.com/2014/11/06/real-world-javascript-anti-patterns/) talking about anti-patterns.

<!-- more -->

Most of the things were small, but I learned about the <code>thisArg</code> and the <code>bind</code> function. They both prevent the use of <code>var _this = this</code> in JavaScript anonymous functions.

{% highlight js %}
items.forEach(function (item) {  
    this.doSomething(item);
}, this)
{% endhighlight %}

Here is a sample for the <code>bind</code> function.
{% highlight js %}
el.click(function (e) {  
    this.clickCount++;
}.bind(this));
{% endhighlight %}

Both are awesome for keeping code clean and simple.