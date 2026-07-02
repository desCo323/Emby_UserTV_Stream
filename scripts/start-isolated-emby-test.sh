#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
TEST_ROOT="${USERTV_TEST_ROOT:-/home/tempuser/usertv-test-emby}"
HTTP_PORT="${USERTV_TEST_HTTP_PORT:-18096}"
HTTPS_PORT="${USERTV_TEST_HTTPS_PORT:-18920}"
EMBY_ROOT="${USERTV_EMBY_ROOT:-/opt/emby-server}"
PLUGIN_DLL="$ROOT_DIR/artifacts/plugin/Emby.UserTV.Plugin.dll"

if [[ ! -x "$EMBY_ROOT/system/EmbyServer" ]]; then
  echo "Missing EmbyServer binary: $EMBY_ROOT/system/EmbyServer" >&2
  exit 1
fi

if [[ ! -f "$PLUGIN_DLL" ]]; then
  "$ROOT_DIR/scripts/package.sh"
fi

mkdir -p "$TEST_ROOT/programdata/config" "$TEST_ROOT/programdata/plugins"
cp "$PLUGIN_DLL" "$TEST_ROOT/programdata/plugins/Emby.UserTV.Plugin.dll"

if [[ ! -s "$TEST_ROOT/programdata/config/system.xml" ]]; then
  cat > "$TEST_ROOT/programdata/config/system.xml" <<EOF
<?xml version="1.0"?>
<ServerConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EnableDebugLevelLogging>true</EnableDebugLevelLogging>
  <EnableAutoUpdate>false</EnableAutoUpdate>
  <LogFileRetentionDays>1</LogFileRetentionDays>
  <RunAtStartup>false</RunAtStartup>
  <IsStartupWizardCompleted>false</IsStartupWizardCompleted>
  <EnableUPnP>false</EnableUPnP>
  <PublicPort>${HTTP_PORT}</PublicPort>
  <PublicHttpsPort>${HTTPS_PORT}</PublicHttpsPort>
  <HttpServerPortNumber>${HTTP_PORT}</HttpServerPortNumber>
  <HttpsPortNumber>${HTTPS_PORT}</HttpsPortNumber>
  <EnableHttps>false</EnableHttps>
  <IsPortAuthorized>true</IsPortAuthorized>
  <AutoRunWebApp>false</AutoRunWebApp>
  <EnableRemoteAccess>false</EnableRemoteAccess>
  <PreferredMetadataLanguage>en</PreferredMetadataLanguage>
  <MetadataCountryCode>US</MetadataCountryCode>
  <UICulture>en-US</UICulture>
  <EnableAutomaticRestart>false</EnableAutomaticRestart>
</ServerConfiguration>
EOF
fi

exec unshare -fpiu -m --mount-proc --propagation private --kill-child sh -c "
  mount -t tmpfs tmpfs /tmp
  mount -t tmpfs tmpfs /run
  hostname usertv-test-emby
  export TMPDIR=/tmp
  export LD_LIBRARY_PATH=$EMBY_ROOT/lib:$EMBY_ROOT/system:\${LD_LIBRARY_PATH:-}
  exec $EMBY_ROOT/system/EmbyServer \
    -programdata '$TEST_ROOT/programdata' \
    -ffdetect '$EMBY_ROOT/bin/ffdetect' \
    -ffmpeg '$EMBY_ROOT/bin/ffmpeg' \
    -ffprobe '$EMBY_ROOT/bin/ffprobe' \
    -restartexitcode 3
"
