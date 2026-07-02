# Scheduler und 24/7-Programmplanung

## Ziel

Der Scheduler erzeugt kein zufaelliges Shuffle, sondern ein Programmfenster. Im beobachteten Setup wird ein Horizont von 36 Stunden geplant und regelmaessig aktualisiert.

## Candidate Pools

Inhalte werden in Pools eingeteilt:

- `hot`: aktuell geschaute Serien, moeglichst sinnvoll fortgesetzt;
- `favorite`: direkte Favoriten und daraus abgeleitete Episoden;
- `similar`: aehnliche lokale Medien;
- `movie`: passende Filme als Abwechslung.

Das `mix_pattern` bestimmt, wie diese Pools kombiniert werden.

## Aehnlichkeit

Aehnliche Inhalte werden ueber Merkmale gebildet:

- Genres;
- Tags;
- Studios;
- Personen;
- Serienbezug;
- Altersfreigabe;
- Produktionsjahr bzw. Jahrzehnt.

Diese Merkmale bilden ein Profil aus den Seed-Inhalten. Andere lokale Items werden gegen dieses Profil bewertet.

## Wiederholungen vermeiden

Der Scheduler nutzt:

- Movie-Cooldown;
- Episode-Cooldown;
- Begrenzung gleicher Serie hintereinander;
- Freeze-Fenster fuer die nahe Zukunft.

Das verhindert harte Spruenge und zu haeufige Wiederholungen.

## Warum State wichtig ist

`state.json` speichert den erzeugten Plan, Historie und IDs. Ohne State waere jeder Lauf zu zufaellig und koennte bereits sichtbare Programmplaetze staendig verschieben.
