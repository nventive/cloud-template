# Open Source Project Template

This repository contains a template to seed a repository for an Open Source
project.

## How to use this template

1. Check out this repository
2. Delete the `.git` folder
3. Git init this repository and start working on your project!
4. Prior to submitting your request for publication, make sure to review the
   [Open Source guidelines for publications](https://nventive.visualstudio.com/Internal/_wiki/wikis/Internal_wiki?wikiVersion=GBwikiMaster&pagePath=%2FOpen%20Source%2FPublishing&pageId=7120).

## Features (to keep as-is, configure or remove)
- [Mergify](https://mergify.io/) is configured. You can edit or remove [.mergify.yml](/.mergify.yml).

The following is the template for the final README.md file:

---

# Project Title

{Project tag line}

{Small description of the purpose of the project}

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

The main prerequisites for a developer setup are:
- either Linux (including WSL) or macOS as OS;
- the Nix package manager with flakes enabled;
- [direnv](https://direnv.net/) and [nix-direnv](https://github.com/nix-community/nix-direnv);
- Docker;
- Visual Studio Code with the C# DevKit extension for debugging;
- an Azure account to deploy and run in the cloud.

Things you need to make sure with WSL:
- If you’re using Docker Desktop, remember to [enable it in your WSL distro](https://docs.docker.com/desktop/wsl/#enabling-docker-support-in-wsl-2-distros)
- VS Code extensions must be activated in WSL.
- The ASP.NET Core developement certificate installed in WSL must be trusted in your Windows browser.

### Steps to trust the ASP.NET Core development certificate used in WSL
1. Install the .NET SDK in Windows [whichever way you want](https://learn.microsoft.com/en-us/dotnet/core/install/windows).
1. Install and activate the ASP.NET Core development certificate (`[password]` being any password you’ll remember in the next minute). In Windows:
```console
$ dotnet dev-certs https --clean
$ dotnet dev-certs https --trust
$ dotnet dev-certs https -ep https.pfx -p [password]
```
3. Restart your browser to make sure it trusts the new certificate.
3. Import the certificate in WSL and trust it for inter-service communications (`[path]` being the path to the windows directory you executed the previous commands, and `[password]` being the password entered previously):
```console
$ sudo apt install dotnet-sdk-8.0
$ dotnet dev-certs https --clean --import /mnt/c/[path]/https.pfx --password [password]
$ sudo -E dotnet dev-certs https -ep /usr/local/share/ca-certificates/aspnet/https.crt --format PEM
$ sudo update-ca-certificates
```

### Steps to install Nix, direnv

Use the [Determinate Nix installer](https://github.com/DeterminateSystems/nix-installer):
```console
$ curl --proto '=https' --tlsv1.2 -sSf -L https://install.determinate.systems/nix | sh -s -- install
```

For direnv, the best way would be [via home-manager](https://github.com/nix-community/nix-direnv?tab=readme-ov-file#via-home-manager), if you have [home-manager](https://nix-community.github.io/home-manager/).

Otherwise:
1. Install direnv (tested with Ubuntu 24.04 in WSL).
```console
$ sudo apt install direnv
```
2. [Hook direnv into your shell](https://direnv.net/docs/hook.html). For bash this means adding the following line in your `~/.bashrc`:
```bash
eval "$(direnv hook bash)"
```
Don’t forget to reload your bash.
```console
$ source ~/.bashrc
```

4. Install nix-direnv in your user Nix profile and hook it up to direnv.
```console
$ nix profile install nixpkgs#nix-direnv
$ source $HOME/.nix-profile/share/nix-direnv/direnvrc
```

### Start for real

1. Get this repository’s content
```console
$ git checkout https://github.com/nventive/cloud-template.git
```

2. Copy the template to wherever you want to have it and enable the development shell. **Warning:** the last step may take a while, as it’s downloading/building all tools and dependencies.
```console
$ cp -r cloud-template/template MyProject
$ cd MyProject
$ direnv allow
```

3. Install the Aspire .NET workload. *TODO: [use local install](https://discourse.nixos.org/t/dotnet-maui-workload/20370/13) as soon as .NET SDK 8.0.40x is in nixpkgs*
```console
$ sudo `which dotnet` workload install aspire 
```

4. Rename the project according to your inspirations (`Placeholder` is the actual string to be replaced).
```console
$ fd Placeholder | tac | xargs rnm -rs '/Placeholder/MyProject/g' -y
$ fd --hidden --type file --exec sd Placeholder MyProject
```

5. Start VS Code. NB: it has to be started this way to ensure the correct .NET frawework is found and debugging works.
```console
$ ./start-code.sh
```

Further instructions are in the `README` of your new project.

## Features

{More details/listing of features of the project}

## Breaking Changes

Please consult [BREAKING_CHANGES.md](BREAKING_CHANGES.md) for more information about version
history and compatibility.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).
