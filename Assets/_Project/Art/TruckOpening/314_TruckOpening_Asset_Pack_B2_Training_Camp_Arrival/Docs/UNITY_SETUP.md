# Unity setup — Training Camp Arrival

## Import

For every production PNG:

- Texture Type: `Sprite (2D and UI)`
- Sprite Mode: `Single` for pre-cropped sprites; `Multiple` only if using the supplied sheets
- Mesh Type: `Full Rect` for background and ground layers; `Tight` for characters and props
- Alpha Source: `Input Texture Alpha`
- Alpha Is Transparency: enabled
- Filter Mode: match B1 (recommended `Bilinear` for this painted style)
- Compression: `None` while evaluating; choose platform compression only after checking alpha fringes

The individual Lam/recruit PNGs avoid runtime slicing. If importing sheets instead, use four equal columns for Lam and three equal columns for the supporting recruits.

## Suggested hierarchy

```text
TruckOpeningSequence
├── PersistentSkyAndMountains        (B1)
├── RouteSegments
│   ├── Village                     (B1)
│   ├── RiceField                   (B1)
│   ├── ForestEdge                  (B1)
│   ├── TrainingGate               (B2)
│   └── TrainingYard               (B2)
├── TruckRig                        (B1)
├── ArrivalCharacters              (B2)
└── RuntimeText
    └── TrainingCampSign            (TextMeshPro)
```

## Sorting order

| Layer | Suggested order | Examples |
|---|---:|---|
| Sky | -50 | B1 sky |
| Far mountains | -40 | B1 mountains |
| Yard midground | -30 | `B2_04` |
| Distant facilities | -20 | `B2_05a–d` |
| Fence / watchtower rear | -10 | `B2_02`, `B2_03` |
| Truck and characters | 0–10 | B1 truck rig, B2 actors |
| Gate foreground posts | 20 | `B2_01` when the truck passes through |
| Near foliage | 30 | optional B1 foreground |
| UI / subtitles | Canvas | Unity UI only |

## Route transition

1. In the menu attract loop, keep the B1 road and rural layers looping.
2. On `Start Journey`, stop spawning new loop tiles after the current tile exits.
3. Queue the non-looping route segments: village, rice field, forest edge, gate, yard.
4. Use `B2_01e_gate_named_complete.png` for the ready-made named gate. For localization or dynamic text, use the unsigned gate plus `B2_01b_gate_nameplate_red_star_blank.png`, then render the title with TextMeshPro instead of the supplied text layer.
5. Fade wheel dust down while the truck decelerates over `B2_06_truck_stop_pad_overlay.png`.
6. Stop wheel rotation and vehicle bob at the same marker.
7. Play Lam frames in order: `B2_07a → B2_07b → B2_07c → B2_07d`.
8. Spawn the two or three supporting recruits with small timing offsets.
9. Keep `B2_09_instructor_waiting.png` facing the arrivals.
10. Transfer control to the player only after the Lam idle frame is active.

Recommended stop markers:

- `ArrivalGateEntered`: switch from forest layers to yard layers.
- `TruckBrakeStart`: fade dust and reduce wheel speed.
- `TruckStop`: disable auto-scroll and lock the truck rig.
- `LamGrounded`: enable player input and tutorial prompt.

## Collider guidance

- Gate, watchtower, fence and distant training props are scenery and need no collider during the opening.
- Use one simple ground collider for the stopping area.
- Lam should receive the normal player capsule/box collider only on or immediately before the landing frame.
- Keep the other recruits on a non-player collision layer during the short arrival animation to prevent blocking the first movement tutorial.
