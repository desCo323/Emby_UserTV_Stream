# Architektur

Ein externes Python-Werkzeug liest Emby-Daten, berechnet pro Benutzer eine Rotation, schreibt verwaltete Playlists und aktualisiert VirtualTV-Kanaele.

## Komponenten

```mermaid
flowchart TB
    subgraph Emby[Emby Server]
        Users[Benutzer und Policies]
        Items[Medienbibliothek]
        Favorites[UserData / Favoriten]
        Playback[Playback Reporting DB]
        Playlists[User Playlists]
        Display[Display Preferences]
    end

    subgraph UserTV[Emby UserTV Stream / favtv-sync]
        Sync[Sync CLI]
        Scheduler[24/7 Scheduler]
        Options[Fins-TV Options API]
        State[State und Options JSON]
        Images[Program Image Overlay]
    end

    subgraph VirtualTV[VirtualTV Plugin]
        Template[Template-Kanal]
        Channels[User-Kanaele]
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

## Hauptpfad

1. Aktive Emby-Benutzer werden ueber die Emby API gelesen.
2. Fuer jeden Benutzer werden Favoriten geladen.
3. Serien-Favoriten werden in Episoden aufgeloest.
4. Manuelle Fins-TV-Optionen werden ergaenzt.
5. Playback Reporting liefert aktuelle Serieninteressen.
6. Der Scheduler baut eine zeitliche Rotation.
7. Die Zielreihenfolge wird in `fav-USERNAME` geschrieben.
8. VirtualTV-Kanaele werden erstellt oder aktualisiert.
9. Emby Live TV zeigt die Kanaele.

## Datenmodell

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

## Scheduler-Entscheidung

```mermaid
flowchart TD
    A[Start pro Benutzer] --> B{Kanal aktiv?}
    B -- nein --> X[Keine Items planen]
    B -- ja --> C[Favoriten + manuelle Quellen sammeln]
    C --> D[Zuletzt geschaute Serien erkennen]
    D --> E{Genug Quellen?}
    E -- nein --> X
    E -- ja --> F[Candidate Pools bauen]
    F --> G[Hot / Favorite / Similar / Movie mischen]
    G --> H[Cooldowns und Serienfolge-Regeln anwenden]
    H --> I[36h Schedule schreiben]
    I --> J[Playlist aktualisieren]
    J --> K[VirtualTV-Kanal aktualisieren]
```

## Sicherheitsgrenzen

- Nicht verwaltete VirtualTV-Kanaele bleiben unberuehrt.
- Verwaltete Objekte sind ueber Namen, State und Template-Bezug erkennbar.
- Vor VirtualTV-Schreibzugriffen werden Backups angelegt.
- Dry-run ist Standard-Sicherheitsmodus in der Beispielkonfiguration.
- Private API-Keys und produktive Konfigurationen gehoeren nicht ins Repository.
