{ pkgs ? import <nixpkgs> { } }:

pkgs.mkShell {
  packages = with pkgs; [
    dotnetCorePackages.sdk_9_0
    sqlite
    postgresql_17
    nginx
  ];
}
