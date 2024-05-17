---
layout: default
title: "HTTPS everywhere!"
bodyclass: blog-article
---

What's this!? Your trafic to this site is now encrypted!

<!-- more -->

No one can know your dirty secrets except me!

Because of [Let's Encrypt](https://letsencrypt.org/) and [Hostineer](http://apisnetworks.com/r?cdroulers),
I now have a fully secure HTTPS website! And it was super easy.

[Let's Encrypt](https://letsencrypt.org/about/) is a recently created Open Certificate Authority
that allows any individual to create and install SSL/TLS certificates on their websites. The certificates
are valid for 90 days and there is even [a tool (CertBot)](https://certbot.eff.org/) that can automate renewal
regularly. Therfore, there is now no reason NOT to serve your website over HTTPS!

[Hostineer](http://apisnetworks.com/r?cdroulers) is my host since 2008 and has been great. It's not very
expensive, it has a lot of features (SSH access, sub-domains, etc.) with the basic package and the service
is fast. The infrastructure is regularly updated as well. They also have a feature to enable SSL via
Let's Encrypt automatically via a GUI instead of manually setting up CertBot. Easy and efficient.

That means that [Block Fly](https://apps.cdroulers.com/block-fly) will soon be fully offline with
service workers since they require HTTPS. Yayyyyyyy!