# Installation and Quickstart

This guide documents the observed target setup. It does not guarantee that another system behaves the same way.

## Requirements

- Emby Server with a working local installation.
- VirtualTV plugin in Emby.
- CollectionManager plugin when dynamic user playlists and collections are used.
- Playback Reporting when recently watched shows should be used as a signal.
- Python 3 and an isolated virtualenv.
- Write permissions for the Emby service user on UserTV state files.

## Target paths

| Purpose | Path |
| --- | --- |
| Project | `/opt/emby-favtv-sync` |
| Configuration | `/etc/emby-favtv-sync/config.json` |
| State and options | `/var/lib/emby-favtv-sync/` |
| Log file | `/var/log/emby-favtv-sync.log` |
| Emby user playlists | `/var/lib/emby/data/userplaylists/` |
| VirtualTV configuration | `/var/lib/emby/plugins/configurations/VirtualTV.xml` |

## Installation

```bash
sudo /opt/emby-favtv-sync/install.sh
```

Then edit the configuration:

```bash
sudo nano /etc/emby-favtv-sync/config.json
```

Important fields:

```json
{
  "emby_url": "http://127.0.0.1:8096",
  "api_key": "PASTE_EMBY_API_KEY_HERE",
  "admin_user_id": "PASTE_ADMIN_USER_ID_HERE",
  "playlist_prefix": "fav-",
  "channel_name_template": "UserTV - {username} Channel"
}
```

Never commit real tokens or API keys to Git.

## Create the VirtualTV template

VirtualTV must contain one manual template channel:

```text
__AUTO_FAV_TEMPLATE__
```

The playlist condition of this template channel must refer to:

```text
__AUTO_FAV_TEMPLATE_PLAYLIST__
```

The tool clones that template for managed user channels and replaces the placeholders with channel and playlist names.

## Safe sequence

1. Discover users and environment:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --discover
   ```

2. Check configuration without changes:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --check
   ```

3. Dry-run:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --dry-run
   ```

4. First real run:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --run-now
   ```

5. Enable the timer only after check and first run succeeded:

   ```bash
   sudo systemctl enable --now emby-favtv-sync.timer
   ```

## Expected result

After a successful run, these artifacts should exist:

- `fav-USERNAME` playlists under `/var/lib/emby/data/userplaylists/`;
- VirtualTV channels such as `UserTV - VIEWER Channel`;
- updated state file under `/var/lib/emby-favtv-sync/state.json`;
- timer status with next run:

  ```bash
  systemctl list-timers 'emby-favtv*' --all
  ```

## Rollback

VirtualTV backups are stored under:

```text
/var/lib/emby-favtv-sync/backups/
```

Restore:

```bash
sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --restore-virtualtv-backup /var/lib/emby-favtv-sync/backups/BACKUP_FILE
```
