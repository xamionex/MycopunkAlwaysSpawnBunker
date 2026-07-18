# MycopunkAlwaysSpawnBunker

Makes the Bunker encounter always spawn.

## Warning

Was annoyed and so I quickly made this, dont expect updates

## What it does

Normally, whenever a level generates, the Bunker encounter has to win a weighted
random draw against every other possible encounter for one of a handful of
available slots. This mod pushes the Bunker's spawn weight far above every other
encounter's, so it wins that draw essentially every time a level is generated.

## Installation

**With a mod manager (recommended):** install through the Gale Mod Manager, click "Install with Mod Manager" on the package page and it will handle BepInEx setup for you.

**Manual install:**
1. Install [BepInExPack for Mycopunk](https://thunderstore.io/c/mycopunk/p/BepInEx/BepInExPack/) first if you haven't already.
2. Drop `MycopunkAlwaysSpawnBunker.dll` into `<Mycopunk>/BepInEx/plugins/`.
3. Launch the game.

## Compatibility

- Requires BepInEx (tested against BepInExPack 5.4.2403).
- Client-side patch — only affects the Bunker's own spawn logic, so it should be
  safe to run in multiplayer, but every player who wants the effect needs it
  installed.
- Should be compatible with other encounter/loot mods unless they also patch
  `BunkerEncounter`'s spawn weight or `CanSpawn`.

## Known limitations

This works by making the Bunker's weight overwhelmingly larger than every other
encounter's during the weighted random draw - it isn't a hard override of the
selection logic. In practice this means it wins essentially every time, but it
is not a mathematically absolute guarantee.
