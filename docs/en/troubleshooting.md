# Troubleshooting

## No channel appears

Check:

1. Are there active Emby users?
2. Does the user have enough favorites or manual sources?
3. Is `scheduler.min_items_for_channel` too high?
4. Does the VirtualTV template `__AUTO_FAV_TEMPLATE__` exist?
5. Did the latest run succeed?

Commands:

```bash
systemctl status emby-favtv-sync.service --no-pager
journalctl -u emby-favtv-sync.service -n 200 --no-pager
```

## Playlist is empty or wrong

Possible causes:

- Favorites were removed from Emby.
- Favorite show has no visible episodes for the user.
- User has restricted library rights.
- Manual excludes remove too many items.
- Emby API key does not have enough access.

## VirtualTV template is not found

The template must be named exactly:

```text
__AUTO_FAV_TEMPLATE__
```

The template playlist placeholder must be exactly:

```text
__AUTO_FAV_TEMPLATE_PLAYLIST__
```

If multiple templates exist, the run should abort because it is unclear which channel should be cloned.

## Timer does not run

```bash
systemctl list-timers 'emby-favtv*' --all
systemctl status emby-favtv-sync.timer --no-pager
```

Enable:

```bash
sudo systemctl enable --now emby-favtv-sync.timer
```

## Options API is unavailable

```bash
systemctl status emby-favtv-options.service --no-pager
journalctl -u emby-favtv-options.service -n 100 --no-pager
```

Default example port: `8097`.

## Program images are missing

Check:

- `program_image_overlay.enabled`
- `program_image_overlay.ffmpeg_path`
- write permissions for Emby images
- logs for FFmpeg errors

## Emergency stop

```bash
sudo systemctl disable --now emby-favtv-sync.timer
sudo systemctl stop emby-favtv-sync.service
sudo systemctl disable --now emby-favtv-options.service
```
