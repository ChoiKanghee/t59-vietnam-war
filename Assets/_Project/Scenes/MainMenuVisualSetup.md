# M0A - main-menu visual integration

## Apply to the existing scene

1. Let Unity 6000.3.24f1 finish compiling, exit Play Mode, then open `Assets/_Project/Scenes/MainMenu.unity`. If several scenes are open, make MainMenu the active scene.
2. Choose **314 > M0 > Apply Main Menu Asset Pack**. Do not run the old scene-creation command for this milestone.
3. Inspect the Game view at **1920 x 1080**. Save with **Ctrl+S**. The command deliberately leaves the scene dirty instead of saving unrelated pending edits. Scene changes form one Undo operation.
4. Enter Play Mode. Start Journey should have gold selection art and a track-link icon. Continue should have disabled art and a 45%-opacity label while no save provider exists.
5. Verify mouse hover/click, keyboard Up/Down + Enter, and all three panels' Back/Escape behavior. Opening a panel hides the menu and logo while ambience remains behind it. Confirm scene routing and Quit still behave as in M0.
6. Exit Play Mode and run the command again. There should still be one `M0A Asset Pack Visuals` root, one logo, six focus icons, three smoke objects, and one presentation component per main button. Existing buttons, IDs, controller references, UnityEvents, navigation, and interactability are retained.
7. Inspect 16:10 and 4:3 views as well. Background/foreground cover the viewport by cropping; the tank preserves its aspect ratio. Very narrow portrait layouts are outside this landscape menu milestone.

No Build Profile changes or new scenes are required. Existing M0 scene entries remain `MainMenu` and `Level01_Training`.

## Asset locations and reference

`Assets/_Project/Art/UI/MainMenu` is absent in this checkout. The pack is already distributed under `Assets/_Project/Art/{Background,Foreground,Vehicle,UI,VFX}`. The command resolves exact PNG basenames recursively under Art, so it also works if the pack is later grouped under UI/MainMenu. Missing or duplicate required basenames abort before scene changes.

Reference documents inspected: `E:/314/314_MainMenu_Asset_Pack_A_v1/314_MainMenu_Asset_Pack_A/README.md`, `Docs/UNITY_SETUP.md`, `Docs/GENERATION_PROMPTS.md`, and `Preview/A10_main_menu_preview_1920x1080.png` / `.svg`. The preview is never imported or used as UI. The existing six English labels are preserved; the preview's additional Credits action is outside M0.

## Implementation

- `MainMenuAssetPackApplier` modifies only the existing Canvas's visual hierarchy and the import metadata of the PNGs it uses. It refuses another active scene or Play Mode. It retains MainMenuController, MenuPanel, EventSystem, and their wiring. Named visual children/components are reused on subsequent runs. Unexpected scene-edit errors roll back the grouped scene Undo operation.
- CanvasScaler uses Scale With Screen Size, 1920 x 1080, Match Width Or Height = 0.5. The menu/logo sit on the left; the stationary vehicle and its separate 314 decal sit at the lower right. Mountains and camp remain the single supplied background, with separate foreground vegetation above the vehicle.
- `MenuButtonVisual` only updates label color/alpha and the focus icon. Unity Button SpriteSwap provides normal, highlighted, selected, pressed, and disabled art. Existing Unity UI Text remains editable; no labels come from preview images. Existing selection logic still prefers Continue if a future valid save provider is available, as in M0; Start Journey is selected with no save.
- `MenuAmbience` uses unscaled time for three staggered six-second smoke wisps, with eight supplied individual smoke sprites, 42 pixels of rise and low opacity. Foreground moves +/-3 reference pixels over ten seconds, with slight overscan to conceal edges. No particles, camera movement, input handling, or gameplay are added.
- All decorative Images have Raycast Target disabled. Existing modal panels remain above the artwork.

## Sprite import settings to verify

The supplied PNGs were imported as Multiple sprites, including cropped button bounds. The command normalizes the **18 referenced PNG importers only** to Single, centered pivot, Full Rect, Sprite (2D and UI), Bilinear, Clamp, sRGB, no mipmaps, no NPOT scaling, Alpha Is Transparency, and Max Size 2048. Source PNG/SVG bytes are never changed. Importer updates persist separately from scene Undo; existing GUIDs are retained, but old sliced sub-sprite references to these assets would need reassignment if used elsewhere.

- Background: High Quality compression; verify camp detail at 1080p.
- Vehicle, foreground, logo, decal, focus icon, button states, and individual smoke frames: no compression; verify clean transparent edges.
- All four buttons: nine-slice borders **Left 32, Bottom 12, Right 100, Top 12**; UI Image Type = Sliced. Verify gold edges and arrow tips across states.
- Per-platform importer overrides are deliberately preserved. Verify they do not force smaller textures, opaque formats, or compression that damages alpha.
- Smoke uses the eight `VFX/Smoke/smoke_0N.png` files as full-canvas Single sprites. No sprite-sheet slicing is required. Unused smoke/leaf sheets, leaf clusters, SVGs, and their metadata are untouched.

When run, the command may update `.png.meta` for exactly these assets:

```text
Art/Background/A01_main_menu_background_1920x1080.png
Art/Vehicle/A02_tank_314_base_no_decal.png
Art/Foreground/A03_foreground_parallax.png
Art/UI/A04_logo_314.png
Art/UI/A05_button_default.png
Art/UI/A05_button_selected.png
Art/UI/A05_button_pressed.png
Art/UI/A05_button_disabled.png
Art/UI/A06_focus_track_link.png
Art/UI/A09_tank_decal_314.png
Art/VFX/Smoke/smoke_01.png
Art/VFX/Smoke/smoke_02.png
Art/VFX/Smoke/smoke_03.png
Art/VFX/Smoke/smoke_04.png
Art/VFX/Smoke/smoke_05.png
Art/VFX/Smoke/smoke_06.png
Art/VFX/Smoke/smoke_07.png
Art/VFX/Smoke/smoke_08.png
```

## Remaining placeholders and validation

Level Select, Journal, and Options contents remain the M0 placeholder panels. There is still no save implementation; Continue remains disabled without a provider. Training gameplay is untouched. Existing built-in UI font remains in use.

The new scripts were compiled with the installed Unity Roslyn compiler against this project's Unity 6 and package assemblies. Compilation succeeded with only CS0649 warnings for Inspector-assigned serialized fields. The active Unity session has not executed the command through this tool environment; scene rendering, repeated application, navigation, smoke appearance, and player-build behavior still require the Editor checks above. No scene YAML was patched directly.
