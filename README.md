# Open Source Project Template

This repository contains a template to seed a repository for an Open Source
project.

Prior to submitting your request for publication, make sure to review the
   [Open Source guidelines for publications](https://nventive.visualstudio.com/Internal/_wiki/wikis/Internal_wiki?wikiVersion=GBwikiMaster&pagePath=%2FOpen%20Source%2FPublishing&pageId=7120).

## Features (to keep as-is, configure or remove)
- [Mergify](https://mergify.io/) is configured. You can edit or remove [.mergify.yml](/.mergify.yml).

The following is the template for the final README.md file:

### Usage 

#### Install template from nuget.org

- execute : ``` dotnet new install <NUGET_PACKAGE_ID> ```

#### Install template from source
 Why ?  to do an end to end check of the nuget package generation , codnifrm naming , version etc .. . 

- git clone the template project
- go to root folder
- execute ``` dotnet pack ```
- execute ```dotnet new install  ./bin/release/nventive.Cloud.Template.1.0.0.nupkg```

#### Create new project

- execute ``` dotnet new nv-aspire -n MyApp ```
- follow the project readme.md