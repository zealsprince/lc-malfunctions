#!/usr/bin/env bash
# Launches the Lethal Company dev install under Proton.
# Usage: scripts/launch.sh [instances], e.g. scripts/launch.sh 2 for a LAN host and a client.
set -euo pipefail

instances="${1:-1}"
game_dir="${LethalCompanyDir:-/mnt/Storage/Windows/Lethal Company IDE}"
prefix_root="${LC_PREFIX_ROOT:-$HOME/.local/share/lethal-company-dev}"

# BepInEx loads through doorstop's winhttp.dll proxy, and Wine uses its own builtin unless told otherwise.
export WINEDLLOVERRIDES="winhttp=n,b"
export GAMEID="umu-1966720"
export STORE="none"

if [[ -z "${PROTONPATH:-}" ]]; then
  ge="$(ls -d "$HOME"/.local/share/Steam/compatibilitytools.d/GE-Proton* 2>/dev/null | sort -V | tail -1 || true)"
  export PROTONPATH="${ge:-GE-Proton}"
fi

# One prefix per instance, so the host and client never share a wineserver or a save folder.
for i in $(seq 1 "$instances"); do
  mkdir -p "$prefix_root/instance-$i"

  WINEPREFIX="$prefix_root/instance-$i" umu-run "$game_dir/Lethal Company.exe" \
    -screen-fullscreen 0 -screen-width 1280 -screen-height 720 &

  sleep 3
done

wait
