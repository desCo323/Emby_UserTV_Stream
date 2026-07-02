# Konfigurationsreferenz

Diese Seite beschreibt die wichtigsten Konfigurationsbereiche. Beispielwerte muessen vor produktiver Nutzung geprueft werden.

## Emby-Zugang

| Feld | Bedeutung |
| --- | --- |
| `emby_url` | Basis-URL des Emby Servers |
| `api_key` | Emby API-Key, niemals committen |
| `admin_user_id` | Admin-User-ID fuer Playlist-Operationen |

## Playlist und Kanalnamen

| Feld | Beispiel | Wirkung |
| --- | --- | --- |
| `playlist_prefix` | `fav-` | erzeugt `fav-USERNAME` |
| `channel_name_template` | `00 Fins Crew TV - {username} Channel` | erzeugt den sichtbaren VirtualTV-Kanalnamen |
| `include_item_types` | `["Movie", "Series"]` | Favoriten-Typen, die ausgewertet werden |

## Scheduler

| Feld | Zweck |
| --- | --- |
| `enabled` | Scheduler ein/aus |
| `horizon_hours` | Wie weit im Voraus geplant wird |
| `rebuild_interval_hours` | Wie lange ein Schedule wiederverwendet werden darf |
| `freeze_next_hours` | Nahe Zukunft bleibt stabil |
| `min_items_for_channel` | Mindestanzahl Quellen fuer einen Kanal |
| `movie_cooldown_days` | Abstand bis ein Film erneut geplant wird |
| `episode_cooldown_days` | Abstand bis eine Episode erneut geplant wird |
| `mix_pattern` | Reihenfolge der Pools, z. B. `hot`, `favorite`, `similar`, `movie` |

## Auto-Rotation

Auto-Rotation wertet Playback Reporting aus. Wiedergegebene UserTV-Kanaele und Audio-Events werden ignoriert, damit das System nicht seine eigenen Ausgaben als neue Interessen missversteht.

Wichtige Felder:

- `lookback_days`
- `min_episode_count`
- `min_play_duration_seconds`
- `max_auto_series`
- `playback_reporting_db`

## VirtualTV

| Feld | Bedeutung |
| --- | --- |
| `enabled` | VirtualTV-Aktualisierung aktivieren |
| `template_channel_name` | Name des Template-Kanals |
| `template_playlist_name` | Platzhalter fuer Playlist-Bedingung |
| `remove_channels_without_favorites` | Kanaele ohne Quellen entfernen/deaktivieren |
| `delete_removed_channels` | Entfernen statt nur deaktivieren |
| `backup_before_virtualtv_write` | Sicherheitsbackup |

## Safety

Die wichtigsten Sicherheitsoptionen:

- `dry_run_default`
- `require_complete_user_scan_for_deletions`
- `require_complete_favorite_scan_for_playlist_clear`
- `backup_before_virtualtv_write`
- `never_touch_manual_channels`
- `abort_on_template_ambiguity`

Diese Werte sollten fuer Experimente konservativ bleiben.
