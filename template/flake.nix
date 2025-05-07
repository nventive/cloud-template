{
  description = "Development environment for the Placeholder project";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixpkgs-unstable";
  };

  outputs = { self, nixpkgs }:
    let

      # Helpers for producing system-specific outputs
      supportedSystems = [ "x86_64-linux" "aarch64-darwin" "x86_64-darwin" "aarch64-linux" ];
      forEachSupportedSystem = f: nixpkgs.lib.genAttrs supportedSystems (system: f {
        pkgs = import nixpkgs {
          inherit system;
          config = { allowUnfree = true; };
          };
      });
    in {

      # Development environments
      devShells = forEachSupportedSystem ({ pkgs }: {
        default =
          let dotnetCombined = (with pkgs.dotnetCorePackages; combinePackages [
            sdk_8_0-bin
            sdk_9_0-bin
          ]);
          in pkgs.mkShell {
          
          # Pinned packages available in the environment
          packages = with pkgs; [
            coreutils
            curl
            dotnetCombined
            dotnet-outdated
            just
            nil
            nixpkgs-fmt
            nodejs_22
            typescript
            typescript-language-server
            yarn-berry_4
          ];

          DOTNET_ROOT = "${dotnetCombined}/share/dotnet";
        };
      });
    };
}
