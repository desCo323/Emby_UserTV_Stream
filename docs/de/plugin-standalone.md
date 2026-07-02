# Standalone-Plugin

UserTV Stream ist ab dieser Projektstufe nicht mehr nur als serverlokale Skript-Installation gedacht. Das Repository enthaelt eine installierbare Emby-Plugin-Basis unter `src/Emby.UserTV.Plugin`.

## Zielbild

- Installation als Emby-Plugin-DLL im Emby-Plugin-Verzeichnis.
- Konfiguration ueber eine Emby-Admin-Seite.
- Dry-Run-Planung pro Emby-Benutzer auf Basis der Favoriten.
- Kein externer API-Key, keine Cronjobs und keine Dateien unter `/opt` fuer den Plugin-Kern.
- Schreiboperationen auf Playlists und VirtualTV bleiben in Version 0 bewusst deaktiviert, bis die Emby- und VirtualTV-Adapter auf einem Testsystem validiert sind.

## Build

```bash
./scripts/build.sh
```

## Paket erzeugen

```bash
./scripts/package.sh
```

Das Ergebnis liegt in `artifacts/plugin`. Fuer einen manuellen Test wird die Datei `Emby.UserTV.Plugin.dll` in das Emby-Plugin-Verzeichnis kopiert. Danach muss Emby neu gestartet werden.

## Smoke-Test

```bash
./scripts/test.sh
```

Das Testskript prueft die Plugin-Ressourcen, die Planerlogik und den Dry-Run mit fiktiven Benutzern und fiktiven Medien. Das aktuelle Testprotokoll steht in [testprotokoll-standalone.md](testprotokoll-standalone.md).

## Isolierter Emby-Test

Auf Hosts mit bereits laufendem produktivem Emby kann die Testinstanz isoliert gestartet werden:

```bash
./scripts/start-isolated-emby-test.sh
```

Das Skript nutzt eigene Linux-Namespaces, eigene Test-Programmdaten und den Standard-Testport `18096`. Es schreibt nicht in produktive Emby-Programmdaten.

## Funktionsumfang der ersten Plugin-Version

- Emby-Plugin-Ladepunkt mit eigener Plugin-ID.
- Admin-Dashboard `UserTV Stream`.
- REST-Endpunkte fuer Status, Konfiguration und Dry-Run-Planung.
- Favoriten-Scan ueber Emby-interne Manager statt externer HTTP-API.
- Planer fuer Playlist- und Kanalnamen pro Benutzer.
- Automatischer Timer fuer Dry-Run-Planung, wenn aktiviert.

## Grenzen

Diese Version ist eine installierbare Grundlage. Sie ersetzt die serverlokalen Cronjobs strukturell, schreibt aber noch keine Playlists oder VirtualTV-Kanaele. Das ist Absicht: Die spaeteren Write-Adapter muessen auf einem separaten Testsystem verifiziert werden, damit echte Benutzerbibliotheken nicht durch experimentellen Code veraendert werden.

Es gibt keine Garantie auf Funktion, Datenintegritaet oder Eignung fuer einen bestimmten Zweck. Das Projekt dokumentiert private Experimente. Kommerzielle Nutzung des geistigen Eigentums ist ohne vorherige Genehmigung nicht erlaubt.
