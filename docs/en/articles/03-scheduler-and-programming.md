# Scheduler and 24/7 Programming

## Goal

The scheduler does not create a random shuffle. It creates a program window. In the observed setup, a 36-hour horizon is planned and refreshed regularly.

## Candidate pools

Content is grouped into pools:

- `hot`: currently watched shows, continued where useful;
- `favorite`: direct favorites and derived episodes;
- `similar`: similar local media;
- `movie`: matching movies as variation.

The `mix_pattern` determines how these pools are combined.

## Similarity

Similar content is derived from metadata:

- genres;
- tags;
- studios;
- people;
- show relation;
- rating;
- production year or decade.

These attributes form a profile from seed content. Other local items are scored against that profile.

## Avoiding repetition

The scheduler uses:

- movie cooldown;
- episode cooldown;
- maximum same-series run;
- freeze window for the near future.

This reduces harsh jumps and excessive repetition.

## Why state matters

`state.json` stores the generated plan, history and IDs. Without state, every run would be too random and could constantly shift already visible program slots.
