# Standalone Plugin Test Report

## Test Environment

- Production Emby detected on `8096/8920` with program data in `/var/lib/emby`.
- Test data is stored only under `/home/tempuser/usertv-test-emby`.
- Planned side-instance ports: `18096/18920`.
- Fictive test media:
  - `Fictional Pilot (2026)`
  - `Signal Garden (2026)`
  - `Example Nights S01E01`
  - `Example Nights S01E02`

## Real Side-Instance Result

A direct second Emby start on the same host was blocked first:

```text
Exiting because another instance is already running.
```

The test was then started with Linux namespace isolation: separate PID, IPC, UTS, and mount namespaces, a private `/tmp`, a private `/run`, a separate program-data path, and test port `18096`. With that isolation, Emby 4.8.10.0 started next to the production server without changing `/var/lib/emby`, `/etc/emby*`, or `/opt/emby-favtv-sync`.

Start script:

```bash
./scripts/start-isolated-emby-test.sh
```

Important on this host: `LD_LIBRARY_PATH` must include `/opt/emby-server/lib` so the Emby-provided `libsqlite3.so` is found in the isolated context.

## Installation Test

Result on 2026-07-03:

- Test Emby reachable at `http://127.0.0.1:18096`.
- Plugin DLL loaded from `/home/tempuser/usertv-test-emby/programdata/plugins/Emby.UserTV.Plugin.dll`.
- Log evidence: `Loading Emby.UserTV.Plugin, Version=0.1.0.0`.
- Log evidence: `[UserTV] Runtime initialized.`
- Plugin configuration page registered as `UserTV Stream`.
- Resources reachable:
  - `web/configurationpage?name=usertv`
  - `web/configurationpage?name=usertvjs`
  - `web/configurationpage?name=usertvcss`

## Test Data

Only fictive users and fictive media were created:

- Users: `viewer-alpha`, `viewer-beta`
- `viewer-alpha`: 3 favorites, expected eligible
- `viewer-beta`: 1 favorite, expected ineligible

Dry-run result:

```text
UsersScanned: 3
EligibleUsers: 1
PlannedItems: 3
viewer-alpha: ready
viewer-beta: minimum_items_not_met:3
```

## Isolated Smoke Test

The repository test `scripts/test.sh` verifies:

- embedded plugin web resources;
- planner behavior with fictive users and fictive media;
- runtime dry-run behavior without writes to Emby.

```bash
./scripts/build.sh
./scripts/package.sh
./scripts/test.sh
```

## Next Safe Test Step

The next safe step is a validated playlist write adapter inside the test instance. VirtualTV writes stay disabled until playlist creation, updates, and deletion protection are proven stable.
