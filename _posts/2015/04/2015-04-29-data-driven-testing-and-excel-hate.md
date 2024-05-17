---
layout: default
title: "Data driven testing and Excel hate"
bodyclass: blog-article
---

Testing business rules that have a lot of input, output and permutations is not a fun job. Getting the data in a decent, readable, diffable format is hard as well.

<!-- more -->

We have a business rule calculation that takes a lot of parameters in and has a few out as well. There's a lot of permutations possible too. The data we need for these
tests comes from a few sources: an older system, manual testing and product owner knowledge.

Hand-coding all these values in `[TestCase]` attributes was starting to get a little heavy and non-manageable anytime a new parameter needed to be added. At first, we
opted for an Excel Spreadsheet read with [ExcelDataReader](https://www.nuget.org/packages/ExcelDataReader/). XLSX is a zip format with XML inside it. So in git, we had no idea
what was happening anytime someone changed things. Merges were impossible (even with Beyond Compare, it was far from usable).

Today, I looked for a text-based format that would be usable by all developers, architects, product owners and etc. Seems like 
[CSV](https://en.wikipedia.org/wiki/Comma-separated_values) is still horribly broken in Excel, even in 2015.

* Delimiter is set according to Windows' locale settings;
* No way to override it in both read and write;
* Encoding support doesn't work well.

Exporting our xlsx to csv just wouldn't work at all. If I exported with commas, it wouldn't read it. If I exported with semi-colons, it would read it, but not as UTF-8. If
I added the BOM, it would start ignoring semi-colons altogether. LibreOffice works perfectly with CSVs. When you open one, it pops a small window of settings with educated
guesses. Encoding, separator and a preview. MAGIC!

The final format we settled on is "TSV", which isn't really a thing, but works well in LibreOffice, Excel and plain text. It's simply tab-separated values. I used LibreOffice
to export the XLSX's sheets as CSV, then put a tab as the separator. The result opens just fine in LibreOffice (obviously!) and opens ok in Excel. Any character in the UTF-8
range (mostly French accented characters in headers) show up as separate characters (two bytes each), but as long as we don't touch those in Excel, everything is fine.

TSV is easy to read and diff. It will also be possible to easily send it to a product owner for new test cases and then merge it back in since it's text!

<script src="https://gist.github.com/cdroulers/1a919d7f9ce701a716b0.js"></script>

You can't see the tabs visually, but any proper text editor can show them.

Once that is done, I simply use [CsvHelper](https://github.com/JoshClose/CsvHelper) to read it in with header support. See 
[Fluent class mapping](https://github.com/JoshClose/CsvHelper/wiki/Fluent-Class-Mapping) for a good sample.

With this data read, we can generate an NUnit `[TestCaseSource]` and have a single test method that goes through ALL of the data in the file.

Whenever you find a bug or a new case arises, simply modify the test data file (TSV) and the test should start running in your suite.