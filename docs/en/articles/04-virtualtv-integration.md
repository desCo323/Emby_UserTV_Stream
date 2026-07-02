# VirtualTV Integration

## Core idea

VirtualTV provides the Live TV surface. Emby UserTV Stream provides the managed sources. The connection is made through generated Emby playlists per user.

## Template model

One manually created VirtualTV channel acts as template:

```text
__AUTO_FAV_TEMPLATE__
```

Its playlist condition points to:

```text
__AUTO_FAV_TEMPLATE_PLAYLIST__
```

The sync tool clones this template and replaces placeholders with real names.

## Why a template?

VirtualTV channels contain many settings. Instead of rebuilding those settings completely in code, a working manual channel is used as a template. This is pragmatic and reduces the risk of wrong default values.

## Managed channels

A generated channel may be named:

```text
UserTV - viewer-b Channel
```

It refers to a playlist such as:

```text
fav-viewer-b
```

## Safety idea

Manual channels should not be overwritten accidentally. The tool therefore uses clear naming patterns, state and template relation.
