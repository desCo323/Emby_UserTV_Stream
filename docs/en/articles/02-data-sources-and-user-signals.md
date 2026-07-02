# Data Sources and User Signals

## Signal sources

The scheduler uses several signals per user:

| Source | Signal | Value |
| --- | --- | --- |
| Emby favorites | deliberately marked movies and shows | stable interests |
| Show episodes | episodes of a favorite show | complete rotation |
| Playback Reporting | recently watched episodes | current interests |
| Fins-TV options | manual includes/excludes | user control |
| Emby library | similar local media | expansion beyond favorites |

## Favorites

Favorites are the strongest manual signal. Movies can enter the rotation directly, shows are expanded into episodes through the Emby API.

Per-user visibility matters: a user should only schedule content that is visible to them in Emby.

## Playback Reporting

Playback Reporting is used to detect hot series. If a user watched several episodes of a show inside the lookback window, that show can be treated as current interest.

The system ignores:

- audio events;
- playback of generated Fins-TV channels;
- very short playback below the configured minimum duration.

This prevents the channel from feeding its own output back as new interest.

## Manual options

The Fins-TV Options API enables per-user control:

- channel enabled/disabled;
- manual movies;
- manual episodes;
- manual shows;
- excluded items;
- disabled auto series.

## Privacy and caution

The signals are personal. State files, options files, logs and real configuration files do not belong in a public repository.
