# Sicherheitsmodell und Grenzen

## Warum Sicherheit hier wichtig ist

Das Projekt arbeitet nahe an produktiven Emby-Daten. Fehler koennen Playlists, VirtualTV-Kanaele, Display-Preferences oder Programmbilder veraendern.

## Sicherheitsprinzipien

- Erst entdecken, dann pruefen, dann Dry-run, erst danach schreiben.
- Keine Secrets in Git.
- Keine produktiven Konfigurationen in Doku-Beispiele.
- Backups vor VirtualTV-Schreibzugriffen.
- Manuelle Kanaele nicht anfassen.
- Bei uneindeutigen Templates abbrechen.

## No-Warranty

Diese Dokumentation beschreibt Experimente. Sie enthaelt keine Garantie. Wer die Idee nachbaut, muss selbst pruefen, ob Emby-Version, Plugin-Versionen, Dateipfade, Rechte und API-Verhalten passen.

## No-Commercial-Use

Die Konzepte und Dokumentation duerfen nicht ohne vorherige Genehmigung kommerziell genutzt werden. Das gilt besonders fuer Produktisierung, Hosting, Beratung, Bundle-Angebote oder abgeleitete Plugins/Skripte.

## Empfohlene Schutzmassnahmen

1. Vollbackup von Emby-Programmdaten.
2. Separate Testinstanz, wenn moeglich.
3. `--check` und `--dry-run` konsequent nutzen.
4. Erst wenige Testbenutzer aktivieren.
5. Logs nach jedem echten Lauf pruefen.
6. API-Key regelmaessig rotieren.
