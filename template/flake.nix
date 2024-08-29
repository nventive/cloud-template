{
  description = "Development environment for the Placeholder project";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixpkgs-unstable";
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
        default = pkgs.mkShell {
          
          # Pinned packages available in the environment
          packages = with pkgs; [
            (azure-cli.withExtensions [
              azure-cli.extensions.ad
              azure-cli.extensions.azure-devops
              azure-cli.extensions.containerapp
            ])
            coreutils
            curl
            fd
            just
            nil
            nixpkgs-fmt
            node2nix
            nodejs
            pnpm
            rnm
            sd
            terraform
            wget
            yarn
          ];
        };
      });
    };
}
