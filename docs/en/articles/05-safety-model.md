# Safety Model and Boundaries

## Why safety matters here

The project works close to production Emby data. Mistakes can change playlists, VirtualTV channels, display preferences or program images.

## Safety principles

- Discover first, then check, then dry-run, write only afterwards.
- No secrets in Git.
- No production configuration in documentation examples.
- Backups before VirtualTV writes.
- Do not touch manual channels.
- Abort on ambiguous templates.

## No warranty

This documentation describes experiments. It contains no guarantee. Anyone rebuilding the idea must verify Emby version, plugin versions, paths, permissions and API behavior themselves.

## No commercial use

The concepts and documentation may not be used commercially without prior permission. This especially applies to productization, hosting, consulting, bundles or derivative plugins/scripts.

## Recommended safeguards

1. Full backup of Emby program data.
2. Separate test instance where possible.
3. Use `--check` and `--dry-run` consistently.
4. Enable only a few test users first.
5. Review logs after every real run.
6. Rotate API keys regularly.
