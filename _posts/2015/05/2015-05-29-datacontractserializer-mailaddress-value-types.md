---
layout: default
title: "DataContractSerializer, MailAddress and Value Types"
bodyclass: blog-article
---

In our quest to be fast, we opted to stick a bunch of data inside an XML field in SQL Server with NHibernate.

<!-- more -->

The feature doesn't have a lot of validation or business logic, so we opted to create a few columns for queryable data and an XML field for the rest. Our
domain object looked just as it would with a full SQL schema, but we told [NHibernate](http://nhibernate.info/) to serialize the object to XML when saving it.

### The mapping

{% highlight csharp %}
    Map(x => x.Data).CustomType<XmlType<AnObjectWeWantedInXml>>(); 
{% endhighlight %}

### The XmlType

It's available [on github](https://gist.github.com/cdroulers/6724d31b65f4060aaa6a).

### Reasoning

We're still not sure what the client needs for this interface to be complete, and it's also prone to changing because of business processes and products.

This is why we opted for an XML field. It means we can easily add new properties to the `Data` object and it will get serialized automatically. Using
[`DataContractSerializer`](https://msdn.microsoft.com/en-us/library/ms731072(v=vs.110).aspx) means we can easily get backwards compatibility because we can set
default values.

### MailAddress and Value Types

We had a `MailAddress` property somwhere in that data and it just wouldn't serialize. That was a bummer at first because we like using semantic value types. We
also had a few other types that had no default constructors that would fail to serialize. After looking into it, I found a decent solution: 
[`IDataContractSurrogate`](https://msdn.microsoft.com/en-us/library/ms733064(v=vs.110).aspx).

`IDataContractSurrogate` let's you define code that will transform the objects during serialization and deserialization. This means that when we encounter a `MailAddress`,
we can return a surrogate class that is serializable. Then, when reading the data, we do the opposite operation.

{% highlight csharp %}
public class XmlDataContractSurrogate : IDataContractSurrogate
{
    public Type GetDataContractType(Type type)
    {
        if (type == typeof(MailAddress))
        {
            return typeof(XmlMailAddressSurrogate);
        }

        if (type == typeof(Money))
        {
            return typeof(XmlMoneySurrogate);
        }

        return type;
    }

    public object GetObjectToSerialize(object obj, Type targetType)
    {
        var address = obj as MailAddress;
        if (address != null && targetType == typeof(XmlMailAddressSurrogate))
        {
            return new XmlMailAddressSurrogate()
            {
                Address = address.Address,
                DisplayName = address.DisplayName 
            };
        }

        if (obj is Money && targetType == typeof(XmlMoneySurrogate))
        {
            return new XmlMoneySurrogate() { Value = ((Money)obj).Value };
        }

        return obj;
    }

    public object GetDeserializedObject(object obj, Type targetType)
    {
        var address = obj as XmlMailAddressSurrogate;
        if (address != null && targetType == typeof(MailAddress))
        {
            return new MailAddress(address.Address, address.DisplayName);
        }

        var money = obj as XmlMoneySurrogate;
        if (money != null && targetType == typeof(Money))
        {
            return new Money(money.Value);
        }

        var array = obj as Array;
        if (array != null &&
            targetType.IsGenericType &&
            (targetType.GetGenericTypeDefinition() == typeof(ICollection<>) || 
            targetType.GetGenericTypeDefinition() == typeof(IList<>)))
        {
            var elementType = targetType.GetGenericArguments()[0];

            var listType = typeof(List<>).MakeGenericType(elementType);
            var result = (IList)Activator.CreateInstance(listType);

            foreach (var item in array)
            {
                result.Add(item);
            }

            return result;
        }

        return obj;
    }

    public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
    {
        return null;
    }

    public object GetCustomDataToExport(Type clrType, Type dataContractType)
    {
        return null;
    }

    public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
    {
    }

    public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
    {
        return Type.GetType(typeNamespace + "." + typeName);
    }

    public CodeTypeDeclaration ProcessImportedType(
        CodeTypeDeclaration typeDeclaration, 
        CodeCompileUnit compileUnit)
    {
        return typeDeclaration;
    }
}
{% endhighlight %}

The code for `Array` in `GetDeserializedObject` is because we used `ICollection<T>` in our data object, which would be deserialized in a `T[]`. This caused problems
later on with [AutoMapper](http://automapper.org/) because it tried to `Clear()` the collection, which was read-only. It also makes more sense to put an actual
collection inside an `ICollection<T>`.

The actual `XmlMailAddressSurrogate` is a very simple POCO object:

{% highlight csharp %}
public class XmlMailAddressSurrogate
{
    public string Address { get; set; }

    public string DisplayName { get; set; }
}
{% endhighlight %}

With all this, we can develop our REST api faster while keeping a modicum of structure in our data. SQL Server's XML field is fully queryable. If we need to extract some data
from the XML into new columns, we can do that with a normal `UPDATE` statement.

SQL Server Management Studio also makes it easy to inspect and modify the XML data.