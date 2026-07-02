#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
dotnet build "$ROOT_DIR/src/Emby.UserTV.Plugin/Emby.UserTV.Plugin.csproj" -c Release
