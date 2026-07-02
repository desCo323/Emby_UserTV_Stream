# Troubleshooting

## Kein Kanal erscheint

Pruefen:

1. Gibt es aktive Emby-Benutzer?
2. Hat der Benutzer genug Favoriten oder manuelle Quellen?
3. Ist `scheduler.min_items_for_channel` zu hoch?
4. Existiert das VirtualTV-Template `__AUTO_FAV_TEMPLATE__`?
5. War der letzte Lauf erfolgreich?

Kommandos:

```bash
systemctl status emby-favtv-sync.service --no-pager
journalctl -u emby-favtv-sync.service -n 200 --no-pager
```

## Playlist leer oder falsch

Moegliche Ursachen:

- Favoriten wurden aus Emby entfernt.
- Serien-Favorit hat keine sichtbaren Episoden fuer den User.
- User hat eingeschraenkte Bibliotheksrechte.
- Manuelle Excludes entfernen zu viele Items.
- Emby API-Key hat nicht genug Zugriff.

## VirtualTV-Template wird nicht gefunden

Das Template muss exakt so heissen:

```text
__AUTO_FAV_TEMPLATE__
```

Die Template-Playlist muss exakt so heissen:

```text
__AUTO_FAV_TEMPLATE_PLAYLIST__
```

Bei mehreren Templates sollte der Lauf abbrechen, weil sonst nicht klar ist, welcher Kanal geklont werden soll.

## Timer laeuft nicht

```bash
systemctl list-timers 'emby-favtv*' --all
systemctl status emby-favtv-sync.timer --no-pager
```

Aktivieren:

```bash
sudo systemctl enable --now emby-favtv-sync.timer
```

## Options-API nicht erreichbar

```bash
systemctl status emby-favtv-options.service --no-pager
journalctl -u emby-favtv-options.service -n 100 --no-pager
```

Standard-Port laut Beispiel: `8097`.

## Programmbilder fehlen

Pruefen:

- `program_image_overlay.enabled`
- `program_image_overlay.ffmpeg_path`
- Schreibrechte fuer Emby-Bilder
- Logs nach FFmpeg-Fehlern

## Sofort stoppen

```bash
sudo systemctl disable --now emby-favtv-sync.timer
sudo systemctl stop emby-favtv-sync.service
sudo systemctl disable --now emby-favtv-options.service
```
