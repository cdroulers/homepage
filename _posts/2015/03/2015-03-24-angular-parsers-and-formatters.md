---
layout: default
title: "AngularJS parsers and formatters"
bodyclass: blog-article
---

AngularJS's ngModel directive is pretty neat. It lets you bind data to form controls. It's also fairly extendable!

<!-- more -->

We're trying to build a decent numeric input in our application and it's kind of a mess. `input[type='number']` doesn't seem to perfectly fit our needs. So I started looking
for third party solutions and I couldn't find any. It also seems like Angular's default i18n/i10n support is kind of lacking. It works for output (displaying data in an
internationalized way) but not for input (parsing numbers or other in other cultures).

So I'm currently looking into building my own directive that uses AngularJS's internals to format the numeric model into a currency or percentage or regular number but also allow
entering it in the current user's culture and then parsing it to a JavaScript float. I might have to do some awful, awful hacks like creating a regex from a formatted number!

If it works well, it shall be open-sourced through [Vooban's GitHub](https://github.com/vooban).

**EDIT:**

See a first draft in action here : [http://plnkr.co/edit/S92bQS?p=preview](http://plnkr.co/edit/S92bQS?p=preview)