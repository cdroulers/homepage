- The Big Rewrite: a failure story
- Chutzpah code coverage!
- Selenium and Specflow and whatever.
- Managing legacy database with FluentMigrator (backup, 7z self-extract, etc.)
- SpecFlow + Selenium on legacy code-base
- webapi.nhibernate-odata, substringof, etc.
- Red, green, refactor in action (especially with legacy code!)
- SQL Server XML field and MVP and agile methods (do the minimum thinger first!)

- WCF service inside MVC app:
http://stackoverflow.com/questions/1752431/wcf-web-config-on-godaddy-medtium-trust-hosting-environment-help-please
<add name="WcfServiceHandler" verb="*" path="*.svc" type="System.ServiceModel.Activation.HttpHandler, System.ServiceModel.Activation, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />
IPortalService

            routes.IgnoreRoute("{resource}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.svc");

See Portal.sln for IPortalService, including configuration.



- Search service with SQL Server full text search.