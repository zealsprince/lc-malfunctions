{
  description = "Lethal Company mod development shell";

  inputs.nixpkgs.url = "github:nixos/nixpkgs/nixos-26.05";

  outputs =
    { nixpkgs, ... }:
    let
      systems = [
        "aarch64-darwin"
        "x86_64-darwin"
        "aarch64-linux"
        "x86_64-linux"
      ];

      forAllSystems = f: nixpkgs.lib.genAttrs systems (system: f nixpkgs.legacyPackages.${system});
    in
    {
      devShells = forAllSystems (pkgs: {
        default = pkgs.mkShell {
          packages =
            [
              pkgs.dotnet-sdk_9
              pkgs.ilspycmd
            ]
            ++ pkgs.lib.optionals pkgs.stdenv.isLinux [ pkgs.avalonia-ilspy ]
            # The game is an x86_64 Windows build that only runs here through Proton.
            ++ pkgs.lib.optionals (pkgs.stdenv.isLinux && pkgs.stdenv.isx86_64) [ pkgs.umu-launcher ];

          DOTNET_ROOT = "${pkgs.dotnet-sdk_9}/share/dotnet";
          DOTNET_CLI_TELEMETRY_OPTOUT = "1";
        };
      });
    };
}
