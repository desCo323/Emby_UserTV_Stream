# Operations Model with systemd Timer

## Why systemd instead of cron?

A systemd timer is visible, queryable and integrated into normal service status. That makes it easier to understand than a hidden cron job.

## Units

```text
emby-favtv-sync.service
emby-favtv-sync.timer
emby-favtv-options.service
```

The timer starts the sync service periodically. The options API runs continuously and enables user settings.

## Typical flow

1. Timer starts the sync.
2. Sync reads Emby users and favorites.
3. Scheduler updates state and playlists.
4. VirtualTV is updated.
5. Guide/home data may be refreshed.
6. Service exits.

## Observability

```bash
systemctl list-timers 'emby-favtv*' --all
systemctl status emby-favtv-sync.service
journalctl -u emby-favtv-sync.service -n 200 --no-pager
```

## Operations decision

The timer should only stay active when the impact is understood and backups exist. For experiments, a manual `--dry-run` is the better first step.
