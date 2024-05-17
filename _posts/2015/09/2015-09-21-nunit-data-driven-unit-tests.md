---
layout: default
title: "NUnit data-driven unit tests"
bodyclass: blog-article
---

Writing unit tests is fun. But sometimes, getting proper test data gets cumbersome.

<!-- more -->

Most examples of unit testing deal with a really small problem that has very few input and even less output. This
is great for learning, obviously, but when you get in a real-world problem, it's never that simple.

We had to write unit tests for a rule engine that had about 15 input variables and between 5 and 10 output
depending on the input. This test data also comes from the client because it's about business rules and porting
older code and manual calculations to a new system. How do we get the data from the client's head into our code
so we can run it through the algorithm?

### Excel to the rescue

Excel is always an obvious choice for *everything*. Everyone knows how to use it and has it installed. It's pretty
and you can easily build and manage data. So you create a file with headers and a few rows of data then you commit.
Now your repo has a nice binary file that you can't diff online (neither bitbucket or github supports it as of
writing this post) or offline, except with special tools. And the special tools aren't top-notch yet. Excel is out.

### CSV to the rescue

CSV is something that Excel still can't figure out properly. If you're lucky enough that everyone that's going to
touch the file runs their OS in the same locale. Otherwise, you'll get people opening the file with the `fr-CA`
locale that won't use `,` as their CSV separator and Excel will happily display everything in one column. Using the
`sep=,` works if everyone uses Excel but LibreOffice doesn't support it and you get weird data.

### TSV the final solution

The best option we've come up with is using a `.tsv` file. Which isn't a real file format, it's simply a tab-separated
CSV using the `.tsv` extension. It has a few drawbacks that were very acceptable to us.

* It doesn't have a default program, so you have to start it with the `open with` dialog or double-click it and let
Windows handle it.
* You can't save formulas or formatting in the file.

But opening it with Excel will work no matter the locale and LibreOffice opens up a parsing dialog with `{Tab}` as
the delimiter pre-selected.

It [diffs properly with any tool and online](https://github.com/cdroulers/nunit-data-driven-tests/commit/cb5334be9d3177d84d293c6f0dc70a0ec8d43c92).
[GitHub even shows the TSV file in a neat HTML table!](https://github.com/cdroulers/nunit-data-driven-tests/blob/master/Cdroulers.Business.Tests.Unit/Data/prime-factors.tsv)
In our use-case it was easy for our Product Owner to open up our sample
file and fill it up with dozens of test cases. Edge cases, large numbers, etc. When we found bugs, we would simply add
a new line in the file. We added a new feature and simply added a new column in the TSV file. All of that shows up
properly in a diff so it's easy to see the data that was added along with a bug fix or a new feature.

### Reading the TSV file

NUnit is a pretty great framework. It has great stuff like `[TestCase()]` attributes for quickly testing the same code
with different input and output. It also has `[TestCaseSource()]` which is exactly what we need for running tests
according to our TSV file.

`[TestCaseSource("FunctionName")]` will, at test discovery time, execute the static method in the parameters and
create a test for each value. The function signature must 
[look like the following](https://github.com/cdroulers/nunit-data-driven-tests/blob/master/Cdroulers.Business.Tests.Unit/GivenPrimeFactorCalculator.cs#L46):

{% highlight csharp %}
private static IEnumerable<object> FunctionName()
{
    return Enumerable.Empty<object>();
}
{% endhighlight %}

[The full test method using this looks like](https://github.com/cdroulers/nunit-data-driven-tests/blob/master/Cdroulers.Business.Tests.Unit/GivenPrimeFactorCalculator.cs#L39):

{% highlight csharp %}
[TestCaseSource("FunctionName")]
public void TestName(object value)
{
    var actual = // Do something with value
    Assert.That(actual, Is.EqualTo(3)); // Assert whatever you want.
}
{% endhighlight %}

The static method's return type must be IEnumerable, but the contents can be any class. Therefore in our case, I created
a simple class that contained each input and output as properties and read them from the TSV.

I created [a full-blown sample](https://github.com/cdroulers/nunit-data-driven-tests) with Visual Studio 2013 available
for free! Each commit represents a step towards the end goal with a simple domain of calculating prime factors.

In that sample, reading the TSV is done using very simple string parsing, which is a pretty bad idea in general,
especially with a larger sample size than 2 columns like my sample. Using a proper CSV parsing library such as
[CsvHelper](https://github.com/JoshClose/CsvHelper) is a much better idea. But that's another blog post!