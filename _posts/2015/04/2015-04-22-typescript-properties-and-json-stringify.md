---
layout: default
title: "TypeScript properties and JSON.stringify"
bodyclass: blog-article
---

TypeScript supports [ECMA 5.1's properties](http://www.ecma-international.org/ecma-262/5.1/#sec-15.2.3.6)
[quite well](http://www.typescriptlang.org/Handbook#classes-accessors) but it seems it doesn't play well with `JSON.stringify()`

<!-- more -->

This entire blog post talks about how I ended up mapping JSON from our REST API into our project's TypeScript classes. The first problem when doing that
was that we wanted classes in our code with all the functions and helpers and static properties and whatnot. So we defined them as classes with TypeScript.

But when the JSON came back, it contained properties that had the same name as some functions, so the function would be overriden by any basic mapping code.

So I set out to create a base class that was more mapping aware.

The result is here: [https://gist.github.com/cdroulers/479c966506c92ed1fac0](https://gist.github.com/cdroulers/479c966506c92ed1fac0)

The mapping magic happens in the constructor. The `data` parameter is the JSON (or undefined when not mapping). It iterates through every property of the data, 
automatically ignoring functions of the class. Then, there are three options:

1. If the property found has a mapping function in the `_mapOptions` object, it calls that function to map the sub-entity. So if you have a tree of classes, they can
  easily be mapped
2. If the property found has a mapping name in the `PropertyNames`, then the property is set on the object as the mapped property name.
3. If the property is not explicitely ignored, then it is simply mapped on the object.

Option 2 is where the magic happens for Properties. Say you have a property like this one:

{% highlight javascript %}
class Test {
    private _name: string;

    public get Name(): string {
        console.log("Accessed Name");
        return this._name;
    }

    public set Name(value: string): void {
        if (!value) {
            throw new Error("You done messed up!");
        }

        this._name = value;
    }
}
{% endhighlight %}

When the JSON comes back from an API call with the `Name` property set to "Roger", you don't necessarily want it to go through the setter. So in the `_mapOptions`,
you pass the following:

{% highlight javascript %}
{
    PropertyNames: {
        "Name": "_name"
    }
}
{% endhighlight %}

Then, the JSON maps to private fields and no accessors are used and we have clean and pretty properties on our classes.

Then when I wanted to serialize the class back into JSON, the properties did NOT show up! I was very confused. So I looked into how TypeScript generates the property
and found it it defines it on the prototype of the class instead of on the object. The behaviour can be seen in 
[this plunkr](http://plnkr.co/edit/NXUo7zjJZaUuyv54TD9i?p=preview). `Class1` is what TypeScript would generate for our class higher up the post. `JSON.stringify` doesn't
serialise properties defined on the prototype and I was a bit annoyed. 

I [asked the question on Stack Overflow](http://stackoverflow.com/questions/29705211/object-defineproperty-on-a-prototype-prevents-json-stringify-from-serializing-it)
and got responses basically indicating that it was standard behaviour and a few workarounds. I set out to work around it using the `toJSON` function. Since I keep the
`_mapOptions` as a private member, I used those to reverse map property names and ignore certain properties. `toJSON` is automatically called by `JSON.stringify` so
any framework that uses that to serialise your classes (AngularJS does!) will get a proper object to serialise.

This is the best way I found to handle mapping to and from JSON with TypeScript. Any other suggestions?