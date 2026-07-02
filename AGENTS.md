# Repository Working Instructions

These instructions apply to this repository.

## Publication Rule

Every meaningful improvement or change to this project documentation must be committed and pushed to GitHub after review.

## Secret Handling

Do not store GitHub tokens, Emby API keys, passwords, private configuration files or other secrets in this repository.

Use one of these safe authentication methods when pushing:

- GitHub CLI authenticated on the machine;
- Git Credential Manager or another OS-level credential store;
- a temporary environment variable provided at runtime;
- a repository connector with scoped write access.

Never write a personal access token into:

- source files;
- Markdown documentation;
- Git remotes;
- shell scripts;
- `.env` files committed to Git;
- logs or command examples.

If a token is accidentally exposed, revoke it immediately and create a new one.

## Documentation Policy

Documentation must stay available in German and English. New conceptual or operational content should either be mirrored in both languages or clearly marked as pending translation.

The project must keep clear disclaimers:

- experimental work only;
- no warranty;
- no affiliation with Emby or VirtualTV;
- no commercial use without prior written permission.
