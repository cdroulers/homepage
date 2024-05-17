---
layout: default
title: "Fluent NHibernate and Persistence Specification"
bodyclass: blog-article
---

When creating mappings for a Domain with [NHibernate](http://nhibernate.info/) and [Fluent NHibernate](http://www.fluentnhibernate.org/), it's quite important to test
them. Sometimes they'll fall out of sync after a refactor, sometimes someone else will change a tiny mapping thinking everything will work but breaks everything.

<!-- more -->

To easily test mappings, I use [FluentNHibernate.Testing](https://github.com/jagregory/fluent-nhibernate/wiki/Persistence-specification-testing).

The actual best reason to write specification is the first time you write a mapping. It gives you a guarantee that you mapped everything properly and gives an easy
way to repeat the test instead of launching your code and trying to go through the code you want to test.

A basic test with a parent/child relationship looks like this:

{% highlight csharp %}
[TestFixture]
[Category("Integration")]
public class DealerMapTest
{
    private ISession _session;

    [TestFixtureSetUp]
    public void Initialize()
    {
        var registry = new Container(new DealersRegistry());
        _session = registry.GetInstance<ISession>();
    }

    [Test]
    public void CanCorrectlyMapDealer()
    {
        using (var transaction = _session.BeginTransaction())
        {
            var addresses = new List<Address>()
                                {
                                    new Address()
                                        {
                                            Address1 = "address",
                                            City = "city",
                                            PostalCode = "sldkjf",
                                            ProvinceCode = "QC"
                                        }
                                };
            new PersistenceSpecification<Dealer>(_session)
                .CheckProperty(x => x.Id, "asdf123")
                .CheckProperty(x => x.LegalName, "Dealer legal name 1")
                .CheckProperty(x => x.NEQ, "q1112345QWET")
                .CheckInverseList(x => x.Addresses, addresses, (d, a) => { d.AddAddress(a); })
                .VerifyTheMappings();

            transaction.Rollback();
        }
    }
}
{% endhighlight %}

This quick test will create a <code>Dealer</code> with the <code>Id</code>, <code>LegalName</code> and <code>NEQ</code> properties filled. It will also create an address.
It will then save everything to the database and retrieve it to compare it. As the class expands with new features, this specification can be updated to test new properties,
new references and new lists. So you always have a guarantee that your mapping works properly!

It even works with private lists since <code>x.Addresses</code> is a read-only collection mapped with Fluent Migrator's <code>.Access.CamelCaseField()</code>