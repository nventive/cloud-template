# Placeholder - Backend

{Project tag line}

{Small description of the purpose of the project}

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

The main prerequisites for a developer setup are:
- either Linux (including WSL) or macOS as OS;
- the Nix package manager with flakes enabled;
- direnv and nix-direnv;
- Docker;
- Visual Studio Code with the C# DevKit extension for debugging;
- an Azure account to deploy and run in the cloud.

Precise installations instructions are as follow:

### For WSL Users (Windows)
- If you’re using Docker Desktop, remember to [enable it in your WSL distro](https://docs.docker.com/desktop/wsl/#enabling-docker-support-in-wsl-2-distros)
- Active WSL extension in VS Code.
- The ASP.NET Core developement certificate installed in WSL must be trusted in your Windows browser.

Then you need to trust the ASP.NET Core development certificate.

1. Install the .NET SDK in Windows [whichever way you want](https://learn.microsoft.com/en-us/dotnet/core/install/windows).
1. Install and activate the ASP.NET Core development certificate (`[password]` being any password you’ll remember in the next minute).
In Windows terminal:

```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
dotnet dev-certs https -ep https.pfx -p [password]
```
If you get a message saying "A valid HTTPS certificate is already present.", it's not an error (your command has succeeded).

3. Restart your browser to make sure it trusts the new certificate.
4. Open a WSL terminal to import the certificate and trust it for inter-service communications (`[path]` being the path to the windows directory you executed the previous commands after `C:`, and `[password]` being the password entered previously. Also, replace `X.0` with the major .NET SDK version you want to install, for example `dotnet-sdk-9.0`):

```bash
sudo apt update
sudo apt install dotnet-sdk-X.0
dotnet dev-certs https --clean --import /mnt/c/[path]/https.pfx --password [password]
sudo mkdir /usr/local/share/ca-certificates/aspnet/
sudo -E dotnet dev-certs https -ep /usr/local/share/ca -certificates/aspnet/https.crt --format PEM 
sudo update-ca-certificates
```

NOTE: If you get an error when installing the second command (`sudo apt install dotnet-sdk-X.0`), try the following:

```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.de
rm packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-host
sudo apt-get install -y dotnet-sdk-X.0
```


### For macOS and straight Linux users

ASP.NET Core development certificate is more simple.

1. [Install a .NET SDK](https://learn.microsoft.com/en-us/dotnet/core/install/).
2. Install and activate the ASP.NET Core development certificate
```console
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### For all OSes: install Nix, direnv

* All the following is done in WSL for windows users
Use the [Determinate Nix installer](https://github.com/DeterminateSystems/nix-installer):
```console
curl --proto '=https' --tlsv1.2 -sSf -L https://install.determinate.systems/nix | sh -s -- install
```

Follow any instructions, including those necessary to get nix in path.

1. Install direnv.
```console
nix profile add nixpkgs#direnv
```

If running on zsh and getting a `no match found nixpkgs#direnv` error do the following and retry installing direnv :
```console
unsetopt extendedGlob
```

If you get `No such file or directory` error do the following and retry installing direnv :
```console
sudo mkdir /nix/var/nix/profiles/per-user/username
sudo chown username /nix/var/nix/profiles/per-user/username
```

2. [Hook direnv into your shell](https://direnv.net/docs/hook.html).

> For bash this means adding the following line in your `~/.bashrc`:
> ```bash
> eval "$(direnv hook bash)"
> ```

> For zsh (macOS), the file is `~/.zshrc` and the line is
> ```zsh
> eval "$(direnv hook zsh)"
> ```

3. Don’t forget to reload your shell
> ```console
> source ~/.bashrc
> ```
> 
> or
> ```console
> source ~/.zshrc
> ```

4. Install nix-direnv in your user Nix profile and hook it up to direnv.
> ```console
> nix profile add nixpkgs#nix-direnv
> source $HOME/.nix-profile/share/nix-direnv/direnvrc
> ```

### Start for real

1. Get this repository’s content

You need to configure the gitconfig to have correct credentials and setup to use git.
- Mount the windows git credential manager into your wsl :
``` git config --global credential.helper "/mnt/c/Program\ Files/Git/mingw64/bin/git-credential-manager.exe" ```
- configure http :
``` git config --global credential.https://dev.azure.com.useHttpPath true ```
- add name and email to be able to push :
``` git config --global user.name=[YourAccountName]```
``` git config --global user.email=[YourEmailAccountName]```

```console
git clone [ProjectRepository]
```



#### Build and Test
*Note : Project config expires so if you're facing issues close the solution and reopen it using `just code`

- Launch docker desktop
 Start VS Code. NB: it has to be started this way to ensure the correct .NET frawework is found and debugging works.
```console
just code
```

- Install C# Dev Kit extension in VS Code
In command palette (Mac: _command+shift+p_, windows: _ctrl+shift+p_) :
- .NET: Clean
- .NET: Build
- Run > Start Debugging > C# > Placeholder.AppHost [Default]

*If you're running into an exception on migration just press **Continue** until everything is running, then wait until the database containers are ready and press **Play** ▶︎ on migration container.

Later it can be run from *run & debug* tab.

To get a list of other recipes:
```console
just --list
```

## Features

{More details/listing of features of the project}

## Migrations
The following recipe can be used to create new migrations based on template :
```console
just create-migration **migration-name**
```

## Dependencies upgrade
There is a justfile recipe to upgrade minor versions of dependencies:
```console
just upgrade
```

It is recommended to then relaunch Visual Studio Code (`just code`). Changes can then be tested and committed to git.


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
