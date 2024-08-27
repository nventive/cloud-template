{
  description = "Development environment for the Placeholder project";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/master";
  };

  outputs = { self, nixpkgs }:
    let
      overlays = [
        (final: prev: rec {
          nodejs = prev.nodejs_20; # Use Node.js 20
          yarn = (prev.yarn.override { inherit nodejs; });
        })
      ];

      # Helpers for producing system-specific outputs
      supportedSystems = [ "x86_64-linux" "aarch64-darwin" "x86_64-darwin" "aarch64-linux" ];
      forEachSupportedSystem = f: nixpkgs.lib.genAttrs supportedSystems (system: f {
        pkgs = import nixpkgs {
          inherit overlays system;
          config = { allowUnfree = true; };
          };
      });
    in {
      # Development environments
      devShells = forEachSupportedSystem ({ pkgs }: {
        default =
          let
            dotnet-local-workloads = (with pkgs.dotnetCorePackages; combinePackages [sdk_8_0])
              .overrideAttrs (finalAttrs: previousAttrs: {
                # This is needed to install workload in $HOME
                # https://discourse.nixos.org/t/dotnet-maui-workload/20370/2
                postBuild = (previousAttrs.postBuild or '''') + ''
                  for i in $out/sdk/*
                  do
                    i=$(basename $i)
                    length=$(printf "%s" "$i" | wc -c)
                    substring=$(printf "%s" "$i" | cut -c 1-$(expr $length - 2))
                    i="$substring""00"
                    mkdir -p $out/metadata/workloads/''${i/-*}
                    touch $out/metadata/workloads/''${i/-*}/userlocal
                  done
                '';
              });
          in pkgs.mkShell {
          
            # Pinned packages available in the environment
            packages = with pkgs; [
              (azure-cli.withExtensions [
                azure-cli.extensions.ad
                azure-cli.extensions.azure-devops
                azure-cli.extensions.containerapp
              ])
              coreutils
              dotnet-local-workloads
              fd
              nil
              nixpkgs-fmt
              node2nix
              nodejs
              pnpm
              rnm
              sd
              terraform
              yarn
            ];

            DOTNET_ROOT = "${dotnet-local-workloads}";
          };
      });
    };
}
