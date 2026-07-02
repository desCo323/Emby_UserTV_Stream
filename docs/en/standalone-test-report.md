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

A second Emby instance could not be started in parallel on this host. Emby exits with:

```text
Exiting because another instance is already running.
```

This happens before test logs are written and before the test port opens. The production server was not stopped, restarted, or changed in its program data.

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

A real installation test needs a separate VM or a container host without a running production Emby instance. On this host, Emby blocks a parallel server start by design.
