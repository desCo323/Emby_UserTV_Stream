# Configuration Reference

This page describes the most important configuration sections. Example values must be reviewed before production-adjacent use.

## Emby access

| Field | Meaning |
| --- | --- |
| `emby_url` | Base URL of the Emby server |
| `api_key` | Emby API key, never commit it |
| `admin_user_id` | Admin user ID for playlist operations |

## Playlist and channel names

| Field | Example | Effect |
| --- | --- | --- |
| `playlist_prefix` | `fav-` | creates `fav-USERNAME` |
| `channel_name_template` | `UserTV - {username} Channel` | creates the visible VirtualTV channel name |
| `include_item_types` | `["Movie", "Series"]` | favorite item types to process |

## Scheduler

| Field | Purpose |
| --- | --- |
| `enabled` | Enables/disables scheduler |
| `horizon_hours` | How far ahead the schedule is planned |
| `rebuild_interval_hours` | How long a schedule may be reused |
| `freeze_next_hours` | Keeps the near future stable |
| `min_items_for_channel` | Minimum source count for a channel |
| `movie_cooldown_days` | Gap before a movie can be planned again |
| `episode_cooldown_days` | Gap before an episode can be planned again |
| `mix_pattern` | Pool order, e.g. `hot`, `favorite`, `similar`, `movie` |

## Auto rotation

Auto rotation evaluates Playback Reporting. Generated UserTV channels and audio events are ignored so the system does not treat its own output as fresh interest.

Important fields:

- `lookback_days`
- `min_episode_count`
- `min_play_duration_seconds`
- `max_auto_series`
- `playback_reporting_db`

## VirtualTV

| Field | Meaning |
| --- | --- |
| `enabled` | Enable VirtualTV updates |
| `template_channel_name` | Template channel name |
| `template_playlist_name` | Placeholder for playlist condition |
| `remove_channels_without_favorites` | Remove/disable channels without sources |
| `delete_removed_channels` | Delete instead of only disabling |
| `backup_before_virtualtv_write` | Safety backup |

## Safety

The most important safety options:

- `dry_run_default`
- `require_complete_user_scan_for_deletions`
- `require_complete_favorite_scan_for_playlist_clear`
- `backup_before_virtualtv_write`
- `never_touch_manual_channels`
- `abort_on_template_ambiguity`

Keep these values conservative for experiments.
