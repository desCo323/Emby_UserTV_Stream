# Architecture

The architecture is intentionally pragmatic: an external Python tool reads Emby data, calculates one rotation per user, writes managed playlists and updates VirtualTV channels.

## Components

```mermaid
flowchart TB
    subgraph Emby[Emby Server]
        Users[Users and policies]
        Items[Media library]
        Favorites[UserData / favorites]
        Playback[Playback Reporting DB]
        Playlists[User playlists]
        Display[Display Preferences]
    end

    subgraph UserTV[Emby UserTV Stream / favtv-sync]
        Sync[Sync CLI]
        Scheduler[24/7 scheduler]
        Options[Fins-TV Options API]
        State[State and options JSON]
        Images[Program image overlay]
    end

    subgraph VirtualTV[VirtualTV plugin]
        Template[Template channel]
        Channels[User channels]
        Guide[Guide / Live TV]
    end

    Users --> Sync
    Items --> Sync
    Favorites --> Sync
    Playback --> Scheduler
    Options --> State
    Sync --> Scheduler
    Scheduler --> Playlists
    Scheduler --> State
    Playlists --> Channels
    Template --> Channels
    Channels --> Guide
    Images --> Channels
    Display --> Guide
```

## Main path

1. Active Emby users are read through the Emby API.
2. Favorites are loaded for every user.
3. Favorite shows are expanded to episodes.
4. Manual Fins-TV options are added.
5. Playback Reporting provides current show-interest signals.
6. The scheduler builds a time-based rotation.
7. The target order is written to `fav-USERNAME`.
8. VirtualTV channels are created or updated.
9. Emby Live TV displays the channels.

## Data model

```mermaid
erDiagram
    USER ||--o{ FAVORITE : owns
    USER ||--o{ PLAYBACK_EVENT : creates
    USER ||--|| USER_OPTIONS : configures
    USER ||--|| USER_PLAYLIST : receives
    USER_PLAYLIST ||--o{ SCHEDULE_ENTRY : contains
    USER_PLAYLIST ||--|| VIRTUALTV_CHANNEL : feeds
    VIRTUALTV_TEMPLATE ||--o{ VIRTUALTV_CHANNEL : clones

    USER {
        string id
        string name
        bool active
    }
    USER_OPTIONS {
        bool channel_enabled
        list manual_items
        list excluded_items
        list disabled_auto_series
    }
    USER_PLAYLIST {
        string name
        string emby_id
    }
    SCHEDULE_ENTRY {
        string item_id
        datetime start
        datetime end
        string reason
    }
    VIRTUALTV_CHANNEL {
        string id
        string name
        string playlist_ref
    }
```

## Scheduler decision

```mermaid
flowchart TD
    A[Start per user] --> B{Channel enabled?}
    B -- no --> X[Plan no items]
    B -- yes --> C[Collect favorites and manual sources]
    C --> D[Detect recently watched shows]
    D --> E{Enough sources?}
    E -- no --> X
    E -- yes --> F[Build candidate pools]
    F --> G[Mix hot / favorite / similar / movie]
    G --> H[Apply cooldowns and series-run rules]
    H --> I[Write 36h schedule]
    I --> J[Update playlist]
    J --> K[Update VirtualTV channel]
```

## Safety boundaries

- Unmanaged VirtualTV channels are left untouched.
- Managed objects are identifiable through names, state and template relation.
- Backups are created before VirtualTV writes.
- Dry-run is the default safety mode in the sample configuration.
- Private API keys and production configuration files do not belong in the repository.
