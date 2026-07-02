# Betriebsmodell mit systemd-Timer

## Warum systemd statt Cron?

Ein systemd-Timer ist sichtbar, abfragbar und in den normalen Service-Status integriert. Das macht ihn fuer diesen Zweck besser nachvollziehbar als ein versteckter Cronjob.

## Einheiten

```text
emby-favtv-sync.service
emby-favtv-sync.timer
emby-favtv-options.service
```

Der Timer startet den Sync-Service periodisch. Die Options-API laeuft dauerhaft und ermoeglicht Benutzereinstellungen.

## Typischer Ablauf

1. Timer startet den Sync.
2. Sync liest Emby-Benutzer und Favoriten.
3. Scheduler aktualisiert State und Playlists.
4. VirtualTV wird aktualisiert.
5. Guide/Home-Daten koennen aktualisiert werden.
6. Service endet.

## Beobachtbarkeit

```bash
systemctl list-timers 'emby-favtv*' --all
systemctl status emby-favtv-sync.service
journalctl -u emby-favtv-sync.service -n 200 --no-pager
```

## Betriebsentscheidung

Der Timer sollte nur aktiv bleiben, wenn man die Auswirkungen versteht und Backups vorhanden sind. Fuer Experimente ist ein manueller Lauf mit `--dry-run` der bessere erste Schritt.
