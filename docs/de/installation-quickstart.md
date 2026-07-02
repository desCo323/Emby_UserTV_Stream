# Installation und Quickstart

Diese Anleitung beschreibt den beobachteten Zielaufbau. Sie ist keine Garantie, dass ein anderes System identisch funktioniert.

## Voraussetzungen

- Emby Server mit funktionierender lokaler Installation.
- VirtualTV-Plugin in Emby.
- CollectionManager-Plugin, wenn dynamische User-Playlists und Collections genutzt werden.
- Playback Reporting, wenn zuletzt geschaute Serien als Signal verwendet werden sollen.
- Python 3 und ein isoliertes Virtualenv.
- Schreibrechte fuer den Emby-Servicebenutzer auf die UserTV-State-Dateien.

## Zielpfade

| Zweck | Pfad |
| --- | --- |
| Projekt | `/opt/emby-favtv-sync` |
| Konfiguration | `/etc/emby-favtv-sync/config.json` |
| State und Optionen | `/var/lib/emby-favtv-sync/` |
| Logdatei | `/var/log/emby-favtv-sync.log` |
| Emby User-Playlists | `/var/lib/emby/data/userplaylists/` |
| VirtualTV-Konfiguration | `/var/lib/emby/plugins/configurations/VirtualTV.xml` |

## Installation

```bash
sudo /opt/emby-favtv-sync/install.sh
```

Danach die Konfiguration bearbeiten:

```bash
sudo nano /etc/emby-favtv-sync/config.json
```

Wichtige Felder:

```json
{
  "emby_url": "http://127.0.0.1:8096",
  "api_key": "PASTE_EMBY_API_KEY_HERE",
  "admin_user_id": "PASTE_ADMIN_USER_ID_HERE",
  "playlist_prefix": "fav-",
  "channel_name_template": "00 Fins Crew TV - {username} Channel"
}
```

Keine echten Tokens oder API-Keys in Git committen.

## VirtualTV-Template anlegen

In VirtualTV muss ein manueller Template-Kanal existieren:

```text
__AUTO_FAV_TEMPLATE__
```

Die Playlist-Bedingung dieses Template-Kanals muss auf folgenden Platzhalter zeigen:

```text
__AUTO_FAV_TEMPLATE_PLAYLIST__
```

Das Tool klont daraus die verwalteten User-Kanaele und ersetzt die Platzhalter durch den Kanal- und Playlistnamen.

## Sicherer Ablauf

1. Benutzer und Umgebung entdecken:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --discover
   ```

2. Konfiguration pruefen, ohne etwas zu aendern:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --check
   ```

3. Trockenlauf:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --dry-run
   ```

4. Erster echter Lauf:

   ```bash
   sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --run-now
   ```

5. Timer erst aktivieren, wenn Check und echter Lauf sauber waren:

   ```bash
   sudo systemctl enable --now emby-favtv-sync.timer
   ```

## Erfolg erkennen

Nach einem erfolgreichen Lauf sollten diese Artefakte sichtbar sein:

- `fav-USERNAME` Playlists unter `/var/lib/emby/data/userplaylists/`;
- VirtualTV-Kanaele wie `00 Fins Crew TV - USERNAME Channel`;
- aktualisierte State-Datei unter `/var/lib/emby-favtv-sync/state.json`;
- Timer-Status mit naechstem Lauf:

  ```bash
  systemctl list-timers 'emby-favtv*' --all
  ```

## Rollback

VirtualTV-Backups liegen unter:

```text
/var/lib/emby-favtv-sync/backups/
```

Restore:

```bash
sudo -u emby /opt/emby-favtv-sync/.venv/bin/python /opt/emby-favtv-sync/favtv_sync.py --config /etc/emby-favtv-sync/config.json --restore-virtualtv-backup /var/lib/emby-favtv-sync/backups/BACKUP_FILE
```
