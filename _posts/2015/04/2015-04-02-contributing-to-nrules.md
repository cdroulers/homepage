---
layout: default
title: "Contributing to NRules open-source project"
bodyclass: blog-article
---

We found a bug, we fixed it, we made a pull request!

<!-- more -->

While using the [NRules project](https://github.com/NRules/NRules), we found two bugs (at the same time)! Obviously, this was frustrating and hard to understand.

So a colleague and I got to work debugging, cloning the project locally and adding breakpoints in the code. We identified the breaking code!

I then opened the solution file and looked at the code and at the tests. The project has a very decent suite of unit tests! So I looked for tests on the method that
was causing the problem, added a few more tests for our use case and looked at the failing tests. First step of TDD! Get some failing tests.

We then worked on fixing the bugs one by one, until all tests passed. We refactored a tiny bit along the way and created a [pull request](https://github.com/NRules/NRules/pull/52).

It was approved and the [new version](https://www.nuget.org/packages/NRules/) came out this morning! Open-source rocks!