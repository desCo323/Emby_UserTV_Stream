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

Eine zweite Emby-Instanz konnte auf diesem Host nicht parallel gestartet werden. Emby beendet sich mit:

```text
Exiting because another instance is already running.
```

Das passiert bereits vor dem Schreiben von Testlogs und vor dem Oeffnen des Testports. Der produktive Server wurde nicht gestoppt, nicht neu gestartet und nicht in seinen Programmdaten veraendert.

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

Fuer einen echten Installationstest wird eine getrennte VM oder ein Container-Host ohne laufende produktive Emby-Instanz benoetigt. Auf diesem Host ist ein paralleler Emby-Start durch Emby selbst blockiert.
