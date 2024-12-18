# Open Source Project Template

This repository contains a template to seed a repository for an Aspire Open Source
project.

Prior to submitting your request for publication, make sure to review the
   [Open Source guidelines for publications](https://nventive.visualstudio.com/Internal/_wiki/wikis/Internal_wiki?wikiVersion=GBwikiMaster&pagePath=%2FOpen%20Source%2FPublishing&pageId=7120).

## Features (to keep as-is, configure or remove)
- [Mergify](https://mergify.io/) is configured. You can edit or remove [.mergify.yml](/.mergify.yml).

The following is the template for the final README.md file:

### Usage 

#### Install template from nuget.org (Not yet available)

- execute : ``` dotnet new install <NUGET_PACKAGE_ID> ```

#### Install template from source

- git clone the template project
- go to root folder

If you want to install the template from source directly :

- execute ``` dotnet new install .\template\   ```
- you can uninstall with the command  ``` dotnet new uninstall .\template\   ```

If you want to install the template from local nuget (just to confirm the end to end creation):
- execute ``` dotnet pack ```
- execute ```dotnet new install  ./bin/release/nventive.Template.1.0.0.nupkg```
- uninstall command :  ```dotnet new uninstall nventive.Templates.Aspire```


#### Create new project

- execute ``` dotnet new nv-aspire -n MyApp ```
- follow the project readme.md

#### Parameters
- appInsights 