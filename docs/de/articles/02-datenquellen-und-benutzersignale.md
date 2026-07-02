# Datenquellen und Benutzer-Signale

## Signalquellen

Der Scheduler nutzt mehrere Signale pro Benutzer:

| Quelle | Signal | Nutzen |
| --- | --- | --- |
| Emby-Favoriten | bewusst markierte Filme und Serien | stabile Interessen |
| Serien-Episoden | Folgen einer favorisierten Serie | vollstaendige Rotation |
| Playback Reporting | zuletzt geschaute Episoden | aktuelle Interessen |
| Fins-TV Optionen | manuelle Includes/Excludes | Benutzerkontrolle |
| Emby-Bibliothek | aehnliche lokale Medien | Erweiterung ueber Favoriten hinaus |

## Favoriten

Favoriten sind das staerkste manuelle Signal. Filme koennen direkt in die Rotation, Serien werden ueber die Emby-API in Episoden aufgeloest.

Wichtig ist die Sichtbarkeit pro User: Ein Benutzer soll nur Inhalte planen koennen, die fuer ihn in Emby sichtbar sind.

## Playback Reporting

Playback Reporting wird genutzt, um "hot series" zu erkennen. Wenn ein Benutzer in einem Lookback-Fenster mehrere Folgen einer Serie gesehen hat, kann diese Serie als aktuelles Interesse gelten.

Das System ignoriert:

- Audio-Events;
- Wiedergaben der generierten Fins-TV-Kanaele;
- zu kurze Wiedergaben unterhalb der Mindestdauer.

Dadurch wird verhindert, dass der Kanal seine eigenen Ausgaben als neues Interesse wieder einspeist.

## Manuelle Optionen

Die Fins-TV Options-API ermoeglicht benutzerspezifische Steuerung:

- Kanal aktiv/inaktiv;
- manuelle Filme;
- manuelle Episoden;
- manuelle Serien;
- ausgeschlossene Items;
- deaktivierte Auto-Serien.

## Datenschutz und Vorsicht

Die Signale sind persoenlich. Deshalb gehoeren State-Dateien, Options-Dateien, Logs und echte Konfigurationen nicht in ein oeffentliches Repository.
