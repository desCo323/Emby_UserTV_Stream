# Testprotokoll Standalone-Plugin

## Testumgebung

- Produktives Emby erkannt auf `8096/8920` mit Programmdaten unter `/var/lib/emby`.
- Testdaten liegen ausschliesslich unter `/home/tempuser/usertv-test-emby`.
- Testports fuer die geplante Nebeninstanz: `18096/18920`.
- Fiktive Testmedien:
  - `Fictional Pilot (2026)`
  - `Signal Garden (2026)`
  - `Example Nights S01E01`
  - `Example Nights S01E02`

## Ergebnis echter Nebeninstanz

Ein direkter zweiter Emby-Start auf demselben Host wurde zuerst blockiert:

```text
Exiting because another instance is already running.
```

Der Test wurde danach mit Linux-Namespaces isoliert gestartet: eigene PID-, IPC-, UTS- und Mount-Namespace, eigenes `/tmp`, eigenes `/run`, eigener Programmdatenpfad und Testport `18096`. Damit konnte Emby 4.8.10.0 parallel zum produktiven Server starten, ohne `/var/lib/emby`, `/etc/emby*` oder `/opt/emby-favtv-sync` zu veraendern.

Startskript:

```bash
./scripts/start-isolated-emby-test.sh
```

Wichtig fuer diesen Host: `LD_LIBRARY_PATH` muss `/opt/emby-server/lib` enthalten, damit die Emby-eigene `libsqlite3.so` im isolierten Kontext gefunden wird.

## Installationstest

Ergebnis am 2026-07-03:

- Test-Emby erreichbar auf `http://127.0.0.1:18096`.
- Plugin-DLL geladen aus `/home/tempuser/usertv-test-emby/programdata/plugins/Emby.UserTV.Plugin.dll`.
- Lognachweis: `Loading Emby.UserTV.Plugin, Version=0.1.0.0`.
- Lognachweis: `[UserTV] Runtime initialized.`
- Plugin-Konfigurationsseite registriert als `UserTV Stream`.
- Ressourcen erreichbar:
  - `web/configurationpage?name=usertv`
  - `web/configurationpage?name=usertvjs`
  - `web/configurationpage?name=usertvcss`

## Testdaten

Angelegt wurden ausschliesslich fiktive Benutzer und fiktive Medien:

- Benutzer: `viewer-alpha`, `viewer-beta`
- `viewer-alpha`: 3 Favoriten, erwartbar eligible
- `viewer-beta`: 1 Favorit, erwartbar nicht eligible

Dry-Run-Ergebnis:

```text
UsersScanned: 3
EligibleUsers: 1
PlannedItems: 3
viewer-alpha: ready
viewer-beta: minimum_items_not_met:3
```

## Isolierter Smoke-Test

Der Repository-Test `scripts/test.sh` prueft:

- eingebettete Plugin-Webressourcen;
- Planerlogik mit fiktiven Benutzern und fiktiven Medien;
- Runtime-Dry-Run ohne Schreibzugriffe auf Emby.

```bash
./scripts/build.sh
./scripts/package.sh
./scripts/test.sh
```

## Naechster sicherer Testschritt

Der naechste sichere Schritt ist die Umsetzung eines validierten Playlist-Write-Adapters in der Testinstanz. VirtualTV-Schreibzugriffe bleiben weiter gesperrt, bis Playlist-Erzeugung, Aktualisierung und Loeschschutz stabil nachgewiesen sind.
