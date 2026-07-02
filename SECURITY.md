# Security Policy

This repository must not contain secrets.

Do not commit:

- GitHub tokens;
- Emby API keys;
- Emby admin user IDs when they identify a private installation;
- production `config.json`;
- `state.json`, `options.json`, backups, logs or database dumps;
- private media paths when they expose personal infrastructure.

Use `config.example.json` style placeholders in documentation. If a token or API key was pasted into an issue, commit, pull request or chat transcript, rotate it immediately.

The documented workflow can modify Emby playlists, VirtualTV channels, display preferences and program images. Always run discovery/check/dry-run before a real run.
