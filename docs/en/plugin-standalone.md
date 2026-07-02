# Standalone Plugin

UserTV Stream is no longer limited to a server-local script setup. The repository now contains an installable Emby plugin foundation in `src/Emby.UserTV.Plugin`.

## Target

- Installation as an Emby plugin DLL in the Emby plugin directory.
- Configuration through an Emby admin page.
- Per-user dry-run planning from Emby favorites.
- No external API key, no cron jobs, and no `/opt` files for the plugin core.
- Playlist and VirtualTV write operations stay disabled in version 0 until the Emby and VirtualTV adapters are validated on a test system.

## Build

```bash
./scripts/build.sh
```

## Package

```bash
./scripts/package.sh
```

The output is written to `artifacts/plugin`. For a manual test, copy `Emby.UserTV.Plugin.dll` into the Emby plugin directory and restart Emby.

## Smoke Test

```bash
./scripts/test.sh
```

The test script verifies plugin resources, planner behavior, and dry-run behavior with fictive users and fictive media. The current test report is in [standalone-test-report.md](standalone-test-report.md).

## Isolated Emby Test

On hosts that already run a production Emby instance, the test instance can be started in isolation:

```bash
./scripts/start-isolated-emby-test.sh
```

The script uses separate Linux namespaces, separate test program data, and the default test port `18096`. It does not write into production Emby program data.

## First Plugin Version Scope

- Emby plugin entry point with its own plugin ID.
- Admin dashboard `UserTV Stream`.
- REST endpoints for status, configuration, and dry-run planning.
- Favorite scanning through Emby internal managers instead of an external HTTP API.
- Planner for per-user playlist and channel names.
- Automatic timer for dry-run planning when enabled.

## Limits

This version is an installable foundation. It structurally replaces server-local cron jobs, but it does not write playlists or VirtualTV channels yet. That is intentional: later write adapters need validation on a separate test system so real user libraries are not modified by experimental code.

There is no warranty for function, data integrity, or fitness for a specific purpose. This project documents private experiments. Commercial use of the intellectual property is not permitted without prior approval.
