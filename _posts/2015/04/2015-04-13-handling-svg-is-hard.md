---
layout: default
title: "Handling SVG is hard"
bodyclass: blog-article
---

SVG is a pretty neat image format. But it's not as easy to implement in a web page.

<!-- more -->

Many different gotchas to watch out for, browser support is not totally there yet. Here are a few things I remember having to deal with.

1. `&lt;img&gt;` tags aren't the best way to inline images
2. Inlining the SVG XML directly in the page works best, but would increase the payload significantly since I have table of the same images
3. I wanted to use the least JavaScript possible
4. It needed to work on all recent browsers.

There were two steps. First, I wanted to use as much of SVG's capabilities as possible, including styling. Since I wasn't inline the SVG, I couldn't
style it using CSS in my HTML page's source. Therefore, I created an `IHttpHandler` to modify a base SVG file with some extra classes.

Then, on the client side, I implement some JavaScript to add hover functionality.

Here's the `IHttpHandler`:

<script src="https://gist.github.com/cdroulers/9cc8f7094923f14ad5c5.js"></script>

The `Web.config` file must be modified:

{% highlight xml %}
<system.webServer>
    <handlers>
        <add name="SvgHandler" verb="*" path="*.svg" type="Namespace.To.SvgHandler"/>
    </handlers>
</system.webServer>
{% endhighlight %}

And here is an AngularJS directive for the JavaScript:

<script src="https://gist.github.com/cdroulers/afa31c57cc48ea9b6539.js"></script>

The `IHttpHandler` receives requests for all SVG files. It parses the file name and looks for class names in the format `svg-file-name.class-names.svg`. I could have used the query
string, but browsers will usually understand a query string to mean that the file cannot be cached properly.

To display an SVG, it's a simple matter of using `&lt;object&gt;`:

{% highlight html %}
<a href="#/some-link" class="btn clickable-svg" svg-hover>
    <object data="/Path/To/SomeSvg.class-name.svg" type="image/svg+xml"></object>
</a>
{% endhighlight %}

I had to do a tiny bit of CSS for `clickable-svg` to work, otherwise, the mouse pointer doesn't react properly:

{% highlight css %}
a.clickable-svg:after {
    content: "";
    position: absolute;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
}

a.clickable-svg {
    position: relative;
}
{% endhighlight %}