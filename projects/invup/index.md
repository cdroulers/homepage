---
layout: default
title: "Projects : Invup"
---

## [Invup](http://invup.com/) (Became [Goodify](http://goodify.com/), which is still running) ##

### Code links ###
* [BitBucket repository](https://bitbucket.org/cdroulers/invup)

### Reddit threads ###
* [/r/programming](http://www.reddit.com/r/programming/comments/19lr03/my_friend_and_i_failed_at_doing_a_startup_so/)
* [/r/dotnet](http://www.reddit.com/r/dotnet/comments/19lkax/my_friend_and_i_failed_at_doing_a_startup_so)

### The product ###
Invup is a startup that aimed at creating a social platform for organizations (companies, fraternities, schools) to track and quantify their philanthropy.

By bringing game dynamics into a good hours and donations tracking software, we thought we could get people &emdash; employees and employers alike &emdash; to join forces for the greater good.
The plan was to allow users to join teams, track their good deeds, see statistics, dashboards and whatnot. Everything to show them they could make a difference and motivate them to give more.

### The team ###
There were four of us to start. Myself and a programmer friend, as well as two business majors. We had a fairly set plan at the start, along with mockups.

We were part of [MassChallenge](http://masschallenge.org/) in Boston and we started developing the product and a client list.

### The failures ###
The biggest failure was trust. As programmers, we saw the project as mostly software and the rest would obviously be easy.
As business devs, the others assumed we could produce faster results than we could.
Therefore, anytime there were discussions on what to do, when to do it and how to it, clashes would occur and not much would end up being done.
I think ego was a big part of it. I wanted to give my input more than I was ready to receive it from other people.

Direction was another problem. After a few months, we had a basic little platform that could have been used by a few beta customers, but our business devs kept finding new clients with different needs.
Everytime, they wanted us to start working on new features for these customers. I think that made us end up with a half-built software that wasn't ready for anyone in particular.

We had personal problems between team members that weren't addressed until it was too late.
We were afraid of hurting someone's feelings and that had a huge impact on the quality of our work and the environment we were in.
I personally started being afraid of having meetings for fear of ending up arguing over trite things. I ended up working from home in Québec instead of staying in Boston with the rest of the team.

### The successes ###
The experience of being in the startup world in Boston was great. I had no idea what I was doing or where we were going with the startup.
I met tons of great people, learned about better ways to run startups, how important your team is and a lot of programming techniques and pitfalls.

### The stack ###
* C# 4
* asp.net MVC 3
* [PostgreSQL](http://www.postgresql.org/) ([SQLite](http://www.sqlite.org/) for local development and testing)
* [NHibernate](http://nhforge.org/)
* [DotNetMigrations](https://github.com/jpoehls/dotnetmigrations)
* [Solr](http://lucene.apache.org/solr/) via [SolrNet](https://code.google.com/p/solrnet/)
* [Mercurial](http://mercurial.selenic.com/) on [BitBucket](https://bitbucket.org/) for code hosting.
* [NUnit](http://www.nunit.org/) for testing.

### Questions / Answers ###

<dl>
    <dt><a href="http://www.reddit.com/r/programming/comments/19lr03/my_friend_and_i_failed_at_doing_a_startup_so/c8p8m0o">Why use C#</a></dt>
    <dd>
        C# was our first choice because we had a lot of experience with it. Neither of us were comfortable enough with other languages or platforms to begin a new project that required speed of development.
        We also think C# is a great language on its own. We also tried to run the project on Mono and Linux, but we had compatibility problems with external libraries (DotNetOpenAuth). So we stayed on .NET and IIS.
    </dd>
</dl>
