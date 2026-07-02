#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="$ROOT_DIR/src/Emby.UserTV.Plugin/Emby.UserTV.Plugin.csproj"
OUTPUT_DIR="$ROOT_DIR/artifacts/plugin"

dotnet publish "$PROJECT" -c Release -o "$OUTPUT_DIR"
find "$OUTPUT_DIR" -type f ! -name 'Emby.UserTV.Plugin.dll' ! -name 'System.Memory.dll' -delete

echo "Plugin package files are in: $OUTPUT_DIR"
