# VirtualTV-Integration

## Grundidee

VirtualTV liefert die Live-TV-Oberflaeche. Emby UserTV Stream liefert die verwalteten Quellen. Die Verbindung erfolgt ueber pro Benutzer erzeugte Emby-Playlists.

## Template-Modell

Ein manuell angelegter VirtualTV-Kanal dient als Vorlage:

```text
__AUTO_FAV_TEMPLATE__
```

Die Playlist-Bedingung zeigt auf:

```text
__AUTO_FAV_TEMPLATE_PLAYLIST__
```

Das Sync-Werkzeug klont dieses Template und ersetzt die Platzhalter durch echte Namen.

## Warum ein Template?

VirtualTV-Kanaele enthalten viele Einstellungen. Statt diese Einstellungen im Code komplett neu zu bauen, wird ein funktionierender manueller Kanal als Vorlage verwendet. Das ist pragmatischer und reduziert das Risiko falscher Default-Werte.

## Verwaltete Kanaele

Ein erzeugter Kanal kann zum Beispiel heissen:

```text
UserTV - viewer-b Channel
```

Er verweist auf eine Playlist wie:

```text
fav-viewer-b
```

## Sicherheitsgedanke

Manuelle Kanaele sollen nicht versehentlich ueberschrieben werden. Das Werkzeug arbeitet deshalb mit klaren Namensmustern, State und Template-Bezug.
