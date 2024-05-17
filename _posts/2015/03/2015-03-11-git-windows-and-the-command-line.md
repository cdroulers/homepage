---
layout: default
title: "Git, Windows, private keys and the command line"
bodyclass: blog-article
---

Getting Git to work on Windows is starting to get easier. Especially with GUIs like [SourceTree](http://www.sourcetreeapp.com/) or [TortoiseGit](https://code.google.com/p/tortoisegit/).

<!-- more -->

For hardcore people like me, a command line interface is almost necessary and that's where it gets trickier. When installing git, you get the command line interface and most features
work. But when you want to use public/private key pairs (awesome features usually supported by the likes of [GitHub](https://github.com/) or [BitBucket](https://bitbucket.org/)), the basic
installation doesn't work. Git tries to use ssh.exe, which tries to load keys via ssh-agent, which doesn't work on Windows.

So [putty](http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html) comes to the rescue!

plink.exe and pageant.exe are required for this setup.

The first step is to tell git to use plink.exe for SSH. It needs an environment variable named `GIT_SSH`

The second step is to load your keys with pageant.exe before calling your commands.

I personally modified my PowerShell profile to do the following:

{% highlight powershell %}
"Setting home path"
(Get-PSProvider "FileSystem").Home = $env:USERPROFILE;

where.exe /Q plink.exe
if ($LASTEXITCODE -eq 0)
{
    # Add the path to plink and pageant to the path.
    "Found plink.exe, setting `$env:GIT_SSH"
    $env:GIT_SSH = "plink.exe";
}

where.exe /Q pageant.exe
if ($LASTEXITCODE -eq 0)
{
    $privateKeys = [string]::Join(" ", (ls ~/.ssh/*.ppk).FullName)
    "Found pageant.exe, loading default keys ({0})" -f $privateKeys;
    iex "pageant.exe $privateKeys"
}
""
{% endhighlight %}

The first part is to make sure that `~` resolves to my home directory in PowerShell.

The second is to check if plink.exe is available on the `PATH` and then update my environment variable to point to it.

The last step is to list all private keys in my `.ssh` folder and tell pageant.exe to load them.

Since I start PowerShell automatically anytime I start my computer, it asks for the private key passwords and then any other tab I open doesn't ask again.

This can also be set up the regular way, by modifying your environment variables directly and setting up a startup script for your Windows user account.

After that, `git fetch`, `git pull` and `git push` will start working like it's no big deal!