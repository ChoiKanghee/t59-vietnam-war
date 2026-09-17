# 314 — Truck Opening Asset Pack B2

## Training Camp Arrival

This is an additive extension for `314_TruckOpening_Asset_Pack_B_v1`. It contains only the new assets required for the transition from the forest edge to the training-camp gate, the stopping area, and the first controllable moment of Level 1.

The original B1 vehicle, wheels, driver, seated recruits, rural backgrounds, road, forest transition and dust VFX are intentionally **not duplicated** in this archive.

## Contents

- Side-view training-camp gate in both unsigned and named variants.
- Split entrance signage: weathered red nameplate with yellow star, exact Vietnamese title layer, and ready-composited version.
- Modular watchtower and fence segment.
- Training-yard midground.
- Separate tent, target, trench and obstacle-course props.
- Truck stopping-pad overlay.
- Four-frame Lam disembark sequence plus individual frame PNGs.
- Three supporting recruit disembark poses plus individual PNGs.
- Training instructor waiting pose.
- Three assembled 1920 × 1080 previews.
- Unity import, reuse and assembly notes.
- Reproducible preview-compositing script and generation prompts.

## Production rules

- All production art is PNG with a real alpha channel.
- UI and gameplay labels remain outside the environment artwork.
- The requested entrance title is supplied as a separate alpha layer and as an optional baked composite: `DOANH TRẠI / QUÂN ĐỘI NHÂN DÂN VIỆT NAM`.
- Use `B2_01e_gate_named_complete.png` for the finished gate, or assemble `B2_01_gate_no_sign.png`, the blank red-star nameplate and the title layer separately in Unity.
- Sheets are supplied for convenient bulk import; pre-cropped individual sprites are also included.
- Preview PNGs are flattened visual references, not production layers.

## Intended sequence

`Village → Rice field → Forest edge → Gate → Training yard → Truck stop → Lam disembarks → Player control`

See `Docs/REUSE_MAP.md` for the exact B1 dependencies and `Docs/UNITY_SETUP.md` for the recommended scene structure.
